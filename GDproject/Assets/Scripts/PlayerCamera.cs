using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityVert = 3.0f;
    public float minimumVert = -75.0f;
    public float maximumVert = 75.0f;
    private float _vertialRot = 0;
    
    void Update()
    {
        _vertialRot -= Input.GetAxis("Mouse Y") * sensitivityVert;
        _vertialRot = Mathf.Clamp(_vertialRot, minimumVert, maximumVert);
        float horizontalRot = transform.localEulerAngles.y;
        transform.localEulerAngles = new Vector3(_vertialRot, horizontalRot, 0);
    }
}
