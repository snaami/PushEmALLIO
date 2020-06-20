using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cane : Weapon
{
    protected override void AddForce(Unit enemy)
    {
        var enemyRigidbody = enemy.GetComponent<Rigidbody>();
        var tempVectorForce = transform.forward;
        enemyRigidbody.AddForce(tempVectorForce * 150);
    }
}
