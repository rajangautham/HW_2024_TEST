using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject canvas; // Reference to the Canvas GameObject
    public GameObject ObjectSpawnAndDestroy; // Reference to the ObjectSpawnAndDestroy script
    public GameObject JustBase;
    private int score = -1; // Player's score
    void Start()
    {
        // Ensure the Canvas is initially inactive
        canvas.SetActive(true);
        ObjectSpawnAndDestroy.SetActive(false);
    }

    public void StartGame()
    {   
        // Set Canvas to inactive
        scoreText.text = "Score: " + score.ToString();
        canvas.SetActive(false);
        JustBase.SetActive(false);
        ObjectSpawnAndDestroy.SetActive(true);

    }
    public void IncreaseScore()
    {
        // Increase the score and update the score text
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
}
