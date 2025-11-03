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

    public async Task<Move?> FindBestMove(Board board, int depth)
    {
        //This method is async so that the UI doesn't freeze during evaluation.
        (_, Move? bestMove) = await Task.Run(() => Evaluate(board, depth, PlayingAs));
        return bestMove ?? throw new NullReferenceException("Best move not found.");
    }

    private (int, Move?) Evaluate(Board board, int depth, PlayerColors playerToMove)
    {
        //Recursive depth cutoff.
        if (depth == 0)
        {
            int evaluation = _evaluationStrategy.Evaluate(board);
            return (playerToMove == PlayerColors.WHITE ? evaluation : -evaluation, null);
        }

        //Get all the legal moves for the player to move.
        List<Move> allLegalMoves = new List<Move>();
        foreach (Piece p in board.Pieces.Where(p => p.Color == playerToMove))
        {
            List<Move> pieceLegalMoves = p.GetLegalMoves(board);
            allLegalMoves.AddRange(pieceLegalMoves);
        }

        //Check if the moving player has been checkmated or a stalemate has been reached.
        if (allLegalMoves.Count == 0)
        {
            //Find the moving player's king. Checkmate if in check, otherwise stalemate.
            King? movingPlayerKing = board.Pieces.Find(p => p is King && p.Color == playerToMove) as King;
            if (movingPlayerKing is null) throw new NullReferenceException("King not found - something has gone seriously wrong");

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
        foreach (Move m in allLegalMoves)
        {
            //The board needs to be cloned at this step rather than any other, because the cloned
            //state needs to carry through the the recursive calls, but the original board needs to
            //be unmodified.
            Board clonedBoard = board.Clone();
            m.CloneAndExecute(clonedBoard);

            PlayerColors nextPlayer = playerToMove == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;
            (int evaluation, _) = Evaluate(clonedBoard, depth - 1, nextPlayer);

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
