using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct HideableData : ITraitData, IEquatable<HideableData>
    {

        public bool Equals(HideableData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Hideable";
        }
    }
}
