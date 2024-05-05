using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class RunExternalScript : MonoBehaviour
{
    private Process process;
    void Start()
    {
        // Path to your Python script
        string pythonScriptPath = "Assets/tes.py";
        //    if (System.IO.File.Exists(pythonScriptPath))
        // {
        //     // Code to run the Python script
        //     print("Python script found at: " + pythonScriptPath);
        // }
        // else
        // {
        //     print("Python script not found at: " + pythonScriptPath);
        // }
        
        // Arguments if needed
        // string arguments = "argument1 argument2";

        // Create new process start info
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "python"; // Python interpreter
        startInfo.Arguments = pythonScriptPath; // Script path and arguments
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = false;

        // Create and start the process
    try
        {
            // Create and start the process
            process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            print("Python script started.");
        }
    catch (System.Exception e)
        {
            print("Error starting Python script: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        // Check if the process is running
        if (process != null && !process.HasExited)
        {
            // Terminate the process
            process.Kill();
            print("Python script terminated.");
        }
    }
}