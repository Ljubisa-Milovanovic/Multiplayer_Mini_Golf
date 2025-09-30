using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
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
   
    private void OnTriggerEnter(Collider other)
    {
       

        Debug.Log("Nesto me je pipnulo");
        
        if (other.CompareTag("player ball"))
        {
            
            
            Udarac udarac = other.GetComponent<Udarac>();
            if (udarac != null)
            {
                if (udarac.Strokes == 1)
                    Debug.Log("Hole in one");
                else
                    Debug.Log("Ball has entered the hole! Number of strokes is: " + udarac.Strokes);

                GameMenager.instance.HoleSound();
                
                GameMenager.instance.NextLevel();

                NameManager.instance.HoleUpdateServerRpc(udarac.OwnerClientId);
                GameMenager.instance.CurrShouldBeReset = true;
            }
            else
            {
                Debug.LogError("Udarac component not found on the colliding object.");
            }
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("player ball"))
        {
            Debug.Log("Ball has exited the hole!");
            
        }
    } 
}
