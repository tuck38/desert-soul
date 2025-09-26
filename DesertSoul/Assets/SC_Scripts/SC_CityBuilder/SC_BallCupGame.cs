using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SC_BallCupGame : MonoBehaviour
{
    public static Action<int> OnCupClicked;
    public static Action<bool> OnGameOver;

    [SerializeField] SC_Cup[] cups;
    [SerializeField] Transform ball;

    bool isGameOn = false;
    int ballCup = 0;

    private void OnEnable()
    {
        OnCupClicked += CupClicked;
    }

    private void OnDisable()
    {
        OnCupClicked -= CupClicked;
    }

    public void StartMinigame()
    {
        if (!isGameOn)
        {
            for (int i = 0; i < cups.Length; i++)
            {
                cups[i].gameObject.SetActive(true);
                cups[i].cupNumber = i;
            }
            ball.gameObject.SetActive(true);

            ballCup = Random.Range(0, 3);
            ball.transform.position = cups[ballCup].transform.position;
            isGameOn = true;
        } 
    }

    void EndMinigame()
    {
        foreach (SC_Cup cup in cups)
        {
            cup.gameObject.SetActive(false);
        }
        ball.gameObject.SetActive(false);
    }

    void CupClicked(int cupNumber)
    {
        if(isGameOn)
        {
            OnGameOver?.Invoke(cupNumber == ballCup);
            cups[cupNumber].gameObject.SetActive(false);
            isGameOn = false;
            StartCoroutine("delayEndOfGame");
        }
    }

    IEnumerator delayEndOfGame()
    {
        yield return new WaitForSeconds(1f);
        EndMinigame();
    }
}
