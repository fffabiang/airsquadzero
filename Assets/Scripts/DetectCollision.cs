using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/*Script used by the bullets shot by the player or the enemies*/
public class DetectCollision : MonoBehaviour
{

    public int gunDamage = 50;
    bool damageDone = false;
    float maxPossibleZ = 20000f;
    PlayerController playerController;
    string shooter;

    public string Shooter { get => shooter; set => shooter = value; }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        //Destroy bullet objects they get too far away from the scene (even if there are sensors too)
        if (transform.position.z >= maxPossibleZ || transform.position.z <= -maxPossibleZ)
        {
            Debug.Log("Se elimino " + gameObject.name + " por sobrepasar el limite z");
            Destroy(gameObject);
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        //Bullet shot from player to the enemy
        if (Shooter.CompareTo("Player") == 0 &&  (  (other.gameObject.CompareTag("Enemy") ) && other.gameObject.GetComponent<Enemy>().hitableByPlayer && !damageDone)  )
        {

            damageDone = true;

            //Debug.Log("damageDone set to " + damageDone);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(gunDamage, gameObject.transform.position);
            Destroy(gameObject);

        }
        // Bullet shot from enemy to the player
        else
        {   

            if (other.gameObject.CompareTag("Player") && Shooter.CompareTo("Player")!=0)
            {
                Debug.Log("Player receives damage from " + Shooter + " gun");
                playerController.DamagePlayer();
                Destroy(gameObject);
            }
        }
    }




}
