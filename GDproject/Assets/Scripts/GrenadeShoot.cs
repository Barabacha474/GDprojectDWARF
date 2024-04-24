using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeShoot : Projectile
{
    private Rigidbody _rigidbody;
    [SerializeField] private float throwForce;
    [SerializeField] private int maxDamage = 50;
    [SerializeField] private int minDamage = 5;
    public string enemyTag = "enemy";
    private int _cost = 20;
    public int explosionRadius;
    private int _impulse = 1;
    private float _explosionTime = 3;
    private float _timeToExplode;
    private void Start()
    {
        _timeToExplode = _explosionTime;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce( transform.forward * throwForce, ForceMode.Impulse);
    }
    
    private void Update()
    {
        _timeToExplode -= Time.deltaTime;
        if (_timeToExplode <= 0)
        {
            DealExplodeDamage();
            PushByExplosion();
            Debug.Log("Boom");
            Destroy(gameObject);
        }
    }

    private void DealExplodeDamage()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            if (!hit.CompareTag(enemyTag))
                continue;
            
            RaycastHit hitInfo;
            bool hasLineOfSight = Physics.Raycast(transform.position, hit.transform.position - transform.position, out hitInfo);
            if (hasLineOfSight && hitInfo.collider.CompareTag(enemyTag))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                hitInfo.collider.GetComponent<Enemy>().TakeDamage((int)Mathf.Lerp(minDamage, maxDamage, distance / explosionRadius));
            }
        }
    }

    private void PushByExplosion()
    {
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
