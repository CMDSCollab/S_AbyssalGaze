using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class M_Skill : Singleton<M_Skill>
{
    public CanvasGroup panel_Skill;
    public float speed_PanelOpen;
    public Transform panel_Upgrade;
    public GameObject pre_MineralRequire;
    private bool isSkillPanelOpen = false;
    private PlayerInput playerInput;
    private bool isFirstOpen = true;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
       
    void Update()
    {
        if (playerInput.actions["Skill"].triggered)
        {
            if (isSkillPanelOpen)
            {
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() => DOTween.To(() => panel_Skill.alpha, x => panel_Skill.alpha = x, 0, speed_PanelOpen));
                s.AppendInterval(speed_PanelOpen);
                s.AppendCallback(() => isSkillPanelOpen = false);
                s.AppendCallback(() => panel_Skill.gameObject.SetActive(false));
            }
            else
            {
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() => panel_Skill.gameObject.SetActive(true));
                s.AppendCallback(() => DOTween.To(() => panel_Skill.alpha, x => panel_Skill.alpha = x, 1, speed_PanelOpen));
                s.AppendInterval(speed_PanelOpen);
                s.AppendCallback(() => isSkillPanelOpen = true);
            }

            if (isFirstOpen)
            {
                FindObjectOfType<M_MineralPanel>().MineralsCheatChange();
                isFirstOpen = false;
            }
        }
    
    }

    public void OpenUpgradePanel(SO_Skill targetSkill)
    {
        panel_Upgrade.DOScale(1, 0.2f);
        M_MineralPanel.Instance.MineralPanel_Open();
        panel_Upgrade.Find("Skill Icon").GetComponent<Image>().sprite = targetSkill.skillIcon;
        panel_Upgrade.Find("Skill Name").GetComponent<TMP_Text>().text = targetSkill.skillName;
        panel_Upgrade.Find("Skill Intro").GetComponent<TMP_Text>().text = targetSkill.skillDes;

        Transform requireParent = panel_Upgrade.Find("Requires");
        if (requireParent.childCount != 0)
        {
            for (int i = 0; i < requireParent.childCount; i++)
            {
                requireParent.GetChild(i).gameObject.SetActive(false);
                Destroy(requireParent.GetChild(i).gameObject, 0.1f);
            }
            requireParent.DetachChildren();
        }

        int targetRequireTypeCount = 0;
        foreach (UpgradeRequire require in targetSkill.upgradeRequires)
        {
            Sprite targetMineralImage = null;
            foreach (MineralInfo mineralData in M_Major.Instance.repository.minerals)
                if (mineralData.type == require.mineralType) targetMineralImage = mineralData.image;
            Transform newRequire = Instantiate(pre_MineralRequire, requireParent).transform;
            newRequire.GetComponent<Image>().sprite = targetMineralImage;
            newRequire.GetComponentInChildren<TMP_Text>().text = require.number.ToString();

            foreach (OnPanelMineralData onPanelMineral in M_MineralPanel.Instance.onPanelMinerals)
                if (onPanelMineral.type == require.mineralType && onPanelMineral.value >= require.number) targetRequireTypeCount++;
        }

        if (targetRequireTypeCount == targetSkill.upgradeRequires.Length)
        {
            if (!O_SkillUI.currentSelectedSkill.GetUnlockState())
                panel_Upgrade.Find("Button").GetComponent<Button>().interactable = true;
            else panel_Upgrade.Find("Button").GetComponent<Button>().interactable = false;
        }
        else panel_Upgrade.Find("Button").GetComponent<Button>().interactable = false;
    }

    public void CloseUpgradePanel()
    {
        M_MineralPanel.Instance.MineralPanel_Close();
        panel_Upgrade.DOScale(0, 0.2f);
    }

    public void UpgradeCertainSkill()
    {
        O_SkillUI.currentSelectedSkill.UnlockThisSkill();
        panel_Upgrade.Find("Button").GetComponent<Button>().interactable = false;
        foreach (UpgradeRequire require in O_SkillUI.currentSelectedSkill.thisSkillInfo.upgradeRequires)
            foreach (OnPanelMineralData onPanelMineral in M_MineralPanel.Instance.onPanelMinerals)
                if (require.mineralType == onPanelMineral.type)
                    M_MineralPanel.Instance.UpdateOnPanelMineralInfo(require.mineralType, -require.number);
    }
}
