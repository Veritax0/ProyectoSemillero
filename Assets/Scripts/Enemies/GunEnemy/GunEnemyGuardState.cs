using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.GunEnemy
{
    public class GunEnemyGuardState : MonoBehaviour, IEnemyState
    {
        private GunEnemyController _context;
        private List<Transform> _points;
        private float _minDistanceToChangePoint;
        private NavMeshAgent _agent;
        private Animator _animator;
        private Coroutine _aimWalkCor;
        private Coroutine _aimAroundCor;
        private float _distanceToNextPoint;
        private int _currentPosition;
        private bool _changeDestination;
        private bool _isAimWalk;
        private static readonly int Aim1 = Animator.StringToHash("Aim");
        private const float AimAroundTime = 3.7f;
        
        private bool _isGuardHit;
        private RaycastHit _guardHit;
        private float _guardRayDistance;
        
        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (GunEnemyController) context;
                _points = _context.Points;
                _minDistanceToChangePoint = _context.minDistanceToChangePoint;
                _agent = _context.Agent;
                _animator = _context.Animator;
                _aimAroundCor = StartCoroutine(VoidCor());
                _aimWalkCor = StartCoroutine(VoidCor());
                _guardRayDistance = _context.guardRayDistance;
                
                _context.AudioEnemy.Walk();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
        
        public void Execute()
        {
            _agent.SetDestination(_points[_currentPosition].position);

            Vector3 pos = transform.position;
            Vector3 rayDirection = 
                Vector3.Normalize(_context.objective.transform.position - pos);
            _isGuardHit = Physics.SphereCast(pos, transform.lossyScale.x / 2, 
                rayDirection, out _guardHit, _guardRayDistance);

            bool change = false;
            
            if (_context.IsHit)
            {
                if (_context.Hit.transform.gameObject.CompareTag("Player"))
                {
                    change = true;
                }
            }
            
            if (_isGuardHit)
            {
                if (_guardHit.transform.gameObject.CompareTag("Player"))
                {
                    change = true;
                }
            }

            if (change)
            {
                StopCoroutine(_aimWalkCor);
                StopCoroutine(_aimAroundCor);
                _context.ChangeState(_context.ChaseState);
                _isAimWalk = false;
                _changeDestination = false;
                return;
            }
            
            if (!_isAimWalk)
            {
                _aimWalkCor = StartCoroutine(AimWalking());
            }
            _distanceToNextPoint = Vector3.Distance(transform.position, _points[_currentPosition].position);
            if (!_changeDestination && _distanceToNextPoint < _minDistanceToChangePoint)
            {
                StopCoroutine(_aimWalkCor);
                _aimAroundCor = StartCoroutine(AimAround());
            }
        }
        
        private IEnumerator AimAround()
        {
            _changeDestination = true;
            _animator.SetInteger(Aim1, 0);
            _context.AudioEnemy.Idle();
            yield return new WaitForFixedUpdate();
            
            _animator.SetInteger(Aim1, 1);
            yield return new WaitForSeconds(AimAroundTime);
            
            _animator.SetInteger(Aim1, 0);
            _currentPosition = _currentPosition < _points.Count - 1 ? _currentPosition + 1 : 0;
            _agent.SetDestination(_points[_currentPosition].position);
            _changeDestination = false;
            _isAimWalk = false;
            
            _context.AudioEnemy.Walk();
        }

        private IEnumerator AimWalking()
        {
            _isAimWalk = true;
            _animator.SetInteger(Aim1, 2);
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            _animator.SetInteger(Aim1, 0);
            _isAimWalk = false;
        }

        private IEnumerator VoidCor()
        {
            yield return new WaitForEndOfFrame();
        }

        private void OnDrawGizmos()
        {
            Vector3 pos = transform.position;
            Vector3 rayDirection = 
                Vector3.Normalize(_context.objective.transform.position - pos);
            Gizmos.DrawRay(pos, rayDirection * _guardRayDistance);
        }
    }
}