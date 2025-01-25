using System;
using UnityEngine;

namespace __Project.Systems.BlockSystem
{
    public static class BlockStatic
    {
        public static Color GetColor(BlockColorEnum color)
        {
            return color switch
            {
                BlockColorEnum.Blue => Color.blue,
                BlockColorEnum.Green => Color.green,
                BlockColorEnum.Red => Color.red,
                BlockColorEnum.Yellow => Color.yellow,
                BlockColorEnum.Pink => new Color(1, 0.5f, 0.5f),
                BlockColorEnum.Gray => Color.gray,
                BlockColorEnum.Purple => new Color(0.5f, 0, 0.5f),
                BlockColorEnum.White => Color.white,
                BlockColorEnum.Black => Color.black,
                BlockColorEnum.SkyBlue => new Color(0.2f, 0.91f, 1f),
                BlockColorEnum.Brown => new Color(0.6f, 0.3f, 0.1f),
                BlockColorEnum.Orange => new Color(1, 0.5f, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };
        }
    }
}