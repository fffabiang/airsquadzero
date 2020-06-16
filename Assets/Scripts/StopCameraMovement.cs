using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCameraMovement : MonoBehaviour
{

    //Script used to stop the camera movement when it passes current object that uses the script
    GameManager gameManager;
    public MoveForward objectToStop;
    MoveForward moveForward;
    bool stopped = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        float lowerBound = gameManager.GetLowCameraBound();
        moveForward = objectToStop.GetComponent<MoveForward>();
        if (moveForward == null)
        {
            Debug.LogError("Object to stop does not have MoveForward component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToStop.gameObject.transform.position.z >= transform.position.z && stopped == false)
        {
            Debug.Log("Stopping object:" + objectToStop.name);
            objectToStop.speed = 0f;
            stopped = true;
        }
    }
}
