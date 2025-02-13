using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player ball"))
        {
            // Reset the ball's position to the last valid location
            if (GameMenager.instance != null)
            {
                other.transform.position = GameMenager.instance.lastLocation;

                // Optional: Reset the ball's velocity to prevent it from flying off again
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    Udarac udarac = other.GetComponent<Udarac>();
                    udarac.isIdle = true;
                }

                Debug.Log("Ball went out of bounds and was reset to the last valid position.");
            }
        }
        }
    }
