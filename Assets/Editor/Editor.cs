using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor (typeof (Generator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Generator mapGen = (Generator)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
            mapGen.GenerateMap();
    }
}
