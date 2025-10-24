using Chess.Controller;
using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Move : ICommand
{
    private readonly Square _from;
    private readonly bool _isCheck;
    private readonly bool _isCheckmate;
    private readonly Piece? _pieceCaptured;
    private readonly Piece _pieceMoved;


    public Move(Square from, Square to, Piece pieceMoved, Piece? pieceCaptured = null, bool isCheck = false, bool isCheckmate = false)
    {
        if (isCheck && isCheckmate) throw new ArgumentException("Move cannot be both Check and Checkmate");

        _from = from;
        To = to;
        _pieceMoved = pieceMoved;
        _pieceCaptured = pieceCaptured;
        _isCheck = isCheck;
        _isCheckmate = isCheckmate;
    }

    public Square To { get; }

    private bool IsKingSideCastling => typeof(King) == _pieceMoved.GetType() && To.GetAlgebraicPosition() is "g1" or "g8" && _from.GetAlgebraicPosition() is "e1" or "e8";
    private bool IsQueenSideCastling => typeof(King) == _pieceMoved.GetType() && To.GetAlgebraicPosition() is "c1" or "c8" && _from.GetAlgebraicPosition() is "e1" or "e8";
    private bool IsPromotion => typeof(Pawn) == _pieceMoved.GetType() && To.Rank is 0 or 7;

    public void Execute()
    {
        if (IsPromotion)
        {
            GameController.Instance.Board.Pieces.Add(new Queen(_pieceMoved.Color, To)); //TODO: Add the ability to promote to other than a Queen.
            GameController.Instance.Board.Pieces.Remove(_pieceMoved);
            if (_pieceCaptured is not null) GameController.Instance.HandleCapture(_pieceCaptured);
            return;
        }

        if (IsKingSideCastling)
        {
            _pieceMoved.Location = To;
            Piece? rookToCastleWith = GameController.Instance.Board.Pieces.Find(p => p.Location.GetAlgebraicPosition() is ("h1" or "h8") && p.Color == _pieceMoved.Color);
            Square? squareToMoveRookTo = GameController.Instance.Board.Squares.Find(s => s.Rank == To.Rank && s.File == 5);

            if (rookToCastleWith is null || squareToMoveRookTo is null) throw new NullReferenceException("Castling attempted but there was no rook to castle with");

            rookToCastleWith.Location = squareToMoveRookTo;
            return;
        }

        if (IsQueenSideCastling)
        {
            _pieceMoved.Location = To;
            Piece? rookToCastleWith = GameController.Instance.Board.Pieces.Find(p => p.Location.GetAlgebraicPosition() is ("a1" or "a8") && p.Color == _pieceMoved.Color);
            Square? squareToMoveRookTo = GameController.Instance.Board.Squares.Find(s => s.Rank == To.Rank && s.File == 3);

            if (rookToCastleWith is null || squareToMoveRookTo is null) throw new NullReferenceException("Castling attempted but there was no rook to castle with");

            rookToCastleWith.Location = squareToMoveRookTo;
            return;
        }

        if (_pieceCaptured is not null) GameController.Instance.HandleCapture(_pieceCaptured);
        _pieceMoved.Location = To;
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }

    public string GetAlgebraicMove() //TODO: Add Piece Disambiguation.
    {
        if (IsQueenSideCastling) return "0-0-0";
        if (IsKingSideCastling) return "0-0";

        string algebraicMove = GetAlgebraicPieceLetter();

        if (_pieceCaptured is not null) algebraicMove += "x";

        algebraicMove += To.GetAlgebraicPosition();

        if (IsPromotion) algebraicMove += "=Q"; //TODO: Add the ability to promote to other than a Queen.

        if (_isCheck) algebraicMove += "+";
        if (_isCheckmate) algebraicMove += "#";

        return algebraicMove;
    }

    private string GetAlgebraicPieceLetter()
    {
        return _pieceMoved switch
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
}
