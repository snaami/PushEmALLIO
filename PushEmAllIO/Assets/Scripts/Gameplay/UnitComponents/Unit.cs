using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Основной компонент юнита (бота или игрока).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour, IInteractivityForAI
{
    // Задержка перед началом ходьбы после удара.
    private const float DelaySecondsToStandUp = 0.5f;

    // События различного поведелия юнита.
    public event Action Damaged, Kill, Fall, Pushed, PickUpCrystal, PickUpBooster, StandUp;
    public new event Action Destroy;

    [SerializeField] private Weapon _weapon;
    public Weapon Weapon => _weapon;

    public UnitScore Score { get; private set; }

    private Unit _enemy;

    private bool _isStandUp = true;

    private IMoveable _moveable;
    private Rigidbody _rigidbody;

    Transform IInteractivityForAI.transform => transform;

    private void Awake()
    {
        _moveable = GetComponent<IMoveable>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _weapon.Init(this);

        Score = new UnitScore(0, (uint)PlayerPrefs.GetInt("Record", 0));
    }

    public void Init(uint scoreEnemy, uint scoreCrystal)
    {
        Kill += () => AddScoreKill(scoreEnemy);
        PickUpCrystal += () => Score.AddScore(scoreCrystal);
    }

    private void Update()
    {
        // Временно, в будущем заменить, отделить ответственность.
        if (GetComponent<CharacterController>() && GetComponent<CharacterController>().isGrounded == false)
            Fall?.Invoke(); 
    }

    private void OnDestroy()
    {
        Destroy?.Invoke();
        if (_enemy != null)
            _enemy.Kill?.Invoke();

        var units = transform.parent.GetComponentsInChildren<Unit>();

        if (_enemy != null && units.Length == 1)
            if (_enemy.GetComponent<WorkWithUI>() != null)
                _enemy.GetComponent<WorkWithUI>().StopGame();
    }

    private void AddScoreKill(uint scoreEnemy)
    {
        Score.AddScore(scoreEnemy);
        Score.AddKill();
    }

    public void OnDamaged(Unit emeny)
    {
        if (_isStandUp == false)
            return;
       
        _rigidbody.isKinematic = false;
        _enemy = emeny;
        Damaged?.Invoke();
        StartCoroutine(OnStandUpDelay());
    }

    public void OnPushed(Unit emeny)
    {
        _rigidbody.isKinematic = false;
        _enemy = emeny;
        Pushed?.Invoke();
        StartCoroutine(OnStandUpDelay());
    }

    public void OnPickUpCrystal()
    {
        PickUpCrystal?.Invoke();
    }

    public void OnPickUpBooster()
    {
        PickUpBooster?.Invoke();
    }

    public IEnumerator OnStandUpDelay()
    {
        _isStandUp = false;
        yield return new WaitForSeconds(DelaySecondsToStandUp);
        _isStandUp = true;

        if (_moveable.IsGrounded())
        {
            StandUp?.Invoke();
            _rigidbody.isKinematic = true;
        }
    }


    public float GetCost()
    {
        return 1;
    }
}

