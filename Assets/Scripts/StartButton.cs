using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

/*Script used to start player's intro when clicking the button*/
public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManager;
    public GameObject startMenu;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartClick()
    {
        //gameManager.gameStart = true;
        startMenu.SetActive(false);
    }


}
