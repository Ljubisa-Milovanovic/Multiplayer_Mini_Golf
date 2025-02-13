using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class HoleDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Nesto me je pipnulo ........ PAUSE");
        // Check if the object that entered is the ball
        if (other.CompareTag("player ball"))
        {
            
            // Get the Udarac component from the colliding object (the ball)
            Udarac udarac = other.GetComponent<Udarac>();
            if (udarac != null)
            {
                if (udarac.Strokes == 1)
                    Debug.Log("Hole in one");
                else
                    Debug.Log("Ball has entered the hole! Number of strokes is: " + udarac.Strokes);

              
                // go to next level
                GameMenager.instance.NextLevel();
            }
            else
            {
                Debug.LogError("Udarac component not found on the colliding object.");
            }
        }
    }

    // Optional: This method is called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited is the ball
        if (other.CompareTag("player ball"))
        {
            Debug.Log("Ball has exited the hole!");
            // Perform any additional actions you want, if needed
        }
    }
}
