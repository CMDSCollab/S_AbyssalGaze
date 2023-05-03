using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Bullet : MonoBehaviour
{
    private Vector3 shootDir;
    private float bulletSpeed;

    public void BulletSetUp(Vector3 shootDirection,float speed)
    {
        shootDir = shootDirection;
        bulletSpeed = speed;
        transform.SetParent(null);
    }

    private void Update()
    {
        float moveSpeed = bulletSpeed;
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
