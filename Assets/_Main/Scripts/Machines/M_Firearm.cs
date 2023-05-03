using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Firearm : MonoBehaviour
{
    public Transform parent_Firearm;
    public Transform parent_Rotation;
    public GameObject[] firearmList;
    public GameObject pre_Bullet;
    private Transform currentFirearm;

    private Vector3 aimDirection;

    void Start()
    {
        EquipWeapon(0);
    }

    // Update is called once per frame
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
    }

    void EquipWeapon(int weaponIndex)
    {
        currentFirearm = Instantiate(firearmList[weaponIndex], parent_Firearm).transform;
    }

    void Firing()
    {
        GameObject bullet = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle"));
        bullet.GetComponent<O_Bullet>().BulletSetUp(aimDirection,5);
    }
    //float Angle_360(Vector3 from_, Vector3 to_)
    //{
    //    //两点的x、y值
    //    float x = from_.x - to_.x;
    //    float z = from_.z - to_.z;

    //    //斜边长度
    //    float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(z, 2f));

    //    //求出弧度
    //    float cos = x / hypotenuse;
    //    float radian = Mathf.Acos(cos);

    //    //用弧度算出角度    
    //    float angle = 180 / (Mathf.PI / radian);

    //    if (z < 0) angle = -angle;
    //    else if ((z == 0) && (x < 0)) angle = 180;

    //    return angle;
    //}
}
