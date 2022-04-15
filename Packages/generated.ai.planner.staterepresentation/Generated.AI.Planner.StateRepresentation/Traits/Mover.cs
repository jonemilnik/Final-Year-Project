using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Mover : ITrait, IBufferElementData, IEquatable<Mover>
    {
        public const string FieldX = "X";
        public const string FieldY = "Y";
        public const string FieldZ = "Z";
        public const string FieldForwardX = "ForwardX";
        public const string FieldForwardY = "ForwardY";
        public const string FieldForwardZ = "ForwardZ";
        public System.Single X;
        public System.Single Y;
        public System.Single Z;
        public System.Single ForwardX;
        public System.Single ForwardY;
        public System.Single ForwardZ;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(X):
                    X = (System.Single)value;
                    break;
                case nameof(Y):
                    Y = (System.Single)value;
                    break;
                case nameof(Z):
                    Z = (System.Single)value;
                    break;
                case nameof(ForwardX):
                    ForwardX = (System.Single)value;
                    break;
                case nameof(ForwardY):
                    ForwardY = (System.Single)value;
                    break;
                case nameof(ForwardZ):
                    ForwardZ = (System.Single)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Mover.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(X):
                    return X;
                case nameof(Y):
                    return Y;
                case nameof(Z):
                    return Z;
                case nameof(ForwardX):
                    return ForwardX;
                case nameof(ForwardY):
                    return ForwardY;
                case nameof(ForwardZ):
                    return ForwardZ;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Mover.");
            }
        }

        public bool Equals(Mover other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && ForwardX == other.ForwardX && ForwardY == other.ForwardY && ForwardZ == other.ForwardZ;
        }

        public override string ToString()
        {
            return $"Mover\n  X: {X}\n  Y: {Y}\n  Z: {Z}\n  ForwardX: {ForwardX}\n  ForwardY: {ForwardY}\n  ForwardZ: {ForwardZ}";
        }
    }
}
