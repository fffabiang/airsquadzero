using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWheel : MonoBehaviour
{

    public float speed = 40f;
    public List<GameObject> wheels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject gameObj in wheels)
        {
            gameObj.transform.Rotate(Vector3.right, speed * Time.deltaTime);
        }
    }
}
