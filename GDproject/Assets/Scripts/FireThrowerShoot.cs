using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrowerShoot : Projectile
{
    private Rigidbody _rigidbody;
    [SerializeField] private GameObject fire;
    public float speed;
    public int damage ;
    public string enemyTag = "enemy";
    private int _cost = 20;
    private float _fireThrowTime = 0.05f;
    private float _currentTimeToThrow;
    private int _maxHeightDistance = 15;
    private int _impulse = 20;
    
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
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentTimeToThrow = _fireThrowTime;
        Destroy(gameObject, 10);
    }

    protected  void Launch()
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
        if (Physics.Raycast(ray, out hit, _maxHeightDistance))
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
        if (collision.collider.CompareTag(enemyTag))
        {
            collision.collider.GetComponent<Enemy>().TakeDamage(damage);
        }
        SelfDestroy();
    }
    
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}