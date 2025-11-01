using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Board
{
    public Board(bool setupInitialPosition = true)
    {
        Squares = new List<Square>();
        Pieces = new List<Piece>();

        //Add all the squares
        for (int i = 0; i < 64; i++)
        {
            int rankToAdd = (int)Math.Floor((double)i / 8);
            int fileToAdd = i % 8;
            PlayerColors colorToAdd = (rankToAdd + fileToAdd) % 2 == 0 ? PlayerColors.BLACK : PlayerColors.WHITE;

            Squares.Add(new Square(this, rankToAdd, fileToAdd, colorToAdd));
        }

        if (setupInitialPosition) Pieces = PieceFactory.SetupInitialPosition(this);
    }

    public List<Piece> Pieces { get; }
    public List<Square> Squares { get; }

    public Square SquareCalled(string algebraicPosition)
    {
        foreach (Square s in Squares)
        {
            if (s.GetAlgebraicPosition() == algebraicPosition)
            {
                return s;
            }
        }

        throw new ArgumentOutOfRangeException($"There is no such square as {algebraicPosition}");
    }

    public Square SquareAt(int rank, int file)
    {
        Square? foundSquare = Squares.Find(s => s.Rank == rank && s.File == file);
        return foundSquare ?? throw new ArgumentOutOfRangeException($"There is no square at rank {rank} and file {file}");
    }

    public Piece? PieceAt(Square square)
    {
        return square.PieceOnSquare(this);
    }

    public Board Clone()
    {
        Board clonedBoard = new Board(false);
        foreach (Piece p in Pieces)
        {
            Piece clonedPiece = p.Clone(clonedBoard);
            clonedBoard.Pieces.Add(clonedPiece);
        }

        return clonedBoard;
    }
}