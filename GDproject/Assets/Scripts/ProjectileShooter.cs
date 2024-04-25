using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private List<Projectile> projectilesList;
    private Projectile _currentProjectile;
    private PlayerCharacter _player;
    private int _currentProjectileIndex = 0;
    [SerializeField] private Vector3 startShootPoint;
    
    private void Start()
    {
        _currentProjectile = projectilesList[0];
        _player = gameObject.GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        CheckForShooting();
        CheckForProjectileChange();
    }

    private void CheckForShooting()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        if (_currentProjectile.GetCost() > _player.GetMana())
        {
            return;
        }
        
        
         RaycastHit hitInfo;
         Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        bool hasPointToFocus = Physics.Raycast(ray, out hitInfo); 
        if(hasPointToFocus)
        {
            Vector3 shift = cameraTransform.right * startShootPoint.x +
                cameraTransform.up * startShootPoint.y + cameraTransform.forward * startShootPoint.z;
            Projectile newObject = Instantiate(_currentProjectile,  cameraTransform.position + shift, Quaternion.identity);
            newObject.transform.LookAt(hitInfo.point);
            _player.SpendMana(_currentProjectile.GetCost());
        }
        else
        {
            Instantiate(_currentProjectile, cameraTransform.position + cameraTransform.forward, cameraTransform.rotation);
            _player.SpendMana(_currentProjectile.GetCost());
        }
        gameObject.GetComponent<Rigidbody>().AddForce(-cameraTransform.forward * _currentProjectile.GetImpulse(), ForceMode.Impulse);
    }

    private void CheckForProjectileChange()
    {
        if (Input.GetAxis("Mouse ScrollWheel") == 0)
        {
            return;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _currentProjectileIndex = (_currentProjectileIndex + 1) % projectilesList.Count;
            _currentProjectile = projectilesList[_currentProjectileIndex];
            //Debug.Log(_currentProjectileIndex);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _currentProjectileIndex--;
            if (_currentProjectileIndex < 0)
                _currentProjectileIndex = projectilesList.Count - 1;
            _currentProjectile = projectilesList[_currentProjectileIndex];
            //Debug.Log(_currentProjectileIndex);
        }
    }

    public void AddProjectile(Projectile projectile)
    {
        if (!(projectilesList.Contains(projectile)))
        projectilesList.Add(projectile);
    }
}

