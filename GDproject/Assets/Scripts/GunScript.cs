using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private float firing_cooldown = 0.2f;
    [SerializeField] private float recoil = 20;
    private bool _ready_to_shoot = true;
    
    [SerializeField] private int capacity = 7;
    [SerializeField] private int ramain_ammo = 14;
    private int reset_ammo;
    [SerializeField] private float reload_per_bullet_cooldown = 1;
    [SerializeField] private ProjectileThrower projectileThrower;
    private int _current_capacity;
    private bool _reloading = false;
    private bool _charging_bullet = false;
    private bool _wait = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (ramain_ammo < 0)
        {
            throw new Exception("Ammo can't be lesser that zero!");
        }
        
        reset_ammo = ramain_ammo;
        
        if (capacity < 0)
        {
            throw new Exception("capacity can't be lesser that zero!");
        }

        GetComponent<ProjectileThrower>().setProjectilesLeft(capacity + ramain_ammo);
        
        _current_capacity = capacity;
        animator.SetBool("Deactivate", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && _ready_to_shoot)
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_reloading)
            {
                _reloading = true;
                animator.SetBool("Charging", true);
            }
            else
            {
                _reloading = false;
                animator.SetBool("Charging", false);
            }
        }

        if (ramain_ammo == 0)
        {
            _reloading = false;
            animator.SetBool("Charging", false);
        }

        if (_reloading)
        {
            if (_current_capacity < capacity && !_charging_bullet)
            {
                _charging_bullet = true;
                Invoke("LoadNextBullet", reload_per_bullet_cooldown);
            } else if (_current_capacity == capacity)
            {
                _reloading = false;
                animator.SetBool("Charging", false);
            }
        }
    }

    void Fire()
    {
        if (_current_capacity > 0)
        {
            projectileThrower.Throw();
            _ready_to_shoot = false;
            _current_capacity--;
            Invoke("ResetReadyToShoot", firing_cooldown);
            animator.SetBool("Firing", true); 
            playerRigidbody.AddForce(-cameraTransform.forward * recoil, ForceMode.Impulse);
        }
    }

    void ResetReadyToShoot()
    {
        _ready_to_shoot = true;
    }

    void LoadNextBullet()
    {
        if (ramain_ammo > 0)
        {
            ramain_ammo--;
            _current_capacity++; 
        }
        _charging_bullet = false; 
        animator.SetBool("Firing", false);
    }

    void StopFiring()
    {
        animator.SetBool("Firing", false);
    }

    public void Hide()
    {
        animator.SetBool("Deactivate", true);
        Invoke("Deactivate", 0.03f);
    } 
    
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public int get_current_capacity()
    {
        return _current_capacity;
    }

    public int get_remain_ammo()
    {
        return ramain_ammo;
    }
}
