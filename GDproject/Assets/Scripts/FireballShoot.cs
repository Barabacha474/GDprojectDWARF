using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShoot : Projectile
{
    private Rigidbody _rigidbody;
    public float speed;
    public int damage;
    [SerializeField] private int _cost = 10;
    private int _impulse = 20;
    [SerializeField] private LayerMask hit_layer;
    [SerializeField] private ExplosiveScript explosiveScript;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * speed;
        Destroy(gameObject, 10);
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == hit_layer)
        {
            Enemy target = collision.collider.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        explosiveScript.Explode();
    }
    
    override 
        public int GetCost()
    {
        return _cost;
    }
    override 
        public int GetImpulse()
    {
        return _impulse;
    }

}
