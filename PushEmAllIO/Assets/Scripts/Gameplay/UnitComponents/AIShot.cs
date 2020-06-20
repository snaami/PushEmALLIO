using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class AIShot : UnitComponent
{
    // Сложность врага.
    [Range(1, 10)]
    [SerializeField] private uint _difficulty = 5;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShotUpdate());
    }

    private void OnDisable()
    {
        StopCoroutine(ShotUpdate());
    }

    /// <summary>
    /// Периодическое наносение ударов ботом, в зависимости от сложности врага.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotUpdate()
    {
        while (true)
        {
            var range = Random.Range(15, 25);
            yield return new WaitForSeconds(range / _difficulty);
            
            if(_rigidbody.isKinematic == true)
                _unit.Weapon.Shot();
        }
    }
}
