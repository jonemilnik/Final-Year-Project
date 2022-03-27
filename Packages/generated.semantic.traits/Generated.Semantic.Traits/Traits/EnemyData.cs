using System;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Collections;
using Unity.Entities;

namespace Generated.Semantic.Traits
{
    [Serializable]
    public partial struct EnemyData : ITraitData, IEquatable<EnemyData>
    {

        public bool Equals(EnemyData other)
        {
            return true;
        }

        public override string ToString()
        {
            return $"Enemy";
        }
    }
}
