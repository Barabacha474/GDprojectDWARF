using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrowerShoot : Projectile
{
    private Rigidbody _rigidbody;
    [SerializeField] private GameObject fire;
    [SerializeField] private float speed;
    [SerializeField] private int damage ;
    [SerializeField] private LayerMask hit_layer;
    [SerializeField] private int cost = 20;
    [SerializeField] private float _fireThrowTime = 0.05f;
    private float _currentTimeToThrow;
    [SerializeField] private int _maxHeightDistance = 15;
    [SerializeField] private int _impulse = 20;
    [SerializeField] private ExplosiveScript explosiveScript;

    override 
        public int GetCost()
    {
        return cost;
    }
    override 
        public int GetImpulse()
    {
        return _impulse;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentTimeToThrow = _fireThrowTime;
        Destroy(gameObject, 10);
    }

    private void Launch()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
        FireThrow();
        
    }

    private void FireThrow()
    {
        _currentTimeToThrow -= Time.deltaTime;
        if(_currentTimeToThrow > 0)
            return;
        _currentTimeToThrow = _fireThrowTime;
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _maxHeightDistance, hit_layer))
        {
            Instantiate(fire, hit.point, transform.rotation);
        }
    }


    void Update()
    {
        Launch();
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
}