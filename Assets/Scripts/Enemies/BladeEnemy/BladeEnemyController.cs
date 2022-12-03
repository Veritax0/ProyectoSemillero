using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BladeEnemy
{
    public class BladeEnemyController : EnemyController{
    
        [Header("Guard config")]
        public List<Transform> points;
        public float minDistanceToChangePoint;
    
        [Header("Atack config")]
        public GameObject objective;
        public float attackDistance = 1.5f;
        public float chaseDistance = 10f;
    
        internal NavMeshAgent Agent;
        internal RaycastHit Hit;
        internal bool IsHit;
    
        internal GameObject Blade;
        internal Animator Animator;

        private PlayerMovement _playerMovement;
        private const float PlayerRunningFactor = 1.2f;
        private const float PlayerSquattingFactor  = 0.8f;

        private string _status;
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

            _playerMovement = objective.GetComponent<PlayerMovement>();
        }
        void Update()
        {
            State.Execute();
            if (State == GuardState) _status = "Guard";
            else if (State == ChaseState) _status = "Chase";
            else if (State == AttackState) _status = "Attack";
        }

        public override void ChangeState(IEnemyState enemyState)
        {
            State = enemyState;
        }
    
        private void OnDrawGizmos()
        {
            Vector3 objPos = objective.transform.position;
            objPos.y += 0.5f;
            Vector3 position = transform.position;
            position.y = 1.5f;
            Vector3 direction = Vector3.Normalize(objPos - position);

            float factor = 1;
            if (_playerMovement != null)
            {
                switch (_playerMovement.GetMovStatus())
                {
                    case PlayerMoveStatus.Squatting: 
                        factor = PlayerSquattingFactor;
                        break;
                    case PlayerMoveStatus.Running :
                        factor = PlayerRunningFactor;
                        break;
                    default:
                        factor = 1;
                        break;
                }
            }
            
            float distance = chaseDistance * factor;
            
            IsHit = Physics.SphereCast(position, transform.lossyScale.x / 2, 
                direction, out Hit, distance);
            if (IsHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(position, direction * Hit.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(position, direction * distance);
            }
        }
    }
}
