using Chess.Global;
using Chess.Model.Pieces;

namespace Chess.Model.Engine;

public class Engine
{
    //Infinity set to arbitrary large constant to avoid integer overflow caused by the use of int.MinValue.
    //Checkmate score set to arbitrary large value less than Infinity.
    private const int Infinity = 1_000_000_000;
    private const int CheckmateScore = 100_000_000;

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
        //This was more useful before I added the King properties to the board, but I'm
        //leaving it here because I think it is more legible than the alternative.
        King whiteKing = clonedBoard.WhiteKing;
        King blackKing = clonedBoard.BlackKing;

        (_, Move? bestMove) = await Task.Run(() => Negamax(clonedBoard, depth, PlayingAs, whiteKing, blackKing, -Infinity, Infinity));
        return bestMove?.Clone(board) ?? throw new NullReferenceException("Best move not found.");
    }

    private (int, Move?) Negamax(Board board, int depth, PlayerColors playerToMove, King whiteKing, King blackKing, int alpha, int beta)
    {
        //Recursive depth cutoff.
        if (depth == 0)
        {
            int evaluation = _evaluationStrategy.Evaluate(board);
            //It needs to be evaluated from white's perspective because that's how the evaluation strategy works.
            //This is very counterintuitive and has caused me many headaches, since we're using Negamax not strict Minimax.
            //At least, I think that's right. Like 80% sure. The other option is return (evaluation, null).
            return (playerToMove == PlayerColors.WHITE ? evaluation : -evaluation, null);
        }

        //Get all the legal moves for the player to move.
        //Must use ToArray() because we're modifying the board by calling GetLegalMoves() on each piece.
        List<Move> allLegalMoves = new List<Move>();
        foreach (Piece p in board.Pieces.ToArray().Where(p => p.Color == playerToMove))
        {
            List<Move> pieceLegalMoves = p.GetLegalMoves();
            allLegalMoves.AddRange(pieceLegalMoves);
        }

        //We need this to check for checks later.
        King movingPlayerKing = playerToMove == PlayerColors.WHITE ? whiteKing : blackKing;
        //Check if the moving player has been checkmated or a stalemate has been reached.
        if (allLegalMoves.Count == 0)
        {
            //Checkmate if  the moving player is in check, otherwise stalemate.
            //This null move return should only ever happen on recursive calls, since if the engine is already checkmated or the game is
            //in stalemate, then the engine will not be called upon at all.
            return (movingPlayerKing.IsInCheck ? -CheckmateScore : 0, null);
        }

        //This is where I would check for draw conditions, if I had time.

        //Start with a very low initial value, then search through all the possible moves
        //and recursively update the evaluation if we find something better.
        Move? bestMove = null;
        int bestEvaluation = -Infinity;
        PlayerColors nextPlayer = playerToMove == PlayerColors.WHITE ? PlayerColors.BLACK : PlayerColors.WHITE;

        foreach (Move m in allLegalMoves)
        {
            m.Execute(true);
            (int evaluation, _) = Negamax(board, depth - 1, nextPlayer, whiteKing, blackKing, -beta, -alpha);
            m.Undo();

            //We need to negate the evaluation for the recursive call, since it's always the other player that is moving.
            evaluation = -evaluation;

            //If this is the best move we've found so far, then we should save it.
            if (evaluation > bestEvaluation)
            {
                bestEvaluation = evaluation;
                bestMove = m;
            }

            if (bestEvaluation > alpha)
            {
                alpha = bestEvaluation;
            }

            if (alpha >= beta)
            {
                //Prune the branch.
                break;
            }
        }

        return bestMove is not null ? (bestEvaluation, bestMove) : throw new Exception("No legal moves found.");
    }
}
