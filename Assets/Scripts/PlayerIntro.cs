using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Script used to show the intro before triggering the start of the game
 Right now the intro consists of the player arriving into the starting point*/

public class PlayerIntro : MonoBehaviour
{

    private Vector3 endPosition;
    public float startZPosition;
    public float speed = 10f;
    private GameManager gameManager;
    float timeToStart = 0f;
    public GameObject startMenu;


    // Start is called before the first frame update
    void Start()
    {
        endPosition = new Vector3(0.0f, 0.0f, startZPosition);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.gameStart || startMenu.activeSelf)
        {
            return;
        }
      
        if (transform.position.z <= endPosition.z)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            StartCoroutine(StartGameFromPlayer());
        }       


    }

    IEnumerator StartGameFromPlayer()
    {
        yield return new WaitForSeconds(timeToStart);
        gameManager.gameStart = true;
    }


}
