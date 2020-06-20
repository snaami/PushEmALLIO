using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент передвижения игрока.
/// </summary>
[RequireComponent(typeof(Unit), typeof(CharacterController))]
public class PlayerMove : UnitComponent, IMoveable
{ 
    // Приблизительная корректирующая скорости = 20
    private const float СorrectiveValueSpeed = 20f;
    
    // Постоянная силы скорости падения вниз.
    private const float ConstForceGravity = 20f;

    [SerializeField] private TouchMediator _touchMediator;
   
    // Получаем корректную скорость.
    public float SpeedCurrent => Mathf.Abs(_touchMediator.TouchController.GetHorizontal()) + Mathf.Abs(_touchMediator.TouchController.GetVertical());

    private bool _isMoving = true;
    private float _speed;
    private float _speedRotation;
    private float _gravityForce;

    private CharacterController _character;

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();

        if (_touchMediator == null)
        {
            Debug.LogError("Для успешной работы PlayerMove необходимо указать TouchMediator.");
            return;
        }

        _character = GetComponent<CharacterController>();
    }

    public void Init(float speed, float speedRotation)
    {
        _speed = speed / СorrectiveValueSpeed;
        _speedRotation = speedRotation;
    }

    private void OnEnable()
    {
        _unit.Damaged += OffMove;
        _unit.StandUp += OnMove;
        _touchMediator.TouchController.PointerUp += Shot;
    }

    private void OnDisable()
    {
        _unit.Damaged -= OffMove;
        _unit.StandUp -= OnMove;
       
        if(_touchMediator != null)
            _touchMediator.TouchController.PointerUp -= Shot;
    }

    private void FixedUpdate()
    {
        if (_isMoving == false)
            return;

        Gravity();
        Move();
        Rotation();
    }

    #endregion

    /// <summary>
    /// Рассчеты движения объекта.
    /// </summary>
    public void Move()
    {
        // Получаем значения тача по горизонтали в вертикали.
        float horizontal = _touchMediator.TouchController.GetHorizontal();
        float vertical = _touchMediator.TouchController.GetVertical();
        
        var moveVector = new Vector3(horizontal, _gravityForce, vertical);
        
        // Двигаем объект на вектор заданный ранее со скоростью, указанной в настройках.
        _character.Move(moveVector * _speed); 
    }

    public bool IsGrounded()
    {
        return _character.isGrounded;
    }

    /// <summary>
    /// Рассчеты псевдогравитации.
    /// </summary>
    private void Gravity()
    {
        if (_character.isGrounded)
            _gravityForce = -1;
        else 
            _gravityForce -= ConstForceGravity * Time.fixedDeltaTime;
    }


    /// <summary>
    /// Рассчеты повотора объекта.
    /// </summary>
    private void Rotation()
    {
        // Получаем значения тача по горизонтали в вертикали.
        float horizontal = _touchMediator.TouchController.GetHorizontal();
        float vertical = _touchMediator.TouchController.GetVertical();

        // Общий вектор передвижения.
        var moveVector = new Vector3(horizontal, 0, vertical);

        // Поворачиваем объект (плавно, с помощью Slerp).
        Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, _speedRotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direct), Time.fixedDeltaTime * _speedRotation);
    }

    // Нужно будет потом вынести в другой класс, разделить ответственность.
    private void Shot()
    {
        if (_isMoving == true)
            _unit.Weapon.Shot();
    }

    private void OffMove()
    {
        _isMoving = false;
    }

    private void OnMove()
    {
        _isMoving = true;
        _character.enabled = true;
    }
}
