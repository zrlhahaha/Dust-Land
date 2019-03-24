using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ResourceManager))]
public class ObjectPoolEditor : Editor {

    public ResourceManager objectPool;

    public SerializedProperty prefabsProperty;
    public SerializedProperty defaultAmountProperty;

    public const string VarName_PrefabsProperty = "prefabs";
    public const string VarName_DefaultAmountProperty = "defaultAmount";

    private void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }
        objectPool = target as ResourceManager;

        prefabsProperty = serializedObject.FindProperty(VarName_PrefabsProperty);
        defaultAmountProperty = serializedObject.FindProperty(VarName_DefaultAmountProperty);
    }

    public override void OnInspectorGUI()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        serializedObject.Update();

        DrawCustomEditor();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawCustomEditor()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Object Pool Editor:");
        EditorGUILayout.Space();

        if(objectPool.prefabs.Count == 0)
            EditorGUILayout.LabelField("There is no pool ,Click \"Add Pool\" Button to Add Pool");

        for (int i = 0;i< objectPool.prefabs.Count;i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("pool " + i+":");
            if (GUILayout.Button("Remove"))
            {
                objectPool.prefabs.RemoveAt(i);
                objectPool.defaultAmount.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(prefabsProperty.GetArrayElementAtIndex(i));
            EditorGUILayout.PropertyField(defaultAmountProperty.GetArrayElementAtIndex(i));
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add Pool"))
        {
            objectPool.prefabs.Add(null);
            objectPool.defaultAmount.Add(0);
        }

        EditorGUILayout.EndVertical();
    }

}
