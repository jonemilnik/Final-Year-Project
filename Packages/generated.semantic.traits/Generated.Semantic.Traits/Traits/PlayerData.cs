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
        public Unity.Entities.Entity SetWaypoint;
        public System.Boolean IsSpotted;
        public System.Boolean IsRunning;
        public System.Single Speed;
        public System.Boolean IsHiding;
        public System.Boolean IsWaypointSet;

        public bool Equals(PlayerData other)
        {
            return SetWaypoint.Equals(other.SetWaypoint) && IsSpotted.Equals(other.IsSpotted) && IsRunning.Equals(other.IsRunning) && Speed.Equals(other.Speed) && IsHiding.Equals(other.IsHiding) && IsWaypointSet.Equals(other.IsWaypointSet);
        }

        public override string ToString()
        {
            return $"Player: {SetWaypoint} {IsSpotted} {IsRunning} {Speed} {IsHiding} {IsWaypointSet}";
        }
    }
}
