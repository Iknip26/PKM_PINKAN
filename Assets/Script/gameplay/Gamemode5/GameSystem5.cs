using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem5 : MonoBehaviour
{
    
[System.Serializable]
    public class questionData{
        public Sprite image;
        public string answer;
    }


    [Header("KOMPONEN, ASSET, DATA OBJEK, DAN LAIN LAIN")]
    [Space]
    public GameObject wordBox; public GameObject wordBox_2; public GameObject wordBox_3; public GameObject guiPause; public GameObject guiLose;
    public TextMeshProUGUI txtLevel, txtTime, txtScore, txtTimeout;
    public HandTracking handTracking;
    public List<ImageBox> listGuessImage = new List<ImageBox>();


    public List<questionData> questionDataList = new List<questionData>(); // Gunakan List sebagai ganti array
    public Slider slider;
    private WordBox5 scriptWordBox;
    private WordBox5 scriptWordBox2;
    private WordBox5 scriptWordBox3;
    public List<WordBox5> listScriptWordbox = new List<WordBox5>();


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
    public static GameSystem5 instance;  


    private void Awake() {
        instance = this;
    }

    void setInfoUi(){
    
        slider.value = gameLevel;
        txtLevel.text = gameLevel.ToString();
    }

    public void setImage(){

        for( int i = 0; i < listGuessImage.Count; i++){
            int tmp = UnityEngine.Random.Range(0, listDataChoosen.Count);
            listGuessImage[i].image.sprite = listDataChoosen[tmp].image;
            ImageBox tmpImageScript = listGuessImage[i].gameObject.GetComponent<ImageBox>();
            tmpImageScript.rightAnswer = listDataChoosen[tmp].answer;
            listGuessImage[i].isFinished = false;
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
                setImage();
        }
        else
        {
            Debug.LogWarning("List soal kosong!");
        }
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
        scriptWordBox = wordBox.GetComponent<WordBox5>();
        scriptWordBox2 = wordBox_2.GetComponent<WordBox5>();
        scriptWordBox3 = wordBox_3.GetComponent<WordBox5>();
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
            foreach(ImageBox obj in listGuessImage){
                obj.lineRenderer.positionCount = 0;
            }
            acakSoal();
            gameLevel++;
            StartCoroutine(gameDelay(3));
        }
    }
}
