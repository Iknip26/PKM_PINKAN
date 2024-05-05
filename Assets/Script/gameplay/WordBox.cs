using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WordBox : MonoBehaviour
{
    public bool inDropArea, inHandArea, isGrabbed;
    public HandTracking handTracking;
    private GameSystem gameSystem;
    private Vector3 startPos, startSize;
    private GameObject dropPlace;
    private BoxCollider2D boxCol;
    public bool isColliderResized, isScorePLus = false;
    public int ID_ITEM;
    public string letterInBox;





    void Start()
    {
        startPos = transform.position;
        startSize = transform.localScale;
        boxCol = handTracking.tangan.GetComponent<BoxCollider2D>();
        GameObject gameManager = GameObject.FindWithTag("gameManager");
        gameSystem = gameManager.GetComponent<GameSystem>();
    }

    void Update()
    {
      if(gameSystem.isGameActive){
          if (inHandArea && handTracking.pose == "grab")
        {
            if(handTracking.ID_ITEM_DIPEGANG == 0){
                handTracking.ID_ITEM_DIPEGANG = ID_ITEM;
            }

            if(ID_ITEM == handTracking.ID_ITEM_DIPEGANG){
                isGrabbed = true;
                transform.position = handTracking.grabPos.position;
                if (!isColliderResized && boxCol != null)
                {
                    Vector2 newSize = new Vector2(boxCol.size.x + 3f, boxCol.size.y + 3f);
                    boxCol.size = newSize;
                    isColliderResized = true;
            }
            }
        }
        else
        {
            isGrabbed = false;
            if (isColliderResized && boxCol != null)
            {
                handTracking.ID_ITEM_DIPEGANG = 0;
                Vector2 newSize = new Vector2(boxCol.size.x - 3f, boxCol.size.y - 3f);
                boxCol.size = newSize;
                isColliderResized = false;
            }
        }
        
        if (!isGrabbed)
        {
            if (inDropArea)
            {
                if(letterInBox == dropPlace.transform.GetComponent<DropPlace>().containLetter){
                    transform.localPosition = dropPlace.transform.localPosition;
                    transform.localScale = dropPlace.transform.localScale;
                    if(dropPlace.transform.GetComponent<DropPlace>().isFilled == false){
                        gameSystem.winCondition--;
                        dropPlace.transform.GetComponent<DropPlace>().isFilled = true;
                        if(isScorePLus == false){
                            gameSystem.gameScore += 20;
                            isScorePLus = true;
                        }
                    }
                
            }  else
            {
                transform.position = startPos;
                transform.localScale = startSize;
            }
            }
            else
            {
                transform.position = startPos;
                transform.localScale = startSize;
            }
        }

      }

    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("droparea"))
        {
            dropPlace = trigger.gameObject;
            inDropArea = true;
        }
        if (trigger.gameObject.CompareTag("tangan"))
        {
            inHandArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("droparea"))
        {
            inDropArea = false;
        }
        if (trigger.gameObject.CompareTag("tangan"))
        {
            inHandArea = false;
        }
    }
}
