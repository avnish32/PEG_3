using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _headingText;

    [SerializeField]
    private Button _resumeRestartButton;

    [SerializeField]
    private TextMeshProUGUI _resumeRestartButtonText;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("In game menu start called.");
    }

    public void SetResumeRestartButton(UnityAction buttonBehavior, string buttonText)
    {
        _resumeRestartButton.onClick.RemoveAllListeners();
        
        _resumeRestartButton.onClick.AddListener(()=> { buttonBehavior.Invoke(); });
        _resumeRestartButtonText.text = buttonText;
    }

    public void SetHeadingText(string headingText)
    {
        _headingText.text = headingText;
    }
}
