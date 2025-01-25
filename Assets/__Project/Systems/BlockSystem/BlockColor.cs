using System;
using System.Collections.Generic;
using __Project.Systems.LevelSystem._MissionSubSystem;
using _NueCore.Common.NueLogger;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public partial class BlockColor : BlockBase
    {
        [SerializeField] private BlockColorEnum colorEnum;
        [SerializeField] private List<Transform> closeOnActivateList = new List<Transform>();
        [SerializeField] private Transform modelRoot;
        [SerializeField] private Transform questionRoot;
        [SerializeField] private Transform activeRoot;
        [SerializeField] private bool isQuestion;

        public BlockColorEnum ColorEnum => colorEnum;

        public Transform ModelRoot => modelRoot;
        
        public bool IsQuestionRemoved { get; private set; }

        public bool IsQuestion => isQuestion;

        private bool _isInInventory;
   
        public override void Build()
        {
            questionRoot.gameObject.SetActive(false);
            activeRoot.gameObject.SetActive(true);
            if (IsQuestion)
            {
                CheckQuestion(false);
            }
            base.Build();
        }

        public override void PlaceToSlot(BlockSlot slot, Transform oldParent, Vector3 oldPos)
        {
            base.PlaceToSlot(slot, oldParent, oldPos);
            if (Slot)
            {
                _isInInventory = true;
            }
        }

        private void CheckQuestion(bool status)
        {
            if (!IsQuestion) return;
            questionRoot.gameObject.SetActive(!status);
            activeRoot.gameObject.SetActive(status);
            IsQuestionRemoved = status;
        }

        public override void ActivateBlock(bool status)
        {
            base.ActivateBlock(status);
         
            if (_isInInventory)
            {
                SetActiveObjectList(false);
            }
            else
            {
                SetActiveObjectList(!status);
            }
        }

        public override void FindNeighbours(GridLayer_Block gridLayerBlock)
        {
            base.FindNeighbours(gridLayerBlock);
            foreach (var blockBase in NeighbourList)
            {
                if (IsQuestion)
                {
                    blockBase.OnBlockClickedAction += @base =>
                    {
                        CheckQuestion(true);
                    };
                }
            }
        }

        public override void UnPlaceFromSlot()
        {
            base.UnPlaceFromSlot();
            _isInInventory = false;
        }

        public override void SetHold()
        {
            base.SetHold();
            _isInInventory = false;
            if (IsQuestion) IsQuestionRemoved = true;

        }
        

        public override bool IsClickable()
        {
            if (_isInInventory)
            {
                return false;
            }

            if (IsQuestion && !IsQuestionRemoved)
            {
                return false;
            }
            var baseValue= base.IsClickable();
            return baseValue;
        }

        public void SetActiveObjectList(bool status)
        {
            foreach (var obj in closeOnActivateList)
            {
                obj.gameObject.SetActive(status);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }
            
            questionRoot.gameObject.SetActive(IsQuestion);
            activeRoot.gameObject.SetActive(true);
            // if (!colorCatalog)
            // {
            //     return;
            // }
            // foreach (var rend in colorRendererList)
            // {
            //     var mat = colorCatalog.GetBlockColorMaterial(colorEnum);
            //     if (mat)
            //         rend.sharedMaterial = mat;
            // }
        }
#endif
      
    }
}