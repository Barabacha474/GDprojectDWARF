using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxMana = 100;
    [SerializeField] private float manaRegenSpeed = 20f;
    [SerializeField] private float burnDamage = 15f;
    private int _health;
    private int _mana;
    private float _tempMana;
    private float _tempHealth;
    private float _burningTime;
   
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        _health = maxHealth;
        _mana = maxMana;
    }

    private void Update()
    {
        Burn();
        ManaRegen();
    }

    private void ManaRegen()
    {
        if (_mana >= maxMana)
            return;
        _tempMana += manaRegenSpeed * Time.deltaTime;
        if (_tempMana < 1)
            return;
        _mana += (int) _tempMana;
        _tempMana -= (int) _tempMana;
        
        //Debug.Log(_mana);
    }

    public void Hurt(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            Debug.Log($"Health: {_health}");
        }
    }

    public int GetMana()
    {
        return _mana;
    }

    public void SpendMana(int value)
    {
        _mana -= value;
    }

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
            Hurt((int) _tempHealth);
            _tempHealth -= (int)_tempHealth;
        }
    }

}