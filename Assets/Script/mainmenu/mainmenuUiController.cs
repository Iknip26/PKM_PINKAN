using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenuUiController : MonoBehaviour
{
    public static Process process;
    // Start is called before the first frame update
    public TextMeshProUGUI txtHighscore;

    private ScoreData[] dataHighscore;

    public void playGame(){
        if (process == null)
        {
            string pythonScriptPath = "Assets/Script/tes.py";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "python"; // Python interpreter
            startInfo.Arguments = pythonScriptPath; // Script path and arguments
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = false;
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

        SceneManager.LoadScene("Level");
    }

    public void exitGame(){
        if (process != null && !process.HasExited)
        {
            // Terminate the process
            process.Kill();
            print("Python script terminated.");
        }

        Application.Quit();
        print("BERHASIL KELUAR APLIKASI");
    }

    void Start(){
        StartCoroutine(DataManagement.GetRequest("http://localhost/PKM/getDataScore.php", dataHighscore));
        print(dataHighscore);
        // txtHighscore.text = "Highscore : " + dataHighscore[0].data[0].score;
    }

}
