using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class O_PatrolEnemy : O_BaseEnemy
{
    public Action EnemyAction;
    public float rotateSpeed;
    public float specialActionRange;
    private float patrolTimer;
    protected bool isSpecialPermitted = true;

    protected virtual void Update()
    {
        detectTimer -= Time.deltaTime;
        if (detectTimer <= 0)
        {
            isPlayerDetected = DetectIsPlayerInSight();
            DetectTimerReset();
        }
        if (EnemyAction != null) EnemyAction();
    }

    protected void InitializeEnemy()
    {
        currentHealth = maxHealth;
        transform.rotation = Quaternion.Euler(90, 0, UnityEngine.Random.Range(0, 180));
        RotateAction();
    }

    private void RotateAction()
    {
        bool isPlayerInRange = DetectIsPlayerInSight();
        Vector3 targetRotation = Vector3.zero;

        if (isPlayerInRange)
        {
            targetRotation = PlayerInRangeRotationSet();
        }
        else
        {
            targetRotation = new Vector3(90, 0, transform.rotation.z + UnityEngine.Random.Range(-40, 40));
        }

        Sequence s = DOTween.Sequence();
        s.Append(transform.DORotate(targetRotation, rotateSpeed));
        s.AppendCallback(() => RotateFinished());

        void RotateFinished()
        {
            if (isPlayerInRange) EnterTrace();
            else EnterPatrol();
        }
    }

    protected virtual Vector3 PlayerInRangeRotationSet()
    {
        Vector3 tempRotation = Vector3.zero;
        Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
        Vector2 meleePos = new Vector2(transform.position.x, transform.position.z);
        Vector2 lookDir = playerPos - meleePos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        tempRotation = new Vector3(90, 0, angle);

        return tempRotation;
    }

    private void EnterPatrol()
    {
        patrolTimer = UnityEngine.Random.Range(2f, 3f);
        EnemyAction = PatrolAction;
    }

    private void EnterTrace()
    {
        EnemyAction = TraceAction;
    }
    protected void EnterRotate()
    {
        EnemyAction = null;
        RotateAction();
    }

    private void PatrolAction()
    {
        patrolTimer -= Time.deltaTime;
        if (patrolTimer > 0) transform.position += transform.up * Time.deltaTime * moveSpeed;
        else EnterRotate();
    }

    private void TraceAction()
    {
        if (isPlayerDetected)
        {
            if (Vector3.Distance(M_Machine.Instance.transform.position, transform.position) > specialActionRange)
            {
                LookAtPlayer();
                transform.position += transform.up * Time.deltaTime * moveSpeed;
            }
            else
            {

                if (isSpecialPermitted)
                {
                    LookAtPlayer();
                    EnterSpecialAction();
                }

                else transform.position += transform.up * Time.deltaTime * moveSpeed;
            }
        }
        else EnterRotate();
    }

    protected virtual void EnterSpecialAction() { }

    protected void LookAtPlayer()
    {
        Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 lookDir = playerPos - enemyPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, 0, angle), 0.1f);
        transform.rotation = Quaternion.Euler(90, 0, angle);
    }
}
