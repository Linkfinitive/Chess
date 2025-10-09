namespace Model;

public class Move : ICommand
{
    private Square _from;
    private Square _to;
    private Piece _pieceMoved;
    private Piece? _pieceCaptured;
    private Boolean _promotion;

    public string getAlgebraicMove()
    {
        return "";
    }

    public void execute()
    {

    }

    public void undo()
    {

    }
}
