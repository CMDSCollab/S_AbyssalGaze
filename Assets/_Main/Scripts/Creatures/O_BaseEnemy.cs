using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_BaseEnemy : MonoBehaviour
{
    public float maxHealth;
    protected float currentHealth;
    public float moveSpeed;

    public float detectRadius;
    protected float detectCoolDown = 2;
    protected float detectTimer;
    protected bool isPlayerDetected = false;

    protected bool DetectIsPlayerInSight()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectRadius);
        foreach (var item in detectedColliders) if (item.CompareTag("Player")) return true;
        return false;
    }

    protected void DetectTimerReset()
    {
        detectTimer = detectCoolDown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    //protected virtual void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Bullet"))
    //    {
    //        Debug.Log("OnHit");
    //        currentHealth -= collision.gameObject.GetComponentInParent<O_Bullet>().damage;
    //        if (currentHealth <= 0)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    public void DamagedByLaser()
    {
        Debug.Log(currentHealth);
        currentHealth -= Time.deltaTime * 10;
        if (currentHealth <= 0)
        {
            Instantiate(FindObjectOfType<M_Firearm>().fx_Explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    protected void DamagedByBullet(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("OnHit");
            currentHealth -= collision.gameObject.GetComponentInParent<O_Bullet>().damage;
            if (currentHealth <= 0)
            {
                GameObject explosion = Instantiate(FindObjectOfType<M_Firearm>().fx_Explosion, transform.position, Quaternion.identity);
                Destroy(explosion, 2f);
                Destroy(gameObject);
            }
        }
    }
}
