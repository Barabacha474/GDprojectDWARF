using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveScript : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    [SerializeField] private float _explosion_force = 1000f;
    [SerializeField] private float _explosion_radius = 10f;
    [SerializeField] private int maxDamage = 50;
    [SerializeField] private int minDamage = 5;
    [SerializeField] private int _max_number_of_hits = 20;
    [SerializeField] private LayerMask _hit_layer;
    [SerializeField] private LayerMask _damage_layer;
    [SerializeField] private LayerMask _obstacle_layer;
    [SerializeField] private GameObject _explosion_effect;
    [SerializeField] private float _explosion_effect_position_height = 0.5f;
    [SerializeField] private bool destroy_after_explosion = true;
    private Collider[] _hits;
    private bool _exploded = false;
    
    void Start()
    {
        _hits = new Collider[_max_number_of_hits];
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("Explosion!");
            Explode();
        }
    }

    public void Explode()
    {
        if (!_exploded)
        {
            _exploded = true;

            int number_of_hits =
                Physics.OverlapSphereNonAlloc(transform.position, _explosion_radius, _hits, _hit_layer);

            for (int i = 0; i < number_of_hits; i++)
            {
                float distance = Vector3.Distance(_hits[i].transform.position, transform.position);

                //Debug.Log(_hits[i].transform.ToString());

                Instantiate(_explosion_effect, transform.position + transform.up * _explosion_effect_position_height,
                    transform.rotation);

                if (!Physics.Raycast(transform.position,
                        (_hits[i].transform.position - transform.position).normalized, distance, _obstacle_layer.value))
                {
                    Rigidbody rb = _hits[i].GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(_explosion_force, transform.position, _explosion_radius);
                    }

                    if ( _damage_layer ==(_damage_layer | (1 << _hits[i].gameObject.layer)))
                    {
                        Debug.Log("Hru");
                        Enemy enemyTarget = _hits[i].GetComponent<Enemy>();
                        if (enemyTarget != null)
                        {
                            int damage = (int)Mathf.Lerp(minDamage, maxDamage, distance / _explosion_radius);
                            enemyTarget.TakeDamage(damage);
                        }

                        PlayerCharacter player = _hits[i].GetComponent<PlayerCharacter>();
                        if (player != null)
                        {
                            int damage = (int)Mathf.Lerp(minDamage, maxDamage, distance / _explosion_radius);
                            player.Hurt(damage);
                        }
                    }
                }
            }

            if (destroy_after_explosion)
            {
                Destroy(gameObject);
            }
        }
    }

    public bool isExploded()
    {
        return _exploded;
    }
}
