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

    private void Awake()
    {
        _panelFader = GetComponent<PanelFader>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        for (int i = 0; i < _tutorialMsgs.Length; i++)
        {
            _msgText.text = _tutorialMsgs[i];
            float msgDisplayDuration = 5f;
            string[] msgCommands = _tutorialMsgCommands[i].Split(":;");

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

            if (msgCommands.Contains("SPAWNENEMY"))
            {
                _enemySpawnerTutorial.enabled = true;
                _enemySpawnerTutorial.SpawnEnemy();
                _enemySpawnerTutorial.enabled = false;
            }

            if (msgCommands.Contains("DURATION"))
            {
                msgDisplayDuration = float.Parse(msgCommands[(Array.IndexOf(msgCommands, "DURATION"))+1]);
            }

            if (msgCommands.Contains("TUTORIALENDED"))
            {
                GameObject.FindFirstObjectByType<LevelController>().OnTutorialEnd();
            }

            //Debug.Log("msgDisplayDuration: " + msgDisplayDuration);
            _panelFader.FadeIn(1f);
            yield return new WaitForSecondsRealtime(msgDisplayDuration);
            Time.timeScale = 1f;
            _panelFader.FadeOut(1f);
            yield return new WaitForSecondsRealtime(1f);
        }      
        
    }
}
