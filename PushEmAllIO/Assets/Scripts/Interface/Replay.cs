using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Replay : MonoBehaviour
{
    [SerializeField] private Text _score;
    [SerializeField] private Text _record;

    public void ShowReplayPanel(uint score)
    {
        Time.timeScale = 0f;
        if (score > PlayerPrefs.GetInt("Record", 0))
            PlayerPrefs.SetInt("Record", (int) score);

        _score.text = score.ToString();
        _record.text = PlayerPrefs.GetInt("Record", 0).ToString();

        gameObject.SetActive(true);
    }

    public void OnReplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
