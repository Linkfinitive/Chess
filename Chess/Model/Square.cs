using Chess.Global;

namespace Chess.Model;

public class Square
{
    public Square(int rank, int file, PlayerColors color)
    {
        if (rank < 0 || rank > 7) throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be between 0 and 7 inclusive.");

        if (file < 0 || file > 7) throw new ArgumentOutOfRangeException(nameof(file), "File must be between 0 and 7 inclusive.");

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

        string algebraicFile = "";
        switch (File)
        {
            case 0:
                algebraicFile = "a";
                break;
            case 1:
                algebraicFile = "b";
                break;
            case 2:
                algebraicFile = "c";
                break;
            case 3:
                algebraicFile = "d";
                break;
            case 4:
                algebraicFile = "e";
                break;
            case 5:
                algebraicFile = "f";
                break;
            case 6:
                algebraicFile = "g";
                break;
            case 7:
                algebraicFile = "h";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(File), "File must be between 0 and 7 inclusive.");
        }

        return $"{algebraicFile}{algebraicRank}";
    }
}