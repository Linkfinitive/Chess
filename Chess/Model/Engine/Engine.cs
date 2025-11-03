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
        if (depth < 1) throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be at least 1.");

        //Get all the legal moves for the player to move.
        List<Move> allLegalMoves = new List<Move>();
        foreach (Piece p in board.Pieces.Where(p => p.Color == PlayingAs))
        {
            List<Move> pieceLegalMoves = p.GetLegalMoves(board);
            allLegalMoves.AddRange(pieceLegalMoves);
        }

        //The first level of the search can be done in parallel since there will only be a non-exponential number
        //of possible moves. The cost of spawning tasks is worth it for the parallelism in this case, but not worth
        //it to do on every level of the recursive calls.
        List<Task<int>> tasks = new List<Task<int>>();
        Move? bestMove = null;
        int bestEvaluation = int.MinValue;
        foreach (Move m in allLegalMoves)
        {
            tasks.Add(Task.Run(async () =>
            {
                //The board needs to be cloned at this step rather than any other, because the cloned
                //state needs to carry through the the recursive calls, but the original board needs to
                //be unmodified.
                Board clonedBoard = board.Clone();
                m.CloneAndExecute(clonedBoard);

                PlayerColors nextPlayer = PlayingAs == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;
                int evaluation = await Evaluate(clonedBoard, depth - 1, nextPlayer);

                //We need to negate the evaluation for the recursive call, since it's always the other player that is moving.
                evaluation = -evaluation;
                return evaluation;
            }));

            int[] evaluations = await Task.WhenAll(tasks);

            foreach (int evaluation in evaluations)
            {
                if (evaluation > bestEvaluation)
                {
                    bestMove = m;
                    bestEvaluation = evaluation;

                }
            }
        }

        return bestMove ?? throw new NullReferenceException("Best move not found.");
    }

    private async Task<int> Evaluate(Board board, int depth, PlayerColors playerToMove)
    {
        //Recursive depth cutoff.
        if (depth == 0)
        {
            int evaluation = _evaluationStrategy.Evaluate(board);
            return playerToMove == PlayerColors.WHITE ? evaluation : -evaluation;
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
            return movingPlayerKing.IsInCheck ? checkMateScore : 0;
        }

        //TODO: Check for other special cases (other draw conditions).

        //Start with a very low initial value, then search through all the possible moves
        //and recursively update the evaluation if we find something better.
        int bestEvaluation = int.MinValue;
        foreach (Move m in allLegalMoves)
        {
            //The board needs to be cloned at this step rather than any other, because the cloned
            //state needs to carry through the the recursive calls, but the original board needs to
            //be unmodified.
            Board clonedBoard = board.Clone();
            m.CloneAndExecute(clonedBoard);

            PlayerColors nextPlayer = playerToMove == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;
            int evaluation = await Evaluate(clonedBoard, depth - 1, nextPlayer);

            //We need to negate the evaluation for the recursive call, since it's always the other player that is moving.
            evaluation = -evaluation;

            //If this is the best move we've found so far, then we should save it.
            if (evaluation > bestEvaluation)
            {
                bestEvaluation = evaluation;
            }
        }

        return bestEvaluation;
    }
}
