using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Player : ITrait, IBufferElementData, IEquatable<Player>
    {
        public const string FieldSetWaypoint = "SetWaypoint";
        public const string FieldIsSpotted = "IsSpotted";
        public const string FieldIsRunning = "IsRunning";
        public const string FieldSpeed = "Speed";
        public const string FieldIsHiding = "IsHiding";
        public const string FieldIsWaypointSet = "IsWaypointSet";
        public Unity.AI.Planner.Traits.TraitBasedObjectId SetWaypoint;
        public System.Boolean IsSpotted;
        public System.Boolean IsRunning;
        public System.Single Speed;
        public System.Boolean IsHiding;
        public System.Boolean IsWaypointSet;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(SetWaypoint):
                    SetWaypoint = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(IsSpotted):
                    IsSpotted = (System.Boolean)value;
                    break;
                case nameof(IsRunning):
                    IsRunning = (System.Boolean)value;
                    break;
                case nameof(Speed):
                    Speed = (System.Single)value;
                    break;
                case nameof(IsHiding):
                    IsHiding = (System.Boolean)value;
                    break;
                case nameof(IsWaypointSet):
                    IsWaypointSet = (System.Boolean)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Player.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(SetWaypoint):
                    return SetWaypoint;
                case nameof(IsSpotted):
                    return IsSpotted;
                case nameof(IsRunning):
                    return IsRunning;
                case nameof(Speed):
                    return Speed;
                case nameof(IsHiding):
                    return IsHiding;
                case nameof(IsWaypointSet):
                    return IsWaypointSet;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Player.");
            }
        }

        public bool Equals(Player other)
        {
            return SetWaypoint == other.SetWaypoint && IsSpotted == other.IsSpotted && IsRunning == other.IsRunning && Speed == other.Speed && IsHiding == other.IsHiding && IsWaypointSet == other.IsWaypointSet;
        }

        public override string ToString()
        {
            return $"Player\n  SetWaypoint: {SetWaypoint}\n  IsSpotted: {IsSpotted}\n  IsRunning: {IsRunning}\n  Speed: {Speed}\n  IsHiding: {IsHiding}\n  IsWaypointSet: {IsWaypointSet}";
        }
    }
}
