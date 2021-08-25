using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Generic placeholder target.
    /// </summary>
    public class PointTarget : ITarget
    {
        public PointTarget()
        {
            
        }
        
        public Vector3 Position { get; set; } = Vector3.zero;
        
        public PointTarget(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }
        
        public Vector3 GetPosition()
        {
            return Position;
        }
    }
}