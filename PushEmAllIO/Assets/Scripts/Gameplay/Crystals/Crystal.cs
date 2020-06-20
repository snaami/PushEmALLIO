using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crystal : MonoBehaviour, IInteractivityForAI
{
    private float _sqrMinDistanceBetweenCrystal;

    /// <summary>
    /// Инициализация кристалла.
    /// </summary>
    /// <param name="sqrMinDistanceBetweenCrystal"></param>
    public void Init(float sqrMinDistanceBetweenCrystal)
    {
        _sqrMinDistanceBetweenCrystal = sqrMinDistanceBetweenCrystal;
        RandomPosition();
    }


    /// <summary>
    /// Нахождение рандомной позиции, удовлетворяющей всем условиям.
    /// </summary>
    private void RandomPosition()
    {
        var neighbors = transform.parent.GetComponentsInChildren<Crystal>().ToList();
        neighbors.Remove(this);

        while (true)
        {
            transform.position = GetRandomPositionCrystal();
            var pastPosition = transform.position;

            foreach (var neighbor in neighbors)
            {
                if ((transform.position - neighbor.transform.position).sqrMagnitude < _sqrMinDistanceBetweenCrystal)
                    transform.position = GetRandomPositionCrystal();
            }

            if (pastPosition == transform.position)
                break;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        var unit = col.GetComponent<Unit>();
        if (unit != null)
        {
            unit.OnPickUpCrystal();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Получение случайной позиции объекта.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPositionCrystal()
    {
        var ground = transform.parent;

        float positionX = UnityEngine.Random.Range(ground.position.x + ground.localScale.x / 2 - transform.localScale.x, ground.position.x - ground.localScale.x / 2 + transform.localScale.x);
        float positionZ = UnityEngine.Random.Range(ground.position.z + ground.localScale.z / 2 - transform.localScale.z, ground.position.z - ground.localScale.z / 2 + transform.localScale.z);

        positionX = Convert.ToSingle(Math.Round(positionX, 1));
        positionZ = Convert.ToSingle(Math.Round(positionZ, 1));
        
        return new Vector3(positionX, ground.transform.position.y + ground.transform.localScale.y / 2, positionZ);
    }

    public float GetCost()
    {
        return 2f;
    }
}
