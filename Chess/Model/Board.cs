namespace Chess.Model;

public class Board
{
    private List<Piece> _pieces;
    private List<Square> _squares;

    public Board() : base()
    {
        _squares = new List<Square>();
        _pieces = new List<Piece>();

        //Add all the squares
        for (int i = 0; i < 64; i++)
        {
            int rankToAdd = (int)Math.Floor((double)i / 8);
            int fileToAdd = i % 8;
            PlayerColors colorToAdd = ((rankToAdd + fileToAdd) % 2 == 0) ? PlayerColors.BLACK : PlayerColors.WHITE;

            _squares.Add(new Square(rankToAdd, fileToAdd, colorToAdd));
        }

        //Add white's pieces
        _pieces.Add(new Rook(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "a1")!));
        _pieces.Add(new Knight(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "b1")!));
        _pieces.Add(new Bishop(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "c1")!));
        _pieces.Add(new Queen(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "d1")!));
        _pieces.Add(new King(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "e1")!));
        _pieces.Add(new Bishop(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "f1")!));
        _pieces.Add(new Knight(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "g1")!));
        _pieces.Add(new Rook(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "h1")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "a2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "b2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "c2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "d2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "e2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "f2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "g2")!));
        _pieces.Add(new Pawn(PlayerColors.WHITE, _squares.Find(s => s.getAlgebraicPosition() == "h2")!));

        //Add black's pieces
        _pieces.Add(new Rook(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "a8")!));
        _pieces.Add(new Knight(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "b8")!));
        _pieces.Add(new Bishop(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "c8")!));
        _pieces.Add(new Queen(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "d8")!));
        _pieces.Add(new King(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "e8")!));
        _pieces.Add(new Bishop(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "f8")!));
        _pieces.Add(new Knight(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "g8")!));
        _pieces.Add(new Rook(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "h8")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "a7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "b7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "c7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "d7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "e7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "f7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "g7")!));
        _pieces.Add(new Pawn(PlayerColors.BLACK, _squares.Find(s => s.getAlgebraicPosition() == "h7")!));
    }

    public List<Square> getSquares()
    {
        return _squares;
    }

    public List<Piece> getPieces()
    {
        return _pieces;
    }
}
