using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShoot : Projectile
{
    private Rigidbody _rigidbody;
    public float speed;
    public int damage;
    public string enemyTag = "enemy";
    private int _cost = 10;
    public int explosionRadius;
    private int _impulse = 20;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * speed;
        Destroy(gameObject, 10);
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(enemyTag))
        {
            collision.collider.GetComponent<Enemy>().TakeDamage(damage);
        }
        Explode();
        SelfDestroy();
    }
    
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (!hit.CompareTag(enemyTag))
                continue;
                // Check line of sight
            RaycastHit hitInfo;
            bool hasLineOfSight = Physics.Raycast(transform.position, hit.transform.position - transform.position, out hitInfo);
            if (hasLineOfSight && hitInfo.collider.CompareTag(enemyTag))
            { 
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            
        }
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
