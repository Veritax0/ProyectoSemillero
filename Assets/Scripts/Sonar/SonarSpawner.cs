using System.Collections;
using UnityEngine;

namespace Sonar
{
    public class SonarSpawner : MonoBehaviour
    {
        public GameObject scanner;
        public Transform player;
        [Range(0.3f,2)]
        public float sonarTimeInterval;
    
        private GameObject _activeScanner;
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
            _activeScanner = Instantiate(scanner, player);
            yield return new WaitForSeconds(sonarTimeInterval);
            _isSpawn = false;
        }
    }
}
