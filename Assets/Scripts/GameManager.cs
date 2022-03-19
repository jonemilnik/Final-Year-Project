using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;
    public static int wins = 0;
    public static int losses = 0;
    private Vector3 initialPlayerPos;

    private void Start()
    {
        initialPlayerPos = FindObjectOfType<Player>().GetComponent<Transform>().position;
        Debug.Log(initialPlayerPos);
    }

    public void LoseGame()
    {
        if (gameOver == false)
        {
            gameOver = true;
            losses++;
            Debug.Log("Losses: " + losses);
            Debug.Log("Wins: " + wins);
            Restart();
        }
        
    }

    public void WinGame()
    {
        gameOver = true;
        wins++;
        Debug.Log("Losses: " + losses);
        Debug.Log("Wins: " + wins);
        Restart();
    }
    void Restart()
    {
        GameObject.Find("Player").GetComponent<NavMeshAgent>().Warp(GameObject.Find("StartState").transform.position);
        Debug.Log("Final Pos: " + FindObjectOfType<Player>().GetComponent<Transform>().position);
    }

    
}
