using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Settings/Unit")]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _speedRotation;
        [SerializeField] private float _forceSuperImpulseWeapon;

        public float Speed => _speed;
        public float SpeedRotation => _speedRotation;

        public float ForceSuperImpulseWeapon => _forceSuperImpulseWeapon;

        private void OnValidate()
        {
            if (_forceSuperImpulseWeapon < 0)
                _forceSuperImpulseWeapon = 0;

            if (_speed < 0)
                _speed = 0;

            if (_speedRotation < 0)
                _speedRotation = 0;
        }
    }
}