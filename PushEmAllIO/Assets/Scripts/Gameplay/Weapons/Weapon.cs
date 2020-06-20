using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Скорость увеличения оружия.
    private const float _speedIncrease = 2.5f;

    [SerializeField] private Settings.WeaponData _weaponData;
    [SerializeField] private PushObject _pushObject;

    private Unit _unit;

    private Vector3 newScale;
    private bool _isShotSuper;

    protected abstract void AddForce(Unit enemy);

    private void Awake()
    {
        if (_weaponData == null)
        {
            Debug.LogError("Укажите настройки оружия в поле оружия " + name);
            return;
        }
        if (_pushObject == null)
        {
            Debug.LogError("Разместите объект толкания PushObject в поле оружия " + name);
            return;
        }

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, _weaponData.BeginLenght);
        newScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider col)
    {
        Unit unit = col.GetComponent<Unit>();

        // Если возможно столкновение между оружиями.
        if (_weaponData.InteractionBetweenEachOther == true)
        {
            var weapon = col.GetComponent<Weapon>();
            if (weapon != null)
                unit = weapon.GetComponentInParent<Unit>();
        }

        if (unit != null)
            OnCheckUnitColission(unit);
    }

    private void Update()
    {
        // Увеличиваем оружие, если это необходимо.
        if ((transform.localScale - newScale).sqrMagnitude > 0.001f)
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * _speedIncrease);
    }


    public void Init(Unit unit)
    {
        _unit = unit;
        _unit.PickUpBooster += () => AddScale(_weaponData.ExtraLengthBooster);
        
        // При убийстве врага - проверяем на доступ к супер удару.
        _unit.Kill += () =>
        { 
            // С задержкой в 0.1f, в будущем переписать, чтобы было последовательное выполнение. Иначе проверка некорректна.
            if (gameObject != null && gameObject.activeInHierarchy == true)
                StartCoroutine(СheckingShotSuper());
        };
    }

    private IEnumerator СheckingShotSuper()
    {
        yield return new WaitForSeconds(0.1f);
        if (_unit.Score.CountKillRow >= _weaponData.NumUnitsNeedPushToSuperImpulse)
            _isShotSuper = true;
    }



    /// <summary>
    /// Проверка на столкновение с противником
    /// </summary>
    /// <param name="enemy"></param>
    private void OnCheckUnitColission(Unit enemy)
    {
        enemy.OnDamaged(_unit);
        var unitRigidbody = enemy.GetComponent<Rigidbody>();

        unitRigidbody.isKinematic = false;

        if (enemy.GetComponent<CharacterController>())
            enemy.GetComponent<CharacterController>().enabled = false;

        AddForce(enemy);     
    }


    /// <summary>
    /// Добавить размер к оружию.
    /// </summary>
    /// <param name="value"></param>
    public void AddScale(float value)
    {
        if (transform.localScale.y + value > _weaponData.MaxLenght)
            value = (transform.localScale.y + value) - _weaponData.MaxLenght;

        newScale = new Vector3(transform.localScale.x, transform.localScale.y + value, transform.localScale.z);
    }

    /// <summary>
    /// Выстрел палкой.
    /// </summary>
    public void Shot()
    {
        if (_pushObject != null)
            _pushObject.gameObject.SetActive(true);

        if (_isShotSuper == true)
        {
            _pushObject.Shot(_weaponData.ShotLenghtSuperImpulse, _weaponData.ShotSpeedSuperImpulse);
            _unit.Score.ResetCountKillRow();
            _isShotSuper = false;
        }
        else
            _pushObject.Shot(_weaponData.ShotLenght, _weaponData.ShotSpeed);
    }
}
