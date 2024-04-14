using UnityEngine;
using System.Net.Sockets;
using System.IO;

public class CameraAccess : MonoBehaviour
{
    private WebCamTexture webcamTexture;
    private Socket clientSocket;
    private BinaryWriter writer;

    void Start()
    {
        StartCoroutine(RequestCameraPermission());
        ConnectToServer();
    }

    System.Collections.IEnumerator RequestCameraPermission()
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

    void ConnectToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect("127.0.0.1", 12345);
            Debug.Log("Connected to Python server");

            writer = new BinaryWriter(new NetworkStream(clientSocket));
        }
        catch (SocketException e)
        {
            Debug.LogError($"Socket connection error: {e.Message}");
        }
    }

    void Update()
    {
        // Send camera frames to Python script
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            Texture2D tex = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
            tex.SetPixels(webcamTexture.GetPixels());
            tex.Apply();

            byte[] bytes = tex.EncodeToJPG();
            SendData(bytes);
        }
    }

    void SendData(byte[] data)
    {
        try
        {
            if (writer != null)
            {
                // Send camera frame data to Python script
                writer.Write(data.Length);
                writer.Write(data);
            }
            else
            {
                Debug.LogError("Error sending data: Socket connection not established.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error sending data: {e.Message}");
        }
    }

    void OnDestroy()
    {
        if (clientSocket != null && clientSocket.Connected)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
