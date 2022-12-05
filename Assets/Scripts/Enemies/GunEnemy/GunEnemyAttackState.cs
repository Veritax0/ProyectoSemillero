using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.GunEnemy
{
    public class GunEnemyAttackState : MonoBehaviour, IEnemyState
    {
        private NavMeshAgent _agent;
        private GunEnemyController _context;
        private Transform _pointer;
        private Transform _bulletGen;
        private GameObject _bullet;
        private bool _isAim;
        private float _aimTime;
        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (GunEnemyController) context;
                _agent = _context.Agent;
                _pointer = _context.pointer;
                _bulletGen = _context.bulletGen;
                _bullet = _context.bullet;
                _aimTime = _context.aimTime;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
        public void Execute()
        {
            if (!_isAim)
            {
                StartCoroutine(AimAndShoot());
            }
        }
        
        private IEnumerator AimAndShoot()
        {
            _isAim = true;
            _agent.SetDestination(transform.position);
            _context.AudioEnemy.Idle();
            yield return new WaitForSeconds(_aimTime);
            
            Shoot();
            _context.AudioEnemy.Walk();
            _context.ChangeState(_context.GuardState);
            _isAim = false;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void Shoot()
        {
            GameObject bulletInstantiate = Instantiate(_bullet, _bulletGen);
            Vector3 dir = Vector3.Normalize(_pointer.position - _bulletGen.position);
            float force = 20;
            Rigidbody rb = bulletInstantiate.GetComponent<Rigidbody>();
            bulletInstantiate.transform.SetParent(null);
            rb.AddForce(dir * force,ForceMode.Impulse);
            Destroy(bulletInstantiate, 3);
            
            _context.AudioEnemy.AttackSound();
        }
    }
}