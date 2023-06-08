using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class OE_Melee : O_PatrolEnemy
{
    private Transform wingLeft;
    private Transform wingRight;
    public float wingSpeed;
    private bool isWingUpperwards = true;
    private bool isWingStageFinished = true;

    public float rushSpeed;
    private float rushTimer;

    void Start()
    {
        wingLeft = transform.Find("Wing Left");
        wingRight = transform.Find("Wing Right");
        InitializeEnemy();
    }

    protected override void Update()
    {
        base.Update();
        if (isWingStageFinished) WingAction(isWingUpperwards, wingSpeed);
    }

    private void WingAction(bool isUpperwards, float targetSpeed)
    {
        isWingStageFinished = false;
        Sequence s = DOTween.Sequence();
        s.AppendInterval(targetSpeed);
        s.AppendCallback(() => isWingStageFinished = true);

        if (isUpperwards)
        {
            wingLeft.DOLocalRotate(new Vector3(0, -12, -180), targetSpeed);
            wingRight.DOLocalRotate(new Vector3(0, 12, -180), targetSpeed);
            isWingUpperwards = false;
        }
        else
        {
            wingLeft.DOLocalRotate(new Vector3(0, 40, -180), targetSpeed);
            wingRight.DOLocalRotate(new Vector3(0,-40, -180), targetSpeed);
            isWingUpperwards = true;
        }
    }

    protected override void EnterSpecialAction()
    {
        rushTimer = UnityEngine.Random.Range(0.5f, 1.5f);
        EnemyAction = RushAction;
    }

    private void RushAction()
    {
        rushTimer -= Time.deltaTime;
        if (rushTimer > 0) transform.position += transform.up * Time.deltaTime * rushSpeed;
        else EnterRotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("OnHit");
            currentHealth -= collision.gameObject.GetComponentInParent<O_Bullet>().damage;
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("HitPlayer");
        }
    }
}
