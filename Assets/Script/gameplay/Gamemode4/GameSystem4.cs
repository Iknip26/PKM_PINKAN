using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem4 : MonoBehaviour
{
    
[System.Serializable]
    public class questionData{
        public AudioClip audio;
        public string answer;
    }


    [Header("KOMPONEN, ASSET, DATA OBJEK, DAN LAIN LAIN")]
    [Space]
    public GameObject wordBox; public GameObject wordBox_2; public GameObject wordBox_3; public GameObject guiPause; public GameObject guiLose;
    public TextMeshProUGUI txtLevel, txtTime, txtScore, txtTimeout;
    public HandTracking handTracking;
    public List<AudioBox2> listGuessAudio = new List<AudioBox2>();


    public List<questionData> questionDataList = new List<questionData>(); // Gunakan List sebagai ganti array
    public Slider slider;
    private WordBox4 scriptWordBox;
    private WordBox4 scriptWordBox2;
    private WordBox4 scriptWordBox3;
    public List<WordBox4> listScriptWordbox = new List<WordBox4>();


    [Space]
    [Header("=============================================")]
    [Space]
    [Header("DATA BUKAN OBJEK PADA GAME PADA GAME")]
    public float posSpawnX = -15.37f;  public float posSpawnY = 6; public float defaultSize = 0.25f;
    public bool winCondition = false;
    float tmpWaktu;
    public string rightAnswer;
    public bool isGameActive = true;
    public bool isGameEnded = false;
    private List<questionData> listDataChoosen = new List<questionData>();
    public int gameLevel = 0, gameTime = 0, gameScore = 0, pointToWin = 0;
    public static GameSystem4 instance;  


    private void Awake() {
        instance = this;
    }

    void setInfoUi(){
        // txtTime.text = (gameTime + 1).ToString();
        // txtScore.text = gameScore.ToString();
        // txtLevel.text = gameLevel.ToString();

        slider.value = gameLevel;
        txtLevel.text = gameLevel.ToString();
    }

    public void setAudio(){

        // int randomizer = UnityEngine.Random.Range(0, 6);
        // List<int> listTmp = new List<int>();
        // listTmp.Add(0);                
        // listTmp.Add(1);
        // listTmp.Add(2);
    
        for( int i = 0; i < listGuessAudio.Count; i++){
            int tmp = UnityEngine.Random.Range(0, listDataChoosen.Count);
            listGuessAudio[i].audio.clip = listDataChoosen[tmp].audio;
            AudioBox2 tmpAudioScript = listGuessAudio[i].gameObject.GetComponent<AudioBox2>();
            tmpAudioScript.rightAnswer = listDataChoosen[tmp].answer;
            listGuessAudio[i].isFinished = false;
            listDataChoosen.RemoveAt(tmp);
        }
       
    
    }

    public void setWordBox(){
        Canvas canvas;
        Canvas canvas2;
        Canvas canvas3;

        canvas = wordBox.GetComponentInChildren<Canvas>();
        canvas2 = wordBox_2.GetComponentInChildren<Canvas>();
        canvas3 = wordBox_3.GetComponentInChildren<Canvas>();
        scriptWordBox.letterInBox = listDataChoosen[0].answer;
        scriptWordBox2.letterInBox = listDataChoosen[1].answer;
        scriptWordBox3.letterInBox = listDataChoosen[2].answer;

        TextMeshProUGUI newWBIDTxt = canvas.GetComponentInChildren<TextMeshProUGUI>();
        // print(scriptWordBox.letterInBox);
        newWBIDTxt.text = scriptWordBox.letterInBox;

        TextMeshProUGUI newWBIDTxt2 = canvas2.GetComponentInChildren<TextMeshProUGUI>();
        newWBIDTxt2.text = scriptWordBox2.letterInBox;
        // print(scriptWordBox.letterInBox);

        
        TextMeshProUGUI newWBIDTxt3 = canvas3.GetComponentInChildren<TextMeshProUGUI>();
        newWBIDTxt3.text = scriptWordBox3.letterInBox;
        // print(scriptWordBox.letterInBox);


       
    }

    void acakSoal()
    {
        if (questionDataList.Count > 0)
        {
            int randomIndex;
            for (int i = 0; i < 3; i++)
            {
                listScriptWordbox[i].Cooldown = true;
                randomIndex = UnityEngine.Random.Range(0, questionDataList.Count);
                listDataChoosen.Add(questionDataList[randomIndex]);
                questionDataList.RemoveAt(randomIndex);
            }
                setWordBox();
                setAudio();
        }
        else
        {
            Debug.LogWarning("List soal kosong!");
        }
    }

    // public void playAudio(int num){
    //     listGuessAudio[0].audio.Play();

    //     if(num == 1){
    //     }else if(num == 2){
    //         listGuessAudio[1].audio.Play();
    //     }else{
    //         listGuessAudio[2].audio.Play();
    //     }
    // }

    // IEnumerator playAudioWithDelay(int time, int num){
    //     yield return new WaitForSeconds(time);

    //     if(num == 1){
    //         listGuessAudio[0].audio.Play();
    //     }else if(num == 2){
    //         listGuessAudio[1].audio.Play();
    //     }else{
    //         listGuessAudio[2].audio.Play();
    //     }
    // }

    // Method untuk menunda permainan
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            isGameActive = false;
            Time.timeScale = 0f; // Mengatur timescale ke 0 untuk menunda permainan
            guiPause.SetActive(true);
        }
        else{
            Time.timeScale = 1f; // Mengatur timescale ke 0 untuk menunda permainan
            isGameActive = true;
            guiPause.SetActive(false);
        }
    }
    
    IEnumerator gameDelay(int time){
        scriptWordBox.Cooldown = true;
        yield return new WaitForSeconds(time);
        scriptWordBox.Cooldown = false;
    }

    void Start()
    {
        scriptWordBox = wordBox.GetComponent<WordBox4>();
        scriptWordBox2 = wordBox_2.GetComponent<WordBox4>();
        scriptWordBox3 = wordBox_3.GetComponent<WordBox4>();
        listScriptWordbox.Add(scriptWordBox);
        listScriptWordbox.Add(scriptWordBox2);
        listScriptWordbox.Add(scriptWordBox3);
        acakSoal();
    }
    void Update()
    {
        setInfoUi();

        if(isGameActive){
            if (gameTime > 0)
            {
                tmpWaktu += Time.deltaTime;
                if(tmpWaktu>=1){
                    gameTime--;
                    tmpWaktu = 0;
                }
            }
        }
        if(pointToWin == 3){
            pointToWin = 0;
            winCondition = false;
            listDataChoosen.Clear();
            foreach(AudioBox2 obj in listGuessAudio){
                obj.lineRenderer.positionCount = 0;
            }
            acakSoal();
            gameLevel++;
            StartCoroutine(gameDelay(3));
        }
    }
}
