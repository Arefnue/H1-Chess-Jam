using __Project.Systems.BlockSystem;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using UniRx;
using UnityEngine;

namespace __Project.Systems.PowerUpSystem._Actions
{
    public class PowerUpAction_Undo : PowerUpActionBase
    {
        public BlockColor BlockColor { get; private set; }
        public class UndoREvent : REvent
        {
            public BlockColor BlockColor { get; private set; }
            public UndoREvent(BlockColor block)
            {
                BlockColor = block;
            }
        }
        public override void Build(PowerUpButton button)
        {
            base.Build(button);
            RBuss.OnEvent<BlockREvents.BlockPlacedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                BlockColor = ev.Block;
                Button.UpdateButton();
            });
            
            RBuss.OnEvent<PowerUpAction_MoreSpace.MoreSpaceREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                BlockColor = null;
                Button.UpdateButton();
            });
            RBuss.OnEvent<BlockREvents.BlocksMergeStartedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                BlockColor = null;
                Button.UpdateButton();
            });
        }

        public override bool CanUse()
        {
            var baseValue = base.CanUse();

            if (!baseValue)
            {
                return false;
            }

            if (!BlockColor)
            {
                return false;
            }

            if (BlockColor.IsHolding)
            {
                return false;
            }
            
            return true;
        }

        public override void Reset()
        {
            BlockColor = null;
            base.Reset();
            
          
        }

        public override void Apply()
        {
            if (!BlockColor)
            {
                return;
            }
            
            BlockColor.UnPlaceFromSlot();
            RBuss.Publish(new UndoREvent(BlockColor));
            BlockColor = null;
        }
    }
}