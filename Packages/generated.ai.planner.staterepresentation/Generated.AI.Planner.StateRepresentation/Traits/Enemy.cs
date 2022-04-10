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
        public System.Boolean IsFacingPlayer;
        public System.Single DistToPlayer;

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
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Enemy.");
            }
        }

        public bool Equals(Enemy other)
        {
            return IsFacingPlayer == other.IsFacingPlayer && DistToPlayer == other.DistToPlayer;
        }

        public override string ToString()
        {
            return $"Enemy\n  IsFacingPlayer: {IsFacingPlayer}\n  DistToPlayer: {DistToPlayer}";
        }
    }
}
