using System.Collections.Generic;
using System.Linq;
using __Project.Systems.BlockSystem;
using __Project.Systems.LevelSystem;
using __Project.Systems.LevelSystem._MissionSubSystem;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using UniRx;
using UnityEngine;

namespace __Project.Systems.PowerUpSystem._Actions
{
    public class PowerUpAction_Magnet : PowerUpActionBase
    {
        public class MagnetREvent : REvent
        {
            public List<BlockColor> BlockList { get; private set; }
            public MagnetREvent(List<BlockColor> blockList)
            {
                BlockList = blockList;
            }
        }

        public override void Build(PowerUpButton button)
        {
            base.Build(button);
            RBuss.OnEvent<BlockREvents.BlockClickedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
               Button.UpdateButton();
            });
            
            RBuss.OnEvent<PowerUpAction_Undo.UndoREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                Button.UpdateButton();
            });
            
            RBuss.OnEvent<BlockREvents.BlockSpawnedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                Button.UpdateButton();
            });
            
            RBuss.OnEvent<BlockREvents.BlocksMergeStartedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                Button.UpdateButton();
            });
            
            RBuss.OnEvent<MissionREvents.ActiveStepSelectedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                Button.UpdateButton();
            });
        }

        public override void Apply()
        {
            if (LevelStatic.CurrentLevel.GridController.ActiveLayer.TryGetComponent<GridLayer_Block>(out var blockLayer))
            {
                var step = LevelStatic.CurrentLevel.MissionController.ActiveMissionStep;
                if (step.TryGetCurrentMission(out var missionInfo))
                {
                    var targetColor = missionInfo.targetColor;
                    var blocks = blockLayer.ActiveBlockListRP.ToList();
                    var items =blocks.FindAll(x =>
                    {

                        if (x.IsSpammed)
                        {
                            return false;
                        }
                        return x.ColorEnum == targetColor;
                    }).Take(3).ToList();
                    RBuss.Publish(new MagnetREvent(items));
                    Button.UpdateButton();
                }
            }
        }
        
        public override bool CanUse()
        {
            var baseValue = base.CanUse();

            if (!baseValue)
            {
                return false;
            }
            if (LevelStatic.CurrentLevel.GridController.ActiveLayer.TryGetComponent<GridLayer_Block>(out var blockLayer))
            {
                var step = LevelStatic.CurrentLevel.MissionController.ActiveMissionStep;
                if (step.TryGetCurrentMission(out var missionInfo))
                {
                    "Magnet Check".NLog();
                    var targetColor = missionInfo.targetColor;
                    var blocks = blockLayer.ActiveBlockListRP;
                    var ct = 0;
                    foreach (var block in blocks)
                    {
                        if (block.ColorEnum != targetColor) continue;
                        if (block.IsSpammed)
                            continue;
                        ct++;
                        if (ct >= 3)
                            return true;
                    }
                }
            }
            return false;
        }

    }
}