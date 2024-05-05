using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;


public class GameSystem : MonoBehaviour
{
    
    [System.Serializable]
    public class questionData{
        public Sprite image;
        public string answer;
    }
    [Header("KOMPONEN, ASSET, DATA OBJEK, DAN LAIN LAIN")]
    [Space]
    public GameObject wordBox; public GameObject dropPlace; public GameObject guiPause; public GameObject guiLose;
    public TextMeshProUGUI txtLevel, txtTime, txtScore, txtTimeout;
    public HandTracking handTracking;
    public SpriteRenderer guessImage;
    public List<GameObject> wordBoxList = new List<GameObject>(),  dropPlaceList = new List<GameObject>();
    public questionData[] questionDataArray;
    public Slider slider;
    [Space]
    [Header("=============================================")]
    [Space]
    [Header("DATA BUKAN OBJEK PADA GAME PADA GAME")]
    public float posSpawnX = -15.37f;  public float posSpawnY = 6; public float defaultSize = 0.25f;
    public int winCondition;
    public bool isGameActive = true;
    public bool isGameEnded = false;

    private questionData dataChoosen;
    public int gameLevel = 0, gameTime = 0, gameScore = 0;
    public static GameSystem instance;  


    private void Awake() {
        instance = this;
    }

    void SpawnObject(int ID_ITEM, string[] dataChoosenArray, float posSpawnX, float posSPawnY, Quaternion spawnRotation)
    {
        if (wordBox != null)
        {

            double tmpPengali = (double)dataChoosenArray.Length / 2;
            // print(arrayDataTerpilih.Length + "dibagi 2 =" + tmpPengali);

            tmpPengali = Math.Ceiling(tmpPengali);
            // print("tmppengali dibulatkan ke atas =" + tmpPengali);

            float pengaliUkuran = (float)tmpPengali - 2;
            // print("pengaliUkuran =" + pengaliUkuran);


            // INISIASI OBJEK BARU
            GameObject tmpNewObject = Instantiate(wordBox, new Vector3(posSpawnX + ( (2 * ID_ITEM) - 0.5f * pengaliUkuran), posSpawnY-1), spawnRotation);
            // wordBox.SetActive(true);
            tmpNewObject.SetActive(true);
           

            // Set ATribut Objek Baru
            tmpNewObject.transform.localScale = new Vector3(1.25f - 0.25f * pengaliUkuran, 1.25f - 0.25f * pengaliUkuran);
            WordBox scriptObjekBaru = tmpNewObject.GetComponent<WordBox>();
            Canvas canvas = tmpNewObject.GetComponentInChildren<Canvas>();
            TextMeshProUGUI newWBIDTxt = canvas.GetComponentInChildren<TextMeshProUGUI>();
            newWBIDTxt.text = dataChoosenArray[ID_ITEM];


            // GameObject gameManager = GameObject.FindWithTag("gameManager");
            // HandTracking handTracking = gameManager.GetComponent<HandTracking>();


            scriptObjekBaru.handTracking = this.handTracking;
            scriptObjekBaru.ID_ITEM = ID_ITEM + 1;
            scriptObjekBaru.letterInBox = dataChoosenArray[ID_ITEM];

            wordBoxList.Add(tmpNewObject);
            // print(wordBoxList)
        }
        else
        {
            Debug.LogError("Prefab objek baru belum ditentukan!");
        }
    }

    void SpawnTempatDrop(int ID_ITEM, string[] arrayDataTerpilih, float posSpawnX, float posSPawnY, Quaternion rotasiSpawn)
    {
        if (dropPlace != null)
        {
            double tmpPengali = (double)arrayDataTerpilih.Length / 2;
            // print(arrayDataTerpilih.Length + "dibagi 2 =" + tmpPengali);

            tmpPengali = Math.Ceiling(tmpPengali);
            // print("tmppengali dibulatkan ke atas =" + tmpPengali);

            float pengaliUkuran = (float)tmpPengali - 2;
            // print("pengaliUkuran =" + pengaliUkuran);

            // INISIASI OBJEK BARU
            GameObject newTempatDrop = Instantiate(dropPlace, new Vector3((posSpawnX) + ( (2 * ID_ITEM) - 0.5f * pengaliUkuran), posSpawnY - 3), rotasiSpawn);
            newTempatDrop.SetActive(true);

            // Set ATribut Objek Baru
        
            DropPlace scriptTempatDropBaru = newTempatDrop.GetComponent<DropPlace>();
            scriptTempatDropBaru.idItem = ID_ITEM + 1;
            scriptTempatDropBaru.containLetter = arrayDataTerpilih[ID_ITEM];
            dropPlaceList.Add(newTempatDrop);
        }

        else
        {
            Debug.LogError("Prefab objek baru belum ditentukan!");
        }
    }

    void setInfoUi(){
        // txtTime.text = (gameTime + 1).ToString();
        // txtScore.text = gameScore.ToString();
        // txtLevel.text = gameLevel.ToString();

        slider.value = gameLevel;
   

        // int menit = UnityEngine.Mathf.FloorToInt(gameTime / 60);
        // int detik = UnityEngine.Mathf.FloorToInt(gameTime % 60);

        // txtTime.text = menit.ToString("00") + ":" + detik.ToString("00");

    }

    static void ShuffleArray<T>(T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T nilai = array[k];
            array[k] = array[n];
            array[n] = nilai;
        }
    }

    void acakSoal(){
        dataChoosen = questionDataArray[UnityEngine.Random.Range(0, questionDataArray.Length)];
        print(dataChoosen.answer);
        guessImage.sprite = dataChoosen.image;
        string[] arraydataTerpilih = dataChoosen.answer.Select(c => c.ToString()).ToArray();
        string[] arraydataTerpilihAcak = dataChoosen.answer.Select(c => c.ToString()).ToArray();
        // foreach(string tmpcek in arraydataTerpilihAcak){
        //     print(tmpcek + " / ");
        // }

        // foreach(string tmpcek in arraydataTerpilih){
        //     print(tmpcek + " / ");
        // }
        winCondition = arraydataTerpilih.Length;
        // print(winCondition);
        // print(arraydataTerpilihAcak.Length);
        // print(string.Join(", ", arraydataTerpilih));
        // print(string.Join(", ", arraydataTerpilihAcak));

        ShuffleArray(arraydataTerpilihAcak);
        for(int i=0; i<arraydataTerpilihAcak.Length; i++){
            if(arraydataTerpilihAcak!=null){
                SpawnObject(i, arraydataTerpilihAcak, posSpawnX, posSpawnY, Quaternion.identity);
            }else{
                print("array kosng");
            }
            
        }

         for(int i=0; i<arraydataTerpilih.Length; i++){
            SpawnTempatDrop(i, arraydataTerpilih, posSpawnX, posSpawnY, Quaternion.identity);
        }
    }

    void timeCheck(){
        if(gameTime == 0){
            guiLose.SetActive(true);
            isGameActive = false;
            isGameEnded = true;
            StartCoroutine(DataManagement.UpdateData("http://localhost/PKM/updateDataScore.php", 1, gameScore));
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

    // Method untuk melanjutkan permainan
    // void ResumeGame()
    // {
    //     Time.timeScale = 1f; // Mengatur timescale ke 1 untuk melanjutkan permainan
    // }

    void Start()
    {
        acakSoal();
    }


    float tmpWaktu;

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

        if(winCondition <= 0){
            foreach(GameObject isi in wordBoxList){
                Destroy(isi);
            }
            foreach(GameObject isi in dropPlaceList){
                Destroy(isi);
            }
            acakSoal();
            gameLevel++;
        }

        timeCheck();
    }
} 
