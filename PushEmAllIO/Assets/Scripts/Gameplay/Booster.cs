using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Бустер увеличения оружия.
/// </summary>
public class Booster : MonoBehaviour, IInteractivityForAI
{
    private void OnTriggerEnter(Collider col)
    {
        var unit = col.GetComponent<Unit>();
        if (unit != null)
        {
            // Увеличиваем оружие.
            unit.OnPickUpBooster();
            Destroy(gameObject);
        }
    }

    public float GetCost()
    {
        return 3f;
    }

}
