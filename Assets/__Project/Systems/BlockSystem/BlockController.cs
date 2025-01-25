using System.Collections.Generic;
using System.Linq;
using __Project.Systems.LevelSystem;
using __Project.Systems.LevelSystem._MissionSubSystem;
using __Project.Systems.PowerUpSystem._Actions;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField] private List<BlockSlot> slotList = new List<BlockSlot>();
        [SerializeField] private TMP_Text debugActiveText;
        [SerializeField] private List<BlockSlot> holdRootList = new List<BlockSlot>();
        
     
        #region Cache
        public List<BlockColor> BlockList { get; private set; } = new List<BlockColor>();
        [ShowInInspector,ReadOnly]public MissionInfo ActiveMission { get; private set; }

        public List<BlockSlot> HoldRootList => holdRootList;

        #endregion

        #region Setup
        public void Build()
        {
            RegisterREvents();
            foreach (var slot in slotList)
            {
                slot.Highlight(false);
            }
            //DetermineMission();
            InvokeRepeating(nameof(CheckFail),3,2);
            
        }
        

        #endregion

        #region Reactive
        private void RegisterREvents()
        {
            RBuss.OnEvent<PowerUpAction_Magnet.MagnetREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (ev.BlockList.Count<3)
                {
                    return;
                }
                foreach (var color in ev.BlockList)
                {
                    color.Magnetize();
                }
                MergeExternal(ev.BlockList);
            });
            RBuss.OnEvent<MissionREvents.ActiveMissionSelectedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActiveMission = ev.MissionInfo;
                DetermineMission();
                var newItem =BlockList.Find(x => x.ColorEnum == ActiveMission.targetColor);
                if (newItem)
                    CheckMerge(newItem);
            });

            RBuss.OnEvent<PowerUpAction_Undo.UndoREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                BlockList.Remove(ev.BlockColor);
            });
            
            RBuss.OnEvent<PowerUpAction_MoreSpace.MoreSpaceREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                foreach (var block in ev.SelectedBlockList)
                {
                    BlockList.Remove(block);
                    block.SetHold();
                }

                var seq = DOTween.Sequence();
                for (var i = 0; i < ev.SelectedBlockList.Count; i++)
                {
                    var block = ev.SelectedBlockList[i];
                    var holdSlot = HoldRootList.FirstOrDefault(x=> x.IsEmpty);
                    if (holdSlot == null)
                        continue;
                    holdSlot.PlaceTemp(block);
                    block.SetHold(true);
                    block.OnBlockPreClickedAction += UnHold;
                    seq.Join(block.transform.DOLocalJump(Vector3.zero, 1, 1, 0.5f).SetEase(Ease.OutSine));
                }
            });
            
            RBuss.OnEvent<BlockREvents.BlockClickedREvent>()
                .TakeUntilDisable(gameObject)
                .Subscribe(ev =>
            {
                ReplaceAll();
                var block = ev.Block;
                var slot = slotList.Find(s => s.IsEmpty);
                if (!slot) return;
                slot.Place(block);
                block.transform.DOLocalJump(Vector3.zero, 2f, 1, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    var tBlock = block as BlockColor;
                    BlockList.Add(tBlock);
                    RBuss.Publish(new BlockREvents.BlockPlacedREvent(tBlock));
                    CheckMerge(tBlock);
                    CheckHighlight();
                    ReplaceAll();
                });
            });
           
        }

        private void ReplaceAll()
        {
            return;
            foreach (var slot in slotList)
                slot.ClearSlot();
            var tList =BlockList.OrderBy(x => x.ColorEnum).ToList();
            for (int i = 0; i < tList.Count; i++)
            {
                var item = tList[i];
                var slot = slotList[i];
                slot.Place(item);
            }
            _replaceSeq?.Kill();
            _replaceSeq = DOTween.Sequence();
            foreach (var slot in slotList)
            {
                if (slot.IsEmpty)
                {
                    continue;
                }

                if (slot.PlacedBlock.transform.localPosition == Vector3.zero)
                {
                    continue;
                }
                _replaceSeq.Join(slot.PlacedBlock.transform.DOLocalJump(Vector3.zero, 0.5f, 1, 0.25f).SetEase(Ease.OutSine));
            }
        }

        private Sequence _replaceSeq;
        
        #endregion

        #region Methods

        private void UnHold(BlockBase @base)
        {
            var slot = HoldRootList.Find(x => x.PlacedBlock == @base);
            if (slot)
                slot.ClearSlot();
            @base.SetHold(false);
            @base.OnBlockPreClickedAction -= UnHold;
        }
        private void CheckHighlight()
        {
            var emptySlots =slotList.FindAll(x => x.IsEmpty);
            if (emptySlots.Count==1)
            {
                emptySlots[0].Highlight(true);
            }
            else
            {
                foreach (var slot in slotList)
                    slot.Highlight(false);
            }
        }
        private void CheckPlacement(BlockColor blockColor)
        {
            if (BlockList.Any(b => b.ColorEnum == blockColor.ColorEnum))
            {
                var sameColorBlock = BlockList.First(b => b.ColorEnum == blockColor.ColorEnum);
                var slot = sameColorBlock.Slot;
                slot.ClearSlot();
                slot.Place(blockColor);
                blockColor.transform.DOLocalJump(Vector3.zero, 2f, 1, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    BlockList.Remove(sameColorBlock);
                    BlockList.Add(blockColor);
                    CheckMerge(blockColor);
                });
            }
        }
        private void DetermineMission()
        {
            var mission = ActiveMission;
            var color = BlockStatic.GetColor(mission.targetColor);
            mission.SetTargetSpriteAsActive(color);
            debugActiveText.color = color;
            debugActiveText.text = mission.targetColor.ToString();
        }
        private void CheckMerge(BlockColor blockColor)
        {
            if (ActiveMission.targetColor != blockColor.ColorEnum)
                return;
            var sameColorBlocks = BlockList.FindAll(x => x.ColorEnum == blockColor.ColorEnum);

            if (sameColorBlocks.Count>=3)
            {
                _failCheckDelay?.Kill();
                Merge(sameColorBlocks);
            }
            else
            {
                CheckFail();
            }
        }

        private Tween _failCheckDelay;
        private bool _isFailed;
        private void CheckFail()
        {
            if (slotList.Any(s=> s.IsEmpty))
                return;
            _failCheckDelay?.Kill();
            _failCheckDelay =DOVirtual.DelayedCall(1.75f, () =>
            {
                "Fail Checked".NLog(Color.red);
                if (slotList.All(s => !s.IsEmpty))
                {
                    if (_isFailed)
                    {
                        return;
                    }

                    _isFailed = true;
                    LevelStatic.IsInteractionEnabled.Value = false;
                    LevelStatic.LoseLevel();
                }
            },false);
        }
        private void Merge(List<BlockColor> sameColorBlocks)
        {
            var first3 = sameColorBlocks.Take(3).ToList();
            var middle = first3[1];
            var seq = DOTween.Sequence();
            foreach (var block in first3)
            {
                BlockList.Remove(block);
                block.Slot.ClearSlot();
                seq.Join(block.transform.DOMove(middle.transform.position + new Vector3(0,0.5f,0), 0.25f));
                seq.Join(block.transform.DOScale(Vector3.one * 1.1f, 0.25f).OnComplete(() =>
                {
                    if (block == middle)
                        return;
                    block.gameObject.SetActive(false);
                }));
            }
            CheckHighlight();
            RBuss.Publish(new BlockREvents.BlocksMergeStartedREvent(first3));
            var targetPos = ActiveMission.targetSprite.transform.position + new Vector3(0, 0, -0.5f);
            seq.Append(middle.transform.DOJump(targetPos, 1f, 1, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
            {
                ActiveMission.SetTargetSpriteAsCompleted();
            }));
                
            seq.OnComplete(() =>
            {
                foreach (var block in first3)
                {
                    Destroy(block.gameObject);
                }
                RBuss.Publish(new MissionREvents.MissionCompletedREvent(ActiveMission));
            });
        }

        private void MergeExternal(List<BlockColor> sameColorBlocks)
        {
            var first3 = sameColorBlocks.Take(3).ToList();
            var middle = first3[1];
            var seq = DOTween.Sequence();
            foreach (var block in first3)
            {
                seq.Join(block.transform.DOMove(middle.transform.position + new Vector3(0,0.5f,0), 0.25f));
                seq.Join(block.transform.DOScale(Vector3.one * 1.1f, 0.25f).OnComplete(() =>
                {
                    if (block == middle)
                        return;
                    block.gameObject.SetActive(false);
                }));
            }
            CheckHighlight();
            RBuss.Publish(new BlockREvents.BlocksMergeStartedREvent(first3));
            var targetPos = ActiveMission.targetSprite.transform.position + new Vector3(0, 0, -0.5f);
            seq.Append(middle.transform.DOJump(targetPos, 1f, 1, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
            {
                ActiveMission.SetTargetSpriteAsCompleted();
            }));
                
            seq.OnComplete(() =>
            {
                foreach (var block in first3)
                {
                    Destroy(block.gameObject);
                }
                RBuss.Publish(new MissionREvents.MissionCompletedREvent(ActiveMission));
            });
        }

        #endregion
    }
}