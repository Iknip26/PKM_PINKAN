using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSystem3 : MonoBehaviour
{
    
    [System.Serializable]
    public class questionData{
        public Sprite image;
        public string answer;
    }


    [Header("KOMPONEN, ASSET, DATA OBJEK, DAN LAIN LAIN")]
    [Space]
    public GameObject wordBox; public GameObject wordBox_2; public GameObject guiPause; public GameObject guiLose;
    public TextMeshProUGUI txtLevel, txtTime, txtScore, txtTimeout, txtAnswer;
    public HandTracking handTracking;
    public SpriteRenderer guessImage;
    public List<char> listAnswerWord = new List<char>();
    public List<questionData> questionDataList = new List<questionData>(); // Gunakan List sebagai ganti array
    public Slider slider;
    private WordBox3 scriptWordBox;
    private WordBox3 scriptWordBox2;

    [Space]
    [Header("=============================================")]
    [Space]
    [Header("DATA BUKAN OBJEK PADA GAME PADA GAME")]
    public float posSpawnX = -15.37f;  public float posSpawnY = 6; public float defaultSize = 0.25f;
    public bool winCondition = false;
    float tmpWaktu;
    private char[] customArrayVovel = { 'A', 'I', 'U', 'E', 'O' };
    public string rightAnswer;
    public bool isGameActive = true;
    public bool isGameEnded = false;
    private questionData dataChoosen;
    public int gameLevel = 0, gameTime = 0, gameScore = 0;
    public static GameSystem3 instance;  


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

    bool IsVowel(char c)
    {
        // Mengubah karakter menjadi huruf kecil untuk mempermudah perbandingan
        c = char.ToLower(c);

        // Mendeteksi vokal
        if (c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u')
        {
            return true; // Karakter adalah vokal
        }
        else
        {
            return false; // Karakter adalah konsonan
        }
    }

    public void setGuessAnswerText(){
        char[] guessWordArray = dataChoosen.answer.ToCharArray();
        List<string> tmpNewGuessWordArray = new List<string>();
        int randomNumber = UnityEngine.Random.Range(0, 2);
        for (int i = 0; i < guessWordArray.Length; i++)
        {
            if (randomNumber == 0)
            {
                if (i == guessWordArray.Length - 2 || i == guessWordArray.Length - 1)
                {
                    listAnswerWord.Add(guessWordArray[i]);
                    tmpNewGuessWordArray.Add(" _");
                }
                else
                {
                    tmpNewGuessWordArray.Add(guessWordArray[i].ToString());
                }
            }
            else
            {
                if (i == 0 || i == 1)
                {
                    listAnswerWord.Add(guessWordArray[i]);
                    tmpNewGuessWordArray.Add("_ ");
                }
                else
                {
                    tmpNewGuessWordArray.Add(guessWordArray[i].ToString());
                }
            }
        }
        string resultString = string.Join("", tmpNewGuessWordArray);
        txtAnswer.text = resultString;
    }

    public void setGuessImage(){
        guessImage.sprite = dataChoosen.image;
    }

    public void setWordBox(int randomizer){
        Canvas canvas;
        Canvas canvas2;
        string resultString = string.Join("", listAnswerWord);
        string resultString2 = listAnswerWord[0].ToString();

        if(IsVowel(listAnswerWord[0])){
            int randomNumber = UnityEngine.Random.Range(65, 91);
            while(randomNumber == 65 || randomNumber == 69 || randomNumber == 73 || randomNumber == 79 || randomNumber == 85 ){
                randomNumber = UnityEngine.Random.Range(65, 91);
            }
            char randomAlphabet = (char)randomNumber;
            resultString2 += randomAlphabet.ToString();
        }else{
            int randomNumber = UnityEngine.Random.Range(0, customArrayVovel.Length);
            char randomAlphabet = customArrayVovel[randomNumber];
            resultString2 += randomAlphabet.ToString();
        }
        
        if(randomizer == 0){
            canvas = wordBox.GetComponentInChildren<Canvas>();
            canvas2 = wordBox_2.GetComponentInChildren<Canvas>();
            scriptWordBox.letterInBox = resultString;
            scriptWordBox2.letterInBox = resultString2;
        }
        else{
            canvas = wordBox_2.GetComponentInChildren<Canvas>();
            canvas2 = wordBox.GetComponentInChildren<Canvas>();
            scriptWordBox2.letterInBox = resultString;
            scriptWordBox.letterInBox = resultString2;
        }

        TextMeshProUGUI newWBIDTxt = canvas.GetComponentInChildren<TextMeshProUGUI>();
        newWBIDTxt.text = resultString;
        rightAnswer = resultString;

       
        TextMeshProUGUI newWBIDTxt2 = canvas2.GetComponentInChildren<TextMeshProUGUI>();
        newWBIDTxt2.text = resultString2;
       
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
            setGuessAnswerText();
            setGuessImage();
            setWordBox(randomIndex);
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
        scriptWordBox = wordBox.GetComponent<WordBox3>();
        scriptWordBox2 = wordBox_2.GetComponent<WordBox3>();
        acakSoal();
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
            listAnswerWord.Clear();
            acakSoal();
            gameLevel++;
            StartCoroutine(gameDelay(3));
        }
    }
}
