using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class DataManagement : MonoBehaviour
{

    public static string tmpGameMode="", tmpLevel="A-C";

    public static List<string> arraySoal = new List<string>();

     static DataManagement()
    {
        arraySoal.Add("A");
        arraySoal.Add("B");
        arraySoal.Add("B");
    }

    public static IEnumerator UpdateData(string uri, int id_data, int score)
    {
        print("KENAAAAAAAAAAA");
        // Persiapkan data yang akan dikirim dalam bentuk JSON
        string jsonData = "{\"id\":"+ id_data + ", \"score\":"+ score +" }";

        print(jsonData);

        // Buat objek UnityWebRequest untuk metode PUT
        UnityWebRequest request = UnityWebRequest.Put(uri, jsonData);

        // Atur header untuk menandai bahwa data yang dikirim adalah JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Kirim request ke API
        yield return request.SendWebRequest();

        // Periksa apakah ada error dalam kirim permintaan
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Cetak pesan jika berhasil
            Debug.Log("Data berhasil diupdate.");
        }
    }

    public static IEnumerator GetRequest(string uri, ScoreData[] arrayData)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string jsonResponse = webRequest.downloadHandler.text;

                    // Mengonversi string JSON menjadi objek C# menggunakan JsonUtility
                    ScoreData responseData = JsonUtility.FromJson<ScoreData>(jsonResponse);
                    // Sekarang coba parsing JSON
                    arrayData  = JsonUtility.FromJson<ScoreData[]>(jsonResponse);
                    
                    // arrayData = scoreDataArray;
                    // Debug.Log(scoreDataArray[0]);
                    // if (scoreDataArray != null && scoreDataArray.Length > 0)
                    // {
                    //     arrayData = scoreDataArray;
                    // }
                    // else
                    // {
                    //     Debug.LogError("Failed to parse JSON or empty array.");
                    // }
                    break;
            }
        }
    }

    public ScoreData[] getHighScore(ScoreData[] data){
        return data;
    }

   
    // Start is called before the first frame updatevoid Start()
    // void Start()
    // {
    //     StartCoroutine(GetRequest("http://localhost/PKM/getDataScore.php"));  
    // }
}
