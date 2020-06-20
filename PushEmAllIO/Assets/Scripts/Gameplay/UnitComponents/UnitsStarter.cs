using UnityEngine;
using System.Collections;

public class UnitsStarter : MonoBehaviour
{
    [SerializeField] private GameSettings _settings;

    private void Start()
    {
        foreach (var unit in GetComponentsInChildren<Unit>())
            unit.Init(_settings.General.NumScoreEnemy, _settings.General.NumScoreCrystal);
        
        foreach (var moveable in GetComponentsInChildren<IMoveable>())
            moveable.Init(_settings.Unit.Speed, _settings.Unit.SpeedRotation);
    }
}
