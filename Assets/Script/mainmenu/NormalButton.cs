using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalButton : MonoBehaviour
{
    public Boolean inHandArea;

    public HandTracking handTracking;

    public string pageNext;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (inHandArea && handTracking.pose=="grab")
        {
            btnPindah();
        }
    }
    public void btnPindah(){
        SceneManager.LoadSceneAsync(pageNext);
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
