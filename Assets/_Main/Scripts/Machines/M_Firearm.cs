using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Firearm : MonoBehaviour
{
    public Transform parent_Firearm;
    public Transform parent_Rotation;
    public GunList[] firearmList;

    public GameObject pre_Bullet;
    private Transform currentFirearm;
    private static GunType currentType;
    private static int currentIndex;

    private Vector3 aimDirection;

    void Start()
    {
        EquipWeapon(0);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            aimDirection = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z);
            float angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
            parent_Rotation.localRotation = Quaternion.Euler(0, angle, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Firing();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(1);
        }
    }

    void EquipWeapon(int weaponIndex)
    {
        if (currentFirearm != null) Destroy(currentFirearm.gameObject);

        currentFirearm = Instantiate(firearmList[weaponIndex].pre_Gun, parent_Firearm).transform;
        currentType = firearmList[weaponIndex].gunType;
        currentIndex = weaponIndex;
    }

    void Firing()
    {
        switch (currentType)
        {
            case GunType.Rifle:
                GameObject bullet = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle"));
                bullet.GetComponent<O_Bullet>().BulletSetUp(aimDirection, 5);
                break;
            case GunType.Shot:
                for (int i = 0; i < 5; i++)
                {
                    GameObject shotBullet = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle"));
                    if (i < 2) shotBullet.transform.Rotate(Vector3.forward, -20 * (i + 1));
                    else if (i > 2) shotBullet.transform.Rotate(Vector3.forward, 20 * (i - 2));

                    shotBullet.GetComponent<O_Bullet>().BulletSetUp(shotBullet.transform.up, 5);
                }
                break;
            case GunType.Mini:
                GameObject miniBulletLeft = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle Left"));
                GameObject miniBulletRight = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle Right"));
                miniBulletLeft.GetComponent<O_Bullet>().BulletSetUp(aimDirection, 5);
                miniBulletRight.GetComponent<O_Bullet>().BulletSetUp(aimDirection, 5);
                break;
            case GunType.Laser:
                break;
        }
    }

    void SwitchWeapon(int IndexChangeAmount)
    {
        currentIndex += IndexChangeAmount;
        if (currentIndex > 3) currentIndex = 0;
        EquipWeapon(currentIndex);
    }
}

[System.Serializable]
public class GunList
{
    public GameObject pre_Gun;
    public GunType gunType;
}

public enum GunType { Rifle, Shot, Mini, Laser }