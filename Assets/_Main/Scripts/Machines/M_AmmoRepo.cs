using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M_AmmoRepo : Singleton<M_AmmoRepo>
{
    public Slider slider_Ammo;
    public int maxAmmoNum = 8;
    private float currentAmmoNum;
    public float GetAmmo { get { return currentAmmoNum; } }
    public float laserComsumingSpeed = 10;
    public float bulletConsumingSpeed = 1;
    public float ammoRecoverySpeed = 5;

    void Start()
    {
        currentAmmoNum = maxAmmoNum;
        slider_Ammo.maxValue = maxAmmoNum;
        slider_Ammo.value = currentAmmoNum;
    }

    void Update()
    {
        AmmoRecovery();
    }

    public void BulletConsume()
    {
        currentAmmoNum -= bulletConsumingSpeed;
        slider_Ammo.value = currentAmmoNum;
    }

    public void LaserConsume()
    {
        currentAmmoNum -= Time.deltaTime * laserComsumingSpeed;
        slider_Ammo.value = currentAmmoNum;
    }

    private void AmmoRecovery()
    {
        currentAmmoNum += Time.deltaTime * ammoRecoverySpeed;
        if (currentAmmoNum > maxAmmoNum) currentAmmoNum = maxAmmoNum;
        int integerAmmo = Mathf.FloorToInt(currentAmmoNum);
        slider_Ammo.value = integerAmmo;
    }
}
