using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class M_MiningGame : Singleton<M_MiningGame>
{
    private Transform miningPanel;
    private RectTransform upperBar;

    private RectTransform greenLeft;
    private RectTransform greenRight;
    private float greenWidth;

    private RectTransform redLeft;
    private RectTransform redRight;
    private float redInGreenMaxXPos;
    private float redInGreenMinXPos;

    private RectTransform bottomBar;
    private RectMask2D bottomBarMask;
    private Vector4 defaultPadding;

    private bool isGameRunning = false;

    private Image img_Mineral;
    private TMPro.TMP_Text text_Number;
    private float shrinkSpeed;
    private float miningTime;
    private float miningTimer;
    private float inGreenTimer;
    private int mineralToGet;

    public float XPosToExpandPerPress = 40;
    public int maxDifficulty = 10;
    public int greenWidthMax = 150;
    public int greenWidthMin = 50;
    public int twoSideOffset = 10;

    public Action GameEnd;

    void Start()
    {
        GetReferences();
        GameEnd += GameEndFeedback;
        GameEnd += CloseMiningPanel;
    }

    void Update()
    {
        if (isGameRunning)
        {
            redRight.anchoredPosition -= new Vector2(shrinkSpeed, 0) * Time.deltaTime * 10;
            redLeft.anchoredPosition += new Vector2(shrinkSpeed, 0) * Time.deltaTime * 10;

            if (redRight.anchoredPosition.x <= 0)
            {
                redRight.anchoredPosition = new Vector2(0, 0);
                redLeft.anchoredPosition = new Vector2(0, 0);
            }

            if (redRight.anchoredPosition.x < redInGreenMaxXPos && redRight.anchoredPosition.x > redInGreenMinXPos)
            {
                inGreenTimer += Time.deltaTime;
                mineralToGet = Mathf.RoundToInt(inGreenTimer);
                UpdateMineralToGetNumber(mineralToGet);
            }

            miningTimer -= Time.deltaTime ;

            if (miningTimer <= 0) GameEnd();
            else
            {
                float timeProportion = miningTimer / miningTime;
                bottomBarMask.padding = defaultPadding * timeProportion;
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                redRight.anchoredPosition += new Vector2(XPosToExpandPerPress, 0);
                redLeft.anchoredPosition-= new Vector2(XPosToExpandPerPress, 0);
            }
        }
    }

    void GetReferences()
    {
        miningPanel = GameObject.Find("Canvas").transform.Find("P_Mining");
        upperBar = miningPanel.Find("Upper Bar").GetComponent<RectTransform>();

        greenLeft = upperBar.transform.Find("Green Left").GetComponent<RectTransform>();
        greenRight = upperBar.transform.Find("Green Right").GetComponent<RectTransform>();

        redLeft = upperBar.transform.Find("Red Left").GetComponent<RectTransform>();
        redRight = upperBar.transform.Find("Red Right").GetComponent<RectTransform>();

        bottomBar = miningPanel.Find("Bottom Bar").GetComponent<RectTransform>();
        bottomBarMask = bottomBar.GetComponent<RectMask2D>();

        img_Mineral = miningPanel.Find("Mineral Image").GetComponent<Image>();
        text_Number = miningPanel.Find("Mineral Number").GetComponent<TMPro.TMP_Text>();
    }

    public void StartMining(MineralType targetMineral)
    {
        RefleshOnPanelElements(targetMineral);
        OpenMiningPanel();
    }

    void OpenMiningPanel()
    {
        Sequence s = DOTween.Sequence();
        s.Append(miningPanel.DOScale(1, 0.5f));
        s.AppendInterval(1f);
        s.AppendCallback(() => isGameRunning = true);
        M_Machine.Instance.isOnMining = true;
    }

    void GameEndFeedback()
    {
        isGameRunning = false;
    }

    void CloseMiningPanel()
    {
        miningPanel.DOScale(0, 0.5f);
        M_Machine.Instance.isOnMining = false;
    }

    void RefleshOnPanelElements(MineralType targetMineral)
    {
        MineralInfo mInfo = GetMineralInfoBaseOnType(targetMineral);
        float upperBarWidth = upperBar.rect.width;

        float greenWidthPerDifficulty = (greenWidthMax - greenWidthMin) / maxDifficulty;
        greenWidth = (maxDifficulty - mInfo.difficulty) * greenWidthPerDifficulty + greenWidthMin;
        float greenXPosMax = upperBarWidth / 2 - greenWidth / 2 - twoSideOffset;
        float greenXPosMin = greenWidth / 2 + twoSideOffset;
        float greenTargetXPos = UnityEngine.Random.Range(greenXPosMin, greenXPosMax);
        greenRight.anchoredPosition = new Vector2(greenTargetXPos, 0);
        greenRight.sizeDelta = new Vector2(greenWidth, greenRight.rect.height);
        greenLeft.anchoredPosition = new Vector2(-greenTargetXPos, 0);
        greenLeft.sizeDelta = new Vector2(greenWidth, greenRight.rect.height);

        float redXPosMax = upperBar.rect.width/2 - redRight.rect.width / 2;
        redRight.anchoredPosition = new Vector2(redXPosMax, 0);
        redLeft.anchoredPosition = new Vector2(-redXPosMax, 0);
        redInGreenMaxXPos = greenRight.anchoredPosition.x + greenRight.rect.width / 2 + redRight.rect.width / 2;
        redInGreenMinXPos = greenRight.anchoredPosition.x - greenRight.rect.width / 2 - redRight.rect.width / 2;

        defaultPadding = new Vector4(bottomBar.rect.width / 2,0, bottomBar.rect.width / 2, 0);
        bottomBarMask.padding = defaultPadding;

        shrinkSpeed = mInfo.shrinkSpeed;
        miningTime = mInfo.miningTime;
        miningTimer = miningTime;
        inGreenTimer = 0;
        mineralToGet = 0;

        img_Mineral.sprite = mInfo.image;
        UpdateMineralToGetNumber(mineralToGet);

        MineralInfo GetMineralInfoBaseOnType(MineralType targetMineral)
        {
            foreach (MineralInfo mineralInfo in M_Major.Instance.repository.minerals)
                if (mineralInfo.type == targetMineral) return mineralInfo;
            return null;
        }
    }

    void UpdateMineralToGetNumber(int targetNumber)
    {
        text_Number.text = mineralToGet.ToString();
    }
}
