using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataManager))]
public class DataManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DataManager myTarget = (DataManager)target;

        if (GUILayout.Button("Add 1000 Gems"))
        {
            myTarget.AddGem(1000);
            Debug.Log("Added 1000 Gems");
        }
        if (GUILayout.Button("Add 1000 Coin"))
        {
            myTarget.AddBlock(1000);
            Debug.Log("Added 1000 Gems");
        }

        if (GUILayout.Button("# RESET ALL DATA #"))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("All data has been reset");
        }
    }
}