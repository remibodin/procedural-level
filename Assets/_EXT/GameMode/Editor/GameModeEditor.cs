using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameMode))]
public class GameModeEditor : Editor 
{
    SerializedProperty gameModeAsset;
    SerializedProperty playerless;


    private void OnEnable() 
    {
        gameModeAsset = serializedObject.FindProperty("_gameMode");
        playerless = serializedObject.FindProperty("_playerlessScene");
    }

    public override void OnInspectorGUI() 
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField (gameModeAsset, new GUIContent ("Game mode profil"));
        EditorGUILayout.PropertyField (playerless, new GUIContent ("Playerless scene"));        
        serializedObject.ApplyModifiedProperties();
    }
}
