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
        public Unity.Entities.Entity PrevWaypoint;

        public bool Equals(HideableData other)
        {
            return PrevWaypoint.Equals(other.PrevWaypoint);
        }

        public override string ToString()
        {
            return $"Hideable: {PrevWaypoint}";
        }
    }
}
