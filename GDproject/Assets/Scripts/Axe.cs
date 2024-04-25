using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("BONK");
            enemy.TakeDamage(damage);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
