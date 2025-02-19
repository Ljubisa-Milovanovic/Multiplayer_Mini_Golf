using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraStacker : MonoBehaviour
{
    void Start()
    {
        // Get the Universal Additional Camera Data component
        var cameraData = GetComponent<UniversalAdditionalCameraData>();
        if (cameraData != null)
        {
            // Find the UI Camera in the scene by tag
            Camera uiCamera = GameObject.FindWithTag("UICamera")?.GetComponent<Camera>();

            if (uiCamera != null)
            {
                // Add the UI Camera to the stack if not already added
                if (!cameraData.cameraStack.Contains(uiCamera))
                {
                    cameraData.cameraStack.Add(uiCamera);
                    Debug.Log("UI Camera added to camera stack.");
                }
            }
            else
            {
                Debug.LogError("UI Camera not found. Ensure it has the 'UICamera' tag and is in the scene.");
            }
        }
        else
        {
            Debug.LogError("UniversalAdditionalCameraData component not found on the Player Camera.");
        }
    }
}
