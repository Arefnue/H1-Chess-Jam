using System.Collections.Generic;
using System.Linq;
using __Project.Systems.BlockSystem;
using __Project.Systems.LevelSystem;
using _NueCore.Common.ReactiveUtils;
using UniRx;
using UnityEngine;

namespace __Project.Systems.PowerUpSystem._Actions
{
    public class PowerUpAction_MoreSpace : PowerUpActionBase
    {
        [SerializeField] private int count = 3;
        
        public class MoreSpaceREvent : REvent
        {
            public List<BlockColor> SelectedBlockList { get; private set; }
            public MoreSpaceREvent(List<BlockColor> blockList)
            {
                SelectedBlockList = blockList;
            }
        }

        public override void Build(PowerUpButton button)
        {
            base.Build(button);
            RBuss.OnEvent<BlockREvents.BlockPlacedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                Button.UpdateButton();
            });
            RBuss.OnEvent<BlockREvents.BlocksMergeStartedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                Button.UpdateButton();
            });
            
            RBuss.OnEvent<PowerUpAction_Undo.UndoREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
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

            // if (LevelStatic.CurrentLevel.BlockController.BlockList.Count<count)
            // {
            //     return false;
            // }
            //
            // var emptySlotCount = LevelStatic.CurrentLevel.BlockController.HoldRootList.FindAll(x => x.IsEmpty);
            //
            // if (emptySlotCount.Count < count)
            // {
            //     return false;
            // }
            //
            if (!LevelStatic.IsInteractionEnabled.Value)
            {
                return false;
            }
            
            return true;
        }

        public override void Apply()
        {
            // var blockColors = LevelStatic.CurrentLevel.BlockController.BlockList.TakeLast(3).ToList();
            // RBuss.Publish(new MoreSpaceREvent(blockColors));
            // Button.UpdateButton();
        }
    }
}