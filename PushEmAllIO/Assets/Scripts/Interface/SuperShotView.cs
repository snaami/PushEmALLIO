using UnityEngine;

public class SuperShotView : MonoBehaviour
{
    [SerializeField] private ResourceItemView _superShotText;

    public void Show()
    {
        _superShotText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _superShotText.gameObject.SetActive(false);
    }
}