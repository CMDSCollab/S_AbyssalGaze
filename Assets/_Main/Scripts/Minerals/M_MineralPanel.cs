using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_MineralPanel : Singleton<M_MineralPanel>
{
    public Transform panel_Mineral;
    private List<OnPanelMineralData> onPanelMinerals = new List<OnPanelMineralData>();

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

    public void UpdateOnPanelMineralInfo()
    {

    }
}

public class OnPanelMineralData{
    public MineralType type;
    public TMPro.TMP_Text text;
    public Image image;
    public int value;

    public OnPanelMineralData(MineralType _type,TMPro.TMP_Text _text,Image _image,int _value)
    {
        type = _type;
        text = _text;
        image = _image;
        value = _value;
    }
}
