using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct WayPoint : ITrait, IBufferElementData, IEquatable<WayPoint>
    {
        public const string FieldLeft = "Left";
        public const string FieldUp = "Up";
        public const string FieldRight = "Right";
        public const string FieldDown = "Down";
        public Unity.AI.Planner.Traits.TraitBasedObjectId Left;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Up;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Right;
        public Unity.AI.Planner.Traits.TraitBasedObjectId Down;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(Left):
                    Left = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Up):
                    Up = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Right):
                    Right = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                case nameof(Down):
                    Down = (Unity.AI.Planner.Traits.TraitBasedObjectId)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait WayPoint.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(Left):
                    return Left;
                case nameof(Up):
                    return Up;
                case nameof(Right):
                    return Right;
                case nameof(Down):
                    return Down;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait WayPoint.");
            }
        }

        public bool Equals(WayPoint other)
        {
            return Left == other.Left && Up == other.Up && Right == other.Right && Down == other.Down;
        }

        public override string ToString()
        {
            return $"WayPoint\n  Left: {Left}\n  Up: {Up}\n  Right: {Right}\n  Down: {Down}";
        }
    }
}
