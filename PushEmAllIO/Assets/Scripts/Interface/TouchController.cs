using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour, ITouchController
{
    // События на действия игрока.
    public event Action<float, float> Drag;
    public event Action PointerDown;
    public event Action PointerUp;

    private StickData _stick;
    private PhoneManaging _phone;

    private Vector2 _inputVector;

    public void Init(StickData stick)
    {
        _stick = stick;
        _phone = gameObject.AddComponent<PhoneManaging>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        PointerDown?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;

        // Считаем положение пальца, двигаем стик, получаем вектор перемещения стика относительно центра задней панели.
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_stick.BackPanel, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / _stick.BackPanel.sizeDelta.x);
            pos.y = (pos.y / _stick.BackPanel.sizeDelta.y);

            _inputVector = new Vector2(pos.x * 2, pos.y * 2);
            _inputVector = (_inputVector.magnitude > 1) ? _inputVector.normalized : _inputVector;
            _stick.Stick.anchoredPosition = new Vector2(_inputVector.x * (_stick.BackPanel.sizeDelta.x / 2), _inputVector.y * (_stick.BackPanel.sizeDelta.y / 2));
        }

        Drag?.Invoke(_inputVector.x, _inputVector.y);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputVector = Vector2.zero;
        _stick.Stick.anchoredPosition = Vector2.zero;
        _phone.VibrateMini();

        PointerUp?.Invoke();
    }

    public float GetHorizontal()
    {
        return _inputVector.x;
    }

    public float GetVertical()
    {
        return _inputVector.y;
    }
}
