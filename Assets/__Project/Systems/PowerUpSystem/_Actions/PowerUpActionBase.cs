using UnityEngine;

namespace __Project.Systems.PowerUpSystem._Actions
{
    public abstract class PowerUpActionBase : MonoBehaviour
    {
        public PowerUpButton Button { get; private set; }
        public virtual void Build(PowerUpButton button)
        {
            Button = button;
        }
        public abstract void Apply();

        public virtual bool CanUse()
        {
            return true;
        }

        public virtual void Reset()
        {
        }
    }
}