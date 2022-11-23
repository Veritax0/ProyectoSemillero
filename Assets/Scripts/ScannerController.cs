using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ScannerController : MonoBehaviour
{
    public float speed;
    public float delayEndTime;
    public GameObject objective;
    public GameObject scanner;
    private GameObject activeScanner;
    // Start is called before the first frame update
    void Start()
    {
        endScanner();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectorMesh = this.transform.localScale;
        float growing = this.speed * Time.deltaTime;
        this.transform.localScale = new Vector3(vectorMesh.x + growing,vectorMesh.y + growing,vectorMesh.z + growing);
    }

    private void endScanner(){
        Destroy(this.gameObject, delayEndTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform pos = objective.transform;
        if (collision.gameObject.CompareTag(objective.tag))
        {
            Scan(pos);
        }
    }

    public void Scan(Transform obj)
    {
        activeScanner = Instantiate(scanner, obj);
    }
}
