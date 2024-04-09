using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CharacterKeyBoardInputMovement : MonoBehaviour
{

    [SerializeField] private SurfaceMovement _surfaceMovement;
    [SerializeField] private ProjectileThrower _projectileThrower;
    private Vector3 _current_direction;
    [SerializeField] public float sensitivityHor = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _current_direction = transform.forward;
        if (_surfaceMovement == null)
        {
            throw new Exception("Surface movement is null!");
        }
        if (_projectileThrower == null)
        {
            throw new Exception("Projectile thrower is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalRot = Input.GetAxis("Mouse X") * sensitivityHor;
        transform.Rotate(0, horizontalRot, 0);
        _current_direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            _current_direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _current_direction += -transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _current_direction += transform.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _current_direction += -transform.right;
        }
        if (_current_direction != Vector3.zero)
        {
            _surfaceMovement.Move(_current_direction);    
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _surfaceMovement.Jump();
        }
        
        if (Input.GetKey(KeyCode.G))
        {
            _projectileThrower.Throw();
        }
    }
}
