using System;
using UnityEngine;

namespace Enemies.GunEnemy
{
    public class BulletDeletion : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collider");
            if(!other.CompareTag("playerScanner"))Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision");
            Destroy(gameObject);
        }
    }
}
