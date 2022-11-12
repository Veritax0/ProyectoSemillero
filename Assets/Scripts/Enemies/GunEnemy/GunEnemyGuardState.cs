using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (GunEnemyController) context;
                _points = _context.points;
                _minDistanceToChangePoint = _context.minDistanceToChangePoint;
                _agent = _context.Agent;
                _animator = _context.Animator;
                _aimAroundCor = StartCoroutine(voidCor());
                _aimWalkCor = StartCoroutine(voidCor());
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
            if (_context.IsHit)
            {
                if (_context.Hit.transform.gameObject.CompareTag("Player")){
                    StopCoroutine(_aimWalkCor);
                    StopCoroutine(_aimAroundCor);
                    _context.ChangeState(_context.ChaseState);
                    _isAimWalk = false;
                    _changeDestination = false;
                    return;
                }
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
            yield return new WaitForFixedUpdate();
            _animator.SetInteger(Aim1, 1);
            yield return new WaitForSeconds(3.7f);
            _animator.SetInteger(Aim1, 0);
            _currentPosition = _currentPosition < _points.Count - 1 ? _currentPosition + 1 : 0;
            _agent.SetDestination(_points[_currentPosition].position);
            _changeDestination = false;
            _isAimWalk = false;
        }

        private IEnumerator AimWalking()
        {
            _isAimWalk = true;
            _animator.SetInteger(Aim1, 2);
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            _animator.SetInteger(Aim1, 0);
            _isAimWalk = false;
        }

        private IEnumerator voidCor()
        {
            yield return new WaitForEndOfFrame();
        } 
    }
}