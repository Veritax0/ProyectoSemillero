using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character;
    public float velocidad;
    public GameObject scanner;
    public Transform player;
    private GameObject activeScanner;
    public float rotationSpeed;
    //public float xRotacion;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        character = GetComponent<CharacterController>();
    }


    void Update()
    {
        Move();
        Scan();
        //CameraMove();
        RotarPersonaje();
    }

    public void Move()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 mov = new Vector3 (horizontal,0,vertical);
        character.Move(mov*velocidad*Time.deltaTime);
    }

    public void Scan()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activeScanner = Instantiate(scanner, player);
        }
    }

    public void CameraMove()    
    {
        float xRotacion = 0;
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        player.Rotate(Vector3.up *mouseX);
        xRotacion -= mouseY;
        xRotacion = Mathf.Clamp(xRotacion, 0, 180);
        transform.localRotation = Quaternion.Euler(xRotacion, 0, 0);
    }

    void RotarPersonaje()
    {
        float inputMouse = Input.GetAxis("Mouse X");

        Vector3 axis = Vector3.up;

        transform.Rotate(inputMouse * rotationSpeed * Time.deltaTime * axis);
    }
}
