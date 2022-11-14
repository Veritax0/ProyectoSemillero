using System;
using System.Collections;
using Enemies.BladeEnemy;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.GunEnemy
{
    public class GunEnemyAttackState : MonoBehaviour, IEnemyState
    {
        private NavMeshAgent _agent;
        private GunEnemyController _context;
        private Transform pointer;
        private Transform bulletGen;
        private GameObject bullet;
        private bool _isAim;
        public float aimTime;
        public void SetContext(EnemyController context)
        {
            try
            {
                _context = (GunEnemyController) context;
                _agent = _context.Agent;
                pointer = _context.pointer;
                bulletGen = _context.bulletGen;
                bullet = _context.bullet;
                
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
            yield return new WaitForSeconds(aimTime);
            Shoot();
            _context.ChangeState(_context.GuardState);
            _isAim = false;
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
    }
}