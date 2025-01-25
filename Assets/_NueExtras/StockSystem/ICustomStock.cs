using UnityEngine;

namespace _NueExtras.StockSystem
{
    public interface ICustomStock
    {
        Transform GetTransform();
        Rigidbody GetRigidBody();
        Collider GetCollider();
        void ActivateDisable();
        void Dispose();
    }
}