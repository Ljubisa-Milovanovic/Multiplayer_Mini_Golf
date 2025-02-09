using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDetector : MonoBehaviour
{
    public Udarac udarac;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Nesto me je pipnulo ........ PAUSE");
        // Check if the object that entered is the ball
        if (other.CompareTag("player ball"))
        {
            if (udarac != null)
            {
                if (udarac.Strokes == 1)
                    Debug.Log("Hole in one");
                else
                    Debug.Log("Ball has entered the hole! Number of strokes is: " + udarac.Strokes);

                // Perform any additional actions you want, like updating the score
            }
            else
            {
                Debug.LogError("Udarac reference is not set in HoleDetector.");

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
