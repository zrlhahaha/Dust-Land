using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EditorWithSubEditor <TEditor,TTarget>: Editor
where TEditor:Editor
where TTarget:Object
{

    public TEditor[] subEditors;

    public void CheckAndCreateSubEditor(TTarget[] targets)
    {
        if (targets == null||targets.Length == 0)
        {
            CleanSubEditor();
            return;
        }

        if (subEditors!=null&& subEditors.Length == targets.Length)
            return;

        CleanSubEditor();
        subEditors = new TEditor[targets.Length];

        for (int i = 0; i < subEditors.Length; i++)
        {
            subEditors[i] = CreateEditor(targets[i]) as TEditor;
        }
        
    }

    public void CleanSubEditor()
    {
        if (subEditors == null)
            return;

        for (int i = 1; i < subEditors.Length; i++)
        {
            DestroyImmediate(subEditors[i]);
        }

        subEditors =null;
    }
    


}
