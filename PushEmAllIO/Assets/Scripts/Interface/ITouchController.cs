using System;
using UnityEngine.EventSystems;

public interface ITouchController : ITouchGetPosition, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    void Init(StickData stick);
    event Action<float, float> Drag;
    event Action PointerDown;
    event Action PointerUp;
}

