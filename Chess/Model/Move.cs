using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Move : ICommand
{
    private readonly Square _from;

    private readonly Piece? _pieceCaptured;

    // State needed to make undo fully reversible, including castling and tracking if the pieces had already moved.
    private readonly bool _pieceMovedHadMovedBefore;
    private Rook? _castlingRook;

    private bool _hasExecuted;

    private bool? _isCheck;
    private bool? _preventsMovement;

    public Move(Square from, Square to, Piece pieceMoved, Piece? pieceCaptured = null)
    {
        _from = from;
        To = to;
        PieceMoved = pieceMoved;
        _pieceCaptured = pieceCaptured;
        _hasExecuted = false;
        _isCheck = null;
        _preventsMovement = null;

        _pieceMovedHadMovedBefore = pieceMoved.HasMoved;
        _castlingRook = null;
    }

    private bool PreventsMovement
    {
        //Needs to be lazy evaluated to avoid an infinite loop in the constructor - but needs to be cached because it must be calculated before execution.
        get
        {
            if (_preventsMovement is null)
            {
                CalculateCheckStatus();
            }

            return _preventsMovement!.Value;
        }
    }

    private bool IsCheck
    {
        //Needs to be lazy evaluated to avoid an infinite loop in the constructor - but needs to be cached because it must be calculated before execution.
        get
        {
            if (_isCheck is null)
            {
                CalculateCheckStatus();
            }

            return _isCheck!.Value;
        }
    }

    public bool IsCheckmate => IsCheck && PreventsMovement;

    public bool IsStalemate => !IsCheck && PreventsMovement;

    public Piece PieceMoved { get; }

    public Square To { get; }

    public bool IsDoublePush
    {
        //This mainly exists to help calculate en passant moves.
        get
        {
            //If it's not a pawn being moved, then it's not a double push.
            if (PieceMoved is not Pawn) return false;

            //Check that the rank of the To and From squares differ by 2.
            return _from.Rank == To.Rank + 2 || _from.Rank == To.Rank - 2;
        }
    }

    private bool IsKingSideCastling => PieceMoved is King && To.GetAlgebraicPosition() is "g1" or "g8" && _from.GetAlgebraicPosition() is "e1" or "e8";
    private bool IsQueenSideCastling => PieceMoved is King && To.GetAlgebraicPosition() is "c1" or "c8" && _from.GetAlgebraicPosition() is "e1" or "e8";
    private bool IsPromotion => PieceMoved is Pawn && To.Rank is 0 or 7;

    public void Execute()
    {
        Execute(false);
    }

    public void Undo()
    {
        //Moves can only be undone if they are the most recently executed move. The way this is intended to be used, this shouldn't become a problem,
        //however, I would like to add a check in here if I can think of how to do it. TODO: Add this check.

        if (!_hasExecuted) throw new InvalidOperationException("Cannot undo a move that hasn't been executed.");
        Board board = _from.Board == To.Board ? _from.Board : throw new ArgumentException("Cannot move between board objects");

        if (IsPromotion)
        {
            // Remove the promoted piece from the destination square
            Piece pieceOnTo = board.PieceAt(To) ?? throw new NullReferenceException("No promoted piece found - you can only undo the most recently executed move.");
            board.Pieces.Remove(pieceOnTo);

            // Restore the original pawn
            //No need to mess with the HasMoved state, since it must have already moved if it got promoted.
            board.Pieces.Add(PieceMoved);
            PieceMoved.Location = _from;

            // Restore any captured piece
            if (_pieceCaptured is not null) board.Pieces.Add(_pieceCaptured);

            _hasExecuted = false;
            return;
        }

        if (IsKingSideCastling || IsQueenSideCastling)
        {
            //The rook's home square should be file 7 for king side castling and file 0 for queen side castling.
            Square rookHomeSquare = board.SquareAt(To.Rank, IsKingSideCastling ? 7 : 0);

            if (_castlingRook is null) throw new NullReferenceException("Attempted to undo castling but this was not a castling move");

            //Move the pieces back
            PieceMoved.Location = _from;
            _castlingRook.Location = rookHomeSquare;

            //The pieces must have never moved since this move was castling.
            PieceMoved.HasMoved = false;
            _castlingRook.HasMoved = false;
            _hasExecuted = false;
            return;
        }

        //Restore any captured piece
        if (_pieceCaptured is not null) board.Pieces.Add(_pieceCaptured);

        // Move the piece back to the original square and restore HasMoved to whatever it was before the move.
        PieceMoved.Location = _from;
        PieceMoved.HasMoved = _pieceMovedHadMovedBefore;
        _hasExecuted = false;
    }


    public void Execute(bool suppressCheckStatusCalculation)
    {
        if (_hasExecuted) throw new InvalidOperationException("Cannot execute a move that has already been executed.");
        Board board = _from.Board == To.Board ? _from.Board : throw new ArgumentException("Cannot move between board objects");

        //We need the ability to suppress this to avoid recursive calls when cloning and executing moves (Because CalculateCheckStatus calls Execute on a clone)
        if (!suppressCheckStatusCalculation && (_preventsMovement is null || _isCheck is null)) CalculateCheckStatus();

        if (IsPromotion)
        {
            board.Pieces.Add(new Queen(PieceMoved.Color, To, true)); //TODO: Add the ability to promote to other than a Queen.
            board.Pieces.Remove(PieceMoved);
            if (_pieceCaptured is not null) board.Pieces.Remove(_pieceCaptured);
            _hasExecuted = true;
            return;
        }

        if (IsKingSideCastling || IsQueenSideCastling)
        {
            Square squareToMoveRookTo;
            if (IsKingSideCastling)
            {
                _castlingRook = board.Pieces.Find(p => p.Location.GetAlgebraicPosition() is "h1" or "h8" && p.Color == PieceMoved.Color) as Rook;
                squareToMoveRookTo = board.SquareAt(To.Rank, 5);
            }
            else
            {
                _castlingRook = board.Pieces.Find(p => p.Location.GetAlgebraicPosition() is "a1" or "a8" && p.Color == PieceMoved.Color) as Rook;
                squareToMoveRookTo = board.SquareAt(To.Rank, 3);
            }

            if (_castlingRook is null) throw new NullReferenceException("Castling attempted but there was no rook to castle with");

            PieceMoved.Location = To;
            _castlingRook.Location = squareToMoveRookTo;
            _hasExecuted = true;
            return;
        }

        if (_pieceCaptured is not null) board.Pieces.Remove(_pieceCaptured);
        PieceMoved.Location = To;
        _hasExecuted = true;
    }


    public string GetAlgebraicMove() //TODO: Add Piece Disambiguation.
    {
        if (IsQueenSideCastling) return "0-0-0";
        if (IsKingSideCastling) return "0-0";

        string algebraicMove = GetAlgebraicPieceLetter();

        if (_pieceCaptured is not null) algebraicMove += "x";

        algebraicMove += To.GetAlgebraicPosition();

        if (IsPromotion) algebraicMove += "=Q"; //TODO: Add the ability to promote to other than a Queen.

        if (IsCheck && !IsCheckmate) algebraicMove += "+";
        if (IsCheckmate) algebraicMove += "#";

        return algebraicMove;
    }

    private string GetAlgebraicPieceLetter()
    {
        return PieceMoved switch
        {
            King => "K",
            Queen => "Q",
            Knight => "N",
            Bishop => "B",
            Rook => "R",
            Pawn => "",
            _ => throw new ArgumentException("Piece must be one of the piece types: King, Queen, Knight, Bishop, Rook, Pawn")
        };
    }

    public Move Clone(Board clonedBoard)
    {
        if (_hasExecuted) throw new InvalidOperationException("Cannot clone a move that has already been executed.");

        Square clonedFrom = clonedBoard.SquareCalled(_from.GetAlgebraicPosition());
        Square clonedTo = clonedBoard.SquareCalled(To.GetAlgebraicPosition());
        Piece clonedPiece = clonedBoard.PieceAt(clonedFrom) ?? throw new NullReferenceException("Piece to be moved is null - something went wrong with the cloning process");

        Piece? clonedCaptured = null;
        if (_pieceCaptured is not null)
        {
            //In a previous implementation, it was assumed that a captured piece would always be at this.To.
            //This did not support en passant, obviously, and so hence why this implementation seems complicated.
            Square capturedLocationOnOriginalBoard = _pieceCaptured.Location;
            Square clonedCapturedSquare = clonedBoard.SquareCalled(capturedLocationOnOriginalBoard.GetAlgebraicPosition());
            clonedCaptured = clonedBoard.PieceAt(clonedCapturedSquare);
        }

        return new Move(clonedFrom, clonedTo, clonedPiece, clonedCaptured);
    }

    private void CalculateCheckStatus()
    {
        //Clone this move and execute on a copy of the board.
        Board clonedBoard = PieceMoved.Location.Board.Clone();
        Clone(clonedBoard).Execute(true);

        //See if the opponent is in check after the execution.
        King? opponentKing = clonedBoard.Pieces.Find(p => p is King && p.Color != PieceMoved.Color) as King;
        _isCheck = opponentKing?.IsInCheck ?? throw new NullReferenceException("King not found - something has gone seriously wrong.");

        //See if the opponent is able to make any legal moves.
        List<Move> opponentLegalMoves = new List<Move>();
        foreach (Piece p in clonedBoard.Pieces.Where(p => p.Color != PieceMoved.Color))
        {
            opponentLegalMoves.AddRange(p.GetLegalMoves());
        }

        _preventsMovement = opponentLegalMoves.Count == 0;
    }
}
