using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class O_SkillUI : MonoBehaviour,IPointerClickHandler
{
    public SO_Skill thisSkillInfo;
    public static O_SkillUI currentSelectedSkill;
    private bool isUnlocked;

    public void OnPointerClick(PointerEventData eventData)
    {
        currentSelectedSkill = this;
        M_Skill.Instance.OpenUpgradePanel(thisSkillInfo);
    }

    void Start()
    {
        InitializeSkill();
    }

    void InitializeSkill()
    {
        transform.Find("Icon").GetComponent<Image>().sprite = thisSkillInfo.skillIcon;
        transform.Find("Skill Color").GetComponent<Image>().color = 
            (thisSkillInfo.SkillInfo.skillType == SkillType.Unlock) ? Color.blue : Color.white;
        //transform.GetComponent<Image>().color = Color.black;
    }

    public void UnlockThisSkill()
    {
        Image uiBG = currentSelectedSkill.GetComponent<Image>();
        DOTween.To(() => uiBG.fillAmount, x => uiBG.fillAmount = x, 1, 0.3f);
        isUnlocked = true;
    }

    public bool GetUnlockState()
    {
        return isUnlocked;
    }
}
