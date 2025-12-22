using System;
using UnityEngine;

namespace Root.Scripts.Grid
{
    [Serializable]
    public struct NodePosition
    {
        public float x;
        public float z;
        [HideInInspector]
        public float y;

        public NodePosition(float x, float z)
        {
            this.x = x;
            this.z = z;
            y = 0;
        }

        public NodePosition(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals<T>(NodePosition otherPosition)
        {
            if (typeof(T) == typeof(int))
            {
                return (int)x == (int)otherPosition.x && (int)z == (int)otherPosition.z;
            }

            if (typeof(T) == typeof(float))
            {
                return Mathf.Approximately(x, otherPosition.x) &&
                       Mathf.Approximately(y, otherPosition.y) &&
                       Mathf.Approximately(z, otherPosition.z);
            }

            return false;
        }

        public void Set(float x, float z)
        {
            this.x = x;
            this.z = z;
            y = 0;
        }

        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"x: {x}, y: {y}, z: {z}";
        }
    }
}