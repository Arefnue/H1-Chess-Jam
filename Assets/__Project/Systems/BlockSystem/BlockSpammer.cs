using System;
using System.Collections.Generic;
using __Project.Systems.LevelSystem;
using __Project.Systems.PowerUpSystem._Actions;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public class BlockSpammer : BlockBase
    {
        [SerializeField] private BlockColorCatalog blockColorCatalog;
        [SerializeField] private List<BlockColorEnum> spamList = new List<BlockColorEnum>();
        [SerializeField] private Transform spamRoot;
        [SerializeField] private Transform spawnRoot;
        [SerializeField] private TMP_Text countText;
        
        public BlockColor LastSpawnedBlock { get; private set; }
        public BlockColor PreviousSpawnedBlock { get; private set; }
        public int SpamCount { get; private set; }

        public override void Build()
        {
            base.Build();
            SetCountText();
            SpawnBlock();
            countText.transform.LookAt(CameraStatic.MainCamera.transform);
            countText.transform.rotation = Quaternion.identity;
            // var pos =countText.transform.position;
            // pos.x = 0;
            // pos.y = 0;
            // countText.transform.position = pos;
            RBuss.OnEvent<PowerUpAction_Undo.UndoREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (PreviousSpawnedBlock == null)
                {
                    return;
                }
                if (ev.BlockColor == PreviousSpawnedBlock)
                {
                    if (LastSpawnedBlock)
                    {
                        Destroy(LastSpawnedBlock.gameObject);
                    }
                    Destroy(PreviousSpawnedBlock.gameObject);
                    SpamCount--;
                    SetCountText();
                    LastSpawnedBlock = null;
                    SpawnBlock();
                }
            });
        }

        private void SetCountText()
        {
            var remainingCount = (spamList.Count - SpamCount - 1);
            if (remainingCount<=0)
            {
                remainingCount = 0;
            }
            countText.SetText(remainingCount.ToString());

        }

        public void SpawnBlock()
        {
            if (SpamCount>=spamList.Count)
            {
                SetCountText();
                return;
            }
            if (LastSpawnedBlock)
                return;
            var color = spamList[SpamCount];
            var prefab = blockColorCatalog.GetBlockColorPrefab(color);
            var block = Instantiate(prefab, LevelStatic.CurrentLevel.GridController.ActiveLayer.Grid.transform);
            block.transform.position = spamRoot.position;
            block.transform.rotation = Quaternion.identity;
            block.DisableInteraction = true;
            block.Build();
            block.SetSpam();
            block.OnBlockClickedAction += @base =>
            {
                if (block.DisableInteraction)
                {
                    return;
                }

                if (!block.IsClickable())
                {
                    return;
                }
                PreviousSpawnedBlock = LastSpawnedBlock;
                LastSpawnedBlock = null;
                SpamCount++;
                SpawnBlock();
            };
            LastSpawnedBlock = block;
            RBuss.Publish(new BlockREvents.BlockSpawnedREvent(block));
            var seq = DOTween.Sequence();
            block.ModelRoot.transform.position = spawnRoot.position;
            block.ModelRoot.transform.localScale = Vector3.one * 0.5f;
            SetCountText();
            seq.Append(block.ModelRoot.DOLocalMove(Vector3.zero, 0.5f));
            seq.Join(block.ModelRoot.DOScale(Vector3.one, 0.5f));
            seq.OnComplete(() =>
            {
                block.DisableInteraction = false;
                ActivateBlock(block.IsClickable());
                if (SpamCount>0)
                {
                    block.ActivateBlock(true);
                }
                SetCountText();
            });
        }

        

        public override bool IsClickable()
        {
            return false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            countText.SetText((spamList.Count).ToString());

        }
#endif
    }
}