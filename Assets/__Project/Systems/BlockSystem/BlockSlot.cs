using DG.Tweening;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public class BlockSlot : MonoBehaviour
    {
        [SerializeField] private Transform snapRoot;
        [SerializeField] private Transform highlightRoot;
        
        public Transform SnapRoot => snapRoot;
        public BlockBase PlacedBlock { get; private set; }
        public bool IsEmpty => !PlacedBlock;

        public void Highlight(bool status)
        {
            highlightRoot.gameObject.SetActive(status);
        }
        public void Place(BlockBase block)
        {
            var oldParent = block.transform.parent;
            var oldPosition = block.transform.position;
            block.transform.SetParent(snapRoot);
            PlacedBlock = block;
            block.PlaceToSlot(this,oldParent,oldPosition);
        }

        public void PlaceTemp(BlockBase block)
        {
            block.transform.SetParent(snapRoot);
            PlacedBlock = block;
            //block.PlaceToSlot(this,oldParent,oldPosition);
        }
        
        public void ClearSlot()
        {
            PlacedBlock = null;
        }
    }
}