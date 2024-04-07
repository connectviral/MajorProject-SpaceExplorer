using UnityEngine;
using System.Diagnostics;

public class GestureControlManager : MonoBehaviour
{
    private Process pythonProcess;
    private string pythonScriptPath = "Assets/Scripts/test.py";

    private void Awake()
    {
        // Start the Python script
        StartPythonScript();
    }

    private void StartPythonScript()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = pythonScriptPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        pythonProcess = new Process { StartInfo = startInfo };
        pythonProcess.Start();
        UnityEngine.Debug.Log("Python script started");
    }

    private void OnDestroy()
    {
        // Stop the Python script when the game object is destroyed
        StopPythonScript();
    }

    private void StopPythonScript()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
            UnityEngine.Debug.Log("Python script stopped");
        }
    }
}