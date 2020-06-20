using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceItemView : MonoBehaviour
{
    [SerializeField] private Text _count;

    public void Init(uint value)
    {
        SetValue(value);
    }

    public void SetValue(uint value)
    {
        _count.text = value.ToString();
    }
}
