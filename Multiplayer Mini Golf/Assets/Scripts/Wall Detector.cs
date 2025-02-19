using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WallDetector : NetworkBehaviour
{
    private Udarac udarac;

    
    void LateUpdate()
    {
        // Reset the local rotation to the initial rotation
        
        transform.rotation = Quaternion.identity;
        //Debug.Log("rotaija = " + transform.rotation);
    }

    public override void OnNetworkSpawn()
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
        
        //Debug.Log("Collider entered: " + other.gameObject.name );//+ " tag:" + other.gameObject.tag
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Average;
        //Debug.Log("enter: " + udarac.ballMaterial.bounceCombine);

    }

    
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
