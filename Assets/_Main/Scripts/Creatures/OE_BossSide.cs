using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OE_BossSide : MonoBehaviour
{
    public enum SideBossState { Across, EnterCircle, ShootBullet, ShootLaser, LeaveCircle }
    public float circleRange;
    public float outsightOffset;
    public SideBossState state = SideBossState.Across;
    private bool isEnteredState = false;
    public float moveSpeed;
    public float rushSpeed;
    public Transform lauchPoint;
    public GameObject pre_Bullet;

    public Transform muzzle;
    public LayerMask layer_Laser;
    private float amingPlayerAngle;
    private bool isDead = false;

    public float maxHealth;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        switch (state)
        {
            case SideBossState.Across:
                Behavior_Across();
                break;
            case SideBossState.EnterCircle:
                Behavior_EnterCircle();
                break;
            case SideBossState.ShootBullet:
                Behavior_ShootBullet();
                break;
            case SideBossState.ShootLaser:
                Behavior_ShootLaser();
                break;
            case SideBossState.LeaveCircle:
                Behavior_LeaveCircle();
                break;
        }

        TraceBackBehavior();
    }

    private void TraceBackBehavior()
    {
        float distance = Vector3.Distance(transform.position, M_Machine.Instance.transform.position);
        if (distance > circleRange + outsightOffset*2f && state != SideBossState.EnterCircle)
        {
            isEnteredState = false;
            state = SideBossState.EnterCircle;
        }
    }

    private void Behavior_Across()
    {
        Transform centerObject = M_Machine.Instance.transform;
        if (!isEnteredState)
        {
            Vector3 randomPosition = GenerateRandomPositionOnCircle();
            transform.position = randomPosition;
            LookAtPlayer();
            isEnteredState = true;
            transform.position += new Vector3(0, 1, 0);
        }
        else
        {
            transform.position += transform.up * rushSpeed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, centerObject.position);
            if (distance > circleRange + outsightOffset)
            {
                transform.position -= new Vector3(0, 1, 0);
                isEnteredState = false;
                state = SideBossState.EnterCircle;
            }
        }

        Vector3 GenerateRandomPositionOnCircle()
        {
            float outSightRadius = circleRange + outsightOffset;
            float angle = Random.Range(0f, 180f);
            float x = centerObject.position.x + outSightRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = centerObject.position.z + outSightRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            return new Vector3(x, transform.position.y, z);
        }
    }

    private void Behavior_EnterCircle()
    {
        if (!isEnteredState)
        {
            isEnteredState = true;
        }
        else
        {
            LookAtPlayer();
            transform.position += transform.up * moveSpeed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, M_Machine.Instance.transform.position);
            if (distance < circleRange)
            {
                isEnteredState = false;
                //state = SideBossState.ShootBullet;
                int random = Random.Range(0, 10);
                state = (random > 4) ? SideBossState.ShootBullet : SideBossState.ShootLaser;
            }
        }
    }

    private void Behavior_ShootBullet()
    {
        if (!isEnteredState)
        {
            //Debug.Log("ENtered Shoot");
            isEnteredState = true;
            Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 lookDir = playerPos - enemyPos;
            amingPlayerAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DORotate(new Vector3(90, 0, amingPlayerAngle - 30), 1f));
            s.AppendCallback(() => StartCoroutine(BulletShooting()));
            s.Append(transform.DORotate(new Vector3(90, 0, amingPlayerAngle + 30), 4f));
            s.AppendCallback(() => isEnteredState = false);
            s.AppendCallback(() => state = SideBossState.LeaveCircle);
        }
    }

    IEnumerator BulletShooting()
    {
        for (int i = 0; i < 100; i++)
        {
            if (isEnteredState)
            {
                Transform bullet = Instantiate(pre_Bullet, lauchPoint.position, Quaternion.Euler(90, 180, Random.Range(0, 360))).transform;
                bullet.GetComponent<Rigidbody>().AddForce(lauchPoint.transform.up * Random.Range(2, 5), ForceMode.Impulse);
                yield return new WaitForSeconds(0.4f);
            }
            else
            {
                break;
            }
        }
        yield return null;
    }

    private void Behavior_ShootLaser()
    {
        if (!isEnteredState)
        {
            isEnteredState = true;
            muzzle.gameObject.SetActive(true);

            Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
            Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 lookDir = playerPos - enemyPos;
            amingPlayerAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DORotate(new Vector3(90, 0, amingPlayerAngle - 30), 1f));
            s.Append(transform.DORotate(new Vector3(90, 0, amingPlayerAngle + 30), 4f));
            s.AppendCallback(() => muzzle.gameObject.SetActive(false));
            s.AppendCallback(() => isEnteredState = false);
            s.AppendCallback(() => state = SideBossState.LeaveCircle);
        }
        else
        {
            Transform beamStart = muzzle.Find("Beam");
            RaycastHit hit;
            Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layer_Laser);
            LineRenderer lr = muzzle.Find("Laser").GetComponent<LineRenderer>();
            float distance = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(beamStart.position.x, 0, beamStart.position.z)).magnitude;
            lr.SetPosition(1, new Vector3(0, 0, distance));
            muzzle.Find("BeamHit").transform.position = hit.point;

            if (hit.collider.gameObject.GetComponent<M_MachineValue>() != null)
            {
                hit.collider.gameObject.GetComponent<M_MachineValue>().DamagedByLaser();
            }
        }
    }

    private void Behavior_LeaveCircle()
    {
        transform.position -= transform.up * rushSpeed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, M_Machine.Instance.transform.position);
        if (distance > circleRange+outsightOffset) state = SideBossState.Across;
    }

    private void LookAtPlayer()
    {
        Vector2 playerPos = new Vector2(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.z);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 lookDir = playerPos - enemyPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        Vector3 tempRotation = new Vector3(90, 0, angle);
        transform.rotation = Quaternion.Euler(tempRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameObject hitExplosion = Instantiate(FindObjectOfType<M_Firearm>().fx_ExplosionSmall, collision.transform.position, Quaternion.identity);
            Destroy(hitExplosion, 2f);
            currentHealth -= collision.gameObject.GetComponentInParent<O_Bullet>().damage;
            Destroy(collision.transform.parent.gameObject);
            if (currentHealth <= 0 && !isDead) StartCoroutine(BossSideDefeated());
        }
    }

    public void DamagedByLaser()
    {
        //Debug.Log("Laser Damage");
        currentHealth -= Time.deltaTime * 10;
        if (currentHealth <= 0 && !isDead) StartCoroutine(BossSideDefeated());
    }

    IEnumerator BossSideDefeated()
    {
        Debug.Log("Enter Final");
        M_Audio.PlayOneShotAudio("Eye Open");
        string[] world1BgAudio = new string[1] { "Underwater" };
        M_Audio.PlayLoopAudio(world1BgAudio);
        isDead = true;
        FindObjectOfType<M_BossFight>().mmf_GameEnd.PlayFeedbacks();
        yield return new WaitForSeconds(3);
        FindObjectOfType<M_BossFight>().DestroyBossAndGround();
        M_Machine.Instance.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        //Transform cam = FindObjectOfType<M_Camera>().transform;
    
        //Camera.main.transform.DOMoveY(-55, 3);
        //Light coneLight = FindObjectOfType<M_BossFight>().light_EN;
        //DOTween.To(() => coneLight.spotAngle, x => coneLight.spotAngle = x, 100, 3);
        //FindObjectOfType<M_BossFight>().OpenEye();
        ////M_Machine.Instance.GetComponent<Rigidbody>().isKinematic = false;
    }
}
