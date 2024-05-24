using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem2 : MonoBehaviour
{
    
    [System.Serializable]
    public class questionData{
        public AudioClip audio;
        public string answer;
    }


    [Header("KOMPONEN, ASSET, DATA OBJEK, DAN LAIN LAIN")]
    [Space]
    public GameObject gameObjStage1; public GameObject gameObjStage2; public GameObject gameObjStage3; public GameObject wordBox; public GameObject wordBox_2; public GameObject guiPause; public GameObject guiWin;
    public TextMeshProUGUI txtSlider, txtScore, txtLevelTittle;
    public HandTracking handTracking;
    public AudioSource guessAudio;
    public List<GameObject> wordBoxList = new List<GameObject>();
    public List<questionData> questionDataList = new List<questionData>(); // Gunakan List sebagai ganti array
    public Slider slider;
    private WordBox2 scriptWordBox;
    private WordBox2 scriptWordBox2;

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
    private questionData dataChoosen;
    public int gamePhase = 1, gameStage = 3, gameScore = 0, tmpSlider = 0;
    public static GameSystem2 instance;



    private void Awake() {
        instance = this;
    }

    void setInfoUi(){
        txtSlider.text = tmpSlider.ToString();
        slider.value = tmpSlider;
        txtScore.text = gameScore.ToString();
        // print(tmpSlider);
    }

    public void setAudio(){
        guessAudio.clip = dataChoosen.audio;
    }


    public void setWordBox(int randomizer){
        Canvas canvas;
        Canvas canvas2;
        int randomNumber = UnityEngine.Random.Range(65, 71);
        char randomAlphabet = (char)randomNumber;

        while(randomAlphabet.ToString() == dataChoosen.answer.ToString()){
            randomNumber = UnityEngine.Random.Range(65, 71);
            randomAlphabet = (char)randomNumber;
        }
        if(randomizer == 0){
            canvas = wordBox.GetComponentInChildren<Canvas>();
            canvas2 = wordBox_2.GetComponentInChildren<Canvas>();
            scriptWordBox.letterInBox = dataChoosen.answer;
            scriptWordBox2.letterInBox = randomAlphabet.ToString();
        }
        else{
            canvas = wordBox_2.GetComponentInChildren<Canvas>();
            canvas2 = wordBox.GetComponentInChildren<Canvas>();
            scriptWordBox2.letterInBox = dataChoosen.answer;
            scriptWordBox.letterInBox = randomAlphabet.ToString();
        }

        TextMeshProUGUI newWBIDTxt = canvas.GetComponentInChildren<TextMeshProUGUI>();
        newWBIDTxt.text = dataChoosen.answer;
        rightAnswer = dataChoosen.answer;

       
        TextMeshProUGUI newWBIDTxt2 = canvas2.GetComponentInChildren<TextMeshProUGUI>();
        newWBIDTxt2.text = randomAlphabet.ToString();

    }

    void acakSoal()
    {
        if (questionDataList.Count > 0) // Pastikan list tidak kosong
        {
            scriptWordBox.Cooldown = true;
            int randomIndex = UnityEngine.Random.Range(0, questionDataList.Count);
            dataChoosen = questionDataList[randomIndex];
            print(dataChoosen.answer);
            questionDataList.RemoveAt(randomIndex);
            randomIndex = UnityEngine.Random.Range(0, 2);
            setAudio();
            setWordBox(randomIndex);
        }
        else
        {
            Debug.LogWarning("List soal kosong!");
        }
    }

    public void playAudio(){
        guessAudio.Play();
    }

    IEnumerator playAudioWithDelay(int time){
        yield return new WaitForSeconds(time);
        guessAudio.Play();
    }

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

    public void initStage(int stage){
        if(stage == 1){
            gameObjStage1.SetActive(true);
        }else if(stage == 2){
            gameObjStage2.SetActive(true);
        }else{
            gameObjStage3.SetActive(true);
        }
    }

    public void playGame1(){
       
    }

    public void playGame2(){

    }

    public void playGame3(){
        acakSoal();
        StartCoroutine(playAudioWithDelay(1));
        StartCoroutine(gameDelay(3));
    }

    void Start()
    {
        initStage(gameStage);
        // print(DataManagement.tmpLevel);
        txtLevelTittle.text = DataManagement.tmpLevel.ToString();
        scriptWordBox = wordBox.GetComponent<WordBox2>();
        scriptWordBox2 = wordBox_2.GetComponent<WordBox2>();
        playGame3();
    }
    void Update()
    {
        setInfoUi();

        if(gamePhase<=3){
            if(gameStage==1){
                if(winCondition == true){
                    winCondition = false;
                    playGame1();
                    gamePhase++;
                    gameScore+=20;
                    tmpSlider++;
                    StartCoroutine(gameDelay(3));
                }

            }else if(gameStage==2){
                if(winCondition == true){
                    winCondition = false;
                    playGame2();
                    gamePhase++;
                    gameScore+=20;
                    tmpSlider++;
                    StartCoroutine(gameDelay(3));
                }
            }else if(gameStage==3){
                if(winCondition == true){
                    winCondition = false;
                    playGame3();
                    gamePhase++;
                    gameScore+=20;
                    tmpSlider++;
                    StartCoroutine(gameDelay(3));
                }
            }

        }else{
            gameStage++;
            initStage(gameStage);
        }

        if(gameStage>3){
            isGameEnded = true;
        }

        if(isGameEnded){
            isGameActive = false;
            guiWin.SetActive(true);
        }

    }
}
