using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine;

public class Chastercontroler : MonoBehaviour
{
    void Start()
    {
        // Path to your Python script
        string pythonScriptPath = "Assets/Scripts/test.py";

        // Run the Python script
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = pythonScriptPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        Process process = new Process { StartInfo = startInfo };
        process.Start();
        UnityEngine.Debug.Log("Python script started");
    }
}

