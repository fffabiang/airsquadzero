using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{

    public float horizontalInput;
    public float verticalInput;
    public float speed = 40f;


    public GameObject projectilePrefab;
    public GameObject missilePrefab;
    private GameManager gameManager;
    Vector3 offsetPosition = new Vector3(0, -0.5f, 2.1f);
    bool takenDamage = false;
    float ghostTime = 0.5f;
    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioClip hurtSound;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.gameOver || !gameManager.gameStart)
        {
            return;
        }

        //Reading player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * speed);
        transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * speed);

        //Limiting horizontal movement

        if (transform.localPosition.x < -gameManager.xRange)
        {
            transform.localPosition = new Vector3(-gameManager.xRange, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.x > gameManager.xRange)
        {
            transform.localPosition = new Vector3(gameManager.xRange, transform.localPosition.y, transform.localPosition.z);
        }

        //Limiting vertical movement

        if (transform.localPosition.z < -gameManager.zRange)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -gameManager.zRange);
        }
        else if (transform.localPosition.z > gameManager.zRange)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, gameManager.zRange);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootGun();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //LaunchMissile();
        }


    }


    void ShootGun()
    {
        audioSource.PlayOneShot(shootSound,1.0f);
        GameObject newProjectile = Instantiate(projectilePrefab,  new Vector3(transform.position.x,2.0f,transform.position.z) + offsetPosition , projectilePrefab.transform.rotation);
        newProjectile.GetComponent<DetectCollision>().Shooter = "Player";
    }

    void LaunchMissile()
    {
        GameObject newProjectile = Instantiate(missilePrefab, transform.position + offsetPosition, missilePrefab.transform.rotation);
        newProjectile.GetComponent<DetectCollision>().Shooter = "Player";
    }


    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound,1.0f);
    }

    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtSound, 1.0f);
    }

    //Reduce player's health and show it ingame
    public void DamagePlayer()
    {
        if (!takenDamage)
        {
            takenDamage = true;
            int damageReceived = 1;
            
            gameManager.UpdatePlayerHealth(damageReceived);
           
            if (gameManager.playerLives > 0)
            {
                StartCoroutine(ShowDamageAndGhost());
            }
            else
            {
                takenDamage = false;
            }
        }
        
    }

    //Showing the damage done to the player and making it invulnerable for ghostTime seconds
    IEnumerator ShowDamageAndGhost()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        yield return gameManager.ShowDamage(gameObject, ghostTime, Color.clear);
        boxCollider.enabled = true;
        takenDamage = false;
    }
    

    //When player collides with other objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collides with enemy:" + other.gameObject.name);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy.DamageByPlayerCollision)
            {
                enemy.TakeDamage(enemy.life, gameObject.transform.position);
            }

            DamagePlayer();
        }
    }




}
