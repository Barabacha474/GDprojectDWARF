using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrower : MonoBehaviour
{
    [SerializeField] private Transform _camera_transform;
    [SerializeField] private Transform _projectile_spawn_transform;
    [SerializeField] private GameObject _projectile;

    [SerializeField] private float _throw_force;
    [SerializeField] private float _throw_upward_force;

    [SerializeField] private bool _able_to_throw = true;

    [SerializeField] private float _cooldown;
    [SerializeField] private int _projectiles_left = 10;

    private float _current_cooldown;
    // Start is called before the first frame update
    void Start()
    {
        if (_projectiles_left < 0)
        {
            throw new Exception("Projectiles left is lesser than zero!");
        }
        if (_cooldown < 0)
        {
            throw new Exception("Cooldown is lesser than zero!");
        }

        _current_cooldown = _cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        _current_cooldown += Time.deltaTime;
        if (_current_cooldown > _cooldown)
        {
            _current_cooldown = _cooldown;
            _able_to_throw = true;
        }
    }

    public bool AbleToThrow()
    {
        return _able_to_throw;
    }

    public void Throw()
    {
        if (_projectiles_left == 0)
        {
            _able_to_throw = false;
        }
        
        if (_able_to_throw)
        {
            _projectiles_left--;
            _able_to_throw = false;
            _current_cooldown = 0;
            
            GameObject _instantiated_projectile =
                        Instantiate(_projectile, _projectile_spawn_transform.position + _projectile_spawn_transform.forward * 1, _camera_transform.rotation);
            Rigidbody _projectile_rigidbody = _instantiated_projectile.GetComponent<Rigidbody>();
            if (_projectile_rigidbody != null)
            {
                Vector3 _force_vector = (_camera_transform.forward * _throw_force + transform.up * _throw_upward_force) * 0; 
                _projectile_rigidbody.AddForce(_force_vector, ForceMode.Impulse);
            }
        }
    }

    public int getProjectilesLeft()
    {
        return _projectiles_left;
    }
    
    public void setProjectilesLeft(int set)
    {
        _projectiles_left = set;
    }
}
