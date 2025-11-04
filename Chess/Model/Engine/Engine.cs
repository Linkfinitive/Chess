using Chess.Global;

namespace Chess.Model.Engine;

public class Engine
{
    private IEvaluationStrategy _evaluationStrategy;

    public Engine()
    {
        _evaluationStrategy = new CountMaterialStrategy();
    }

    public Move FindBestMove(Board board, int depth)
    {
        throw new NotImplementedException();
    }
}
