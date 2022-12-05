using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.BladeEnemy
{
    public class BladeEnemyAttackState : MonoBehaviour, IEnemyState
    {
        private BladeEnemyController _context;
        private NavMeshAgent _agent;
        private bool _attacking;
        private GameObject _blade;
        private Animator _animator;
        private static readonly int Attack1 = Animator.StringToHash("attack");
        private const float AttackTime = 2;

        public void Execute()
        {
            if (!_attacking) StartCoroutine(Attack());
        }
        
        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (BladeEnemyController) context;
                _agent = _context.Agent;
                _blade = _context.Blade;
                _blade.SetActive(false);
                _animator = _context.Animator;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
        
        private IEnumerator Attack()
        {
            _attacking = true;
            _blade.SetActive(true);
            _agent.SetDestination(transform.position);
            _context.AudioEnemy.Idle();
            
            _animator.SetBool(Attack1, true);
            _context.AudioEnemy.AttackSound();
            yield return new WaitForSeconds(AttackTime);
            
            _context.ChangeState(_context.GuardState);
            _animator.SetBool(Attack1, false);
            _blade.SetActive(false);
            _attacking = false;
        }
    }
}