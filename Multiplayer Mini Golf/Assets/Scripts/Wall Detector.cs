using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WallDetector : NetworkBehaviour
{
    private Udarac udarac;

    
    void LateUpdate()
    {
        
        transform.rotation = Quaternion.identity;
        
    }

    public override void OnNetworkSpawn()
    {
        
        udarac = GetComponentInParent<Udarac>();

        
        if (udarac == null)
        {
            Debug.LogError("Udarac reference not found on the parent object!");
        }
        
    }
 
    
    private void OnTriggerEnter(Collider other)
    {
        
        
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Average;
        

    }

    
    private void OnTriggerExit(Collider other)
    {
        
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
        
    }

    private void OnTriggerStay(Collider other)
    {
        
            if (udarac.isIdle)
            {
                udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            }
            else
            {
                udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Average;
            }
            
        
        
    }

}
