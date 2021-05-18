using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexMapSystem))]
public class HexMapManagerEditor : Editor
{
    HexMapSystem hexMapManager;

    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        hexMapManager = target as HexMapSystem;
        if(GUILayout.Button("Spawn New Map")){
            hexMapManager.init();
        }
    }
    
}
