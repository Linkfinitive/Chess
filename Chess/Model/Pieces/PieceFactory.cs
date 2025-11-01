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

        //Add White's pieces
        pieces.Add(CreatePiece("rook", PlayerColors.WHITE, board.SquareCalled("a1")));
        pieces.Add(CreatePiece("knight", PlayerColors.WHITE, board.SquareCalled("b1")));
        pieces.Add(CreatePiece("bishop", PlayerColors.WHITE, board.SquareCalled("c1")));
        pieces.Add(CreatePiece("queen", PlayerColors.WHITE, board.SquareCalled("d1")));
        pieces.Add(CreatePiece("king", PlayerColors.WHITE, board.SquareCalled("e1")));
        pieces.Add(CreatePiece("bishop", PlayerColors.WHITE, board.SquareCalled("f1")));
        pieces.Add(CreatePiece("knight", PlayerColors.WHITE, board.SquareCalled("g1")));
        pieces.Add(CreatePiece("rook", PlayerColors.WHITE, board.SquareCalled("h1")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("a2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("b2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("c2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("d2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("e2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("f2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("g2")));
        pieces.Add(CreatePiece("pawn", PlayerColors.WHITE, board.SquareCalled("h2")));

        //Add Black's pieces
        pieces.Add(CreatePiece("rook", PlayerColors.BLACK, board.SquareCalled("a8")));
        pieces.Add(CreatePiece("knight", PlayerColors.BLACK, board.SquareCalled("b8")));
        pieces.Add(CreatePiece("bishop", PlayerColors.BLACK, board.SquareCalled("c8")));
        pieces.Add(CreatePiece("queen", PlayerColors.BLACK, board.SquareCalled("d8")));
        pieces.Add(CreatePiece("king", PlayerColors.BLACK, board.SquareCalled("e8")));
        pieces.Add(CreatePiece("bishop", PlayerColors.BLACK, board.SquareCalled("f8")));
        pieces.Add(CreatePiece("knight", PlayerColors.BLACK, board.SquareCalled("g8")));
        pieces.Add(CreatePiece("rook", PlayerColors.BLACK, board.SquareCalled("h8")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("a7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("b7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("c7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("d7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("e7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("f7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("g7")));
        pieces.Add(CreatePiece("pawn", PlayerColors.BLACK, board.SquareCalled("h7")));

        return pieces;
    }
}
