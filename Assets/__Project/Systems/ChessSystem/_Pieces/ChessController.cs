using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessController : MonoBehaviour
    {
        public int ActiveStepCount { get; private set; }
        public void Build()
        {

        }

        public bool IsAllStepsCompleted()
        {
            if (ActiveStepCount >= 3)
                return true;
            return false;
        }
    }
}