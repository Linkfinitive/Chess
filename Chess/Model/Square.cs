namespace Chess.Model;

public class Square
{
    private PlayerColors _color;
    private int _rank;
    private int _file;

    public Square(int rank, int file, PlayerColors color)
    {

        if (rank < 0 || rank > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(rank), "Rank must be between 0 and 7 inclusive.");
        }

        if (file < 0 || file > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(file), "File must be between 0 and 7 inclusive.");
        }

        _color = color;
        _rank = rank;
        _file = file;
    }

    public string getAlgebraicPosition()
    {
        string algebraicRank = $"{_rank + 1}";

        string algebraicFile = "";
        switch (_file)
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
                throw new ArgumentOutOfRangeException(nameof(_file), "File must be between 0 and 7 inclusive.");
        }
        return $"{algebraicFile}{algebraicRank}";
    }

    public (int, int) getRankAndFile()
    {
        return (_rank, _file);
    }

    public PlayerColors getColor() { return _color; }
}
