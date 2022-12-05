using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BladeEnemy
{
    public class BladeEnemyChaseState: MonoBehaviour, IEnemyState
    {
        private GameObject _objective;
        private float _attackDistance;
    
        private NavMeshAgent _agent; //Todos
        private BladeEnemyController _context;

        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (BladeEnemyController) context;
                _agent = _context.Agent;
                _objective = _context.objective;
                _attackDistance = _context.attackDistance;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public void Execute()
        {
            if (_context.Hit.IsUnityNull())
            {
                _context.ChangeState(_context.GuardState);
            }

            switch (_context.IsHit)
            {
                case false:
                    _context.ChangeState(_context.GuardState);
                    return;
                case true when !_context.Hit.transform.gameObject.CompareTag("Player"):
                    _context.ChangeState(_context.GuardState);
                    return;
                case true:
                    Vector3 objPos = _objective.transform.position;
                    _agent.SetDestination(objPos);
                    _context.AudioEnemy.Walk();
                    
                    float objDistance = Vector3.Distance(transform.position, objPos);
                    if (objDistance < _attackDistance)
                    {
                        _context.ChangeState(_context.AttackState);
                    }

                    break;
            }
            
        }
    }
}