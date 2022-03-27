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
        public const string FieldWaypoint = "Waypoint";
        public const string FieldIsSpotted = "IsSpotted";
        public Unity.AI.Planner.Traits.TraitBasedObjectId Waypoint;
        public System.Boolean IsSpotted;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    Waypoint = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(IsSpotted):
                    IsSpotted = (System.Boolean)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Player.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Waypoint):
                    return Waypoint;
                case nameof(IsSpotted):
                    return IsSpotted;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Player.");
            }
        }

        public bool Equals(Player other)
        {
            return Waypoint == other.Waypoint && IsSpotted == other.IsSpotted;
        }

        public override string ToString()
        {
            return $"Player\n  Waypoint: {Waypoint}\n  IsSpotted: {IsSpotted}";
        }
    }
}
