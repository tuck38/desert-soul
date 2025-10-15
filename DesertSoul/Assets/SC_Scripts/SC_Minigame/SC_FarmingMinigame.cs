using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class SC_FarmingMinigame : MonoBehaviour
{
    public static Action<bool> OnFruitBroken;
    static int farmingHighScore = 0;

    [SerializeField] GameObject fruitPrefab;
    [Tooltip("")] //fill out later
    [SerializeField] Vector2 throwRange;
    [SerializeField] float initialThrowSpeed;
    [SerializeField] int playerScore;
    [SerializeField] int scoreGainedPerBrokenFruit = 10;
    [SerializeField] int gameDuration = 15;
    [SerializeField] float timeBetweenFruitThrows = 0.1f;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    bool gameStarted = false;
    //float gameTime = 0.0f;

    private void OnEnable()
    {
        OnFruitBroken += UpdateFruit;
    }

    private void OnDisable()
    {
        OnFruitBroken -= UpdateFruit;
    }

    void UpdateFruit(bool brokenByPlayer)
    {
        if(brokenByPlayer)
        {
            playerScore += scoreGainedPerBrokenFruit;
        }
    }

    public void OnStartGame()
    {
        if (gameStarted) return;
        Debug.Log("Start Minigame");
        gameStarted = true;
        //gameTime = 0.0f;
        StartCoroutine("GameTimer");
        StartCoroutine("FruitThrowingFunction");
    }

    void OnGameOver()
    {
        if (!gameStarted) return;
        if (farmingHighScore < playerScore) farmingHighScore = playerScore;
        StopAllCoroutines();
        gameStarted = false;
        Debug.Log("End Minigame");
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);
        OnGameOver();
    }

    IEnumerator FruitThrowingFunction()
    {
        while(gameStarted)
        {
            Instantiate(fruitPrefab, transform.position + new Vector3(0f,0.5f,0f), Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(throwRange.x,throwRange.y),1) * initialThrowSpeed);
            yield return new WaitForSeconds(timeBetweenFruitThrows);
        }
    }
}
