using UnityEngine;
using System.Collections;

/// <summary>
/// Посредник между UI и ITouchController.
/// </summary>
public class TouchMediator : MonoBehaviour
{
    [SerializeField] private StickData _stickData;

    public ITouchController TouchController
    {
        get
        {
            if (GetComponent<ITouchController>() == null)
            {
                var newTouchController = gameObject.AddComponent<TouchController>();
                newTouchController.Init(_stickData);
            }
            
            return GetComponent<ITouchController>();
        }
    }
}
