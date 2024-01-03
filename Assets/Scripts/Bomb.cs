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

    private void Init()
    {
        _defusalSequence = new List<Towers>();
        _defusalSeqInstdSprites = new List<GameObject>();
        _towersMasterList = Enum.GetValues(typeof(Towers)).Cast<Towers>().ToList();
        Debug.Log("Towers master list: " + ConstructStringFromList(_towersMasterList));
        _towerToSpriteMap = new Dictionary<Towers, Sprite>();

        foreach (TowerSpriteInfo towerSpriteInfo in _towerSprites)
        {
            _towerToSpriteMap[towerSpriteInfo.towerEnum] = towerSpriteInfo.towerSprite;
        }
    }

    private void _ConstructBomDefusalSequence()
    {
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

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            _ConstructBomDefusalSequence();
        }*/
    }
}
