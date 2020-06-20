using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Settings;

// Сложно создать кристалы на поверхности, когда суммарная длинна между каждым из кристаллов
// БОЛЬШЕ, чем гипотенуза размера объекта, поэтому, был сделан именно такой подход.
// Но все равно лучший вариант - по минимальной стороне объекта.

/// <summary>
/// Создание кристаллов.
/// </summary>
public class SpawnerCrystal : MonoBehaviour
{
    // Общие настройки игры.
    [SerializeField] private GameSettings _settings;
    
    // Префаб кристалла.
    [SerializeField] private GameObject _prefab;

    // Поверхности, на которых необходимо их создать.
    [SerializeField] private List<Transform> _grounds;

    // Считать по гипотенузе.
    [SerializeField] private bool _isCountingHypotenuse;

    private uint _countCrystal;
    private List<int> _countMaxCrystalsOnGrounds = new List<int>(0);

    private void Start()
    {
        _countCrystal = _settings.General.CountCrystals;
        
        // Проверки на невозможность создать кристаллы.

        if (_countCrystal == 0)
        {
            return;
        }
        
        if (_prefab == null)
        {
            Debug.LogError("Добавьте префаб в компонент SpawnerCrystal, чтобы кристалы успешно создались.");
            return;
        }
        
        if(_prefab.GetComponent<Crystal>() == null)
        {
            Debug.LogError("Добавьте в префаб кристалла компонент Crystal, для успешного создания кристалов на сцене.");
            return;
        }

        if (IsPossibleToCreate() == false)
        {
            Debug.LogError("Невозможно создать кристалы при текущих параметрах.\n" +
                            "ИЛИ 1) Уменьшите минимальное расстояние между кристаллами;\n" +
                            "ИЛИ 2) Уменьшите кол-во кристалов необходимых для создания;\n" +
                            "ИЛИ 3) Добавьте новые зоны создания кристаллов.;");
            return;
        }

        // Высчитываем какое максимальное кол-во кристаллов, можно разместить на поверхностях.
        ComputeMaxNumCrystalsInGrounds();

        RandomSpawn();
    }

    /// <summary>
    /// Спаун случайного числа кристаллов на поверхностях.
    /// </summary>
    private void RandomSpawn()
    {
        while (_countCrystal > 0)
        {
            for (int i = 0; i < _countMaxCrystalsOnGrounds.Count && _countCrystal > 0; i++)
            {
                uint conutRandomCrystals = (uint) Random.Range(0, _countMaxCrystalsOnGrounds[i] + 1);

                if (conutRandomCrystals > _countCrystal)
                    conutRandomCrystals = _countCrystal;

                _countCrystal -= conutRandomCrystals;

                while (conutRandomCrystals-- > 0)
                    Spawn(_grounds[i]);

            }
        }     
    }

    /// <summary>
    /// Спаун кристалла.
    /// </summary>
    private void Spawn(Transform ground)
    {
        var newCrystal = Instantiate(_prefab).GetComponent<Crystal>();
        newCrystal.transform.parent = ground;
        newCrystal.Init(_settings.General.SqrMinDistanceBetweenCrystal);
    }

    /// <summary>
    ///  Высчитывает какое максимальное кол-во кристаллов, можно разместить на поверхностях.
    /// </summary>
    private void ComputeMaxNumCrystalsInGrounds()
    {
        foreach (var ground in _grounds)
        {
            float maxDistance = 0;

            // Вычисляем квадрат длины гипотенузы объекта.
              if (_isCountingHypotenuse == true)
                maxDistance = GetLenghtHypotenuse(ground) * 0.2f;
            else
                maxDistance = Mathf.Min(ground.localScale.x, ground.localScale.z);

            // Количество кристаллов, которые можно создать на текущей поверхности.
            var countCrystalOnObjects = Mathf.FloorToInt(Mathf.Sqrt(maxDistance / _settings.General.MinDistanceBetweenCrystal));
            
            _countMaxCrystalsOnGrounds.Add(countCrystalOnObjects);
        }
    }

    /// <summary>
    /// Есть ли возможность создать кристаллы на повехностях.
    /// </summary>
    /// <returns></returns>
    private bool IsPossibleToCreate()
    {
        float sumMaxDistances = 0;

        if (_isCountingHypotenuse == true)
        {
            // Вычисляем длины гипотенуз поверхности объектов.
            foreach (var ground in _grounds)
                sumMaxDistances += GetLenghtHypotenuse(ground) * 0.2f;
        }
        else
        {
            // Суммарная дистанция объектов.
            foreach (var ground in _grounds)
                sumMaxDistances += Mathf.Min(ground.localScale.x, ground.localScale.z);
        }

        // Общая суммарная длина между возможными кристалами.
        var totalLenght = (_countCrystal - 1) * _settings.General.MinDistanceBetweenCrystal;

        if (sumMaxDistances < totalLenght)
            return false;

        return true;
    }

    /// <summary>
    /// Вычисление длины гипотенузы. 
    /// </summary>
    /// <returns></returns>
    private float GetLenghtHypotenuse(Transform ground)
    {
        return Mathf.Sqrt(Mathf.Pow(ground.localScale.x, 2) + Mathf.Pow(ground.localScale.z, 2));
    }

}
