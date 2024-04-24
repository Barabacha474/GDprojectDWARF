using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class SurfaceMovement : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float mass = 2f;
    private Vector3 _currentGravity;
    
    [Header("Movement settings")]
    [SerializeField] private float walk_speed = 6.0f;
    [SerializeField] private float _jump_force = 20f;
    [SerializeField] private float max_angle = 45f;
    [SerializeField] private float _player_height = 2f;
    [SerializeField] private LayerMask _ground_lm;
    private Vector3 _normal;
    private Vector3 _offset;
    private bool _grounded;
    private float _angle;
    private MovingState _movingState;
    private Dictionary<String, ContactPoint[]> CurrentContactPointsMap = new Dictionary<string, ContactPoint[]>();
    
    [Header("Debug")]
    [SerializeField] public bool draw_vectors;
    [SerializeField] public bool show_debug_log;

    enum MovingState
    {
        STANDING,
        WALKING,
        RUNNING,
        CROUCHING,
        AIR
    }
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (max_angle < 0f || max_angle > 90f)
        {
            throw new Exception("Incorrect max angle! It should be between 0 and 90");
        }

        _rigidbody.useGravity = false;
        _normal = Vector3.up;
    }
    
    void FixedUpdate()
    {
        ObeyGravity();
        //_grounded = CheckGrounded();
    }

    private void Update()
    {
    }
    
    void ObeyGravity()
    {
        if (_grounded)
        {
            _currentGravity = -_normal * Physics.gravity.magnitude;
        }
        else
        {
            _currentGravity = Physics.gravity;
        }
        _rigidbody.AddForce(_currentGravity * mass, ForceMode.Force);
    }

    private bool validAngle(float angle)
    {
        return angle >= 0 && angle < max_angle;
    }

    public void Move(Vector3 direction)
    {
        _offset = Vector3.ProjectOnPlane(direction, _normal).normalized;
        if (!validAngle(_angle))
        {
            _offset.y = 0;
        }
        _rigidbody.MovePosition(_rigidbody.position + _offset * (walk_speed * Time.deltaTime));
    }

    public void Jump()
    {
        if (_grounded)
        {
            _grounded = false;
            _rigidbody.AddForce(transform.up * _jump_force, ForceMode.Impulse);    
        }
    }
    
    void OnCollisionEnter (Collision other) {
        if (_ground_lm == (_ground_lm | (1 << other.gameObject.layer)))
        {
            CurrentContactPointsMap.Add(other.gameObject.name, other.contacts);
            
            CalculateNormal();

            if (show_debug_log)
            {
                foreach (var contact in CurrentContactPointsMap) {
                    Debug.Log((contact.Value[0].point - transform.position) + " " + contact.Value[0].point + " " 
                              + Vector3.Angle(Vector3.up, contact.Value[0].normal) + " " + contact.Key);
                }
                Debug.Log("_______________________________");
            }
            
            _grounded = CheckGrounded();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (_ground_lm == (_ground_lm | (1 << other.gameObject.layer)))
        {
            CurrentContactPointsMap[other.gameObject.name] = other.contacts;
        }
    }

    void OnCollisionExit (Collision other) {
        if (_ground_lm == (_ground_lm | (1 << other.gameObject.layer)))
        {
            CurrentContactPointsMap.Remove(other.gameObject.name);
            
            CalculateNormal();

            if (show_debug_log)
            {
                foreach (var contact in CurrentContactPointsMap) {
                    Debug.Log((contact.Value[0].point - transform.position) + " " + contact.Value[0].point + " " 
                              + Vector3.Angle(Vector3.up, contact.Value[0].normal) + " " + contact.Key);
                }
                Debug.Log("_______________________________");
            }
         
            _grounded = CheckGrounded();
        }
    }

    void CalculateNormal()
    {
        if (CurrentContactPointsMap.Count == 0)
        {
            _normal = Vector3.up;
            return;
        }   
        
        _normal = Vector3.zero;
        foreach (var contact in CurrentContactPointsMap)
        {
            if (validAngle(Vector3.Angle(Vector3.up, contact.Value[0].normal)))
            {
                _normal += contact.Value[0].normal;
            }
        }
        _normal = _normal.normalized;
    }

    bool CheckGrounded()
    {
        foreach (var contact in CurrentContactPointsMap)
        {
            if (validAngle(Vector3.Angle(Vector3.up, contact.Value[0].normal)))
            {
                return true;
            }
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
        if (draw_vectors)
        {
            foreach (var contact in CurrentContactPointsMap)
            {
                Gizmos.color = Color.blue; 
                Gizmos.DrawLine(transform.position, contact.Value[0].point); 
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + _normal * 2);
            //drawing forward projected line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.ProjectOnPlane(transform.forward, _normal).normalized * 3);
            //drawing forward and up line
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
        }
    }
}
