using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : Enemy
{
    [SerializeField] private int health = 10; 
    private ExplosiveScript explosive;
    
    // Start is called before the first frame update
    void Start()
    {
        explosive = GetComponent<ExplosiveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log(health);
        health -= damage;
        if (health < 0)
        {
            explosive.Explode();
        }
        Debug.Log(health);
    }

    public override void Ignite()
    {
        explosive.Explode();
    }
}
