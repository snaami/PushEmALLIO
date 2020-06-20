using UnityEngine;
using System.Collections;
using Settings;

public class ScoreStarter : MonoBehaviour
{
    [SerializeField] private GameSettings _settings;

    private void Awake()
    {
        foreach (var unitWorkUI in GetComponentsInChildren<WorkWithUI>())
            unitWorkUI.Init(_settings.General.NumScoreEnemy, _settings.General.NumScoreCrystal);
    }
}
