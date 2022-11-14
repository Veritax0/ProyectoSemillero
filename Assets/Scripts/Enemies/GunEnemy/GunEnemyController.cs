using System.Collections;
using System.Collections.Generic;
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
        public PlayerController objective;//Chase
        public float aimTime; //Attack
        public float aimDistance; //Chase
        public float shootDistance = 10f; //Chase
    
        private EnemyStateEnum _status = EnemyStateEnum.GUARD;
        
        internal NavMeshAgent Agent;
        internal RaycastHit Hit;
        internal bool IsHit;
        
        
        internal Animator Animator;
        private Coroutine _aimWalkCor; //Guard
        private Coroutine _aimAroundCor; //Guard
        private float _distanceToNextPoint; //Guard
        private int _currentPosition; //Guard
        private bool _changeDestination; //Guard
        private bool _isAimWalk; //Guard
        private static readonly int Aim1 = Animator.StringToHash("Aim"); //Guard
        
        private float _distanceToObjective; //Chase
        private bool _isSetDestination; //Chase
        private bool _isAim; //Attack

        public override void ChangeState(IEnemyState enemyState)
        {
            State = enemyState;
            Animator.SetInteger(Aim1, 0);
            _isAim = false;
            _isAimWalk = false;
            _changeDestination = false;
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
        }

        // Update is called once per frame
        void Update()
        {
            State.Execute();
            //CheckStatus();
            if (State == GuardState)
            {
                _status = EnemyStateEnum.GUARD;
            }
            if (State == ChaseState)
            {
                _status = EnemyStateEnum.CHASE;
            }
            if (State == AttackState)
            {
                _status = EnemyStateEnum.ATTACK;
            }
        }
    
        private void CheckStatus()
        {
            switch (_status)
            {
                case EnemyStateEnum.GUARD:
                    Guard();
                    break;
                case EnemyStateEnum.CHASE:
                    Chase();
                    break;
                case EnemyStateEnum.AIMANDSHOOT:
                    Aim();
                    break;
            }
        }

        private void Aim()
        {
            if (!_isAim)
            {
                StartCoroutine(AimAndShoot());
            }
        }

        private void Guard()
        {
            if (IsHit)
            {
                if (Hit.transform.gameObject.CompareTag("Player")){
                    StopCoroutine(_aimWalkCor);
                    StopCoroutine(_aimAroundCor);
                    ChangeStatus(EnemyStateEnum.CHASE);
                    return;
                }
            }
            if (!_isAimWalk)
            {
                _aimWalkCor = StartCoroutine(AimWalking());
            }
            _distanceToNextPoint = Vector3.Distance(transform.position, points[_currentPosition].position);
            if (!_changeDestination && _distanceToNextPoint < minDistanceToChangePoint)
            {
                StopCoroutine(_aimWalkCor);
                _aimAroundCor = StartCoroutine(AimAround());
            }
            else
            {
                Agent.SetDestination(points[_currentPosition].position);
            }
        }
    
        private void Chase()
        {
            Vector3 pos = transform.position;
            Vector3 direction = Vector3.Normalize(objective.transform.position - pos);
            IsHit =  Physics.SphereCast(pos, transform.lossyScale.x / 2, 
                direction, out Hit, aimDistance);
            if (IsHit)
            {
                if (!Hit.transform.gameObject.CompareTag("Player")){
                    ChangeStatus(EnemyStateEnum.GUARD);
                    return;
                }
                if(!_isSetDestination) StartCoroutine(SetDestinationToPlayer());
            }
            else
            {
                ChangeStatus(EnemyStateEnum.GUARD);
            }
        }
    
        private IEnumerator AimAround()
        {
            _changeDestination = true;
            Animator.SetInteger(Aim1, 0);
            yield return new WaitForFixedUpdate();
            Animator.SetInteger(Aim1, 1);
            yield return new WaitForSeconds(3.7f);
            Animator.SetInteger(Aim1, 0);
            _currentPosition = _currentPosition < points.Count - 1 ? _currentPosition + 1 : 0;
            Agent.SetDestination(points[_currentPosition].position);
            _changeDestination = false;
            _isAimWalk = false;
        }

        private IEnumerator AimWalking()
        {
            _isAimWalk = true;
            Animator.SetInteger(Aim1, 2);
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            Animator.SetInteger(Aim1, 0);
            _isAimWalk = false;
        }
    
        private IEnumerator AimAndShoot()
        {
            _isAim = true;
            Agent.SetDestination(transform.position);
            yield return new WaitForSeconds(aimTime);
            Shoot();
            ChangeStatus(EnemyStateEnum.GUARD);
        }

        private IEnumerator SetDestinationToPlayer()
        {
            _isSetDestination = true;
            Vector3 objPos = objective.transform.position;
            Agent.SetDestination(objPos);
            yield return new WaitForSeconds(0.5f);
            _distanceToObjective = Vector3.Distance(transform.position,objPos);
            ChangeStatus(_distanceToObjective <= shootDistance ? EnemyStateEnum.AIMANDSHOOT : EnemyStateEnum.CHASE);
            _isSetDestination = false;
        }

        private void Shoot()
        {
            GameObject bulletInstantiate = Instantiate(bullet, bulletGen);
            Vector3 dir = Vector3.Normalize(pointer.position - bulletGen.position);
            float force = 20;
            Rigidbody rb = bulletInstantiate.GetComponent<Rigidbody>();
            bulletInstantiate.transform.SetParent(null);
            rb.AddForce(dir * force,ForceMode.Impulse);
            Destroy(bulletInstantiate, 3);
        }
    
        private void OnDrawGizmos()
        {
            Vector3 pointerPos = pointer.position;
            Vector3 bulletGenPos = bulletGen.position;
            Vector3 direction = Vector3.Normalize(pointerPos - bulletGenPos);
        
            IsHit = Physics.SphereCast(bulletGenPos, transform.lossyScale.x / 2, 
                direction, out Hit, aimDistance);
            if (IsHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(bulletGenPos, direction * aimDistance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(bulletGenPos, direction * aimDistance);
            }
        }

        private void ChangeStatus(EnemyStateEnum state)
        {
            
        }
    }
}
