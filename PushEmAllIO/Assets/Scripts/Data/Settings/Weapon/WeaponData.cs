using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Settings/Weapon")]
    public class WeaponData : ScriptableObject
    {
        [SerializeField] private float _beginLenght;
        [SerializeField] private float _extraLengthBooster;
        [SerializeField] private float _maxLenght;

        [SerializeField] private float _shotLenght;
        [SerializeField] private float _shotSpeed;

        [SerializeField] private float _coeffIncreaseLenghtSuperImpulse;
        [SerializeField] private float _coeffIncreaseSpeedSuperImpulse;

        [SerializeField] private uint _numUnitsNeedPushToSuperImpulse;
        [SerializeField] private bool _interactionBetweenEachOther;

        public float BeginLenght => _beginLenght;
        public float ExtraLengthBooster => _extraLengthBooster;
        public float MaxLenght => _maxLenght;
        public float ShotLenght => _shotLenght;
        public float ShotSpeed => _shotSpeed;

        public float ShotLenghtSuperImpulse => _shotLenght * _coeffIncreaseLenghtSuperImpulse;
        public float ShotSpeedSuperImpulse => _shotSpeed * _coeffIncreaseSpeedSuperImpulse;

        public uint NumUnitsNeedPushToSuperImpulse => _numUnitsNeedPushToSuperImpulse;
       
        public bool InteractionBetweenEachOther => _interactionBetweenEachOther;

        private void OnValidate()
        {
            if (_maxLenght < 0)
                _maxLenght = 0;

            if (_extraLengthBooster < 0)
                _extraLengthBooster = 0;

            if (_shotLenght < 0)
                _shotLenght = 0;

            if (_shotSpeed < 0)
                _shotSpeed = 0;

            if (_coeffIncreaseLenghtSuperImpulse < 0)
                _coeffIncreaseLenghtSuperImpulse = 0;

            if (_coeffIncreaseSpeedSuperImpulse < 0)
                _coeffIncreaseSpeedSuperImpulse = 0;

            if (_extraLengthBooster > _maxLenght)
                _extraLengthBooster = _maxLenght;

            if (_beginLenght > _maxLenght)
                _beginLenght = _maxLenght;
        }

    }
}