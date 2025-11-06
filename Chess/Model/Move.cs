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
    private bool? _isCheckmate;


    public Move(Square from, Square to, Piece pieceMoved, Piece? pieceCaptured = null)
    {
        _from = from;
        To = to;
        PieceMoved = pieceMoved;
        _pieceCaptured = pieceCaptured;
        _hasExecuted = false;

        _pieceMovedHadMovedBefore = pieceMoved.HasMoved;
        _castlingRook = null;

        _isCheck = null;
        _isCheckmate = null;
    }

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

    public void Undo()
    {
        //Moves can only be undone if they are the most recently executed move. The way this is intended to be used, this shouldn't become a problem,
        //however, I would like to add a check in here if I can think of how to do it.

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

    public void Execute()
    {
        Execute(false);
    }

    public void Execute(bool suppressCheckStatusCalculation)
    {
        if (_hasExecuted) throw new InvalidOperationException("Cannot execute a move that has already been executed.");

        Board board = _from.Board == To.Board ? _from.Board : throw new ArgumentException("Cannot move between board objects");

        if (IsPromotion)
            //Currently it's only possible to promote to a Queen. This is fine for most games but I would like to add the ability
            //to promote to other pieces if possible in the future.
        {
            board.Pieces.Add(new Queen(PieceMoved.Color, To, true));
            board.Pieces.Remove(PieceMoved);
            if (_pieceCaptured is not null) board.Pieces.Remove(_pieceCaptured);
            _hasExecuted = true;
            if (!suppressCheckStatusCalculation) CalculateCheckStatus(board);
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

            _castlingRook.Location = squareToMoveRookTo;
            _hasExecuted = true;
            if (!suppressCheckStatusCalculation) CalculateCheckStatus(board);
            return;
        }

        if (_pieceCaptured is not null) board.Pieces.Remove(_pieceCaptured);
        PieceMoved.Location = To;
        _hasExecuted = true;
        if (!suppressCheckStatusCalculation) CalculateCheckStatus(board);
    }

    public string GetAlgebraicMove()
        //Unfortunately, this function does not have "piece disambiguation." If two pieces of the same type and colour could move to the same
        //square, then this gives no indication of which piece moved. While it isn't perfect, it's okay for seeing an overview of the game that
        //has been played, and also checking the engine moves if it moves too quickly and you miss it. Hopefully this limitation can be addressed in the future.
    {
        if (IsQueenSideCastling) return "0-0-0";
        if (IsKingSideCastling) return "0-0";

        string algebraicMove = GetAlgebraicPieceLetter();

        if (_pieceCaptured is not null)
        {
            //If it's a pawn capture we need to add in the file letter.
            if (PieceMoved is Pawn) algebraicMove += _from.GetAlgebraicPosition()[0];
            algebraicMove += "x";
        }

        algebraicMove += To.GetAlgebraicPosition();

        if (IsPromotion) algebraicMove += "=Q";

        //These variables will never be null if the move has executed. If the move has not been executed, then we can't get the algebraic move.
        // This should not be a problem, based on the way this is used, however it is not ideal and I would like to fix it at some point.
        if (!_hasExecuted) throw new InvalidOperationException("Cannot get algebraic move of a move that has not been executed.");
        if (_isCheck is null || _isCheckmate is null) throw new InvalidOperationException("Check and checkmate status have not been calculated for this move.");
        if (_isCheckmate!.Value)
        {
            algebraicMove += "#";
        }
        else if (_isCheck!.Value) algebraicMove += "+";

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

    private void CalculateCheckStatus(Board board)
    {
        if (!_hasExecuted) throw new InvalidOperationException("Cannot calculate check status for a move that has not been executed.");

        King opponentKing = PieceMoved.Color == PlayerColors.WHITE ? board.BlackKing : board.WhiteKing;
        _isCheck = opponentKing.IsInCheck;

        //See if the opponent is able to make any legal moves.
        //Cloning must be used here - otherwise it suffers from a strange bug where pieces can take pieces but then
        //both pieces are present on the same square. This must be related somehow to the almighty superbug.
        int numberOfLegalMoves = 0;
        Board clonedBoard = board.Clone();
        foreach (Piece p in board.Pieces.Where(p => p.Color != PieceMoved.Color))
        {
            Piece? clonedPiece = clonedBoard.PieceAt(clonedBoard.SquareCalled(p.Location.GetAlgebraicPosition()));
            if (clonedPiece is null) throw new NullReferenceException("Piece not found - something went wrong with the cloning process.");
            numberOfLegalMoves += clonedPiece.GetLegalMoves().Count;
        }

        _isCheckmate = numberOfLegalMoves == 0 && _isCheck.Value;
    }
}
