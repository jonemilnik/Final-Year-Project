using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct MoverData : ITraitData, IEquatable<MoverData>
    {
        public System.Single X;
        public System.Single Y;
        public System.Single Z;
        public System.Single ForwardX;
        public System.Single ForwardY;
        public System.Single ForwardZ;

        public bool Equals(MoverData other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && ForwardX.Equals(other.ForwardX) && ForwardY.Equals(other.ForwardY) && ForwardZ.Equals(other.ForwardZ);
        }

        public override string ToString()
        {
            return $"Mover: {X} {Y} {Z} {ForwardX} {ForwardY} {ForwardZ}";
        }
    }
}
