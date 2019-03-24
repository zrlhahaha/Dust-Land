using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponSystem))]
public class WeaponSystemEditor : EditorWithSubEditor<Editor,WeaponHandle>{

    public WeaponSystem weaponSystem;
    public SerializedProperty modules;
    public bool[] isExpend;

    public const string VarName_Module = "modules";

    private void OnEnable()
    {
        if (target == null)
            DestroyImmediate(this);

        weaponSystem = target as WeaponSystem;

        isExpend = weaponSystem.weaponHandles==null? new bool[0]: new bool[weaponSystem.weaponHandles.Length];
        for (int i = 0; i < isExpend.Length; i++)
        {
            isExpend[i] = false;
        }

        
        modules = serializedObject.FindProperty(VarName_Module);
        GetModules();
        CheckAndCreateSubEditor(weaponSystem.weaponHandles);
    }

    //public override void OnInspectorGUI()
    //{

    //    serializedObject.Update();
    //    base.OnInspectorGUI();

    //    CheckAndCreateSubEditor(weaponSystem.weaponHandles);


    //    if (subEditors == null)
    //    {
    //        EditorGUILayout.Space();
    //        EditorGUILayout.LabelField("There is no module is weaponSystem,Create a module or drag one from prefab");
    //    }
    //    else
    //        for (int i = 0; i < subEditors.Length; i++)
    //        {
    //            EditorGUILayout.BeginVertical(GUI.skin.box);

    //            isExpend[i] = EditorGUILayout.Foldout(isExpend[i], weaponSystem.weaponHandles[i].name);
    //            if (isExpend[i])
    //            {
    //                subEditors[i].OnInspectorGUI();
    //            }

    //            EditorGUILayout.EndVertical();
    //        }
    //    serializedObject.ApplyModifiedProperties();
    //}


    void GetModules()
    {
        weaponSystem.weaponHandles = weaponSystem.GetComponentsInChildren<WeaponHandle>();
    }


}
