using GUI_;
using UnityEngine;

namespace Player
{
    public class WinCheck : MonoBehaviour
    {
        public GameObject objetivo;
        public GameObject rutaEscape;
        private bool _escape;
        private PlayerMovement _movement;
        private Rigidbody _rb;


        // Start is called before the first frame update
        void Start()
        {
            rutaEscape.SetActive(false);
            _escape = false;
            _movement = GetComponent<PlayerMovement>();
            _rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider collision)
        {
            if (collision.gameObject.CompareTag("Objective"))
            {
                Debug.Log("objetivo");
                GuiController.GetInstance().Stole();
                ObjectiveComplete();
            }
            if (collision.gameObject.CompareTag("RutaEscape") && _escape)
            {
                Debug.Log("Victoria");
                Win();
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.CompareTag("Objective"))
            {
                GuiController.GetInstance().NotStole();
            }
        }

        private void ObjectiveComplete()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                objetivo.SetActive(false);
                rutaEscape.SetActive(true);
                Debug.Log("Archivos robados");
                _escape = true;
                GuiController.GetInstance().NotStole();
            }
        }

        private void Win()
        {
            GuiController.GetInstance().Victory();
            _movement.SetDied();
            _movement.enabled = false;
            Destroy(_rb);
        }
    }
}
