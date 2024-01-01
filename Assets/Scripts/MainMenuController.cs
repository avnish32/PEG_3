using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenuButtonsPanel;

    [SerializeField]
    private GameObject _levelSelectPanel;

    [SerializeField]
    private GameObject _controlsPanel;

    [SerializeField]
    private GameObject _creditsPanel;

    private GameObject _currentActivePanel;

    private void Start()
    {
        _currentActivePanel = _mainMenuButtonsPanel;
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("L1");
    }

    public void DisplayLevelSelect()
    {
        _currentActivePanel.SetActive(false);
        _levelSelectPanel.SetActive(true);
        _currentActivePanel = _levelSelectPanel;
    }

    public void DisplayControls()
    {
        _currentActivePanel.SetActive(false);
        _controlsPanel.SetActive(true);
        _currentActivePanel = _controlsPanel;
    }

    public void DisplayMainMenu()
    {
        _currentActivePanel.SetActive(false);
        _mainMenuButtonsPanel.SetActive(true);
        _currentActivePanel = _mainMenuButtonsPanel;
    }

    public void DisplayCredits()
    {
        _currentActivePanel.SetActive(false);
        _creditsPanel.SetActive(true);
        _currentActivePanel = _creditsPanel;
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
