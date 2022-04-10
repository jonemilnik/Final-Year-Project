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
        public System.Boolean IsFacingPlayer;
        public System.Single DistToPlayer;

        public bool Equals(EnemyData other)
        {
            return IsFacingPlayer.Equals(other.IsFacingPlayer) && DistToPlayer.Equals(other.DistToPlayer);
        }

        public override string ToString()
        {
            return $"Enemy: {IsFacingPlayer} {DistToPlayer}";
        }
    }
}
