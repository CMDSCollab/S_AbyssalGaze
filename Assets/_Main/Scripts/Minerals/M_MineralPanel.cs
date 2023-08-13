using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class M_MineralPanel : Singleton<M_MineralPanel>
{
    public Transform panel_Mineral;
   [HideInInspector] public List<OnPanelMineralData> onPanelMinerals = new List<OnPanelMineralData>();
    private bool isMineralOpened = false;
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    public void InitializeMineralPanel()
    {
        Transform layoutGroup = panel_Mineral.GetChild(0);
        MineralInfo[] minerals = M_Major.Instance.repository.minerals;

        for (int i = 0; i < 10; i++)
        {
            OnPanelMineralData newMData = new(
                minerals[i].type,
                layoutGroup.GetChild(i).GetComponentInChildren<TMPro.TMP_Text>(),
                layoutGroup.GetChild(i).GetComponent<Image>(),
                0
            );
            onPanelMinerals.Add(newMData);
            newMData.text.text = newMData.value.ToString();
            newMData.image.sprite = minerals[i].image;
        }
    }

    public void UpdateOnPanelMineralInfo(MineralType targetMineralType, int mineralToAdd)
    {
        foreach (OnPanelMineralData mineralData in onPanelMinerals)
            if (mineralData.type == targetMineralType) OnPanelMineralValueChange(mineralData, mineralToAdd);
    }

    private void OnPanelMineralValueChange(OnPanelMineralData targetData,int changeAmount)
    {
        targetData.value += changeAmount;
        targetData.text.text = targetData.value.ToString();
    }

    public void MineralPanel_Open()
    {
        panel_Mineral.DOScale(Vector3.one, 0.2f);
        isMineralOpened = true;
    }

    public void MineralPanel_Close()
    {
        panel_Mineral.DOScale(Vector3.zero, 0.2f);
        isMineralOpened = false;
    }

    public void OnMineralButtonClick()
    {
        if (isMineralOpened) MineralPanel_Close();
        else MineralPanel_Open();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    foreach (OnPanelMineralData panelMineral in onPanelMinerals)
        //    {
        //        OnPanelMineralValueChange(panelMineral, Random.Range(20, 80));
        //    }
        //}

        if (playerInput.actions["Mineral"].triggered)
        {
            if (isMineralOpened) MineralPanel_Close();
            else MineralPanel_Open();
        }
    }

    public void MineralsCheatChange()
    {
        foreach (OnPanelMineralData panelMineral in onPanelMinerals)
        {
            OnPanelMineralValueChange(panelMineral, Random.Range(20, 80));
        }
    }
}

public class OnPanelMineralData{
    public MineralType type;
    public TMPro.TMP_Text text;
    public Image image;
    public int value;

    public OnPanelMineralData(MineralType _type, TMPro.TMP_Text _text, Image _image, int _value)
    {
        type = _type;
        text = _text;
        image = _image;
        value = _value;
    }
}
