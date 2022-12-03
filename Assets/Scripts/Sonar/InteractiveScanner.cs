using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveScanner : MonoBehaviour
{
    public GameObject scanner;
    public Transform position;
    private GameObject newScanner;
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
            newScanner = Instantiate(scanner, position);
        }
    }
}
