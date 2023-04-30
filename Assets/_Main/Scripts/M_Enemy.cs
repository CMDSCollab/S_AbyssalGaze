using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class M_Enemy : Singleton<M_Enemy>
{
    public float spawnTime;
    private float timer;
    public GameObject pre_SeaSnake;
    public GameObject pre_Bullet;
    public float spawnRadius;

    void Update()
    {

        if (timer>spawnTime)
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

    void MonsterGeneration()
    {
        int genNum = Random.Range(1, 3);
        for (int i = 0; i < genNum; i++)
        {
            Vector3 spawnPos = GetRandomPos();
            Transform snake = Instantiate(pre_SeaSnake, spawnPos, Quaternion.Euler(90,0,0)).transform;
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.5f);
            s.AppendCallback(() => ShootBullet(snake.GetChild(0)));
            s.AppendCallback(() => Destroy(snake.gameObject, 1f));
      
        }

        void ShootBullet(Transform pos)
        {
            Transform bullet = Instantiate(pre_Bullet, pos.position, Quaternion.identity).transform;
            Vector3 direction = (M_Machine.Instance.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
        }
    }

    Vector3 GetRandomPos()
    {
        float x = Random.Range(0, spawnRadius);
        float y = Mathf.Sqrt(Mathf.Pow(spawnRadius, 2) - Mathf.Pow(x, 2));
        Vector3 spawnPos = new Vector3((Random.Range(0, 10) > 5) ? x : -x, M_Machine.Instance.transform.position.y, (Random.Range(0, 10) > 5) ? y : -y);
        return spawnPos;
    }
}
