using System;
using System.Collections;
using UnityEngine;

namespace Sonar
{
    public class SonarSpawner : MonoBehaviour
    {
        public GameObject scanner;
        [Range(0.3f,2)]
        public float sonarTimeInterval;
        private bool _isSpawn;

        public void Scan()
        {
            if (!_isSpawn)
            {
                StartCoroutine(SpawnSonar());
            }
        }

        private IEnumerator SpawnSonar()
        {
            _isSpawn = true;
            Instantiate(scanner, null);
            yield return new WaitForSeconds(sonarTimeInterval);
            _isSpawn = false;
        }
    }
}
