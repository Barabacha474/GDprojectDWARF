using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshBehaviour : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform player_transform;
    [SerializeField] private float range_of_detection = 20f;
    [SerializeField] private float range_of_attack = 5f;
    private float _distance_to_player;
    
    [SerializeField] private ProjectileThrower _projectileThrower;
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null)
        {
            throw new Exception("NavMeshAgent component cannot be null!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _distance_to_player = (transform.position - player_transform.position).magnitude;
        if (_distance_to_player <= range_of_detection)
        {
            _navMeshAgent.destination = player_transform.position;
            transform.LookAt(player_transform);
        }

        if (_distance_to_player <= range_of_attack)
        {
            Attack();
        }
    }
    
    public void Attack()
    {
        _projectileThrower.Throw();
    }
}
