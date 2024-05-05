using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashSCreen : MonoBehaviour
{
   IEnumerator PindahScene(string nama)
    {
        yield return new WaitForSeconds(3); // Delay selama 3 detik\
        GetComponent<Animator>().Play("pinkan_out_anim");
        yield return new WaitForSeconds(1); // Delay selama 3 detik\
        SceneManager.LoadSceneAsync(nama);
    }

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(PindahScene("mainmenu"));
    }
}
