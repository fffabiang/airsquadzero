using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

//Script used by scene sensors to destroy out of bounds objects

public class DestroyOutOfBounds : MonoBehaviour
{
    private PlayerController playerController;
    private SpawnManager spawnManager;
    public bool damagePlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.CompareTag("Player"))
        {
            return;
        } 
        else if (other.gameObject.CompareTag("Enemy"))  //Applies for First Wave enemies and MiniTanks
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            spawnManager.RemoveSpawnedEnemy(other.gameObject);

            if (damagePlayer)
            {
                Debug.Log("Player damaged by " + other.gameObject.name + " hit by sensor " + gameObject.name);
                playerController.DamagePlayer();
            }

            if (enemy.destroyedBySensors)
            {
                Destroy(other.gameObject);
            }

        } 
        else if (other.gameObject.CompareTag("Gun"))
        {
            Destroy(other.gameObject);
        } 
      

    }






}
