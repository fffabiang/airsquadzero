using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Script used to disable the spawning of the first wave of Enemies*/
public class DisableFirstWave : MonoBehaviour
{

    SpawnManager spawnManager;
    public GameObject spawnPosition;
    bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (disabled == false && spawnPosition.transform.position.z >= transform.position.z)
        {
            spawnManager.SetFirstWaveActive(false);
            spawnManager.backSensor.GetComponent<DestroyOutOfBounds>().damagePlayer = false;
            disabled = true;
        }
    }


}
