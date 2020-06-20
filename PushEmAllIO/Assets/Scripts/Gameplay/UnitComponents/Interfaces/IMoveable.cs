using System;
using System.Collections;

internal interface IMoveable
{
    void Init(float speed, float speedRotation);
    float SpeedCurrent { get; }
    bool IsGrounded();
}

