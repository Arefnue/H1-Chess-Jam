using __Project.Systems.ChessSystem._Grid;
using UnityEngine;

namespace __Project.Systems.ChessSystem._Features
{
    public abstract class ChessFeatureBase : MonoBehaviour
    {
        public GridLayer_Chess GridLayerChess { get; private set; }
        public virtual void Build(GridLayer_Chess gridLayerChess)
        {
            GridLayerChess = gridLayerChess;
        }

        public virtual void UpdateFeature()
        {
            
        }

        public virtual void RemoveFeature()
        {
            GridLayerChess.FeatureList.Remove(this);
        }
        
        
    }
}