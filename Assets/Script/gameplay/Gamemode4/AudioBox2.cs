using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBox2 : MonoBehaviour
{
    public bool inDropArea, inHandArea, isGrabbed;
    public HandTracking handTracking;
    private GameSystem4 gameSystem;
    private BoxCollider2D boxCol;
    private GameObject handGameObj;
    public GameObject startPos;
    public int idBox;
    public AudioSource audio = new AudioSource();
    public string rightAnswer;
    public LineRenderer lineRenderer;
    private bool isDragging;
    private Vector3 linePosition;
    private Vector3 endPoint;
    private WordBox4 wordBox;
    public bool isFinished = false;


    IEnumerator playAudio(){
        audio.Play();
        yield return new WaitForSeconds(3);
    }

    void Start()
    {
        boxCol = handTracking.tangan.GetComponent<BoxCollider2D>();
        GameObject gameManager = GameObject.FindWithTag("gameManager");
        gameSystem = gameManager.GetComponent<GameSystem4>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

    }

    void Update()
    {
        if (gameSystem.isGameActive && isFinished == false){
            if ( inHandArea && handTracking.pose == "grab" )
            {
                StartCoroutine(playAudio());
                isDragging = true;
                lineRenderer.SetPosition(0, startPos.transform.position);
            }
            else if (isDragging && handTracking.pose =="grab" ){
                lineRenderer.SetPosition(1,handGameObj.transform.position);
                linePosition = handGameObj.transform.position;
            }
            else{
                isDragging = false;
                RaycastHit2D hit = Physics2D.Raycast(linePosition, Vector2.zero);
                if (hit.collider != null)
                {
                    GameObject hitGameObject = hit.collider.gameObject.GetComponent<WordBox4>().endPoint;
                    endPoint = hitGameObject.transform.position;
                    if (hit.collider.TryGetComponent(out wordBox) && rightAnswer == wordBox.letterInBox)
                    {
                        Debug.Log("Correct Form!");
                        gameSystem.pointToWin++;
                        isFinished = true;
                        gameSystem.gameScore += 20;
                    }else{
                        lineRenderer.positionCount = 0;
                    }
                }
                else
                {
                    lineRenderer.positionCount = 0;
                }

                lineRenderer.positionCount = 2;
            }
        
        }
      }


    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.gameObject.CompareTag("tangan"))
        {
            inHandArea = true;
            handGameObj = trigger.gameObject;
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
