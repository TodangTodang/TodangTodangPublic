using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestSaveEraser))]
public class TestSaveEraserEditorScript : Editor
{
    private TestSaveEraser Eraser;

    public void Awake()
    {
        Eraser = target as TestSaveEraser;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("세이브 데이터 삭제"))
        {
            Eraser.EraseSaveData();
        }
    }
}
