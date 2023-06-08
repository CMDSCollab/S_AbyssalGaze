using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OE_Turret : O_BaseEnemy
{
    public float shootCoolDown;
    private float shootTimer;
    public GameObject pre_Bullet;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        detectTimer -= Time.deltaTime;
        if (detectTimer <= 0)
        {
            isPlayerDetected = DetectIsPlayerInSight();
            DetectTimerReset();
        }

        if (isPlayerDetected)
        {
            LookAtPlayer();
            ShootAction();
        }
    }

    protected void LookAtPlayer()
    {
        Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 lookDir = playerPos - enemyPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, 0, angle), 0.1f);
        //transform.rotation = Quaternion.Euler(90, 0, angle);
    }

    private void ShootAction()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer<0)
        {
            ShootBullet(transform);
            shootTimer = shootCoolDown;
        }

        void ShootBullet(Transform pos)
        {
            Transform bullet = Instantiate(pre_Bullet, pos.position, Quaternion.identity).transform;
            Vector3 direction = (M_Machine.Instance.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
        }
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
    }
}
