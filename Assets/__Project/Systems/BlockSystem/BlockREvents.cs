using System.Collections.Generic;
using _NueCore.Common.ReactiveUtils;

namespace __Project.Systems.BlockSystem
{
    public abstract class BlockREvents
    {
        public class BlockSpawnedREvent : REvent
        {
            public BlockBase Block { get; private set; }
            public BlockSpawnedREvent(BlockBase block)
            {
                Block = block;
            }
        }
        public class BlockClickedREvent : REvent
        {
            public BlockBase Block { get; private set; }
            public BlockClickedREvent(BlockBase block)
            {
                Block = block;
            }
        }
        
        public class BlockPlacedREvent : REvent
        {
            public BlockColor Block { get; private set; }
            public BlockPlacedREvent(BlockColor block)
            {
                Block = block;
            }
        }
        
        public class BlocksMergeStartedREvent : REvent
        {
            public List<BlockColor> BlockList { get; private set; }
            public BlocksMergeStartedREvent(List<BlockColor> blockList)
            {
                BlockList = blockList;
            }
        }
    }
}