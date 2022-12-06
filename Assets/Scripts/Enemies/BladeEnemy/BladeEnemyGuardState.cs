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
                _points = _context.Points;
                _agent = _context.Agent;
                
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
            Vector3 currentDestination = _points[_currentPosition].position;
            currentDestination.y = transform.position.y;
            _agent.SetDestination(currentDestination);

            if (_context.IsHit)
            {
                if (_context.Hit.transform.gameObject.CompareTag("Player")){
                    _context.ChangeState(_context.ChaseState);
                    return;
                }
            }
            _distanceToNextPoint = Vector3.Distance(transform.position, currentDestination);
            if (!_changeDestination && _distanceToNextPoint < _minDistanceToChangePoint)
            {
                StartCoroutine(WaitAndChange());
            }
        }
        
        private IEnumerator WaitAndChange() //Guard
        {
            _changeDestination = true;
            _context.AudioEnemy.Idle();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            
            _currentPosition = _currentPosition < _points.Count - 1 ? _currentPosition + 1 : 0;
            _agent.SetDestination(_points[_currentPosition].position);
            _changeDestination = false;
            
            _context.AudioEnemy.Walk();
        }
    }
}