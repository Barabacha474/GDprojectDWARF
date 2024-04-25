using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private float burnDamage = 15f;
    private float _tempHealth;
    private float _burningTime;
    private int _health;

    void Start()
    {
        _health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Burn();
    }

    override
        public void TakeDamage(int damage)
    {
        Destroy(gameObject);
        if (_health > 0)
        {
            _health -= damage;
            //Debug.Log($"Health: {_health}");
        }

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    override
        public void Ignite()
    {
        _burningTime = 0.2f;
    }

    private void Burn()
    {
        if (_burningTime <= 0)
            return;
        _burningTime -= Time.deltaTime;
        _tempHealth += burnDamage * Time.deltaTime;
        if (_tempHealth > 1)
        {
            TakeDamage((int)_tempHealth);
            _tempHealth -= (int)_tempHealth;
        }
    }
}
