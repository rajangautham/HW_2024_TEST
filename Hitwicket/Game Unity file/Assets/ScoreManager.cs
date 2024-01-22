using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Call the IncreaseScore method from GameManager
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.IncreaseScore();
            }
             BoxCollider boxCollider = GetComponent<BoxCollider>();
             Destroy(boxCollider);
            
        }
    }
}
