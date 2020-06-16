using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*Script used by the tank enemies to shoot bullets towards player's temporary position*/
public class ShootPlayer : MonoBehaviour
{

    public float bulletSpeed = 10f;
    GameObject player;
    public float timeBtwBullets = 1f;
    public float timeBtwBullet = 0.5f;
    SpawnManager spawnManager;
    public GameObject bulletPrefab;
    GameObject bulletOutput;
    GameObject pivotGun;
    GameManager gameManager;
    Enemy enemyComponent;
    public string shooterName;
    public bool shootingEnabled;
    public int bulletsPerRound = 3;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        pivotGun = transform.Find("PivotGun").gameObject;
        bulletOutput = pivotGun.transform.Find("BulletOutput").gameObject;
        enemyComponent = gameObject.GetComponent<Enemy>();

        StartCoroutine(ShootGun());
    }

    Quaternion pivotGunRot;
    Vector3 currentPlayerPosition;
    Vector3 bulletOutputPos;

    //Instantiate bullets to shoot
    IEnumerator ShootGun()
    {
        int bulletsShot = bulletsPerRound;

        while (true)
        {
            yield return new WaitForSeconds(timeBtwBullets);
            if (shootingEnabled &&  bulletOutput!=null && !gameManager.gameOver)
            {
                pivotGunRot = pivotGun.transform.rotation;
                bulletOutputPos = bulletOutput.transform.position;
                currentPlayerPosition = player.transform.position;

                //Debug.Log("CurrentPlayerPos: " + currentPlayerPosition + " BulletOutputPos: " + bulletOutputPos + " CurrentTankRot: " + pivotGunRot);

                for (int i = 0; i < bulletsShot; i++)
                {
                    GameObject newBullet = Instantiate(bulletPrefab, bulletOutputPos, pivotGunRot, spawnManager.transform);
                    newBullet.SetActive(false);
                    newBullet.GetComponent<DetectCollision>().Shooter = shooterName;
                    yield return ShootBullet(newBullet);
                    yield return new WaitForSeconds(timeBtwBullet);
                }

            }


        }
    }

    //Shoot bullet to the player's position with a constant velocity
    IEnumerator ShootBullet(GameObject bullet)
    {
        bullet.SetActive(true);
        Vector3 directionNormalized = (currentPlayerPosition - bulletOutputPos).normalized;
        Vector3 direction = new Vector3(directionNormalized.x, 0.0f, directionNormalized.z);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * bulletSpeed;
        //Debug.Log("Direction of bullet = " + direction + " speed = " + rb.velocity);
        yield return null;
    }

    float offsetPosZ = 5f;

    private void Update()
    {

        if (gameManager.GetTopCameraBound() + offsetPosZ >= transform.position.z)
        {
            shootingEnabled = true;
            enemyComponent.destroyedBySensors = true;

        }
        else if (gameManager.GetLowCameraBound() <= transform.position.z)
        {
            shootingEnabled = false;
        }

    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (!gameManager.gameOver)
        {
            Vector3 pivotPos = pivotGun.transform.position;
            pivotGun.transform.LookAt(player.transform);
            pivotGun.transform.position = pivotPos;
        }


    }



}
