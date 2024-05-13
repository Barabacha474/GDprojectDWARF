using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxMana = 100;
    [SerializeField] private float manaRegenSpeed = 20f;
    [SerializeField] private float burnDamage = 15f;
    [SerializeField] private Camera camera;
    private int _health;
    private int _mana;
    private float _tempMana;
    private float _tempHealth;
    private float _burningTime;
    
    [Header("UI")] 
    [SerializeField] private TMP_Text HP_text;
    [SerializeField] private TMP_Text AMMO_LEFT_text;
    [SerializeField] private TMP_Text AMMO_IN_CHAMBER_text;
    [SerializeField] private TMP_Text GRENADE_LEFT_text;

    [Header("Links")] 
    [SerializeField] private GunScript gun;

    [SerializeField] private ProjectileThrower granade;
   
    
    private void Start()
    {
        _health = maxHealth;
        _mana = maxMana;
    }

    private void Update()
    {
        Burn();
        ManaRegen();
        ShowUI();
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
    }

    private void ShowUI()
    {
        HP_text.SetText("HP LEFT: " + _health);
        AMMO_LEFT_text.SetText("AMMO LEFT: " + gun.get_remain_ammo());
        AMMO_IN_CHAMBER_text.SetText("AMMO IN CHAMBER: " + gun.get_current_capacity());
        GRENADE_LEFT_text.SetText("GRENADES LEFT: " + granade.getProjectilesLeft());
    }


    public void Hurt(int damage)
    {
        if (_health > 0)
        {
            _health -= damage;
        }

        if (_health <= 0)
        {
            SceneManager.LoadScene("Scenes/Levels");
            //camera.transform.parent = null;
            //Destroy(gameObject);
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