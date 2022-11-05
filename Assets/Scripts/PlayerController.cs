using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Material died;
    private Collider _collider;
    private Renderer _rend;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        _rend.sharedMaterial = died;
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade"))
        {
            Die();
        }
    }
}
