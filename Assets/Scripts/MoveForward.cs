using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    
    public float speed = 30f;
    public int forceFactor = 1;
    Rigidbody rb;
    bool ownForward = false;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    private void Update()
    {


        if (rb == null && gameManager.gameStart)
        {
            if (ownForward)
            {
                transform.Translate( transform.forward * Time.deltaTime * speed);

            }
            else
            {
                transform.Translate(forceFactor * Vector3.forward * Time.deltaTime * speed);
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null && gameManager.gameStart)
        {
            
           if (ownForward)
            {
                rb.velocity = transform.forward * speed;

            }
            else
            {
                rb.velocity = forceFactor * Vector3.forward * speed;
                //rb.AddForce(forceFactor * Vector3.forward * speed, ForceMode.Force);
            }

        }

    }

    public void setOwnForward(bool value)
    {
        ownForward = value;
    }


}
