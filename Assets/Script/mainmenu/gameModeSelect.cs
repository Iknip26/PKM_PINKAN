using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameModeSelect : MonoBehaviour
{
    public TextMeshProUGUI txtGameModeNow;
    public void gantiMode(string nama){
        DataManagement.tmpGameMode = nama;
        if(nama == "1"){
            txtGameModeNow.text = "MENGENAL HURUF";
        }else if(nama == "2"){
            txtGameModeNow.text = "LENGKAPI KATA";
        }else if(nama == "3"){
            txtGameModeNow.text = "MENYUSUN HURUF";
        }else if(nama == "4"){
            txtGameModeNow.text = "MENYUSUN HURUF";
        }
        print(DataManagement.tmpGameMode);
    }

    public void gantiLevel(string nama){
        DataManagement.tmpLevel = nama;
        print(DataManagement.tmpGameMode);
        print(DataManagement.tmpGameMode);
        if(DataManagement.tmpGameMode == "1"){
            btnPindah("gamemode_2");
        }else if(DataManagement.tmpGameMode == "2"){
            btnPindah("gamemode_3");
        }
    }
    public void btnPindah(string nama){
        SceneManager.LoadSceneAsync(nama);
    }

}
