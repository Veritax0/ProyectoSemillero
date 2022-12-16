using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen;
    public GameObject eje;
    public float angles;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        angles = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(angles);
    }
    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("puerta");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isOpen)
                {
                    Vector3 rotacion = new Vector3(0, angles - 270, 0);
                    eje.transform.Rotate(rotacion);
                    isOpen = false;
                }
                else
                {
                    Vector3 rotacion = new Vector3(0, angles + 270, 0);
                    eje.transform.Rotate(rotacion);
                    isOpen = true;
                }
            }
        }
    }
}
