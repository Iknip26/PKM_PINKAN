using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WordBox2 : MonoBehaviour
{
    public bool inDropArea, inHandArea, isGrabbed;
    public HandTracking handTracking;
    private GameSystem2 gameSystem;
    private Vector3 startPos, startSize;
    private BoxCollider2D boxCol;
    public string letterInBox;
    public bool Cooldown = false;

    void Start()
    {
        startPos = transform.position;
        startSize = transform.localScale;
        boxCol = handTracking.tangan.GetComponent<BoxCollider2D>();
        GameObject gameManager = GameObject.FindWithTag("gameManager");
        gameSystem = gameManager.GetComponent<GameSystem2>();
    }

    void Update()
    {
      if(gameSystem.isGameActive){
          if (inHandArea && handTracking.pose == "grab" && Cooldown == false)
        {
           if(gameSystem.rightAnswer == letterInBox){
                gameSystem.winCondition = true;
           }
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
