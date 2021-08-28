using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Wall))]
public class WallEditor : Editor
{
    /*
    Wall wall;
    void OnEnable()
    {
        wall = (Wall)target;
    }
    public override void OnInspectorGUI()
    {
        wall.type = (Wall.WallType)EditorGUILayout.EnumPopup("PlayerType", wall.type);
        switch (wall.type)
        {
            case Wall.WallType.basic:
                break;
            case Wall.WallType.glass:
                wall.destroyTime = EditorGUILayout.FloatField("Destroy Time", wall.destroyTime);
                break;
            case Wall.WallType.rotating:
                wall.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", wall.rotationSpeed);
                break;
        }

    }
    */
}