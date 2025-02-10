using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    private Udarac udarac;

    private Quaternion initialLocalRotation;
    void LateUpdate()
    {
        // Reset the local rotation to the initial rotation
        //transform.localRotation = initialLocalRotation;
        transform.rotation = Quaternion.identity;
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
        initialLocalRotation = transform.localRotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Nesto me je pipnulo ...zid..... PAUSE");
        // set bouncines to 1
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Average;
        Debug.Log("Blizu sam zida i bounce mode je: " + udarac.ballMaterial.bounceCombine);

    }

    // Optional: This method is called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        //set bouncines to 0
        udarac.ballMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
        Debug.Log("nisam blizu zida i bounce mode je: " + udarac.ballMaterial.bounceCombine);
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
