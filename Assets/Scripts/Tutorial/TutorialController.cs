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

    [SerializeField]
    private TextMeshProUGUI _enterToContinueText;

    [SerializeField]
    private RectTransform _highlighter;

    [SerializeField]
    private GameObject _darkPanel;

    [SerializeField]
    private EnemySpawnerTutorial _enemySpawnerTutorial;

    [SerializeField]
    private string[] _tutorialMsgs;

    [SerializeField]
    private string[] _tutorialMsgCommands;

    [SerializeField]
    private GameObject[] _gameobjectsToHighlight;

    private PanelFader _panelFader;
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
        //int gameobjectToHighlightIndex = -1;

        for (_currentMsgIndex = 0; _currentMsgIndex < _tutorialMsgs.Length; _currentMsgIndex++)
        {
            _enterToContinueText.enabled = false;
            _msgText.text = _tutorialMsgs[_currentMsgIndex];

            _panelFader.FadeIn(1f);
            yield return new WaitForSeconds(1f);

            float delayBeforeNextMsg = 1f, msgDisplayDuration = 0f;
            string[] msgCommands = _tutorialMsgCommands[_currentMsgIndex].Split(":;");
            
            CheckToHighlightObject(msgCommands);

            if (msgCommands.Contains("SECSB4NEXTMSG"))
            {
                delayBeforeNextMsg = float.Parse(msgCommands[(Array.IndexOf(msgCommands, "SECSB4NEXTMSG")) + 1]);
            }

            if (msgCommands.Contains("DISPLAYDURATION"))
            {
                msgDisplayDuration = float.Parse(msgCommands[(Array.IndexOf(msgCommands, "DISPLAYDURATION")) + 1]);
            }

            if (msgCommands.Contains("PAUSE"))
            {
                if (msgCommands.Contains("HIGHLIGHTOBJECT"))
                {
                    _darkPanel.SetActive(false);
                }
                else
                {
                    _darkPanel.SetActive(true);
                }

                _enterToContinueText.enabled = true;
                _levelController.PauseGame();
                yield return new WaitUntil(() => !LevelController.isGamePaused);
            }
            
            _highlighter.gameObject.SetActive(false);
            _darkPanel.SetActive(false);

            if (msgCommands.Contains("SPAWNSHOOTER"))
            {
                //_enemySpawnerTutorial.enabled = true;
                _enemySpawnerTutorial.SpawnShooter();
                //_enemySpawnerTutorial.enabled = false;
            }
            else if (msgCommands.Contains("SPAWNBOMBER"))
            {
                //_enemySpawnerTutorial.enabled = true;
                _enemySpawnerTutorial.SpawnBomber();
                //_enemySpawnerTutorial.enabled = false;
            }

            if (msgCommands.Contains("WAITFORDEFUSALSEQREAD"))
            {
                yield return !_isDefusalSeqRead ? new WaitUntil(() => _isDefusalSeqRead) : new WaitForSeconds(delayBeforeNextMsg);
            }
            else if (msgCommands.Contains("WAITFORBOMBDEFUSAL"))
            {
                yield return !_isBombDefused ? new WaitUntil(() => _isBombDefused) : new WaitForSeconds(delayBeforeNextMsg);
            }

            yield return new WaitForSeconds(msgDisplayDuration);
            _panelFader.FadeOut(1f);
            yield return new WaitForSeconds(delayBeforeNextMsg);

            if (msgCommands.Contains("TUTORIALENDED"))
            {
                GameObject.FindFirstObjectByType<LevelController>().OnTutorialEnd();
            }
        }

    }

    private void CheckToHighlightObject(string[] msgCommands)
    {
        if (msgCommands.Contains("HIGHLIGHTOBJECT"))
        {
            int gameobjectToHighlightIndex = int.Parse(msgCommands[(Array.IndexOf(msgCommands, "HIGHLIGHTOBJECT")) + 1]);
            GameObject gameobjectToHighlight = _gameobjectsToHighlight[gameobjectToHighlightIndex];
            Vector3 highlightAreaScreenPos = Camera.main.WorldToScreenPoint(gameobjectToHighlight.transform.position);
            _highlighter.position = new Vector3(highlightAreaScreenPos.x, highlightAreaScreenPos.y, 0f); ;

            float highlightAreaScale = 1.5F;
            if (msgCommands.Contains("HIGHLIGHTSCALE"))
            {
                highlightAreaScale = float.Parse(msgCommands[(Array.IndexOf(msgCommands, "HIGHLIGHTSCALE")) + 1]);

            }
            _highlighter.localScale = new Vector3(highlightAreaScale, highlightAreaScale, highlightAreaScale);
            _highlighter.gameObject.SetActive(true);
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
