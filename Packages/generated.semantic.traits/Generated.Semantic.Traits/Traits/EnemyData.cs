using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct EnemyData : ITraitData, IEquatable<EnemyData>
    {
        public System.Boolean IsFacingPlayer;
        public System.Single DistToPlayer;
        public System.Single Speed;
        public System.Single DistToWaypoint;
        public System.Single FOVRadius;
        public System.Single WaypointX;
        public System.Single WaypointY;
        public System.Single WaypointZ;

        public bool Equals(EnemyData other)
        {
            return IsFacingPlayer.Equals(other.IsFacingPlayer) && DistToPlayer.Equals(other.DistToPlayer) && Speed.Equals(other.Speed) && DistToWaypoint.Equals(other.DistToWaypoint) && FOVRadius.Equals(other.FOVRadius) && WaypointX.Equals(other.WaypointX) && WaypointY.Equals(other.WaypointY) && WaypointZ.Equals(other.WaypointZ);
        }

        public override string ToString()
        {
            return $"Enemy: {IsFacingPlayer} {DistToPlayer} {Speed} {DistToWaypoint} {FOVRadius} {WaypointX} {WaypointY} {WaypointZ}";
        }
    }
}
