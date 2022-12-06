using UnityEngine;

namespace Sonar
{
    public class InteractiveScanner : MonoBehaviour
    {
        public GameObject scanner;
        public Transform position;
        private GameObject activeScanner;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("playerScanner"))
            {
                Debug.Log("colision");
                activeScanner = Instantiate(scanner, position);
            }
        }
    }
}
