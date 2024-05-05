using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    string sceneName;

       public Slider slider;
    // Start is called before the first frame update

   public void btnRestart(){
          sceneName = SceneManager.GetActiveScene().name;
          GameSystem.instance.guiPause.SetActive(false);
          GameSystem.instance.guiLose.SetActive(false);
          Time.timeScale = 1f;
          GetComponent<Animator>().Play("scene_end");
   }

    public void btnPindah(string nama){
          sceneName = nama;
          Time.timeScale = 1f;
          GameSystem.instance.guiPause.SetActive(false);
          GetComponent<Animator>().Play("scene_end");
   }

   public void pindahScene(){
          GameSystem.instance.GetComponent<UDPReceive>().disableScript();
          SceneManager.LoadScene(sceneName);
   }


}
