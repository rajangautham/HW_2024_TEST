using UnityEngine;

public class OpenCanvasOnCollision : MonoBehaviour
{
    public GameObject canvasToShow; // Assign the Canvas you want to show in the inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            // Show the canvas
            canvasToShow.SetActive(true);
            // Add any other actions you want to perform when the canvas opens
        }
    }
}
