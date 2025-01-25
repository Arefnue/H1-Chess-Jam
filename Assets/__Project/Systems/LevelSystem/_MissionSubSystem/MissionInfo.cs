using System;
using __Project.Systems.BlockSystem;
using UnityEngine;

namespace __Project.Systems.LevelSystem._MissionSubSystem
{
    [Serializable]
    public class MissionInfo
    {
        public SpriteRenderer targetSprite;
        public BlockColorEnum targetColor;

        public void SetTargetSpriteAsActive(Color highlightColor)
        {
            var targetMat =targetSprite.material;
            targetMat.EnableKeyword("GREYSCALE_ON");
            targetMat.EnableKeyword("OUTBASE_ON");
            targetMat.SetColor("_OutlineColor",highlightColor);
        }

        public void SetTargetSpriteAsInactive()
        {
            var mat = targetSprite.material;
            mat.EnableKeyword("GREYSCALE_ON");
            mat.DisableKeyword("OUTBASE_ON");
        }

        public void SetTargetSpriteAsCompleted()
        {
            var targetMat =targetSprite.material;
            targetMat.DisableKeyword("GREYSCALE_ON");
            targetMat.DisableKeyword("OUTBASE_ON");
        }
    }

}