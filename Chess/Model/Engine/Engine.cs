namespace Model.Engine;

public class Engine
{
    private IEvaluationStrategy _evaluationStrategy;

    public Engine()
    {
        _evaluationStrategy = new ExampleEvaluationStrategy();
    }

    public Move findBestMove(Board board, int depth)
    {
        //the below is, naturally, a placeholder to stop the lsp from yelling

        return new Move(new Square(0, 0, PlayerColors.WHITE), new Square(0, 0, PlayerColors.WHITE), new King(PlayerColors.WHITE, new Square(0, 0, PlayerColors.WHITE)));
    }
}
