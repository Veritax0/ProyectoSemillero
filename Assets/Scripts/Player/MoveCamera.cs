using UnityEngine;

namespace Player
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;

        // Update is called once per frame
        void Update()
        {
            transform.position = cameraPosition.position;
        }
    }
}
