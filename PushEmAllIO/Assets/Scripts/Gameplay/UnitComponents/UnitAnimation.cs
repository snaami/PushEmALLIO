using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UnitAnimation : UnitComponent
{
    private float _speedAnimation;

    private Animator _animator;
    private IMoveable _moveable;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _moveable = GetComponent<IMoveable>();
    }

    private void OnEnable()
    {
        _unit.Damaged += OnDamage;
        _unit.Pushed += OnDamage;
        _unit.StandUp += OnStandUp;
    }

    private void OnDestroy()
    {
        _unit.Damaged -= OnDamage;
        _unit.Pushed -= OnDamage;
        _unit.StandUp -= OnStandUp;
    }


    private void OnDisable()
    {
        _animator.speed = 0;
    }

    private void Update()
    {
        _speedAnimation = _moveable.SpeedCurrent;
        _animator.speed = _speedAnimation;
    }

    private void OnDamage()
    {
        _animator.SetBool("Pain", true);
    }
    private void OnStandUp()
    {
        _animator.SetBool("Pain", false);
    }
}
