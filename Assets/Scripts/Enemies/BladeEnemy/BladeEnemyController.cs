using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BladeEnemy
{
    public class BladeEnemyController : EnemyController{
    
        [Header("Guard config")]
        public List<Transform> points;
        public float minDistanceToChangePoint;
    
        [Header("Atack config")]
        public PlayerController objective;
        public float attackDistance = 1.5f;
        public float chaseDistance = 10f;
    
        internal NavMeshAgent Agent;
        internal RaycastHit Hit;
        internal bool IsHit;
    
        internal GameObject Blade;
        internal Animator Animator;

        /*private IEnemyState _state;
    internal IEnemyState GuardState;
    internal IEnemyState ChaseState;
    internal IEnemyState AttackState;*/
    

        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Blade = transform.GetChild(0).gameObject;
            Animator = GetComponent<Animator>();
            GuardState = gameObject.AddComponent<BladeEnemyGuardState>();
            GuardState.SetContext(this);
            ChaseState = gameObject.AddComponent<BladeEnemyChaseState>();
            ChaseState.SetContext(this);
            AttackState = gameObject.AddComponent<BladeEnemyAttackState>();
            AttackState.SetContext(this);
            State = GuardState;
        }
        void Update()
        {
            State.Execute();
        }

        public override void ChangeState(IEnemyState enemyState)
        {
            State = enemyState;
        }
    
        private void OnDrawGizmos()
        {
            Vector3 objPos = objective.transform.position;
            Vector3 position = transform.position;
            Vector3 direction = Vector3.Normalize(objPos - position);

            IsHit = Physics.SphereCast(position, transform.lossyScale.x / 2, 
                direction, out Hit, chaseDistance);
            if (IsHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(position, direction * Hit.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(position, direction * chaseDistance);
            }
        }
    }
}
