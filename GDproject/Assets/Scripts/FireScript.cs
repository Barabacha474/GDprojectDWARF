using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("FIRE!");
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Ignite();
            return;
        }

        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            Debug.Log("FIRE!");
            player.Ignite();
        }
        
    }
}