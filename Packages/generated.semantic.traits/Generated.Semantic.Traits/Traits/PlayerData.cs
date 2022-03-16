using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct PlayerData : ITraitData, IEquatable<PlayerData>
    {
        public Unity.Entities.Entity Waypoint;

        public bool Equals(PlayerData other)
        {
            return Waypoint.Equals(other.Waypoint);
        }

        public override string ToString()
        {
            return $"Player: {Waypoint}";
        }
    }
}
