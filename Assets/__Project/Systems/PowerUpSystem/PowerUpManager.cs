using _NueCore.ManagerSystem.Core;
using UnityEngine;

namespace __Project.Systems.PowerUpSystem
{
    public class PowerUpManager : NManagerBase
    {
        [SerializeField] private PowerUpController controller;
        
        public override void NStart()
        {
            base.NStart();
            controller.Build();
        }
    }
}