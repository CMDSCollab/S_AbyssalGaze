using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class OE_Ranged : O_PatrolEnemy
{
    private float shootTimer;
    public GameObject pre_Bullet;

    void Start()
    {
        InitializeEnemy();
    }

    protected override void Update()
    {
        base.Update();
        if (!isSpecialPermitted)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer < 0)
            {
                isSpecialPermitted = true;
            }
        }
    }

    protected override void EnterSpecialAction()
    {
        shootTimer = UnityEngine.Random.Range(5f, 7f);
        EnemyAction = ShootAction;
    }

    private void ShootAction()
    {
        if (isSpecialPermitted)
        {
            ShootBullet(transform);
            isSpecialPermitted = false;
        }
        EnterRotate();

        void ShootBullet(Transform pos)
        {
            Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 lookDir = playerPos - enemyPos;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            Transform bullet = Instantiate(pre_Bullet, pos.position, Quaternion.Euler(90, 180, angle)).transform;
            //Transform bullet = Instantiate(pre_Bullet, pos.position, Quaternion.identity).transform;
            Vector3 direction = (M_Machine.Instance.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
        }
    }

    protected override Vector3 PlayerInRangeRotationSet()
    {
        Vector3 tempRotation = Vector3.zero;
        Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 lookDir = playerPos - enemyPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        int randomDegree = UnityEngine.Random.Range(0, 40);
        angle = UnityEngine.Random.Range(0, 10) > 4 ? angle + randomDegree : angle - randomDegree;
        tempRotation = new Vector3(90, 0, angle);

        return tempRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamagedByBullet(collision);
    }
}
