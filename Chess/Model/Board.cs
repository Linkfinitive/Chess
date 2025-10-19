using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Board
{
    public Board()
    {
        Squares = new List<Square>();
        Pieces = new List<Piece>();

        //Add all the squares
        for (int i = 0; i < 64; i++)
        {
            int rankToAdd = (int)Math.Floor((double)i / 8);
            int fileToAdd = i % 8;
            PlayerColors colorToAdd = (rankToAdd + fileToAdd) % 2 == 0 ? PlayerColors.BLACK : PlayerColors.WHITE;

            Squares.Add(new Square(rankToAdd, fileToAdd, colorToAdd));
        }

        //Add white's pieces
        Pieces.Add(new Rook(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "a1")!));
        Pieces.Add(new Knight(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "b1")!));
        Pieces.Add(new Bishop(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "c1")!));
        Pieces.Add(new Queen(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "d1")!));
        Pieces.Add(new King(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "e1")!));
        Pieces.Add(new Bishop(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "f1")!));
        Pieces.Add(new Knight(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "g1")!));
        Pieces.Add(new Rook(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "h1")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "a2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "b2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "c2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "d2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "e2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "f2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "g2")!));
        Pieces.Add(new Pawn(PlayerColors.WHITE, Squares.Find(s => s.GetAlgebraicPosition() == "h2")!));

        // //Add black's pieces
        Pieces.Add(new Rook(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "a8")!));
        Pieces.Add(new Knight(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "b8")!));
        Pieces.Add(new Bishop(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "c8")!));
        Pieces.Add(new Queen(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "d8")!));
        Pieces.Add(new King(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "e8")!));
        Pieces.Add(new Bishop(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "f8")!));
        Pieces.Add(new Knight(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "g8")!));
        Pieces.Add(new Rook(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "h8")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "a7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "b7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "c7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "d7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "e7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "f7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "g7")!));
        Pieces.Add(new Pawn(PlayerColors.BLACK, Squares.Find(s => s.GetAlgebraicPosition() == "h7")!));
    }

    public List<Piece> Pieces { get; }
    public List<Square> Squares { get; }
}
