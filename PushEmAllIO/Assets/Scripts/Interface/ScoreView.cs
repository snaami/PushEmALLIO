using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отображение очков в UI.
/// </summary>
public class ScoreView : MonoBehaviour
{
    // Ресурс очков.
    [SerializeField] private ResourceItemView _score;

    private uint _count;

    public void SetScore(uint value)
    {
        _count = value;
        _score.SetValue(_count);
    }

    public void AddScore(uint count)
    {
        _count += count;
        _score.SetValue(_count);
    }
}
