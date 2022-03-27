using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Enemy : ITrait, IBufferElementData, IEquatable<Enemy>
    {

        public void SetField(string fieldName, object value)
        {
        }

        public object GetField(string fieldName)
        {
            throw new ArgumentException("No fields exist on trait Enemy.");
        }

        public bool Equals(Enemy other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Enemy";
        }
    }
}
