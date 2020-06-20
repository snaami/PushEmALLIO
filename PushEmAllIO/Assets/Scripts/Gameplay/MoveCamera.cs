using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Vector3 _offset;

    private void OnEnable()
    {
        if (_target == null)
        {
            Debug.LogError("Укажите цель для слежения в компоненте MoveCamera");
            return;
        }

        _offset = transform.position - _target.position;
    }

    private void Update()
    {
        if (_target != null)
            transform.position = _target.position + _offset;
    }
}
