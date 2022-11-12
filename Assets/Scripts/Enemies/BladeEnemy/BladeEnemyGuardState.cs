using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies.BladeEnemy
{
    public class BladeEnemyGuardState : MonoBehaviour, IEnemyState
    {
        private int _currentPosition;
        private float _distanceToNextPoint;
        private bool _changeDestination;
        private BladeEnemyController _context;
        private float _minDistanceToChangePoint;
        private List<Transform> _points;
        private NavMeshAgent _agent;

        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (BladeEnemyController) context;
                _minDistanceToChangePoint = _context.minDistanceToChangePoint;
                _points = _context.points;
                _agent = _context.Agent;
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
                    _context.ChangeState(_context.ChaseState);
                    return;
                }
            }
            _distanceToNextPoint = Vector3.Distance(transform.position, _points[_currentPosition].position);
            if (!_changeDestination && _distanceToNextPoint < _minDistanceToChangePoint)
            {
                StartCoroutine(WaitAndChange());
            }
        }
        
        private IEnumerator WaitAndChange() //Guard
        {
            _changeDestination = true;
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            _currentPosition = _currentPosition < _points.Count - 1 ? _currentPosition + 1 : 0;
            _agent.SetDestination(_points[_currentPosition].position);
            _changeDestination = false;
        }
    }
}