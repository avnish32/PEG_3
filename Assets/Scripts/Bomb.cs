using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour, IInteractable
{
    private List<Towers> _defusalSequence;
    private List<Towers> _towersMasterList;
    private Dictionary<Towers, Sprite> _towerToSpriteMap;
    private List<GameObject> _defusalSeqInstdSprites;
    private List<Towers> _playerTeleportHistory;
    private SpriteRenderer _spriteRenderer;
    private bool _isBombDefused = false;
    private float _maxTimerPanelOpacity;
    private EnemySpawner _enemySpawner;

    protected Timer _timer;

    [SerializeField]
    private GameObject _defusalSeqSpriteObject;

    [SerializeField]
    private RectTransform _defusalSeqPanel;

    [SerializeField]
    private Image _timerPanel;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private Sprite _defusedBombSprite;

    [SerializeField]
    private SpriteRenderer _damageRadiusSprite;

    [SerializeField]
    private float _fadeOutTime = 3f;

    [SerializeField]
    private GameObject _explosionEffect;

    [SerializeField]
    private AudioClip _explosionSound;

    [SerializeField]
    private AudioClip _tickSound;

    [SerializeField]
    private AudioClip _defusedSound;

    [SerializeField]
    private AudioPlayer _audioPlayer;

    [SerializeField]
    [Tooltip("This needs to be same as radius of damage radius sprite. Move bomb in the scene to get a measure.")]
    private float _damageRadius;

    [SerializeField]
    private float _damage = 10f;

    [Serializable]
    struct TowerSpriteInfo
    {
        public Towers towerEnum;
        public Sprite towerSprite;
    }

    [SerializeField]
    private TowerSpriteInfo[] _towerSprites;

    protected void Awake()
    {
        _timer = GetComponent<Timer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_audioPlayer == null)
        {
            _audioPlayer = GameObject.FindFirstObjectByType<AudioPlayer>();
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Init();
        ConstructBombDefusalSequence();
        _timer.SetInitialTime(6 * _defusalSequence.Count);
        InvokeRepeating("PlayTickSound", 0f, 1f);
    }

    private void Init()
    {
        _defusalSequence = new List<Towers>();
        _defusalSeqInstdSprites = new List<GameObject>();
        _playerTeleportHistory = new List<Towers>();
        _towerToSpriteMap = new Dictionary<Towers, Sprite>();

        //_towersMasterList = FindObjectOfType<LevelController>().GetTowersAtBeginning();
        Tower[] towersInScene = GameObject.FindObjectsOfType<Tower>();
        if (towersInScene.Length <2)
        {
            _towersMasterList = FindObjectOfType<LevelController>().GetTowersAtBeginning();
        }
        else
        {
            _towersMasterList = new();
            foreach (var towerInScene in towersInScene)
            {
                _towersMasterList.Add(towerInScene.GetThisTower());
            }
        }
        
        //_towersMasterList = Enum.GetValues(typeof(Towers)).Cast<Towers>().ToList();

        //Debug.Log("Towers master list: " + ConstructStringFromList(_towersMasterList));
        foreach (TowerSpriteInfo towerSpriteInfo in _towerSprites)
        {
            _towerToSpriteMap[towerSpriteInfo.towerEnum] = towerSpriteInfo.towerSprite;
        }

        _maxTimerPanelOpacity = _timerPanel.color.a;
    }

    private void ConstructBombDefusalSequence()
    {
        _playerTeleportHistory.Clear();
        _defusalSequence.Clear();
        foreach (GameObject child in _defusalSeqInstdSprites)
        {
            Destroy(child);
        }
        _defusalSeqInstdSprites.Clear();
        
        int seqLength = UnityEngine.Random.Range(2, 6);
        RectTransform defusalSeqPanelRect = _defusalSeqPanel.GetComponent<RectTransform>();
        defusalSeqPanelRect.sizeDelta = new Vector2(seqLength + 1, defusalSeqPanelRect.sizeDelta.y);

        List<Towers> towerListForNextPass = new(_towersMasterList);
        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        if (player1 != null )
        {
            Towers p1CurrentTower = player1.GetComponent<Teleporter>().GetCurrentTower();
            towerListForNextPass.Remove( p1CurrentTower );
        }

        for (int i = 0; i < seqLength; i++)
        {
            Towers randomTower = towerListForNextPass[UnityEngine.Random.Range(0, towerListForNextPass.Count)];
            _defusalSequence.Add(randomTower);

            GameObject randomTowerSprite = Instantiate(_defusalSeqSpriteObject, _defusalSeqPanel.transform);
            randomTowerSprite.GetComponent<Image>().sprite = _towerToSpriteMap[randomTower];
            _defusalSeqInstdSprites.Add(randomTowerSprite);

            towerListForNextPass = new List<Towers>(_towersMasterList);
            towerListForNextPass.Remove(randomTower);
        }

        //Debug.Log("Defusal sequence: " + ConstructStringFromList(_defusalSequence));
    }

    //To print list in debug logs
    private string ConstructStringFromList(List<Towers> towerList)
    {
        string towersMasterListLog = "";
        foreach (Towers towerENum in towerList)
        {
            towersMasterListLog += towerENum.ToString() + " ";

        }
        return towersMasterListLog;
    }

    private void PlayTickSound()
    {
        if (_audioPlayer == null)
        {
            return;
        }
        _audioPlayer.PlaySFX(_tickSound);
    }

    private List<Towers> GetNElementsFromBack(int n, List<Towers> towersList)
    {
        List<Towers> resultList = new();
        if (n >= towersList.Count)
        {
            return towersList;
        }

        for (int i = towersList.Count - 1; n > 0; i--, n--) {
            resultList.Add(towersList[i]);
        }
        resultList.Reverse();
        return resultList;
    }

    private bool AreListsEqualBackwards(List<Towers> l1, List<Towers> l2)
    {
        if (l1.Count != l2.Count) { 
            return false; 
        }

        for (int i = l1.Count - 1; i > -1; i--)
        {
            if (l1[i] != l2[i])
            {
                return false;
            }
        }
        return true;
    }

    private void OnTimerFinished()
    {
        foreach (var colliderInDamageRadius in Physics2D.OverlapCircleAll(transform.position, _damageRadius))
        {
            Health bombVictim = colliderInDamageRadius.GetComponent<Health>();
            if (bombVictim != null && bombVictim.enabled)
            {
                //Debug.Log("Reducing health of bomb victim: "+bombVictim.gameObject.name);
                bombVictim.ReduceHealth(_damage);
            }
        }
        GameObject explosionObject = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        _audioPlayer.PlaySFX(_explosionSound);
        Destroy(explosionObject, 2f);
        Destroy(transform.root.gameObject);
    }

    private void OnLastFewSecsRemaining()
    {
        CancelInvoke("PlayTickSound");
        InvokeRepeating("PlayTickSound", 0f, 0.25f);
    }

    public void FadeOut(float fadeOutTime)
    {
        StartCoroutine(StartFadingOut(Time.time, fadeOutTime));
    }

    private IEnumerator StartFadingOut(float callTime, float fadeOutTime)
    {
        float coroutineRunningTime = Time.time - callTime;

        while (coroutineRunningTime <= fadeOutTime)
        {
            //Bomb sprite
            float opacity = Mathf.Lerp(1f, 0f, coroutineRunningTime / fadeOutTime);
            Color color = new(1,1,1,opacity);
            _spriteRenderer.color = color;

            //Timer panel
            Color timerColor = _timerPanel.color;
            timerColor.a = opacity * _maxTimerPanelOpacity;
            _timerPanel.color = timerColor;

            //Timer text
            timerColor = _timerText.color;
            timerColor.a = opacity;
            _timerText.color = timerColor;

            coroutineRunningTime = Time.time - callTime;
            yield return null;
        }
    }

    public void OnPlayerTeleport(Towers towerTeleportedTo)
    {
        if (_isBombDefused)
        {
            return;
        }

        _playerTeleportHistory.Add(towerTeleportedTo);
        //Debug.Log("Player teleprt history: " + ConstructStringFromList(_playerTeleportHistory));

        for (int i = _defusalSequence.Count; i > 0; i--)
        {
            if (!AreListsEqualBackwards(_defusalSequence.GetRange(0, i), GetNElementsFromBack(i, _playerTeleportHistory)))
            {
                continue;
            }

            //Debug.Log("Last " + i + " teleportations match the first " + i + " towers in defusal sequence.");
            if (i == _defusalSequence.Count)
            {
                DefuseBomb();
                return;
            }
            else
            {
                UpdateDefusalSeqSpritesOnSubseqMatch(i);
                return;
            }
        }

        //Debug.Log("Sequence reset.");
        //No subsequence in teleport history matched any subsequence in defusal sequence.
        //So reset the sprites.
        ResetDefusalSeqSpritesOpacity();
    }

    virtual protected void DefuseBomb()
    {
        //Debug.Log("Bomb defused.");
        _audioPlayer.PlaySFX(_defusedSound);
        _isBombDefused = true;
        Destroy(_defusalSeqPanel.gameObject);
        
        _timer.PauseTimer();
        CancelInvoke("PlayTickSound");

        transform.root.GetComponent<Animator>().enabled = false;
        _damageRadiusSprite.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        _spriteRenderer.sprite = _defusedBombSprite;
        FadeOut(_fadeOutTime);
        Destroy(transform.root.gameObject, _fadeOutTime);
        return;
    }

    private void UpdateDefusalSeqSpritesOnSubseqMatch(int i)
    {
        //fade i-1th child as all children before this will have already been faded.
        Color translucent = new(1, 1, 1, 0.5f);
        _defusalSeqInstdSprites[i - 1].GetComponent<Image>().color = translucent;
        //Also reset opacity of all children after this one.
        for (int j = i; j < _defusalSequence.Count; j++)
        {
            _defusalSeqInstdSprites[j].GetComponent<Image>().color = Color.white;
        }
    }

    private void ResetDefusalSeqSpritesOpacity()
    {
        foreach (var defusalSeqSprite in _defusalSeqInstdSprites)
        {
            defusalSeqSprite.GetComponent<Image>().color = Color.white;
        }
    }

    private void OnDestroy()
    {
        if (_enemySpawner == null)
        {
            return;
        }
        _enemySpawner.DeRegisterBomb();
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
        _enemySpawner.RegisterBomb(this);
    }

    public void Interact()
    {
        //No interact behavior
        return;
    }

    public virtual void DisplayInteractMsg()
    {
        if (_defusalSeqPanel != null)
        {
            _defusalSeqPanel.gameObject.SetActive(true);
        }
    }

    public void HideInteractMsg()
    {
        if (_defusalSeqPanel != null)
        {
            _defusalSeqPanel.gameObject.SetActive(false);
        }
    }
}
