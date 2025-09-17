using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class HoleDetector : MonoBehaviour
{
    

    private void Awake()
    {
        GameMenager.instance.UpdateNavBar();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        /*
        Ace - hole in one
        Par - predetermined number of strokes (expected number) 
        Bogey - one stroke over par
        Double Bogey - two strokes over par
        Birdie - one under par
        Eagle - two strokes uner par
        Albatross/double eagle - three strokes under par
        
        my imagined therminology:
        wraith - three holes over par
        phantom - two holes over par
        */

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

                GameMenager.instance.HoleSound();
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
