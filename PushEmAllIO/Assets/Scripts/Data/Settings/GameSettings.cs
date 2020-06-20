using Settings;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GeneralData _general;
    [SerializeField] private UnitData _unit;

    public GeneralData General => _general;
    public Settings.UnitData Unit => _unit;
}
