using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _force = 1f;

    private void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.AddForce(transform.up * 5f * _force);
        }
    }

    
}
