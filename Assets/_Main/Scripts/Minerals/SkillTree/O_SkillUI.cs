using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class O_SkillUI : MonoBehaviour,IPointerClickHandler
{
    public SO_Skill thisSkillInfo;
    private bool isUnlocked;

    public void OnPointerClick(PointerEventData eventData)
    {
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
        transform.GetComponent<Image>().color = Color.black;
    }
}
