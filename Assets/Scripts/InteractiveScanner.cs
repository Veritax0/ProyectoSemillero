using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("playerScanner"))
        {
            activeScanner = Instantiate(scanner, position);
        }
    }
}
