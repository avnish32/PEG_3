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

    private bool _canProceedAhead = false, _isDefusalSeqRead = false;
    private int _currentMsgIndex = 0;

    private void Awake()
    {
        _panelFader = GetComponent<PanelFader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_enemySpawnerTutorial.gameObject.SetActive(false);
        //_enemySpawnerTutorial.Start();
        _canProceedAhead = false;
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

            if (msgCommands.Contains("PAUSE"))
            {
                Time.timeScale = 0f;
            }

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
            }

            if (msgCommands.Contains("SPAWNBOMBER"))
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

            if (msgCommands.Contains("WAITFOREXTGOAHEAD"))
            {
                if (!_canProceedAhead)
                {
                    yield return new WaitUntil(() => _canProceedAhead);
                }
                else
                {
                    yield return new WaitForSeconds(msgDisplayDuration);
                }
                _canProceedAhead = false;
            }
            else
            {
                yield return new WaitForSeconds(msgDisplayDuration);
            }

            Time.timeScale = 1f;
            _panelFader.FadeOut(1f);
            yield return new WaitForSeconds(1f);

            if (msgCommands.Contains("TUTORIALENDED"))
            {
                GameObject.FindFirstObjectByType<LevelController>().OnTutorialEnd();
            }
        }

    }

    public void ProceedWithTutorial()
    {
        _canProceedAhead = true;
    }

    public void OnDefusalSeqRead()
    {
        if (_isDefusalSeqRead)
        {
            return;
        }
        _isDefusalSeqRead = true;
        ProceedWithTutorial();
    }
}
