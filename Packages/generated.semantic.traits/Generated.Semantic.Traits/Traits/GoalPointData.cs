using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct GoalPointData : ITraitData, IEquatable<GoalPointData>
    {

        public bool Equals(GoalPointData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"GoalPoint";
        }
    }
}
