using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject wordBox; public GameObject wordBox_2; public GameObject guiPause; public GameObject guiLose;
    public TextMeshProUGUI txtLevel, txtTime, txtScore, txtTimeout;
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
    public int gameLevel = 0, gameTime = 0, gameScore = 0;
    public static GameSystem2 instance;  


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

    void Start()
    {
        scriptWordBox = wordBox.GetComponent<WordBox2>();
        scriptWordBox2 = wordBox_2.GetComponent<WordBox2>();
        acakSoal();
        StartCoroutine(playAudioWithDelay(1));
        StartCoroutine(gameDelay(3));
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

        if(winCondition == true){
            winCondition = false;
            acakSoal();
            playAudio();
            gameLevel++;
            StartCoroutine(gameDelay(3));
        }
    }
}
