using Chess.Global;

namespace Chess.Model;

public class Square
{
    public Square(int rank, int file, PlayerColors color)
    {
        if (rank is < 0 or > 7) throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be between 0 and 7 inclusive.");

        if (file is < 0 or > 7) throw new ArgumentOutOfRangeException(nameof(file), "File must be between 0 and 7 inclusive.");

        Color = color;
        Rank = rank;
        File = file;
    }

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
}
