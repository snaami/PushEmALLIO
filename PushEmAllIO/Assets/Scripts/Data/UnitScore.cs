using UnityEngine;
using System.Collections;

public class UnitScore
{
    private uint _countKill;
    private uint _countKillRow;
    private uint _score;
    private uint _record;

    public uint Score => _score;
    public uint CountKill => _countKill;
    public uint CountKillRow => _countKillRow;

    public UnitScore(uint score, uint record)
    {
        _score = score;
        _record = record;
    }

    public void SetScore(uint value)
    {
        _score = value;
    }

    public void AddScore(uint value)
    {
        _score += value;
    }

    public void AddKill()
    {
        _countKill++;
        _countKillRow++;
    }

    public void ResetCountKillRow()
    {
        _countKillRow = 0;
    }

    public void SetRecord(uint record)
    {
        _record = record;
    }
}
