using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PathDrawer : MonoBehaviour
{

    public Path path;

    private LineRenderer myLineRenderer;

    public void createPath(){
        path = new Path(transform.position);

        myLineRenderer = this.AddComponent<LineRenderer>();
        myLineRenderer.widthMultiplier = 0.2f;
    }

    public void drawPath(List<Vector2> points){
        this.GetComponent<LineRenderer>().positionCount = points.Count;
        for(int i = 0; i<points.Count; i++){
            this.GetComponent<LineRenderer>().SetPosition(i, points[i]);
        }
    }
}
