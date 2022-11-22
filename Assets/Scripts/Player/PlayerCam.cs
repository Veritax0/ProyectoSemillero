using System;
using UnityEngine;

namespace Player
{
    public class PlayerCam : MonoBehaviour
    {
        public float sensX;
        public float sensY;
        [Range(0,70)]
        public float amplitudeY;

        public Transform orientation;

        private float _xRotation;
        private float _yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            //Get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Math.Clamp(_xRotation, -amplitudeY, amplitudeY);
            
            //Rotate cam and orientation
            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
    }
}