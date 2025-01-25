using System.Collections.Generic;
using _NueCore.ManagerSystem.Core;
using UnityEngine;

namespace _NueCore.ManagerSystem
{
    [CreateAssetMenu(fileName = nameof(NManagersCatalog),menuName = "Data/ManagerSystem/Catalog")]
    public class NManagersCatalog : ScriptableObject
    {
        [SerializeField] private List<NManagerBase> managerList;

        public List<NManagerBase> ManagerList => managerList;
    }
}