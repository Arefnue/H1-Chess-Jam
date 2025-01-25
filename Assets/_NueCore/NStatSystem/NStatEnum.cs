using System;

namespace _NueCore.NStatSystem
{
    public enum NStatEnum
    {
        None =0,
        Point =1,
        Weight =2,
        Bounciness =3,
        BounceMultiplier =4,
        PocketMultiplier =5,
        Attraction =6,
        Repulsion =7,
        Power = 8,
        PocketPoint =9,
        SleepTurn =10,
        Curse = 11,
        Ghost = 12,
        Scale = 13
    }
    
    public static class NStatEnumExtension
    {
        public static string GetStatName(this NStatEnum statEnum)
        {
            return statEnum switch
            {
                NStatEnum.Point => "Point",
                NStatEnum.Weight => "Weight",
                NStatEnum.Bounciness => "Bounciness",
                NStatEnum.BounceMultiplier => "BounceMultiplier",
                NStatEnum.PocketMultiplier => "PocketMultiplier",
                NStatEnum.Attraction => "Attraction",
                NStatEnum.Repulsion => "Repulsion",
                NStatEnum.Power => "Power",
                NStatEnum.PocketPoint => "PocketPoint",
                NStatEnum.SleepTurn => "SleepTurn",
                NStatEnum.Curse => "Curse",
                NStatEnum.Ghost => "Ghost",
                NStatEnum.Scale => "Scale",
                _ => "None"
            };
        }

    }
}