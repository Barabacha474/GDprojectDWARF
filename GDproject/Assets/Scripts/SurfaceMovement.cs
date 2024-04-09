using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float walk_speed = 3.0f;
    [SerializeField] private float max_angle = 0f;
    [SerializeField] public bool draw_vectors;
    private Vector3 _normal;
    private Vector3 _previous_normal;
    private bool _grounded;
    [SerializeField] private float _jump_force = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (max_angle < 0f || max_angle > 90f)
        {
            throw new Exception("Incorrect max angle! It should be between 0 and 90");
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 offset = Project(direction.normalized);
        if (90 - Vector3.Angle(transform.up, offset) <= max_angle)
        {
            Debug.Log(Vector3.Angle(transform.up, offset));
            _rigidbody.MovePosition(_rigidbody.position + offset * walk_speed * Time.deltaTime);
        }
        else
        {
            _normal = _previous_normal;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        _previous_normal = _normal;
        _normal = other.contacts[0].normal;
        if (other.collider.CompareTag("Ground"))
        {
            _grounded = true;
        }
    }

    public Vector3 Project(Vector3 forward)
    {
        return forward - Vector3.Dot(forward, _normal) * _normal;
    }

    public void Jump()
    {
        if (_grounded)
        {
            _grounded = false;
            _rigidbody.AddForce(transform.up * _jump_force);    
        }
    }

    private void OnDrawGizmos()
    {
        if (draw_vectors)
        {
            //drawing normal line
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _normal * 2);
            //drawing forward projected line
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward) * 3);
            //drawing forward and up line
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
            Gizmos.DrawLine(transform.position, transform.position + transform.up * 2);
        }
    }
}
