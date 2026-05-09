using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game if it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
