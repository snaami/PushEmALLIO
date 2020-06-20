using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Объект стрельбы/удара.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PushObject : MonoBehaviour
{
    // Основные параметры движения объекта стрельбы.
    private Vector3 _endPosition;
    private Vector3 _beginPosition;
    private bool _isMoveForward;
    private bool _isMove;

    private float _speed;

    // Основные ссылки.
    private Unit _unit;
    private Rigidbody _rigidbody;
    private Collider _collider;

    private void Awake()
    {
        // Кешируем ссылки.
        _unit = GetComponentInParent<Unit>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        if (_unit == null)
        {
            Debug.LogError("Добавьте объект толкания в дочерние объекты персонажа.");
            return;
        }

        _beginPosition = transform.localPosition;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision col)
    {
        // Если обнаружен враг - толкаем его.
        var enemy = col.collider.GetComponent<Unit>();
        if (enemy != null && _collider.isTrigger == false)
            enemy.OnPushed(_unit);
    }

    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Задание движения объекту.
    /// </summary>
    private void Move()
    {
        if (_isMove == true)
        {
            if (_isMoveForward)
            {
                // Двигаем вперед.
                _rigidbody.velocity = transform.forward * _speed;
                if (_endPosition.z - transform.localPosition.z < 0.1f)
                {
                    _collider.isTrigger = true;
                    _isMoveForward = false;
                }
            }
            else
            {
                // Двигаем назад.
                _rigidbody.velocity = -transform.forward * _speed;
                if (transform.localPosition.z - _beginPosition.z < 0.1f)
                {
                    _collider.isTrigger = false;
                    _isMoveForward = true;
                    _isMove = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    // Выстрел объектом.
    public void Shot(float lenght, float speed)
    {
        if (_isMove == true)
            return;

        transform.localPosition = _beginPosition;
        _endPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + lenght);
        _speed = speed;
        _isMoveForward = true;
        _isMove = true;
    }
}
