using System;
using System.Collections.Generic;
using __Project.Systems.GridSystem;
using __Project.Systems.GridSystem._PathSubSystem;
using __Project.Systems.LevelSystem;
using __Project.Systems.LevelSystem._MissionSubSystem;
using __Project.Systems.PowerUpSystem._Actions;
using _NueCore.Common.NueLogger;
using _NueCore.Common.ReactiveUtils;
using _NueCore.Common.Utility;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public abstract class BlockBase : MonoBehaviour
    {
        [SerializeField] private LayerMask blockLayer;
        [SerializeField] private Collider col;
        
        #region Cache
        public Vector2Int GridPosition { get; protected set; }
        public BlockSlot Slot { get; private set; }
        public Action<BlockBase> OnBlockClickedAction { get; set; }
        public Action<BlockBase> OnBlockPreClickedAction { get; set; }
        public bool DisableInteraction { get; set; }
        public GridLayer_Block GridLayerBlock { get; private set; }
        [ShowInInspector,ReadOnly]public List<BlockBase> NeighbourList { get; private set; }= new List<BlockBase>();
        private bool _isBuilt;
        #endregion

        #region Setup
        
        public virtual void Build()
        {
            if (_isBuilt)
                return;
            _isBuilt = true;
            RegisterREvents();
            GridPosition = ConvertPositionToGridPosition(transform.position);
            ActivateBlock(IsClickable());
        }
        #endregion
        
        #region Reactive
        public void RegisterREvents()
        {
            RBuss.OnEvent<BlockREvents.BlockClickedREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActivateBlock(IsClickable());
            });
            
            RBuss.OnEvent<PowerUpAction_Undo.UndoREvent>().TakeUntilDisable(gameObject).Subscribe(ev =>
            {
                ActivateBlock(IsClickable());
                DOVirtual.DelayedCall(0.03f, () =>
                {
                    ActivateBlock(IsClickable());
                }, false);
            });
            
            LevelStatic.IsInteractionEnabled.Subscribe(isEnabled =>
            {
                ActivateBlock(IsClickable());
            }).AddTo(gameObject);
        }

        #endregion
        
        #region Methods
        public virtual void FindNeighbours(GridLayer_Block gridLayerBlock)
        {
            GridLayerBlock = gridLayerBlock;
            NeighbourList.Clear();
            var directions = new[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
            foreach (var direction in directions)
            {
                if (Physics.Raycast(transform.position, direction, out var hit, 1, blockLayer))
                {
                    if (hit.collider.TryGetComponent<BlockBase>(out var @base))
                    {
                        NeighbourList.Add(@base);
                    }
                }
            }
        }

        public virtual void ActivateBlock(bool status)
        {
            
        }

        private Transform _oldParent;
        private Vector3 _oldPosition;
        public bool IsHolding { get; private set; }

        public void SetHold(bool status)
        {
            IsHolding = status;
        }
        public virtual void PlaceToSlot(BlockSlot slot,Transform oldParent,Vector3 oldPos)
        {
            _oldParent = oldParent;
            _oldPosition = oldPos;
            col.enabled = false;
            Slot = slot;
        }

        public virtual void UnPlaceFromSlot()
        {
            if (!Slot)
                return;
            transform.SetParent(_oldParent);
            Slot.ClearSlot();
            Slot = null;
            col.enabled = true;
            transform.position = _oldPosition;
            

        }
        public virtual void Magnetize()
        {
            IsMagnetized = true;
        }
        public bool IsMagnetized { get; private set; }
        public bool IsSpammed { get; private set; }
        public virtual void SetSpam()
        {
            IsSpammed = true;
        }
        public virtual void SetHold()
        {
            Slot.ClearSlot();
            Slot = null;
            col.enabled = true;
        }
        public Vector2Int ConvertPositionToGridPosition(Vector3 pos)
        {
            var offset = Vector3.zero;
            var x = offset.x + pos.x;
            var y = offset.y + pos.y;
            return new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        }

        public virtual bool IsClickable()
        {
          
            if (DisableInteraction)
            {
                return false;
            }
            if (Slot)
            {
                return false;
            }
            if (!LevelStatic.IsInteractionEnabled.Value)
                return false;
            //TODO Change to pathfinding later
            if (!HasEmptyFace())
                return false;

            // if (!HasPath())
            // {
            //     return false;
            // }
            return true;
        }

        private bool HasPath()
        {
            var level = LevelStatic.CurrentLevel;
            if (level == null)
            {
                return false;
            }
            var activeLayer = level.GridController.ActiveLayer;
            if (!gameObject.TryGetComponent<GridTileBase>(out var tile))
                return true;
            var node = activeLayer.GetNode(tile.GetCellPosition());
            if (node == null)
            {
                "Node is null".NLog(Color.red);
                return true;
            }
            if (!activeLayer.CheckPathToTopRow(node,out var path))
            {
                return false;
            }
            return true;
        }

        public bool HasEmptyFace()
        {
            //TODO This will be changed when pathfinding is implemented
            Physics.Raycast(transform.position, Vector3.up, out var hit, 1, blockLayer);
            if (hit.collider == null)
                return true;
            
            Physics.Raycast(transform.position, Vector3.left, out hit, 1, blockLayer);
            if (hit.collider == null)
                return true;
            
            Physics.Raycast(transform.position, Vector3.right, out hit, 1, blockLayer);
            if (hit.collider == null)
                return true;
            
            Physics.Raycast(transform.position, Vector3.down, out hit, 1, blockLayer);
            if (hit.collider == null)
                return true;
            
            return false;
        }
        #endregion

        #region Events

        private void OnMouseDown()
        {
            ClickBlock();
        }

        public void ClickBlock()
        {
            if (UIHelper.IsMouseOverUI())
            {
                return;
            }
            if (!IsClickable())
            {
                "Block is not placeable".NLog(Color.red);
                Punch();
                return;
            }
            OnBlockPreClickedAction?.Invoke(this);
            OnBlockClickedAction?.Invoke(this);
            RBuss.Publish(new BlockREvents.BlockClickedREvent(this));
        }

        private Tween _punchTween;
        private void Punch()
        {
            _punchTween?.Kill(true);
            _punchTween =transform.DOPunchScale(Vector3.one * 0.1f, 0.2f,1,0.1f).OnComplete(() =>
            {
                transform.localScale = Vector3.one;
            });
        }

        #endregion

    }
}