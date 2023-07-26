using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class M_MachineValue : Singleton<M_MachineValue>
{
    public float maxOxygen;
    private float currentOxygen;
    public Slider slider_Oxygen;
    public TMPro.TMP_Text text_Oxygen;

    public float oxygenToDecreaseOnHit;
    public float MineralOxygenAmount;

    public Image hpImage;

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
        hpImage.fillAmount = currentOxygen / maxOxygen;

        if (currentOxygen<=0)
        {
            Debug.Log("Player Died");
            FindObjectOfType<M_BossFight>().AllGameBlack();
        }
    }

    public void DamagedByLaser()
    {
        currentOxygen -= Time.deltaTime;
        hpImage.fillAmount = currentOxygen / maxOxygen;
        if (currentOxygen <= 0)
        {
            Destroy(gameObject);
        }
    }
}
