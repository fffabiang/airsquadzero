using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private GameManager gameManager;

    //First Wave Enemies
    public List<GameObject> firstWaveEnemies;
    public float spawnRate = 3.0f;
    public float firstWaveSpeed = 10f;
    public int enemiesPerLevel = 20;

    bool startFirstWave = false;
    bool firstWaveActive = true;

    //Road Enemies
    public List<GameObject> roadEnemies;
    bool spawnFirstRoad = true;
    public float spawnRoadRate = 1.25f;
    public Transform startPosLeft;
    public Transform startPosRight;
    public float roadSpeed = 30f;

    //MiniTank Zone


    //Levels
    public Transform spawnPosition;

    //General
    [SerializeField]
    private List<GameObject> spawnedEnemies;
    public GameObject level1Boss;
    public float timeToStart;
    public GameObject frontSensor;
    public GameObject backSensor;
    public AudioClip enemyDeathSound;
    public AudioClip enemyHitSound;


    // Start is called before the first frame update
    void Start()
    {
        spawnedEnemies = new List<GameObject>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(SpawnFirstWave(firstWaveEnemies, enemiesPerLevel, level1Boss));
        StartCoroutine(SpawnFirstRoadEnemies(roadEnemies));
    }

    // Update is called once per frame
    float timePassed = 0f;
    float firstWave_speedIncrease = 1f;
    float firstWave_spawnDecrease = 0.03f;
    float firstWave_timeToUpdate = 5f;
    void Update()
    {

        if (firstWaveActive)
        { 

            timePassed += Time.deltaTime;
            if (timePassed >= firstWave_timeToUpdate)
            {
                firstWaveSpeed += firstWave_speedIncrease;
                spawnRate -= firstWave_spawnDecrease;
                timePassed = 0f;
            }
        }

    }


    IEnumerator SpawnFirstRoadEnemies(List<GameObject> firstRoadEnemies)
    {
        while (spawnFirstRoad && gameManager.gameOver==false)
        {
            yield return new WaitForSeconds(spawnRoadRate);

            if (!gameManager.gameStart)
                continue;

            int index = Random.Range(0, firstRoadEnemies.Count);
            GameObject newVehicle = Instantiate(firstRoadEnemies[index]);
            newVehicle.GetComponent<Enemy>().DamageByPlayerCollision = true;
            newVehicle.GetComponent<Enemy>().destroyedBySensors = true;
            PlaceInRoadAndMove(newVehicle,startPosLeft.position,startPosRight.position);
        }

    }

    /*Function to set random start position of road vehicle: 
        - Left and moves to the right or..
        - Right and moves to the left*/
    void PlaceInRoadAndMove(GameObject newVehicle, Vector3 startPosLeft,Vector3 startPosRight)
    {

        newVehicle.SetActive(false);
        Vector3[] startPositions = {startPosLeft,startPosRight };
        int index = Random.Range(0, startPositions.Length);
        newVehicle.transform.position = startPositions[index];
        if (index == 0) /*Goes from left to right*/
        {
            newVehicle.transform.Rotate(newVehicle.transform.up, -90);
        }
        else  /*Goes from right to left*/
        {
            newVehicle.transform.Rotate(newVehicle.transform.up, 90);

        }
        MoveForward moveForwScript = newVehicle.AddComponent<MoveForward>();
        moveForwScript.setOwnForward(true);
        moveForwScript.speed = roadSpeed;
        newVehicle.SetActive(true);
    }

    public void SetFirstWaveActive(bool value)
    {
        firstWaveActive = value;
    }

    /*Coroutine to spawn the first wave of enemies in the scene*/
    IEnumerator SpawnFirstWave(List<GameObject> targets, int maxSpawned, GameObject levelBoss)
    {

        //Spawning the level enemies
        int enemiesSpawned = 0;
        while (!gameManager.gameOver && enemiesSpawned<maxSpawned)
        {
            /**/

            yield return new WaitForSeconds(spawnRate);

            if (!gameManager.gameStart)
            {
                continue;
            }

            if (!startFirstWave)
            {
                yield return new WaitForSeconds(timeToStart);
                startFirstWave = true;
            }

            if (firstWaveActive)
            {
                int index = Random.Range(0, targets.Count);
                GameObject newTarget = Instantiate(targets[index]);
                newTarget.SetActive(false);
                newTarget.transform.position = RandomSpawnPos();
                newTarget.GetComponent<Enemy>().DamageByPlayerCollision = true;
                newTarget.GetComponent<Enemy>().destroyedBySensors = true;
                newTarget.GetComponent<MoveForward>().speed = firstWaveSpeed;
                spawnedEnemies.Add(newTarget);
                newTarget.SetActive(true);
                enemiesSpawned++;
            }

            
        }

        //Spawning the boss
        spawnedEnemies.Clear();

    }

    //Variables for handling position of spawned object
    private float ySpawnPos = 1f;
    float posSeparation = 3f;
    float maxZforCheking = 0f;
    float offsetSpawn = -10.0f;
    bool CheckIfBehind(float posX)
    {
        //Debug.Log("Checking if posX = " + posX + " is ok..");

        foreach (GameObject gameObj in spawnedEnemies)
        {
            if (gameObj != null && gameObj.transform.position.z >= maxZforCheking && Math.Abs(gameObj.transform.position.x - posX) <= posSeparation)
            {
                return true;
            }
        }

        return false;
    }
   
    /*Calculate a random position for an object along the X-axis*/
    Vector3 RandomSpawnPos()
    {

        bool foundPos = false;
        float randomXPos = 0;
        while (!foundPos)
        {
            randomXPos = Random.Range(-gameManager.xRange- offsetSpawn, gameManager.xRange+ offsetSpawn);
            if (CheckIfBehind(randomXPos) == false)
            {
                //Debug.Log("New spawning position for " + gameObject + " is not behind other spawned objects.. OK");
                foundPos = true;
            }
            else
            {
                //Debug.Log("New spawning position for " + gameObject + "BAD.. will retry.");
            }
        }

        return new Vector3(randomXPos, ySpawnPos, spawnPosition.position.z);
    }

    /*Method called to free memory from the spawnedEnemies list*/
    public void RemoveSpawnedEnemy(GameObject gameObject)
    {
        if (spawnedEnemies.Contains(gameObject))
        {
            spawnedEnemies.Remove(gameObject);
        }
    }
}
