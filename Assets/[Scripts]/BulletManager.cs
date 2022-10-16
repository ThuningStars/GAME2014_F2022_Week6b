using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[System.Serializable]
public class BulletManager : MonoBehaviour
{
    [Header("Bullet Properties")]
    [Range(10, 50)]
    public int playerBulletNumber = 50;
    public int playerBulletCount = 0;
    public int activePlayerBullets = 0;
    [Range(10, 50)]
    public int enemyBulletNumber = 50;
    public int enemyBulletCount = 0;
    public int activeEnemyBullets = 0;

    private BulletFactory factory;

    private Queue<GameObject> playerBulletPool;
    private Queue<GameObject> enemyBulletPool;

    // Start is called before the first frame update
    void Start()
    {
        playerBulletPool = new Queue<GameObject>();// create an empty queue container
        enemyBulletPool = new Queue<GameObject>(); // create an empty queue container

        factory = GameObject.FindObjectOfType<BulletFactory>();
        BuildBulletPools();
    }

    void BuildBulletPools()
    {
        for (int i = 0; i < playerBulletNumber; i++)
        {
            playerBulletPool.Enqueue(factory.createBullet(BulletType.PLAYER));
        }

        for (int i = 0; i < enemyBulletNumber; i++)
        {
            enemyBulletPool.Enqueue(factory.createBullet(BulletType.ENEMY));
        }


        playerBulletCount = playerBulletPool.Count;
        enemyBulletCount = enemyBulletPool.Count;

    }

    public GameObject GetBullet(Vector2 position, BulletType type)
    {
        GameObject bullet = null;

        switch (type)
        {
            case BulletType.PLAYER:
                {
                    if (playerBulletPool.Count < 1)
                    {
                        playerBulletPool.Enqueue(factory.createBullet(BulletType.PLAYER));
                    }
                    bullet = playerBulletPool.Dequeue();
                    //stats
                    playerBulletCount = playerBulletPool.Count;
                    activePlayerBullets++;
                }
                break;
            case BulletType.ENEMY:
                {
                    if (enemyBulletPool.Count < 1)
                    {
                        enemyBulletPool.Enqueue(factory.createBullet(BulletType.ENEMY));
                    }
                    bullet = enemyBulletPool.Dequeue();
                    //stats
                    enemyBulletCount = enemyBulletPool.Count;
                    activeEnemyBullets++;
                }
                break;
        }

        bullet.SetActive(true);
        bullet.transform.position = position;

        return bullet;
    }

    public void ReturnBullet(GameObject bullet, BulletType type)
    {
        bullet.SetActive(false);

        switch (type)
        {
            case BulletType.PLAYER:
                {
                    playerBulletPool.Enqueue(bullet);
                    //stats
                    playerBulletCount = playerBulletPool.Count;
                    activePlayerBullets--;
                }
                break;
            case BulletType.ENEMY:
                {
                    enemyBulletPool.Enqueue(bullet);
                    //stats
                    enemyBulletCount = enemyBulletPool.Count;
                    activeEnemyBullets--;
                }
                break;
        }
    }
}
