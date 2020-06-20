using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        var unit = col.GetComponent<Unit>();
        if (unit != null)
        {
            Destroy(col.gameObject);
            if (unit.GetComponent<WorkWithUI>() != null)
                unit.GetComponent<WorkWithUI>().StopGame();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube(transform.position, transform.localScale);
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
