
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int life = 100;
    private int fullLife;
    public ParticleSystem explosion;
    public int enemyLevel = 1;
    private bool damageByPlayerCollision = false;
    GameManager gameManager;
    SpawnManager spawnManager;
    public bool destroyedBySensors = true;
    public bool hitableByPlayer = true;

    public bool DamageByPlayerCollision { get => damageByPlayerCollision; set => damageByPlayerCollision = value; }

    // Start is called before the first frame update

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        fullLife = life;
    }

    float offsetZ = 5f;
    // Update is called once per frame
    void Update()
    {
        if (gameManager.GetTopCameraBound() + offsetZ >= transform.position.z)
        {
            hitableByPlayer = true;
        }
        else if (gameManager.GetLowCameraBound() <= transform.position.z)
        {
            hitableByPlayer = false;
        }
    }

    /*Flag activated for the only boss in the scene (as it works right now)*/
    public bool bossFlag = false;

    public void TakeDamage(int damage, Vector3 damagePos )
    {
        if (life - damage > 0)
        {
            life -= damage;
            spawnManager.GetComponent<AudioSource>().PlayOneShot(spawnManager.enemyHitSound,1.0f);
            StartCoroutine(gameManager.ShowDamage(gameObject));
        }
        else
        {
            life = 0;

            Instantiate(explosion, damagePos, explosion.transform.rotation);
            spawnManager.GetComponent<AudioSource>().PlayOneShot(spawnManager.enemyDeathSound,1.0f);
            if (enemyLevel == 1)
            {
                spawnManager.RemoveSpawnedEnemy(gameObject);
                //spawnManager.spawn.Remove(gameObject);
            }
            if (bossFlag)
            {
                gameManager.FirstLevelFinished();
            }
            Destroy(gameObject);
            gameManager.UpdateScore(fullLife);

        }    
        //Debug.Log(gameObject.name + " life reduced to:" + life);

    }





}
