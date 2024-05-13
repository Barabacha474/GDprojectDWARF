using System;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMovement : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float mass = 2f;
    private Vector3 _currentGravity;

    [Header("Movement settings")] 
    private float _movement_speed;
    [SerializeField] private float walk_speed = 6.0f;   
    [SerializeField] private float run_speed = 12f;
    [SerializeField] private float _jump_force = 15f;
    [SerializeField] private float _dash_force = 100f;
    [SerializeField] private float max_angle = 45f;
    [SerializeField] private float _player_height = 2f;
    [SerializeField] private LayerMask _ground_lm;
    private Vector3 _normal;
    private Vector3 _offset;
    private bool _grounded;
    private bool _command_dashing;
    private bool _command_running;
    private bool _command_move;
    private MovingState _movingState;
    private Dictionary<String, ContactPoint[]> CurrentContactPointsMap = new Dictionary<string, ContactPoint[]>();

    enum MovingState
    {
        STANDING,
        WALKING,
        RUNNING,
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
        _offset = Vector3.zero;
        _movement_speed = walk_speed;
    }
    
    void FixedUpdate()
    {
        ObeyGravity();
    }

    private void Update()
    {
    }

    public void setRunningBool(bool setBool)
    {
        _command_running = setBool;
    }
    private void setState()
    {
        if (_grounded && _command_move)
        {
            if (_command_running)
            {
                _movement_speed = run_speed;
                _movingState = MovingState.RUNNING;
            }
            else
            {
                _movement_speed = walk_speed;
                _movingState = MovingState.WALKING;
            }
        }
        else if (_grounded)
        {
            _movingState = MovingState.STANDING;
        }
        else
        {
            _movingState = MovingState.AIR;
        }
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
        _command_move = true;
        setState();
        CalculateNormal(direction);
        
        _offset = Vector3.ProjectOnPlane(direction, _normal).normalized;
        FixDirection();
        
        _rigidbody.AddForce(_offset * (_movement_speed * 3000f * Time.deltaTime), ForceMode.Force);
        
        LimitSpeed();
    }

    public void Dash(Vector3 direction)
    {
        _offset = direction;
        FixDirection();
        _rigidbody.AddForce(_offset.normalized * _dash_force, ForceMode.Impulse);
    }

    private void FixDirection()
    {
        _offset = new Vector3(-_offset.z, _offset.y, _offset.x);
    }

    private void LimitSpeed()
    {
        Vector3 speed;
        if (_grounded)
        {
            speed = _rigidbody.velocity;
        }
        else
        {
            speed = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        }

        if (speed.magnitude > _movement_speed)
        {
            if (_grounded)
            {
                _rigidbody.velocity = speed.normalized * _movement_speed;
            }
            else
            {
                var y = _rigidbody.velocity.y;
                var new_speed = speed.normalized * _movement_speed;
                new_speed.y = y;
                _rigidbody.velocity = new_speed;
            }
        }
    }

    public void DoNotMove()
    {
        _command_move = false;
        setState();
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
            if (CurrentContactPointsMap.ContainsKey(other.gameObject.name)) {
                CurrentContactPointsMap.Remove(other.gameObject.name);
            }
            CurrentContactPointsMap.Add(other.gameObject.name, other.contacts);
            
            _grounded = CheckGrounded();
            setState();
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
         
            _grounded = CheckGrounded();
            setState();
        }
    }

    void CalculateNormal(Vector3 direction)
    {
        if (CurrentContactPointsMap.Count == 0)
        {
            _normal = Vector3.up;
            return;
        }   
        
        _normal = Vector3.zero;
        float max_angle = 0f;
        foreach (var contact in CurrentContactPointsMap)
        {
            if (validAngle(Vector3.Angle(Vector3.up, contact.Value[0].normal)))
            {
                float cur_angle = Vector3.Angle(
                    new Vector3(contact.Value[0].normal.x, 0, contact.Value[0].normal.z),
                    direction);
                if (cur_angle > max_angle)
                {
                    _normal = contact.Value[0].normal;
                    max_angle = cur_angle;
                } 
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
}
