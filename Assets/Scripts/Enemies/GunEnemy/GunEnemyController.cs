using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.GunEnemy
{
    public class GunEnemyController : EnemyController
    {  
        [Header("Guard config")]
        public List<Transform> points; //Guard
        public float minDistanceToChangePoint; //Guard
    
        [Header("Gun config")]
        public Transform pointer; //Attack
        public Transform bulletGen; //Attack
        public GameObject bullet; //Attack
    
        [Header("Attack config")]
        public GameObject objective;//Chase
        public float aimTime; //Attack
        public float aimDistance; //Chase
        public float shootDistance = 10f; //Chase
    
        
        internal NavMeshAgent Agent;
        internal RaycastHit Hit;
        internal bool IsHit;
        
        
        internal Animator Animator;
        private int _currentPosition;
        private static readonly int Aim1 = Animator.StringToHash("Aim");

        private PlayerMovement _playerMovement;
        private const float PlayerRunningFactor = 1.05f;
        private const float PlayerSquattingFactor  = 0.8f;
        public override void ChangeState(IEnemyState enemyState)
        {
            State = enemyState;
            Animator.SetInteger(Aim1, 0);
        }

        void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.SetDestination(points[_currentPosition].position);
            Animator = GetComponent<Animator>();
            aimDistance = Vector3.Distance(pointer.position, bulletGen.position);
            GuardState = gameObject.AddComponent<GunEnemyGuardState>();
            GuardState.SetContext(this);
            ChaseState = gameObject.AddComponent<GunEnemyChaseState>();
            ChaseState.SetContext(this);
            AttackState = gameObject.AddComponent<GunEnemyAttackState>();
            AttackState.SetContext(this);
            State = GuardState;
            
            _playerMovement = objective.GetComponent<PlayerMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            State.Execute();
        }
        private void OnDrawGizmos()
        {
            Vector3 pointerPos = pointer.position;
            Vector3 bulletGenPos = bulletGen.position;
            Vector3 direction = Vector3.Normalize(pointerPos - bulletGenPos);
        
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
            float distance = aimDistance * factor;
            
            IsHit = Physics.SphereCast(bulletGenPos, transform.lossyScale.x / 2, 
                direction, out Hit, distance);
            if (IsHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(bulletGenPos, direction * Hit.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(bulletGenPos, direction * distance);
            }
        }
    }
}
