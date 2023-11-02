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
    private float fillTime = 0.6f;

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
        transform.Find("Icon").GetComponent<Image>().color = Color.black;
        //transform.GetComponent<Image>().color = Color.black;
    }

    public void UnlockThisSkill()
    {
        //Image uiBG = currentSelectedSkill.GetComponent<Image>();
        //DOTween.To(() => uiBG.fillAmount, x => uiBG.fillAmount = x, 1, 0.3f);
        StartCoroutine(UnlockingEffect());
        isUnlocked = true;
    }

    IEnumerator UnlockingEffect()
    {
        if(transform.childCount == 7)
        {
            LineFill(transform.GetChild(6));
            yield return new WaitForSeconds(fillTime);
        }
        LineFill(transform.GetChild(0));
        yield return new WaitForSeconds(fillTime);

        Image frame = transform.Find("Frame Fill").GetComponent<Image>();
        DOTween.To(() => frame.fillAmount, x => frame.fillAmount = x, 1, fillTime);
        yield return new WaitForSeconds(fillTime);
        transform.Find("Icon").GetComponent<Image>().color = M_Major.Instance.orange;

        void LineFill(Transform targetLine)
        {
            Image targetImage = targetLine.GetChild(0).GetComponent<Image>();
            DOTween.To(() => targetImage.fillAmount, x => targetImage.fillAmount = x, 1, fillTime);
        }
    }

    public bool GetUnlockState()
    {
        return isUnlocked;
    }

    void OnValidate()
    {
        if (thisSkillInfo != null)
        {
            transform.Find("Icon").GetComponent<Image>().sprite = thisSkillInfo.skillIcon;
        }
        else
        {
            transform.Find("Icon").GetComponent<Image>().sprite = null;
        }
    }
}
