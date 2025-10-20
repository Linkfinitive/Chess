using Chess.Controller;
using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Move : ICommand
{
    private readonly Piece? _pieceCaptured;
    private readonly Piece _pieceMoved;
    private Square _from;


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
        if (_pieceCaptured is not null) GameController.Instance.HandleCapture(_pieceCaptured);
        _pieceMoved.Location = To;

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
