using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public class BlockWall : BlockBase
    {
        public override void Build()
        {
            GridPosition = ConvertPositionToGridPosition(transform.position);
        }

        public override bool IsClickable()
        {
            return false;
        }

        public override void PlaceToSlot(BlockSlot slot, Transform oldParent, Vector3 oldPos)
        {
            base.PlaceToSlot(slot, oldParent, oldPos);
        }

        public override void ActivateBlock(bool status)
        {
            // base.ActivateBlock(status);
        }
    }
}