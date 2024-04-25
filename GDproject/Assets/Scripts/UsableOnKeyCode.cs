using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableOnKeyCode : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            throw new Exception("Animator can't be null!");
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
