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
        public const string FieldIsFacingPlayer = "IsFacingPlayer";
        public const string FieldDistToPlayer = "DistToPlayer";
        public const string FieldSpeed = "Speed";
        public const string FieldDistToWaypoint = "DistToWaypoint";
        public const string FieldFOVRadius = "FOVRadius";
        public const string FieldWaypointX = "WaypointX";
        public const string FieldWaypointY = "WaypointY";
        public const string FieldWaypointZ = "WaypointZ";
        public System.Boolean IsFacingPlayer;
        public System.Single DistToPlayer;
        public System.Single Speed;
        public System.Single DistToWaypoint;
        public System.Single FOVRadius;
        public System.Single WaypointX;
        public System.Single WaypointY;
        public System.Single WaypointZ;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(IsFacingPlayer):
                    IsFacingPlayer = (System.Boolean)value;
                    break;
                case nameof(DistToPlayer):
                    DistToPlayer = (System.Single)value;
                    break;
                case nameof(Speed):
                    Speed = (System.Single)value;
                    break;
                case nameof(DistToWaypoint):
                    DistToWaypoint = (System.Single)value;
                    break;
                case nameof(FOVRadius):
                    FOVRadius = (System.Single)value;
                    break;
                case nameof(WaypointX):
                    WaypointX = (System.Single)value;
                    break;
                case nameof(WaypointY):
                    WaypointY = (System.Single)value;
                    break;
                case nameof(WaypointZ):
                    WaypointZ = (System.Single)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Enemy.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(IsFacingPlayer):
                    return IsFacingPlayer;
                case nameof(DistToPlayer):
                    return DistToPlayer;
                case nameof(Speed):
                    return Speed;
                case nameof(DistToWaypoint):
                    return DistToWaypoint;
                case nameof(FOVRadius):
                    return FOVRadius;
                case nameof(WaypointX):
                    return WaypointX;
                case nameof(WaypointY):
                    return WaypointY;
                case nameof(WaypointZ):
                    return WaypointZ;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Enemy.");
            }
        }

        public bool Equals(Enemy other)
        {
            return IsFacingPlayer == other.IsFacingPlayer && DistToPlayer == other.DistToPlayer && Speed == other.Speed && DistToWaypoint == other.DistToWaypoint && FOVRadius == other.FOVRadius && WaypointX == other.WaypointX && WaypointY == other.WaypointY && WaypointZ == other.WaypointZ;
        }

        public override string ToString()
        {
            return $"Enemy\n  IsFacingPlayer: {IsFacingPlayer}\n  DistToPlayer: {DistToPlayer}\n  Speed: {Speed}\n  DistToWaypoint: {DistToWaypoint}\n  FOVRadius: {FOVRadius}\n  WaypointX: {WaypointX}\n  WaypointY: {WaypointY}\n  WaypointZ: {WaypointZ}";
        }
    }
}
