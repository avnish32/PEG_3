using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    private List<Towers> _defusalSequence;
    private List<Towers> _towersMasterList;
    private Dictionary<Towers, Sprite> _towerToSpriteMap;
    private List<GameObject> _defusalSeqInstdSprites;
    private List<Towers> _playerTeleportHistory;

    [SerializeField]
    private GameObject _defusalSeqSpriteObject;

    [SerializeField]
    private RectTransform _defusalSeqPanel;

    [Serializable]
    struct TowerSpriteInfo
    {
        public Towers towerEnum;
        public Sprite towerSprite;
    }

    [SerializeField]
    private TowerSpriteInfo[] _towerSprites;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        _ConstructBomDefusalSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _ConstructBomDefusalSequence();
        }
    }

    private void Init()
    {
        _defusalSequence = new List<Towers>();
        _defusalSeqInstdSprites = new List<GameObject>();
        _playerTeleportHistory = new List<Towers>();
        _towersMasterList = new List<Towers>();
        _towerToSpriteMap = new Dictionary<Towers, Sprite>();

        Tower[] towersInScene = GameObject.FindObjectsOfType<Tower>();
       for (int i = 0; i < towersInScene.Length; i++)
        {
            _towersMasterList.Add(towersInScene[i].GetThisTower());
        }

        //_towersMasterList = Enum.GetValues(typeof(Towers)).Cast<Towers>().ToList();

        Debug.Log("Towers master list: " + ConstructStringFromList(_towersMasterList));
        foreach (TowerSpriteInfo towerSpriteInfo in _towerSprites)
        {
            _towerToSpriteMap[towerSpriteInfo.towerEnum] = towerSpriteInfo.towerSprite;
        }
    }

    private void _ConstructBomDefusalSequence()
    {
        _playerTeleportHistory.Clear();
        _defusalSequence.Clear();
        foreach (GameObject child in _defusalSeqInstdSprites)
        {
            Destroy(child);
        }
        _defusalSeqInstdSprites.Clear();
        
        int seqLength = UnityEngine.Random.Range(2, 7);
        RectTransform defusalSeqPanelRect = _defusalSeqPanel.GetComponent<RectTransform>();
        defusalSeqPanelRect.sizeDelta = new Vector2(seqLength + 1, defusalSeqPanelRect.sizeDelta.y);

        List<Towers> towerListForNextPass = new List<Towers>(_towersMasterList);


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

        Debug.Log("Defusal sequence: " + ConstructStringFromList(_defusalSequence));
    }

    private string ConstructStringFromList(List<Towers> towerList)
    {
        string towersMasterListLog = "";
        foreach (Towers towerENum in towerList)
        {
            towersMasterListLog += towerENum.ToString() + " ";

        }
        return towersMasterListLog;
    }

    private List<Towers> GetNElementsFromBack(int n, List<Towers> towersList)
    {
        List<Towers> resultList = new List<Towers>();
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

    public void OnPlayerTeleport(Towers towerTeleportedTo)
    {
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
                Debug.Log("Bomb defused.");
                return;
            }
            else
            {
                //fade i-1th child as all children before this will have already been faded.
                Color translucent = new Color(1, 1, 1, 0.5f);
                _defusalSeqInstdSprites[i - 1].GetComponent<Image>().color = translucent;
                //Also reset opacity of all children after this one.
                for (int j = i; j < _defusalSequence.Count; j++)
                {
                    _defusalSeqInstdSprites[j].GetComponent<Image>().color = Color.white;
                }
                return;
            }
        }

        //Debug.Log("Sequence reset.");
        //No subsequence in teleport history matched any subsequence in defusal sequence.
        //So reset the sprites.
        ResetDefusalSeqSpritesOpacity();
    }

    private void ResetDefusalSeqSpritesOpacity()
    {
        foreach (var defusalSeqSprite in _defusalSeqInstdSprites)
        {
            defusalSeqSprite.GetComponent<Image>().color = Color.white;
        }
    }
}
