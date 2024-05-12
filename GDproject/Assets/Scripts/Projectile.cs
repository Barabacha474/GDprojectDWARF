using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public abstract class Projectile : MonoBehaviour
{

    public abstract int GetCost();
    public abstract int GetImpulse();
    
}
