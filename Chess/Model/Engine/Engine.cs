namespace Chess.Model.Engine;

public class Engine
{
    private IEvaluationStrategy _evaluationStrategy;

    public Engine()
    {
        _evaluationStrategy = new ExampleEvaluationStrategy();
    }

    public Move FindBestMove(Board board, int depth)
    {
        throw new NotImplementedException();
    }
}
