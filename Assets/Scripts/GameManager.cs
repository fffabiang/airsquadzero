using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/*TODO:
    - Agregar Powerups
*/


public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool gameOver = false;  //Seteado en true cuando el jugador muere
    public bool gameStart = false; 
    public int score = 0;

    

    //Movement bounds
    public Camera camera; 
    public float xRange = 10;
    public float zRange;
    private float offsetBoundX = 4f;
    private float offsetBoundZ = 3f;

    //Player
    public int playerLives = 10;
    private int maxLife;
    public ParticleSystem playerExplosion;
    public GameObject player;
    public GameObject gameView;


    //UI
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public ProgressBar progressBar;
    public TextMeshProUGUI deathMessage;
    public Button restartButton;

    //Levels
    bool level1Cleared = false;

    void Start()
    {
        gameStart = false;
        SetMovementBounds();
        SetUIText();
    }

    void SetUIText()
    {
        UpdateScore(0);
        livesText.text = "Lives: " + playerLives;
        maxLife = playerLives;
        progressBar.BarValue = 100 * playerLives / maxLife;
        deathMessage.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    void SetMovementBounds()
    {
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.transform.position.y));
        xRange = Math.Abs(p.x) - offsetBoundX;
        zRange = Math.Abs(p.z) - offsetBoundZ;
        Debug.Log("Movement Bound X = " + xRange);
        Debug.Log("Movement Bound z = " + zRange);
    }

    public float GetTopCameraBound()
    {
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.transform.position.y));
        return p.z ;
    }

    public float GetLowCameraBound()
    {
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.transform.position.y));
        return p.z ;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = /*"SCORE: " +*/ score.ToString().PadLeft(9, '0'); ;
    }

    public AudioClip victoryMusic;


    IEnumerator ShowVictoryText()
    {

        deathMessage.text = "MISSION COMPLETE" + "\n" + "Thank you for playing!";
        while (true)
        { 
            deathMessage.color = Color.red;
            yield return new WaitForSeconds(1f);
            deathMessage.color = Color.yellow;
            yield return new WaitForSeconds(1f);

        }
    }

    /*Method called when the game ends either by losing or winning (killing the main boss)*/
    public void GameOver()
    {
        if (level1Cleared)
        {
            StartCoroutine(ShowVictoryText());
            AudioSource audioSrc = GetComponent<AudioSource>();
            audioSrc.Stop();
            audioSrc.clip = victoryMusic;
            audioSrc.Play();
            player.GetComponent<MoveForward>().enabled = true;

        }
        else
        {
            restartButton.gameObject.SetActive(true);
        }
        gameView.GetComponent<MoveForward>().speed = 0;
        deathMessage.gameObject.SetActive(true);
        gameOver = true;
    }

    public void UpdatePlayerHealth(int damage)
    {

        if (gameOver)
        {
            return;
        }

        playerLives -= damage ;
        livesText.text = "Lives: " + playerLives;
        progressBar.BarValue = 100 * playerLives / maxLife;

        if (playerLives <= 0)
        {
            player.GetComponent<PlayerController>().PlayDeathSound();
            GameOver();
            Instantiate(playerExplosion, player.transform.position, playerExplosion.transform.rotation);
            Destroy(player);
        }
        else
        {
            player.GetComponent<PlayerController>().PlayHurtSound();
        }
    }

    void GainLife()
    {

    }

    //Called when first level is finished (by killing the boss): ends the game as it is right now
    public void FirstLevelFinished()
    {
        level1Cleared = true;
        GameOver();
    }

    bool showingdEnemyDmg = false;
    public IEnumerator ShowDamage(GameObject gameObject)
    {


        if (showingdEnemyDmg) yield return null;
        
        showingdEnemyDmg = true;

        var renderer = gameObject.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        Color originalColor = renderer.material.color;

        for (int i = 0; i < 2; i++)
        {
            renderer.material.SetColor("_Color", Color.red);

            yield return new WaitForSeconds(0.025f);

            renderer.material.SetColor("_Color", originalColor);

            yield return new WaitForSeconds(0.025f);

        }

        showingdEnemyDmg = false;

    }

    public IEnumerator ShowDamage(GameObject gameObject, float time, Color damageColor)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        float colorChangeTime = 0.05f;
        float iterations = time / (colorChangeTime * 2 );
        int iter = (iterations >= 1) ? Convert.ToInt32(iterations) : 1;
       
        //Call SetColor using the shader property name "_Color" and setting the color to red

        for (int i = 0; i < iter; i++)
        {
            renderer.material.SetColor("_Color", damageColor);

            yield return new WaitForSeconds(colorChangeTime);

            renderer.material.SetColor("_Color", Color.white);

            yield return new WaitForSeconds(colorChangeTime);

        }
    }


}
