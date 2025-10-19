using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Move : ICommand
{
    private Square _from;
    private Piece? _pieceCaptured;
    private Piece _pieceMoved;


    public Move(Square from, Square to, Piece pieceMoved, Piece? pieceCaptured)
    {
        _from = from;
        To = to;
        _pieceMoved = pieceMoved;
        _pieceCaptured = pieceCaptured;
    }

    public Square To { get; }

    public void Execute()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }

    public string GetAlgebraicMove()
    {
        throw new NotImplementedException();
    }
}
