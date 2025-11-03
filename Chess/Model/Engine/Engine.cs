using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model.Engine;

public class Engine
{
    private readonly IEvaluationStrategy _evaluationStrategy;

    public Engine(PlayerColors playingAs)
    {
        _evaluationStrategy = new CountMaterialStrategy();
        PlayingAs = playingAs;
    }

    public PlayerColors PlayingAs { get; }

    public async Task<Move> FindBestMove(Board board, int depth)
    {
        //This method is async so that the UI doesn't freeze during evaluation.
        //We're running the entire recursive evaluation on a cloned board, so that it doesn't
        //mess up the UI when the engine is rapidly trying moves.
        Board clonedBoard = board.Clone();

        //Precompute the king objects so that it doesn't have to be done more than once.
        King whiteKing = clonedBoard.Pieces.Find(p => p is King && p.Color == PlayerColors.WHITE) as King ?? throw new Exception("White king not found");
        King blackKing = clonedBoard.Pieces.Find(p => p is King && p.Color == PlayerColors.BLACK) as King ?? throw new Exception("Black king not found");

        (_, Move? bestMove) = await Task.Run(() => Evaluate(clonedBoard, depth, PlayingAs, whiteKing, blackKing));
        return bestMove?.Clone(board) ?? throw new NullReferenceException("Best move not found.");
    }

    private (int, Move?) Evaluate(Board board, int depth, PlayerColors playerToMove, King whiteKing, King blackKing)
    {
        //Recursive depth cutoff.
        if (depth == 0)
        {
            int evaluation = _evaluationStrategy.Evaluate(board);
            return (playerToMove == PlayerColors.WHITE ? evaluation : -evaluation, null);
        }

        //Get all the legal moves for the player to move.
        //Using a for loop instead of LINQ to avoid the overhead of 2 IEnumerators.
        List<Move> allLegalMoves = new List<Move>();
        for (int i = 0; i < board.Pieces.Count; i++)
        {
            Piece p = board.Pieces[i];
            if (p.Color != playerToMove) continue;
            List<Move> pieceLegalMoves = p.GetLegalMoves();
            allLegalMoves.AddRange(pieceLegalMoves);
        }

        //Check if the moving player has been checkmated or a stalemate has been reached.
        if (allLegalMoves.Count == 0)
        {
            //Find the moving player's king. Checkmate if in check, otherwise stalemate.
            King movingPlayerKing = playerToMove == PlayerColors.WHITE ? whiteKing : blackKing;

            //This is just an arbitrary extremely low value for checkmates. We are avoiding int.MinValue due to overflow concerns.
            const int checkMateScore = -100_000_000;

            //This null move return should only ever happen on recursive calls, since if the engine is already checkmated or the game is
            //in stalemate, then the engine will not be called upon at all.
            return (movingPlayerKing.IsInCheck ? checkMateScore : 0, null);
        }

        //TODO: Check for other special cases (other draw conditions).

        //Start with a very low initial value, then search through all the possible moves
        //and recursively update the evaluation if we find something better.
        Move? bestMove = null;
        int bestEvaluation = int.MinValue;
        PlayerColors nextPlayer = playerToMove == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;

        foreach (Move m in allLegalMoves)
        {
            m.Execute(true);
            (int evaluation, _) = Evaluate(board, depth - 1, nextPlayer, whiteKing, blackKing);
            m.Undo();
            //We need to negate the evaluation for the recursive call, since it's always the other player that is moving.
            evaluation = -evaluation;

            //If this is the best move we've found so far, then we should save it.
            if (evaluation > bestEvaluation)
            {
                bestEvaluation = evaluation;
                bestMove = m;
            }
        }

        return bestMove is not null ? (bestEvaluation, bestMove) : throw new Exception("No legal moves found.");
    }
}
