using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class M_MiningGame : Singleton<M_MiningGame>
{
    private Transform miningPanel;
    private RectTransform upperBar;
    private RectTransform greenLeft;
    private RectTransform greenRight;
    private RectTransform redLeft;
    private RectTransform refRight;
    private RectTransform bottomBar;
    private RectTransform bottomGreen;

    private Image img_Mineral;
    private TMPro.TMP_Text text_Number;

    private bool isOnMining = false;

    void Start()
    {
        VariablesAchieve();
    }

    void Update()
    {
        
    }

    void VariablesAchieve()
    {
        miningPanel = GameObject.Find("Canvas").transform.Find("P_Mining");
        upperBar = miningPanel.Find("Upper Bar").GetComponent<RectTransform>();
        greenLeft = upperBar.transform.Find("Green Left").GetComponent<RectTransform>();
        greenRight = upperBar.transform.Find("Green Right").GetComponent<RectTransform>();
        redLeft = upperBar.transform.Find("Red Left").GetComponent<RectTransform>();
        refRight = upperBar.transform.Find("Red Right").GetComponent<RectTransform>();
        bottomBar = miningPanel.Find("Bottom Bar").GetComponent<RectTransform>();
        bottomGreen = bottomBar.transform.Find("Green").GetComponent<RectTransform>();

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
        miningPanel.DOScale(1, 0.5f);
        isOnMining = true;
    }

    void CloseMiningPanel()
    {
        miningPanel.DOScale(0, 0.5f);
        isOnMining = false;
    }

    void RefleshOnPanelElements(MineralType targetMineral)
    {
        //MineralInfo mInfo = 
        //img_Mineral.sprite = targetMineral.image;
        text_Number.text = 0.ToString();
    }
}
