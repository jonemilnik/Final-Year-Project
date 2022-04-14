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
        public Unity.AI.Planner.Traits.TraitBasedObjectId SetWaypoint;
        public System.Boolean IsSpotted;

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
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Player.");
            }
        }

        public bool Equals(Player other)
        {
            return SetWaypoint == other.SetWaypoint && IsSpotted == other.IsSpotted;
        }

        public override string ToString()
        {
            return $"Player\n  SetWaypoint: {SetWaypoint}\n  IsSpotted: {IsSpotted}";
        }
    }
}
