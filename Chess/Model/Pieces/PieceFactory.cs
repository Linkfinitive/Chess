using Chess.Global;

namespace Chess.Model.Pieces;

public static class PieceFactory
{
    public static Piece CreatePiece(string type, PlayerColors color, Square location, bool hasMoved = false)
    {
        return type.ToLower() switch
        {
            "king" => new King(color, location, hasMoved),
            "queen" => new Queen(color, location, hasMoved),
            "rook" => new Rook(color, location, hasMoved),
            "bishop" => new Bishop(color, location, hasMoved),
            "knight" => new Knight(color, location, hasMoved),
            "pawn" => new Pawn(color, location, hasMoved),
            _ => throw new ArgumentException($"Invalid piece type: {type}")
        };
    }

    public static List<Piece> SetupInitialPosition(Board board)
    {
        List<Piece> pieces = new List<Piece>();

        List<Square> squares = board.Squares;

        //Add White's pieces
        pieces.Add(new Rook(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "a1")!));
        pieces.Add(new Knight(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "b1")!));
        pieces.Add(new Bishop(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "c1")!));
        pieces.Add(new Queen(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "d1")!));
        pieces.Add(new King(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "e1")!));
        pieces.Add(new Bishop(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "f1")!));
        pieces.Add(new Knight(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "g1")!));
        pieces.Add(new Rook(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "h1")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "a2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "b2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "c2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "d2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "e2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "f2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "g2")!));
        pieces.Add(new Pawn(PlayerColors.WHITE, squares.Find(s => s.GetAlgebraicPosition() == "h2")!));

        // //Add Black's pieces
        pieces.Add(new Rook(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "a8")!));
        pieces.Add(new Knight(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "b8")!));
        pieces.Add(new Bishop(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "c8")!));
        pieces.Add(new Queen(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "d8")!));
        pieces.Add(new King(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "e8")!));
        pieces.Add(new Bishop(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "f8")!));
        pieces.Add(new Knight(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "g8")!));
        pieces.Add(new Rook(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "h8")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "a7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "b7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "c7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "d7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "e7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "f7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "g7")!));
        pieces.Add(new Pawn(PlayerColors.BLACK, squares.Find(s => s.GetAlgebraicPosition() == "h7")!));

        return pieces;
    }
}