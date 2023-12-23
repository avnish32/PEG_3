using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField]
    [Tooltip("Time that the player needs to protect their towers to win this level (in seconds)")]
    private int _timeToLast;

    [SerializeField]
    private int _numberOfTowersLeft;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    int minutes, seconds;


    // Start is called before the first frame update
    void Start()
    {
        minutes = _timeToLast / 60;
        seconds = _timeToLast % 60;
        UpdateTimerText();

        InvokeRepeating("UpdateTimeToLast", 0f, 1f);
    }

    private void UpdateTimerText()
    {
        if (_timerText == null)
        {
            return;
        }
        _timerText.text = "Just " + minutes.ToString("00") + ":" + seconds.ToString("00") + " more...";
    }

    private void UpdateTimeToLast()
    {
        _timeToLast--;

        minutes = _timeToLast / 60;
        seconds = _timeToLast % 60;

        UpdateTimerText();

        if (_timeToLast == 0)
        {
            Debug.Log("Players won!");
            isGamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void OnTowerDeath()
    {
        --_numberOfTowersLeft;

        if (_numberOfTowersLeft == 0)
        {
            Debug.Log("Enemies won.");
            isGamePaused = true;
            Time.timeScale = 0;
        }
    }

}
