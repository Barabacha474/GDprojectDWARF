using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Projectile
{
    
    private Rigidbody _rigidbody;
    [SerializeField] private float throwForce;
    private int _cost = 20;
    private int _impulse = 1;
    [SerializeField] private ExplosiveScript explosiveScript;

    [SerializeField] private float _delay = 1f;

    private float _current_delay;
    // Start is called before the first frame update
    void Start()
    {
        if (_delay < 0)
        {
            throw new Exception("Delay is lesser than zero!");
        }

        _current_delay = _delay;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce( transform.forward * throwForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        _current_delay -= Time.deltaTime;
        if (_current_delay <= 0 && !explosiveScript.isExploded())
        {
            Debug.Log("Time to explode!");
            explosiveScript.Explode();
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
