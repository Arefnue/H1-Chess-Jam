using System.Collections.Generic;
using UnityEngine;

namespace __Project.Systems.PowerUpSystem
{
    public class PowerUpController : MonoBehaviour
    {
        [SerializeField] private List<PowerUpButton> powerUpButtonList;
        
        public void Build()
        {
            foreach (var button in powerUpButtonList)
            {
                button.Build();
            }
        }
    }
}