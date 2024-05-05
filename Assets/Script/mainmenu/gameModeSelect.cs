using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameModeSelect : MonoBehaviour
{
    public void gantiMode(string nama){
        DataManagement.tmpGameMode = nama;
        print(DataManagement.tmpGameMode);
    }

    public void gantiLevel(string nama){
        DataManagement.tmpLevel = nama;
        print(DataManagement.tmpGameMode);
    }
    public void btnPindah(string nama){
        SceneManager.LoadSceneAsync(nama);
    }

}
