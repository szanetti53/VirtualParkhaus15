
namespace Flow
{
    public interface IFlowState
    {
        void StartState(bool force = false);
        void EndState(bool force = false);
    }
}

