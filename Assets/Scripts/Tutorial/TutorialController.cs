using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _msgText;

    private PanelFader _panelFader;

    [SerializeField]
    private EnemySpawnerTutorial _enemySpawnerTutorial;

    [SerializeField]
    private string[] _tutorialMsgs;

    [SerializeField]
    private string[] _tutorialMsgCommands;

    private bool _isBombDefused = false, _isDefusalSeqRead = false;
    private int _currentMsgIndex = 0;
    private LevelController _levelController;

    private void Awake()
    {
        _panelFader = GetComponent<PanelFader>();
        _levelController = GameObject.FindFirstObjectByType<LevelController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_enemySpawnerTutorial.gameObject.SetActive(false);
        //_enemySpawnerTutorial.Start();
        _isBombDefused = false;
        _isDefusalSeqRead = false;
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        for (_currentMsgIndex = 0; _currentMsgIndex < _tutorialMsgs.Length; _currentMsgIndex++)
        {
            _msgText.text = _tutorialMsgs[_currentMsgIndex];
            float msgDisplayDuration = 5f;
            string[] msgCommands = _tutorialMsgCommands[_currentMsgIndex].Split(":;");

            if (msgCommands.Contains("ENABLETELEPORT")) {
                GameObject.FindGameObjectWithTag("Player1").GetComponent<Teleporter>().enabled = true;
            } else if (msgCommands.Contains("DISABLETELEPORT"))
            {
                GameObject.FindGameObjectWithTag("Player1").GetComponent<Teleporter>().enabled = false;
            }

            if (msgCommands.Contains("SPAWNSHOOTER"))
            {
                //_enemySpawnerTutorial.enabled = true;
                _enemySpawnerTutorial.SpawnShooter();
                //_enemySpawnerTutorial.enabled = false;
            } else if (msgCommands.Contains("SPAWNBOMBER"))
            {
                //_enemySpawnerTutorial.enabled = true;
                _enemySpawnerTutorial.SpawnBomber();
                //_enemySpawnerTutorial.enabled = false;
            }

            if (msgCommands.Contains("DURATION"))
            {
                msgDisplayDuration = float.Parse(msgCommands[(Array.IndexOf(msgCommands, "DURATION"))+1]);
            }

            //Debug.Log("msgDisplayDuration: " + msgDisplayDuration);
            _panelFader.FadeIn(1f);

            if (msgCommands.Contains("PAUSE"))
            {
                yield return new WaitForSeconds(1f);
                _levelController.PauseGame();
                msgDisplayDuration = 0.5f;
            }

            if (msgCommands.Contains("WAITFORDEFUSALSEQREAD"))
            {
                yield return !_isDefusalSeqRead ? new WaitUntil(() => _isDefusalSeqRead) : new WaitForSeconds(msgDisplayDuration);
            } else if (msgCommands.Contains("WAITFORBOMBDEFUSAL"))
            {
                yield return !_isBombDefused ? new WaitUntil(() => _isBombDefused) : new WaitForSeconds(msgDisplayDuration);
            }
            else
            {
                yield return new WaitForSeconds(msgDisplayDuration);
            }

            Debug.Log("Game resumed in coroutine.");
            Time.timeScale = 1f;
            _panelFader.FadeOut(1f);
            yield return new WaitForSeconds(1f);

            if (msgCommands.Contains("TUTORIALENDED"))
            {
                GameObject.FindFirstObjectByType<LevelController>().OnTutorialEnd();
            }
        }

    }

    public void OnBombDefused()
    {
        _isBombDefused = true;
    }

    public void OnDefusalSeqRead()
    {
        if (_isDefusalSeqRead)
        {
            return;
        }
        _isDefusalSeqRead = true;
    }

    public void ResumeTutorial()
    {
        Time.timeScale = 1f;
    }

    public void OnContinue()
    {
        Debug.Log("Game resumed in continue.");
        _levelController.ResumeGame();
    }
}
