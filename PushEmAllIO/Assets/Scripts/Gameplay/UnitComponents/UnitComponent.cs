using UnityEngine;
using System.Collections;

public abstract class UnitComponent : MonoBehaviour
{ 
    protected Unit _unit;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
