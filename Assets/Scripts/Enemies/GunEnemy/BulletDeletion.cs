using System;
using UnityEngine;

namespace Enemies.GunEnemy
{
    public class BulletDeletion : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collider");
            Destroy(this.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Collision");
            Destroy(this.gameObject);
        }
    }
}
