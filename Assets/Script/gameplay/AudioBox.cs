using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioBox : MonoBehaviour
{
    public bool inDropArea, inHandArea, isGrabbed;
    public HandTracking handTracking;
    private GameSystem2 gameSystem;
    private BoxCollider2D boxCol;


    IEnumerator playAudio(){
        gameSystem.playAudio();
        yield return new WaitForSeconds(3); // Delay selama 3 detik\
    }

    void Start()
    {
        boxCol = handTracking.tangan.GetComponent<BoxCollider2D>();
        GameObject gameManager = GameObject.FindWithTag("gameManager");
        gameSystem = gameManager.GetComponent<GameSystem2>();
    }

    void Update()
    {
      if(gameSystem.isGameActive){
          if (inHandArea && handTracking.pose=="grab")
        {
            StartCoroutine(playAudio());
        }

      }

    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("tangan"))
        {
            inHandArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("tangan"))
        {
            inHandArea = false;
        }
    }
}
