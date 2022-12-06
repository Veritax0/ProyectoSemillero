using System;
using System.Collections.Generic;
using Audio;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.GunEnemy
{
    public class GunEnemyController : EnemyController
    {
        [Header("Guard config")] 
        public Transform pointsParent; 
        internal List<Transform> Points; //Guard
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

        internal AudioControllerEnemy AudioEnemy;
        
        internal Animator Animator;
        private int _currentPosition;
        private static readonly int Aim1 = Animator.StringToHash("Aim");

        private PlayerMovement _playerMovement;
        private const float PlayerRunningFactor = 1.05f;
        private const float PlayerSquattingFactor  = 0.8f;
        
        //RayCast
        private Vector3 _bulletGenPos;
        private Vector3 _rayDirection;
        private float _rayDistance;

        public override void ChangeState(IEnemyState enemyState)
        {
            State = enemyState;
            Animator.SetInteger(Aim1, 0);
        }

        void Start()
        {
            Points = new List<Transform>();
            switch (pointsParent.childCount)
            {
                case 0:
                    throw new Exception("Debe ingresar una ruta.");
                case 1:
                    Points.Add(pointsParent);
                    break;
                default:
                    for (int i = 0; i < pointsParent.childCount; i++)
                        Points.Add(pointsParent.GetChild(i));
                    break;
            }

            _playerMovement = objective.GetComponent<PlayerMovement>();
            AudioEnemy = GetComponent<AudioControllerEnemy>();
            
            Agent = GetComponent<NavMeshAgent>();
            Agent.SetDestination(Points[_currentPosition].position);
            Animator = GetComponent<Animator>();
            aimDistance = Vector3.Distance(pointer.position, bulletGen.position);
            GuardState = gameObject.AddComponent<GunEnemyGuardState>();
            GuardState.SetContext(this);
            ChaseState = gameObject.AddComponent<GunEnemyChaseState>();
            ChaseState.SetContext(this);
            AttackState = gameObject.AddComponent<GunEnemyAttackState>();
            AttackState.SetContext(this);
            State = GuardState;
        }

        void Update()
        {
            DoRayCast();
            State.Execute();
        }
        private void DoRayCast()
        {
            Vector3 pointerPos = pointer.position;
            _bulletGenPos = bulletGen.position;
            _rayDirection = Vector3.Normalize(pointerPos - _bulletGenPos);
        
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
            _rayDistance = aimDistance * factor;
            
            IsHit = Physics.SphereCast(_bulletGenPos, transform.lossyScale.x / 2, 
                _rayDirection, out Hit, _rayDistance);
        }

        private void OnDrawGizmos()
        {
            if (IsHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(_bulletGenPos, _rayDirection * Hit.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(_bulletGenPos, _rayDirection * _rayDistance);
            }
        }
    }
}
