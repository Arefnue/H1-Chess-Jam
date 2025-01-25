using System.Collections.Generic;
using __Project.Systems.BlockSystem;
using __Project.Systems.GridSystem;
using __Project.Systems.PowerUpSystem._Actions;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace __Project.Systems.LevelSystem._MissionSubSystem
{
    public class GridLayer_Block : GridLayer
    {
        [SerializeField] private Transform root;
        [SerializeField] private List<BlockBase> blockList = new List<BlockBase>();
        
        #region Cache
        public Transform Root => root;
        public List<BlockBase> BlockList => blockList;

        [ShowInInspector,ReadOnly]public ReactiveCollection<BlockColor> ActiveBlockListRP { get; private set; } = new ReactiveCollection<BlockColor>();
        #endregion

        #region Methods

        public override void Shuffle()
        {
            base.Shuffle();
            // var cellPositions = new List<Vector3>();
            // foreach (var block in BlockList)
            // {
            //     block.transform.SetParent(null);
            //     block.transform.SetParent(Root);
            //     block.transform.localPosition = Vector3.zero;
            // }
        }

        public override void Build()
        {
            base.Build();
            foreach (var blockBase in BlockList)
            {
                if (blockBase.TryGetComponent<BlockColor>(out var colorBlock))
                    ActiveBlockListRP.Add(colorBlock);
            }
            RegisterREvents();
        }

        private void RegisterREvents()
        {
            RBuss.OnEvent<BlockREvents.BlockClickedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (LevelStatic.CurrentLevel.GridController.ActiveLayer != this)
                    return;
                if (ev.Block.TryGetComponent<GridTileBase>(out var tile))
                {
                    if (ev.Block.TryGetComponent<BlockColor>(out var colorBlock))
                        ActiveBlockListRP.Remove(colorBlock);
                    RemoveTile(tile);
                }
              
            });
            
            RBuss.OnEvent<PowerUpAction_Undo.UndoREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (LevelStatic.CurrentLevel.GridController.ActiveLayer != this)
                    return;
                if (ev.BlockColor.TryGetComponent<GridTileBase>(out var tile))
                {
                    if (ev.BlockColor.TryGetComponent<BlockColor>(out var colorBlock))
                        ActiveBlockListRP.Add(colorBlock);
                    AddTile(tile);
                }
            });
            
            RBuss.OnEvent<BlockREvents.BlockSpawnedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                if (LevelStatic.CurrentLevel.GridController.ActiveLayer != this)
                    return;
                if (ev.Block.TryGetComponent<GridTileBase>(out var tile))
                {
                    if (ev.Block.TryGetComponent<BlockColor>(out var colorBlock))
                        ActiveBlockListRP.Add(colorBlock);
                    AddTile(tile);
                }
            });
        }

        public override void ActivateLayer()
        {
            base.ActivateLayer();
            foreach (var block in BlockList)
                block.Build();

            foreach (var blockBase in BlockList)
                blockBase.FindNeighbours(this);
        }

        public override void UpdateLayer()
        {
            base.UpdateLayer();
            foreach (var block in BlockList)
                block.ActivateBlock(block.IsClickable());
        }
        #endregion

#if UNITY_EDITOR

        public override void FindTiles()
        {
            base.FindTiles();
            BlockList.Clear();
            var c = GetComponentsInChildren<BlockBase>();
            foreach (var tileBase in c)
                BlockList.Add(tileBase);
        }
#endif
    }
}