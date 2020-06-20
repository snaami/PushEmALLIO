using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System.Security.Cryptography;
using System;

/// <summary>
/// Компонент передвижения бота.
/// </summary>
[RequireComponent(typeof(Unit), typeof(NavMeshAgent))]
public class AIMove : UnitComponent, IMoveable
{
    // Приблизительная корректирующая скорости поворота = 37.5
    private const float СorrectiveValueSpeedRotation = 37.5f;

    [SerializeField] private Transform _parentForSearch;

    public float SpeedCurrent => _agent.speed;

    private Transform _target;

    private bool _isGrounded;

    // Кешируем ссылки.
    private NavMeshAgent _agent;
    private NavMeshHit _navMeshHit;

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();

        if(_parentForSearch == null)
        {
            Debug.LogError("Укажите родителя для объектов для поиска ParentForSearch в AIMove.");
            return;
        }

        _agent = GetComponent<NavMeshAgent>();
    }

    public void Init(float speed, float speedRotation)
    {
        _agent.speed = speed;
        _agent.angularSpeed = speedRotation * СorrectiveValueSpeedRotation;
    }

    private void OnEnable()
    {
        StartCoroutine(DetermineBestDestinationUpdate());

        _agent.enabled = true;

        _unit.Damaged += OnDamaged;
        _unit.Pushed += OnPushed;
        _unit.StandUp += OnStandUp;
    }

    private void OnDisable()
    {
        StopCoroutine(DetermineBestDestinationUpdate());

        _unit.Damaged -= OnDamaged;
        _unit.Pushed -= OnPushed;
        _unit.StandUp -= OnStandUp;
    }

    private void Update()
    { 
        // Передвигаем бота к цели.
        if (_agent.enabled == true)
        {
            if (_target != null)
                _agent.destination = _target.position;
            else
                _target = GetTarget();
        }

        
        CheckGrounded();
    }

    #endregion

    public bool IsGrounded()
    {
        return _isGrounded;
    }

    /// <summary>
    /// Проверка на нахождение на поверхности.
    /// </summary>
    private void CheckGrounded()
    {
        _isGrounded = NavMesh.SamplePosition(transform.position, out _navMeshHit, 0.5f, NavMesh.AllAreas);
        if (_isGrounded == false)
        {
            enabled = false;
            _agent.enabled = false;
        }
    }

    /// <summary>
    /// Получение цели для бота.
    /// </summary>
    /// <returns></returns>
    private Transform GetTarget()
    {
        var objsInteractive = _parentForSearch.GetComponentsInChildren<IInteractivityForAI>().ToList();
        objsInteractive.Remove(_unit);

        if (objsInteractive.Count == 0)
            return null;

        float bestValue = float.MaxValue;
        Transform target = objsInteractive[0].transform;


        // Ищем лучшую цель по "стоимости". Т.е. кто ближе и кто дешевле.
        foreach (var obj in objsInteractive)
        {
            var value = (transform.position - obj.transform.position).sqrMagnitude * obj.GetCost();
            if (value < bestValue)
            {
                bestValue = value;
                target = obj.transform;
            }
        }

        return target;
    }

    /// <summary>
    /// Выбираем лучшую цель каждую секунду (не чаще, чтобы не грузить устройство).
    /// </summary>
    /// <returns></returns>
    private IEnumerator DetermineBestDestinationUpdate()
    {
        while (enabled == true)
        {
            _target = GetTarget();
            yield return new WaitForSeconds(1f);
        }
    }


    private void OnPushed()
    {
        _agent.enabled = false;
    }

    private void OnDamaged()
    {
        _agent.enabled = false;
    }

    private void OnStandUp()
    {
        _agent.enabled = true;
    }
}
