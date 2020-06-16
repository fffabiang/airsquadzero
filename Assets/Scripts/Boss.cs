using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{

    GameManager gameManager;
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemy.life <= 0)
        {
            Debug.Log("Boss killed. Level finished.");
            gameManager.FirstLevelFinished();
        }

    }
}
