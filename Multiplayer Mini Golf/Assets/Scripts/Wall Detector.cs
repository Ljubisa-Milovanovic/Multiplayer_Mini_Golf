using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private Udarac udarac;

    
    void LateUpdate()
    {
        // Reset the local rotation to the initial rotation
        
        transform.rotation = Quaternion.identity;
        //Debug.Log("rotaija = " + transform.rotation);
    }
    

    void Start()
    {
        // Get reference to the Udarac script on the parent GameObject (the ball)
        udarac = GetComponentInParent<Udarac>();

        // Check if the reference is set correctly
        if (udarac == null)
        {
            Debug.LogError("Udarac reference not found on the parent object!");
        }
        // Store the initial local rotation of the WallDetector
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Nesto me je pipnulo ...zid..... PAUSE");
        // set bouncines to 1
        Debug.Log("Collider entered: " + other.gameObject.name );//+ " tag:" + other.gameObject.tag
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Average;
        //Debug.Log("enter: " + udarac.ballMaterial.bounceCombine);

    }

    // Optional: This method is called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        //set bouncines to 0
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
        //Debug.Log("exit: " + udarac.ballMaterial.bounceCombine);
    }

    private void OnTriggerStay(Collider other)
    {
        //if (!other.CompareTag("bound box"))
        //{
        //}
            if (udarac.isIdle)
            {
                udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            }
            else
            {
                udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Average;
            }
            //Debug.Log("stay: " + udarac.ballMaterial.bounceCombine);
        
        
    }

}
