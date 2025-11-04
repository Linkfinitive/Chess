using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model;

public class Square
{
    public Square(Board board, int rank, int file, PlayerColors color)
    {
        if (rank is < 0 or > 7) throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be between 0 and 7 inclusive.");

        if (file is < 0 or > 7) throw new ArgumentOutOfRangeException(nameof(file), "File must be between 0 and 7 inclusive.");

        Color = color;
        Rank = rank;
        File = file;
        Board = board;
    }

    public Board Board { get; }

    public PlayerColors Color { get; }
    public int Rank { get; }
    public int File { get; }

    public string GetAlgebraicPosition()
    {
        string algebraicRank = $"{Rank + 1}";

        string[] letters = ["a", "b", "c", "d", "e", "f", "g", "h"];
        string algebraicFile = letters[File];

        return $"{algebraicFile}{algebraicRank}";
    }

    public Piece? PieceOnSquare(Board board)
    {
        foreach (Piece p in board.Pieces)
        {
            if (p.Location == this)
            {
                return p;
            }
        }

        return null;
    }

    public bool IsAttackedBy(PlayerColors player)
    {
        foreach (Piece p in Board.Pieces.Where(p => p.Color == player))
        {
            //We are iterating the attacked squares, not legal moves, here on purpose: the reason is that pinned
            //pieces are still considered to be controlling their squares and can give check and checkmate.
            //Thanks to https://www.chess.com/forum/view/general/pinned-piece-allowing-mate for the clarification.
            foreach (Square s in p.GetAttackedSquares())
            {
                if (s == this)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
