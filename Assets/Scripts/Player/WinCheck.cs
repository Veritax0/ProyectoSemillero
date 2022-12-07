using GUI_;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WinCheck : MonoBehaviour
{
    public GameObject objetivo;
    public GameObject rutaEscape;
    private Boolean escape;
    private PlayerMovement _movement;
    private Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        rutaEscape.SetActive(false);
        escape = false;
        _movement = GetComponent<PlayerMovement>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            Debug.Log("objetivo");
            HudController.GetInstance().Stole();
            ObjectiveComplete();
        }
        if (collision.gameObject.CompareTag("RutaEscape") && escape == true)
        {
            Debug.Log("Victoria");
            Win();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            HudController.GetInstance().NotStole();
        }
    }

    private void ObjectiveComplete()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            objetivo.SetActive(false);
            rutaEscape.SetActive(true);
            Debug.Log("Archivos robados");
            escape = true;
            HudController.GetInstance().NotStole();
        }
    }

    private void Win()
    {
        HudController.GetInstance().Victory();
        _movement.SetDied();
        _movement.enabled = false;
        Destroy(_rb);
    }

    
}
