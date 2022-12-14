using UnityEngine;

namespace Enemies
{
    public abstract class EnemyController : MonoBehaviour
    {
        protected IEnemyState State;
        protected internal IEnemyState GuardState;
        protected internal IEnemyState ChaseState;
        protected internal IEnemyState AttackState;

        public abstract void ChangeState(IEnemyState enemyState);

        public abstract void AddRadarPoint();
    }
}