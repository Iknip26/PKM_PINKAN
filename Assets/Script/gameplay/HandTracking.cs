using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    // Start is called before the first frame update
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    public GameObject tangan;

    [HideInInspector]public float pos_X, pos_Y, prePos_X=0, prePos_Y=0;

    [HideInInspector]public float speed_X, speed_Y;

    private int poseCounter = 0;
    [HideInInspector]public string pose = "iddle";

    public SpriteRenderer ikonTangan;
    public Sprite grab, iddle, thumbsup;

    public Transform raycastTangan, grabPos;
    [SerializeField] private float rayDistance;

    private bool isObjColliderResized = false;

    [HideInInspector]public bool isGrabbing = false;

    public int ID_ITEM_DIPEGANG = 0;

    private float layerObjects;


    void Start()
    {
        layerObjects = LayerMask.NameToLayer("Objects");
        tangan = GameObject.FindWithTag("tangan");
    }

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;
        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        // print(data);
        string[] points = data.Split(',');
        // print(points[0]);

        //0        1*3      2*3
        //x1,y1,z1,x2,y2,z2,x3,y3,z3

        for (int i = 0; i < 21; i++)
        {

            pos_X = (5 - float.Parse(points[i * 3]) / 100);
            pos_Y = ((float.Parse(points[i * 3 + 1]) / 100));

            if (i == 0)
            {
                if(Math.Abs(pos_X-prePos_X) <= 5 && Math.Abs(pos_Y-prePos_Y) <= 5
                 ){

                    tangan.transform.localPosition = new Vector2( (pos_X * speed_X * 0.8f), ((pos_Y - 1) * speed_Y * 0.8f));
                }

            }

            prePos_X = pos_X;
            prePos_Y = pos_Y;


            handPoints[i].transform.localPosition = new Vector2(pos_X, pos_Y);
        }

        poseCounter++;
        if (poseCounter % 13 == 0)
        {
            pose = poseCheck(handPoints);

        }

        if (pose == "iddle")
        {
            ikonTangan.sprite = iddle;
            // print("iddle");
        }
        else if (pose == "grab")
        {
            ikonTangan.sprite = grab;
            // print("grab");
        }


        // RaycastHit2D itemDetectionInfo = Physics2D.Raycast(raycastTangan.position, transform.right, rayDistance);

        // if (itemDetectionInfo.collider != null)
        // {
        //     if (itemDetectionInfo.collider.gameObject.layer == layerObjects)
        //     {
        //         BoxCollider2D colliderObject = itemDetectionInfo.collider as BoxCollider2D;

        //         if (pose == "grab")
        //         {
        //             objectGrabbed = itemDetectionInfo.collider.gameObject;
        //             isGrabbing = true;
        //             if(!isObjColliderResized){
        //                 Vector2 newSize = new Vector2(colliderObject.size.x + 20f, colliderObject.size.y + 20f);
        //                 colliderObject.size = newSize;
        //                 isObjColliderResized = true;
        //             }
                   
        //             itemDetectionInfo.collider.gameObject.transform.position = grabPos.position;
        //             itemDetectionInfo.collider.gameObject.transform.SetParent(transform);
        //         }

        //         else if (pose == "iddle")
        //         {
        //             objectGrabbed = null;
        //             isGrabbing = false;
        //             if(isObjColliderResized){
        //                 Vector2 newSize = new Vector2(colliderObject.size.x - 20f, colliderObject.size.y - 20f);
        //                 colliderObject.size = newSize;
        //                 isObjColliderResized = false;
        //             }
        //             itemDetectionInfo.collider.gameObject.transform.SetParent(null);
        //         }
        //     }
        // }

    }

    public string poseCheck(GameObject[] hands)
    {
        bool[] syarat = new bool[5];
        int syaratTerpenuhi = 0;
        syarat[0] = (hands[1].transform.position.x < hands[4].transform.position.x || hands[1].transform.position.x - hands[4].transform.position.x < 1);
        syarat[1] = hands[8].transform.position.y < hands[5].transform.position.y;
        syarat[2] = hands[12].transform.position.y < hands[9].transform.position.y;
        syarat[3] = hands[16].transform.position.y < hands[13].transform.position.y;
        syarat[4] = hands[20].transform.position.y < hands[17].transform.position.y;

        if (syarat[0] && syarat[1] && syarat[2] && syarat[3] && syarat[4])
        {
            return "grab";
        }
        else
        {
            foreach (bool isiSyarat in syarat)
            {
                if (isiSyarat == true)
                {
                    syaratTerpenuhi++;
                }
            }

            if (syaratTerpenuhi >= 2)
            {
                syaratTerpenuhi = 0;
                return pose;
            }

            else
            {
                syaratTerpenuhi = 0;
                return "iddle";
            }

        }
    }
}