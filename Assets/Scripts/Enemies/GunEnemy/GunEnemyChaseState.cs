using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.GunEnemy
{
    public class GunEnemyChaseState : MonoBehaviour, IEnemyState
    {
        private NavMeshAgent _agent;
        private GunEnemyController _context;
        
        private GameObject _objective;
        private float _aimDistance;
        private float _shootDistance;
        private float _distanceToObjective;
        private float _minDistanceToChangePoint;
        private bool _isSetDestination;
        private bool _isWaitGuard;

        private Vector3 _destination;
        
        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (GunEnemyController) context;
                _agent = _context.Agent;
                _objective = _context.objective;
                _aimDistance = _context.aimDistance;
                _shootDistance = _context.shootDistance;
                _minDistanceToChangePoint = _context.minDistanceToChangePoint;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
        public void Execute()
        {
            Vector3 pos = transform.position;
            _destination = _agent.destination;
            if (!_isWaitGuard && Math.Abs(pos.x - _destination.x) <= _minDistanceToChangePoint
                && Math.Abs(pos.z - _destination.z)  <= _minDistanceToChangePoint)
            {
                StartCoroutine(WaitAndGuard());
            }
            Vector3 direction = Vector3.Normalize(_objective.transform.position - pos);
            _context.IsHit =  Physics.SphereCast(pos, transform.lossyScale.x, 
                direction, out _context.Hit, _aimDistance);
            if (_context.IsHit)
            {
                if (!_context.Hit.transform.gameObject.CompareTag("Player"))
                {
                    _context.ChangeState(_context.GuardState);
                    return;
                }
                if(!_isSetDestination) StartCoroutine(SetDestinationToPlayer());
            }
            else 
            {
                _context.ChangeState(_context.GuardState);
            }
        }
        
        private IEnumerator SetDestinationToPlayer()
        {
            _isSetDestination = true;
            Vector3 objPos = _objective.transform.position;
            _agent.SetDestination(objPos);
            yield return new WaitForSeconds(0.5f);
            _distanceToObjective = Vector3.Distance(transform.position,objPos);
            _context.ChangeState(_distanceToObjective <= _shootDistance ? 
                _context.AttackState : _context.ChaseState);
            _isSetDestination = false;
        }

        private IEnumerator WaitAndGuard()
        {
            _isWaitGuard = true;
            yield return new WaitForSeconds(0.5f);
            _context.ChangeState(_context.GuardState);
            _isWaitGuard = false;
        }
    }
}