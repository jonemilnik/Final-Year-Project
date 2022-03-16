using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;
    public static int wins = 0;
    public static int losses = 0;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
}
