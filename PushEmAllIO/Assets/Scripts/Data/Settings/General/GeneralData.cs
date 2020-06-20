using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "General", menuName = "Settings/General")]
    public class GeneralData : ScriptableObject
    {
        [SerializeField] private uint _numScoreEnemy;
        [SerializeField] private uint _numScoreCrystal;
        [SerializeField] private float _minDistanceBetweenCrystal;
        [SerializeField] private uint _countCrystals;


        public uint NumScoreEnemy => _numScoreEnemy;
        public uint NumScoreCrystal => _numScoreCrystal;
        public float MinDistanceBetweenCrystal => _minDistanceBetweenCrystal;
        public float SqrMinDistanceBetweenCrystal => MinDistanceBetweenCrystal * MinDistanceBetweenCrystal;
        public uint CountCrystals => _countCrystals;

        private void OnValidate()
        {
            if (_minDistanceBetweenCrystal <= 0)
                _minDistanceBetweenCrystal = 0.01f;
        }
    }
}