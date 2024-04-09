using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class CameraAccess : MonoBehaviour
{
    // Import Windows API functions for accessing devices
    [DllImport("avicap32.dll")]
    public static extern IntPtr capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    public static extern void ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("avicap32.dll")]
    public static extern bool capGetDriverDescription(int wDriverIndex, IntPtr lpszName, int cbName, IntPtr lpszVer, int cbVer);
    private WebCamTexture webcamTexture;

    void Start()
    {
        StartCoroutine(RequestCameraPermission());
    }

    IEnumerator RequestCameraPermission()
    {
        // Request permission to access the camera
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        // Check if permission was granted
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("Camera permission granted");

            // Get available cameras
            WebCamDevice[] devices = WebCamTexture.devices;

            if (devices.Length > 0)
            {
                // Use the first available camera
                webcamTexture = new WebCamTexture(devices[0].name);
                webcamTexture.Play();

                // Display camera feed on a plane or object
                GetComponent<Renderer>().material.mainTexture = webcamTexture;
            }
            else
            {
                Debug.LogError("No camera device found");
            }
        }
        else
        {
            Debug.Log("Camera permission denied");
        }
    }
    List<string> EnumerateCameras()
    {
        List<string> cameraNames = new List<string>();

        for (int i = 0; i < 5; i++) // Assume maximum of 5 cameras
        {
            IntPtr namePtr = Marshal.AllocHGlobal(100);
            IntPtr versionPtr = Marshal.AllocHGlobal(100);

            if (capGetDriverDescription(i, namePtr, 100, versionPtr, 100))
            {
                string name = Marshal.PtrToStringAnsi(namePtr);
                cameraNames.Add(name);
            }

            Marshal.FreeHGlobal(namePtr);
            Marshal.FreeHGlobal(versionPtr);
        }

        return cameraNames;
    }
}
