using UnityEngine;
using UnityEngine.UI;

public class CameraAccess : MonoBehaviour
{
    public RawImage rawImage; // Reference to the RawImage component

    private void Start()
    {
        // Check if the device has a camera
        if (WebCamTexture.devices.Length > 0)
        {
            // Access the first available camera
            WebCamTexture webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);

            // Set the texture of the RawImage component to display the camera feed
            rawImage.texture = webCamTexture;

            // Start capturing from the camera
            webCamTexture.Play();
        }
        else
        {
            Debug.LogError("No camera found on the device.");
        }
    }

    private void OnDestroy()
    {
        // Stop capturing from the camera when the script is destroyed
        rawImage.texture = null; // Clear the texture
    }
}
