using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelController : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField]
    [Tooltip("Time that the player needs to protect their towers to win this level (in seconds)")]
    private int _timeToLast;

    [SerializeField]
    private int _numberOfTowersLeft;

    [SerializeField]
    InGameMenu _pauseMenuPanel;

    private List<Towers> _towersInSceneBeginning = new List<Towers>();
    int minutes, seconds;


    // Start is called before the first frame update
    void Start()
    {
        Tower[] towersInScene = GameObject.FindObjectsOfType<Tower>();
        for (int i = 0; i < towersInScene.Length; i++)
        {
            _towersInSceneBeginning.Add(towersInScene[i].GetThisTower());
        }

        GetComponent<Timer>().SetInitialTime(_timeToLast);
        ResumeGame();
        //Debug.Log("In game menu disabled from levelController.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Escape key down event from "+gameObject.name);
            TogglePause();
        }
    }

    private void RestartLevel()
    {
        Debug.Log("Level restarting.");
        //ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    

    private void TogglePause()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
             PauseGame();
        }
    }

    private void OnTimerFinished()
    {
        OnLevelEnd("Players won!");
    }

    private void OnLevelEnd(string levelEndText)
    {
        isGamePaused = true;
        Time.timeScale = 0;

        _pauseMenuPanel.SetHeadingText(levelEndText);

        //If current level is the last level
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            _pauseMenuPanel.SetResumeRestartButton(() => { RestartLevel(); }, "Restart");
        } else
        {
            _pauseMenuPanel.SetResumeRestartButton(() => { LoadNextLevel(); }, "Next");
        }
        
        _pauseMenuPanel.gameObject.SetActive(true);
    }

    private void OnEnemyWon()
    {
        isGamePaused = true;
        Time.timeScale = 0;

        if (_pauseMenuPanel != null)
        {
            _pauseMenuPanel.SetHeadingText("Enemies won");
            _pauseMenuPanel.SetResumeRestartButton(delegate { RestartLevel(); }, "Restart");
            _pauseMenuPanel.gameObject.SetActive(true);
        }
    }

    public void OnTowerDeath()
    {
        Debug.Log("On tower death called");
        --_numberOfTowersLeft;

        if (_numberOfTowersLeft == 0)
        {
            Debug.Log("Enemies won.");
            OnEnemyWon();
        }
    }

    public void OnTutorialEnd()
    {
        OnLevelEnd("End of tutorial.");
    }

    public void ResumeGame()
    {
        Debug.Log("Resume game called.");
        isGamePaused = false;
        Time.timeScale = 1;
        _pauseMenuPanel.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;

        _pauseMenuPanel.SetHeadingText("Game paused");
        Debug.Log("Resume game added to onclick listeners.");
        _pauseMenuPanel.SetResumeRestartButton(delegate { ResumeGame(); }, "Resume");
        _pauseMenuPanel.gameObject.SetActive(true);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public List<Towers> GetTowersAtBeginning()
    {
        return _towersInSceneBeginning;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
