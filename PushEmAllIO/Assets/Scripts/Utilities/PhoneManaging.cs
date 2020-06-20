 using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;

public class PhoneManaging : MonoBehaviour
{
    private bool _isVibration;
   
    private void Update()
    {
        if (_isVibration == true)
            Handheld.Vibrate();
    }

    public void VibrateMini()
    {
        Vibrate(0.1f);
    }

    private void Vibrate(float second)
    {
        StartCoroutine(VibrateDelay(second));
    }

    private IEnumerator VibrateDelay(float second)
    {
        _isVibration = true;
        yield return new WaitForSeconds(second);
        _isVibration = false;
    }
}
