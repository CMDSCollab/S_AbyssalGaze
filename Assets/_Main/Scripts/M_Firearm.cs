using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Firearm : MonoBehaviour
{
    public Transform parent_Firearm;
    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            Vector3 direction = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //float angle= Angle_360(new Vector3(hit.point.x, 0, hit.point.z), new Vector3(transform.position.x, 0, transform.position.z));
            parent_Firearm.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }

    float Angle_360(Vector3 from_, Vector3 to_)
    {
        //两点的x、y值
        float x = from_.x - to_.x;
        float z = from_.z - to_.z;

        //斜边长度
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(z, 2f));

        //求出弧度
        float cos = x / hypotenuse;
        float radian = Mathf.Acos(cos);

        //用弧度算出角度    
        float angle = 180 / (Mathf.PI / radian);

        if (z < 0) angle = -angle;
        else if ((z == 0) && (x < 0)) angle = 180;

        return angle;
    }
}
