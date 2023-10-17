using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class M_MachineValue : Singleton<M_MachineValue>
{
    public float maxOxygen;
    private float currentOxygen;
    public Slider slider_Oxygen;
    public TMPro.TMP_Text text_Oxygen;

    public float oxygenToDecreaseOnHit;
    public float MineralOxygenAmount;

    public Image hpImage;

    public Image[] hpDots;
    //private int hpDecrease = 0;
    private int dotHp = 0;
    private float noDamgeTimer = 1f;

    void Start()
    {
        //slider_Oxygen.maxValue = maxOxygen;
        currentOxygen = maxOxygen;
        hpImage.fillAmount = 1;
        //slider_Oxygen.value = currentOxygen;
    }

    private void Update()
    {
        //currentOxygen -= Time.deltaTime;
        SliderTextValueSync();
        if (currentOxygen <= 0) SceneManager.LoadScene(0);
        noDamgeTimer -= Time.deltaTime;
    }

    private void SliderTextValueSync()
    {
        slider_Oxygen.value = currentOxygen;
        text_Oxygen.text = currentOxygen.ToString("f0") + " / " + maxOxygen.ToString();
    }

    //public void OxygenDecrease(object obj)
    //{
    //    currentOxygen -= oxygenToDecreaseOnHit;
    //    if (currentOxygen > maxOxygen) currentOxygen = 0;
    //}

    //public void OxygenIncrease(object obj)
    //{
    //    currentOxygen += MineralOxygenAmount;
    //    if (currentOxygen > maxOxygen) currentOxygen = maxOxygen;
    //}

    public void HPDecrease(int amount)
    {
        currentOxygen -= amount;
        UpdateDottedHP();
        //UpdateDottedHP(currentOxygen / maxOxygen);
        //hpImage.fillAmount = currentOxygen / maxOxygen;

        if (currentOxygen<=0 || dotHp >= 7)
        {
            Debug.Log("Player Died");
            FindObjectOfType<M_BossFight>().AllGameBlack();
        }
    }

    public void DamagedByLaser()
    {
        currentOxygen -= Time.deltaTime;
        UpdateDottedHP();
        //UpdateDottedHP(currentOxygen / maxOxygen);
        //hpImage.fillAmount = currentOxygen / maxOxygen;
        if (currentOxygen <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateDottedHP()
    {
        //int hpLevel = percentage switch
        //{
        //    >= 0.857f and < 1 => 0,
        //    >= 0.714f and < 0.857f => 1,
        //    >= 0.571f and < 0.714f => 2,
        //    >= 0.428f and < 0.571f => 3,
        //    >= 0.285f and < 0.428f => 4,
        //    >= 0.142f and < 0.285f => 5,
        //    <= 0 => 6,
        //    _ => 6
        //};
        //if (hpLevel != hpDecrease)
        //{
        //    DOTween.To(() => hpDots[hpLevel].color, x => hpDots[hpLevel].color = x, new Color(0, 0, 0, 0), 0.3f);
        //    hpDecrease = hpLevel;
        //}
        if (noDamgeTimer < 0f)
        {
            DOTween.To(() => hpDots[dotHp].color, x => hpDots[dotHp].color = x, new Color(255, 0, 0, 255), 0.3f).OnComplete(() => dotHp++);
            //Debug.Log(dotHp);
            //dotHp++;
            noDamgeTimer = 1f;
        }
    }
}

[System.Serializable]
public class HPInfo
{
    public Image targetPart;
    public Sprite damagedState;
}
