using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxMana = 100;
    [SerializeField] private float manaRegenSpeed = 20f;
    private int _health;
    private int _mana;
    private float _tempMana;
    
    
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
        ManaRegen();
    }

    private void ManaRegen()
    {
        if (_mana >= maxMana)
            return;
        _tempMana += manaRegenSpeed * Time.deltaTime;
        if (_tempMana < 1)
            return;
        _tempMana -= 1;
        _mana += 1;
        Debug.Log(_mana);
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

}