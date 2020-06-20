using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class StickData
{
    [SerializeField] private RectTransform _stick;
    [SerializeField] private RectTransform _backPanel;

    public RectTransform Stick => _stick;
    public RectTransform BackPanel => _backPanel;

}
