using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
// using System.Numerics;

[CustomEditor(typeof(PathDrawer))]
public class PathEditor : Editor {

    PathDrawer creator;

    Path path => creator.path;

    public HandTracking htrack;

    private void OnEnable() {
        creator = (PathDrawer)target;

        if(creator.path == null){
            creator.createPath();
        }
    }

    private void OnSceneGUI() {
        handleInput();
        drawPoints();
    }

    private void handleInput(){
        Vector2 pointerPos = new Vector2(htrack.pos_X, htrack.pos_Y);
        if(htrack.pose == "grab"){
            Undo.RecordObject(creator, "Add Segment");
            path.addSegment(pointerPos);
        }
    }

    private void drawPoints(){
        creator.drawPath(path.points);

        Handles.color = Color.red;
        for(int i = 0; i< path.numPoints; i++){
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.5f, Vector2.zero, Handles.CylinderHandleCap);

            if(path[i] != newPos){
                Undo.RecordObject(creator, "Move Points");
                path.movePoint(i, newPos);
            }
        }
    
    }
    
    // public override void OnInspectorGUI() {
    //     base.OnInspectorGUI();
    // }
}

