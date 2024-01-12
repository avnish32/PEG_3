using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    Button _pauseMenuRestartButton;

    [SerializeField]
    private GameObject _optionsPanel;

    [SerializeField]
    private string _playersWonMsgAtLevelEnd = "Room clear.";

    private List<Towers> _towersInSceneBeginning = new List<Towers>();
    int minutes, seconds;
    private GameObject _currentPanel;
    private bool _hasLevelEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        Tower[] towersInScene = GameObject.FindObjectsOfType<Tower>();
        for (int i = 0; i < towersInScene.Length; i++)
        {
            _towersInSceneBeginning.Add(towersInScene[i].GetThisTower());
        }

        GetComponent<Timer>().SetInitialTime(_timeToLast);
        _currentPanel = _pauseMenuPanel.gameObject;
        OnUserResumeEvent();
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

    public void RestartLevel()
    {
        Debug.Log("Level restarting.");
        //ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    

    private void TogglePause()
    {
        if (_hasLevelEnded)
        {
            return;
        }
        if (isGamePaused)
        {
            //Below check for when game is paused during the tutorial
            if (_currentPanel == null)
            {
                ConstructAndDisplayPauseMenu();
            }
            else
            {
                OnUserResumeEvent();
            }
        }
        else
        {
             OnUserPauseEvent();
        }
    }

    private void OnTimerFinished()
    {
        OnLevelEnd(_playersWonMsgAtLevelEnd);
    }

    private void OnLevelEnd(string levelEndText)
    {
        _hasLevelEnded = true;
        isGamePaused = true;
        Time.timeScale = 0;

        _pauseMenuPanel.SetHeadingText(levelEndText);

        //If current level is the last level
        if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            _pauseMenuPanel.SetResumeRestartButton(() => { RestartLevel(); }, "Restart");
            _pauseMenuRestartButton.gameObject.SetActive(false);
        } else
        {
            _pauseMenuPanel.SetResumeRestartButton(() => { LoadNextLevel(); }, "Next");
        }
        
        ShowPauseMenu();
    }

    private void OnEnemyWon()
    {
        _hasLevelEnded = true;
        isGamePaused = true;
        Time.timeScale = 0;

        if (_pauseMenuPanel != null)
        {
            _pauseMenuPanel.SetHeadingText("Ah, defeat.");
            _pauseMenuRestartButton.gameObject.SetActive(false);
            _pauseMenuPanel.SetResumeRestartButton(delegate { RestartLevel(); }, "Restart");
            ShowPauseMenu();
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
        if (isGamePaused)
        {
            ResumeGame();
        }
        OnLevelEnd("End of tutorial.");
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Debug.Log("Resume game called.");
        isGamePaused = false;
        Time.timeScale = 1;
    }

    private void ShowPanel(GameObject panel)
    {
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
        }
        _currentPanel = panel;
        _currentPanel.SetActive(true);
    }

    private void ResetCurrentPanel()
    {
        _currentPanel.SetActive(false);
        _currentPanel = null;
    }

    public void OnUserResumeEvent()
    {
        ResumeGame();
        ResetCurrentPanel();
    }

    public void OnUserPauseEvent()
    {
        PauseGame();
        ConstructAndDisplayPauseMenu();
    }

    private void ConstructAndDisplayPauseMenu()
    {
        _pauseMenuPanel.SetHeadingText("Game paused");
        _pauseMenuRestartButton.gameObject.SetActive(true);
        Debug.Log("Resume game added to onclick listeners.");
        _pauseMenuPanel.SetResumeRestartButton(delegate { ResumeGame(); }, "Resume");
        ShowPanel(_pauseMenuPanel.gameObject);
    }

    public void ShowPauseMenu()
    {
        ShowPanel(_pauseMenuPanel.gameObject);
    }

    public void ShowOptions()
    {
        ShowPanel(_optionsPanel.gameObject);
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
