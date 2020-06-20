using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Работа с UI для игроков. У каждого потенциального игрока UI будет свой. Поэтому тут будут иные параметры;
/// Необходимо будет спаунить некоторый UI только для определенных игроков.
/// </summary>
public class WorkWithUI : UnitComponent
{
    // Основные компоненты UI игрока.
    [SerializeField] private ScoreView _score;
    [SerializeField] private Replay _replay;

    private uint _scoreEnemy;
    private uint _scoreCrystal;

    public void Init(uint scoreEnemy, uint scoreCrystal)
    {
        _scoreEnemy = scoreEnemy;
        _scoreCrystal = scoreCrystal;
    }
    
    private void OnEnable()
    {
        if (_score == null)
            Debug.LogError("Укажите ScoreView в WorkWithUI!");
        if (_replay == null)
            Debug.LogError("Укажите Replay в WorkWithUI!");

        _unit.Kill += AddScoreEnemy;
        _unit.PickUpCrystal += AddScoreCrystal;
        _unit.Destroy += StopGame;
    }

    private void OnDisable()
    {
        _unit.Kill -= AddScoreEnemy;
        _unit.PickUpCrystal -= AddScoreCrystal;
        _unit.Destroy -= StopGame;
    }
    
    public void StopGame()
    {
         _replay.ShowReplayPanel(_unit.Score.Score);
    }


    private void AddScoreEnemy()
    {
        _score.AddScore(_scoreEnemy);
    }

    private void AddScoreCrystal()
    {
        _score.AddScore(_scoreCrystal);
    }
}
