using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class M_Enemy : Singleton<M_Enemy>
{
    public float spawnTime;
    private float timer;
    public GameObject pre_Melee;
    public GameObject pre_Ranged;
    public GameObject pre_Turret;
    public int turretDensity;
    public float spawnRadius;
    public Transform parent_Enemy;

    private void Start()
    {
   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //for (int i = 0; i < 3; i++) RangedGeneration();
            for (int i = 0; i < 10; i++) MeleeGeneration();
        }

        if (timer > spawnTime)
        {
            MonsterGeneration();
            timer = 0;
        }
    }

    public void EnemyGenerationProcess()
    {
        timer += Time.deltaTime;
    }

    public void EnemyGenerationStop(float machineYValue)
    {
        timer = 0;
    }

    //void MonsterGeneration()
    //{
    //    int genNum = Random.Range(1, 3);
    //    for (int i = 0; i < genNum; i++)
    //    {
    //        Vector3 spawnPos = GetRandomPos();
    //        Transform snake = Instantiate(pre_SeaSnake, spawnPos, Quaternion.Euler(90,0,0)).transform;
    //        Sequence s = DOTween.Sequence();
    //        s.AppendInterval(0.5f);
    //        s.AppendCallback(() => ShootBullet(snake.GetChild(0)));
    //        s.AppendCallback(() => Destroy(snake.gameObject, 1f));
      
    //    }

    //    void ShootBullet(Transform pos)
    //    {
    //        Transform bullet = Instantiate(pre_Bullet, pos.position, Quaternion.identity).transform;
    //        Vector3 direction = (M_Machine.Instance.transform.position - bullet.transform.position).normalized;
    //        bullet.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
    //    }
    //}

    void MonsterGeneration()
    {
        int toGenMonsterConfirm = Random.Range(0, 10);
        if (toGenMonsterConfirm <= 4)
        {
            MeleeGeneration();
        }
        else
        {
            RangedGeneration();
        }
        spawnTime = Random.Range(10, 20);
    }

    void MeleeGeneration()
    {
        //Debug.Log("New Melee Gen");
        Vector3 circlePos = GetRandomPos();
        Vector3 spawnPos = new Vector3(Random.Range(0, circlePos.x), circlePos.y, Random.Range(0, circlePos.z));
        Transform newMelee = Instantiate(pre_Melee, spawnPos, Quaternion.Euler(90, 0, 0)).transform;
        newMelee.SetParent(parent_Enemy.Find("Melees"));
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(0, spawnRadius);
        float y = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) - Mathf.Pow(x, 2));
        Vector3 spawnPos = new Vector3((Random.Range(0, 10) > 5) ? x : -x, M_Machine.Instance.transform.position.y, (Random.Range(0, 10) > 5) ? y : -y);
        return spawnPos;
    }

    void RangedGeneration()
    {
        //Debug.Log("New Ranged Gen");
        Vector3 circlePos = GetRandomPos();
        Vector3 spawnPos = new Vector3(Random.Range(0, circlePos.x), circlePos.y, Random.Range(0, circlePos.z));
        Transform newRanged = Instantiate(pre_Ranged, spawnPos, Quaternion.Euler(90, 0, 0)).transform;
        newRanged.SetParent(parent_Enemy.Find("Rangeds"));
    }

    public void TurretGeneration(Transform colliderParent)
    {
        GameObject turretsParent = new GameObject();
        turretsParent.transform.SetParent(parent_Enemy.Find("Turrets"));
        List<Transform> residuePos = new List<Transform>();
        foreach (SphereCollider sc in colliderParent.GetComponentsInChildren<SphereCollider>()) residuePos.Add(sc.transform);

        int randomizedDensity = Random.Range(turretDensity - 3, turretDensity + 3);
        for (int i = 0; i < randomizedDensity; i++)
        {
            int random = Random.Range(0, residuePos.Count);
            Vector3 spawnPos = residuePos[random].position + new Vector3(0, 0.5f, 0);
            Transform newTurret = Instantiate(pre_Turret, spawnPos, Quaternion.Euler(90, 0, 0)).transform;
            newTurret.SetParent(turretsParent.transform);
            residuePos.RemoveAt(random);
        }
    }

    public void ClearUpperLayerEnemys()
    {
        foreach (O_BaseEnemy ranged in parent_Enemy.Find("Rangeds").GetComponentsInChildren<O_BaseEnemy>())
        {
            Destroy(ranged.gameObject, 0.1f);
        }

        foreach (O_BaseEnemy melee in parent_Enemy.Find("Melees").GetComponentsInChildren<O_BaseEnemy>())
        {
            Destroy(melee.gameObject, 0.1f);
        }

        Destroy(parent_Enemy.Find("Turrets").GetChild(0).gameObject);
    }
}
