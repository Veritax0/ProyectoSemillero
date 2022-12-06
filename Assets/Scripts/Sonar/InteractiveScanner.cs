using UnityEngine;

namespace Sonar
{
    public class InteractiveScanner : MonoBehaviour
    {
        public ScannerControllerEnemy scanner;
        public Transform position;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("playerScanner"))
            {
                Debug.Log("colision");
                Instantiate(scanner, position);
            }
        }
    }
}
