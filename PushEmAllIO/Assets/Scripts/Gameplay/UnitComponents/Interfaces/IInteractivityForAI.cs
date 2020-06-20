using UnityEngine;

public interface IInteractivityForAI
{
    float GetCost();
    Transform transform { get; }
}