using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Material died;
    public Material alive;
    public bool isDead;
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
        if (isDead)
        {
            Die();
        }
        else
        {
            Live();
        }
    }

    private void Live()
    {
        _rend.sharedMaterial = alive;
        _collider.enabled = true;
    }

    public void Die()
    {
        _rend.sharedMaterial = died;
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Blade") || other.CompareTag("Bullet") )
        {
            isDead = true;
        }
    }
}
