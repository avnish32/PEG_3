using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int _initialTime;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private int _lastFewSecsThreshold;

    int minutes, seconds;

    // Start is called before the first frame update
    void Start()
    {
        minutes = _initialTime / 60;
        seconds = _initialTime % 60;
        UpdateTimerText();

        InvokeRepeating("UpdateTimeToLast", 0f, 1f);
    }

    private void UpdateTimerText()
    {
        if (_timerText == null)
        {
            return;
        }
        _timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void UpdateTimeToLast()
    {
        _initialTime--;

        minutes = _initialTime / 60;
        seconds = _initialTime % 60;

        UpdateTimerText();

        if (_initialTime <= _lastFewSecsThreshold)
        {
            BroadcastMessage("OnLastFewSecsRemaining", SendMessageOptions.DontRequireReceiver);
        }

        if (_initialTime == 0)
        {
            BroadcastMessage("OnTimerFinished");
        }
    }

    public void SetInitialTime(int initialTime)
    {
        _initialTime = initialTime;
    }

    public void PauseTimer()
    {
        CancelInvoke("UpdateTimeToLast");
    }
}
