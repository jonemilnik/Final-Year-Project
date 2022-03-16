using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct WayPointData : ITraitData, IEquatable<WayPointData>
    {
        public Unity.Entities.Entity Left;
        public Unity.Entities.Entity Up;
        public Unity.Entities.Entity Right;
        public Unity.Entities.Entity Down;

        public bool Equals(WayPointData other)
        {
            return Left.Equals(other.Left) && Up.Equals(other.Up) && Right.Equals(other.Right) && Down.Equals(other.Down);
        }

        public override string ToString()
        {
            return $"WayPoint: {Left} {Up} {Right} {Down}";
        }
    }
}
