using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterKeyBoardInputMovement : MonoBehaviour
{
    [Header("External Scripts")]
    [SerializeField] private SurfaceMovement _surfaceMovement;
    [SerializeField] private ProjectileThrower _projectileThrower;
    
    [Header("Animators")]
    [SerializeField] private Animator _left_leg_animator;
    [SerializeField] private Animator _right_leg_animator;
    [SerializeField] private List<Activation_struct> _tools_and_weapons = new List<Activation_struct>();
    [SerializeField] private GameObject gun;
    
    [Header("Other")]
    private Vector3 _current_direction;
    [SerializeField] public float sensitivityHor = 3.0f;


    [Serializable]
    struct Activation_struct
    {
        public GameObject gameObject;
        public KeyCode keyCode;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
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
        
        _surfaceMovement.setRunningBool(Input.GetKey(KeyCode.LeftShift));
        
        if (_current_direction != Vector3.zero)
        {
            _surfaceMovement.Move(_current_direction);
                
            _left_leg_animator.SetBool("walk", true);
            _right_leg_animator.SetBool("walk", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _left_leg_animator.speed = 2;
                _right_leg_animator.speed = 2;
            }
            else
            {
                _left_leg_animator.speed = 1;
                _right_leg_animator.speed = 1;
            }
        }
        else
        {
            _surfaceMovement.DoNotMove();
            
            _left_leg_animator.SetBool("walk", false);
            _right_leg_animator.SetBool("walk", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _surfaceMovement.Jump();
            _left_leg_animator.SetBool("walk", false);
            _right_leg_animator.SetBool("walk", false);
        }

        foreach (var tool_or_weapon in _tools_and_weapons)
        {
            if (Input.GetKey(tool_or_weapon.keyCode))
            {
                DeactivateGun();
                Invoke("ActivateGun", 0.5f);
                
                if (tool_or_weapon.keyCode == KeyCode.G)
                {
                    if (_projectileThrower.AbleToThrow())
                    {
                        _projectileThrower.Throw();
                        tool_or_weapon.gameObject.SetActive(true);
                    }
                }
                else
                {
                    tool_or_weapon.gameObject.SetActive(true);
                }
            }
        }
    }

    void DeactivateGun()
    {
        try
        {
            gun.GetComponent<GunScript>().Hide();
        }
        catch (Exception e)
        {
            gun.SetActive(false);
        }
    }

    void ActivateGun()
    {
        gun.SetActive(true);
    }
}
