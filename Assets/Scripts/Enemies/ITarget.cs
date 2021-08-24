using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Interface for arbitrary target point in 3D environment.
    /// </summary>
    public interface ITarget
    {
        public Vector3 GetPosition();
    }
}