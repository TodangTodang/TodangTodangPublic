using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomerFaceDecoSetSO))]
public class CustomerFaceDecoSetSOEditorScript : Editor
{
    private CustomerFaceDecoSetSO _data;
    
    private void Awake()
    {
        _data = target as CustomerFaceDecoSetSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        int Length = Enum.GetValues(typeof(Enums.FaceType)).Length;    
        for (int i = 0; i <Length;++i)
        {
            Material face = _data.GetFaceSet((Enums.FaceType)i);
            GUILayout.Box(face.mainTexture,GUILayout.Height(100),GUILayout.Width(100));    
        }
        GUILayout.EndHorizontal();
    }
}
