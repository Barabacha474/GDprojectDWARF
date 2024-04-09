using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField] private ExplosiveScript _explosiveScript;

    [SerializeField] private float _delay = 1f;

    private float _current_delay = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (_delay < 0)
        {
            throw new Exception("Delay is lesser than zero!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        _current_delay += Time.deltaTime;
        if (_current_delay >= _delay && !_explosiveScript.isExploded())
        {
            Debug.Log("Time to explode!");
            _explosiveScript.Explode();
        }
    }
}
