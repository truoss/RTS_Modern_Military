using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestFieldManager))]
public class TestFieldManagerEditor : Editor {

    public override void OnInspectorGUI () {
        DrawDefaultInspector();
        TestFieldManager tfield = (TestFieldManager) target;
        if (GUILayout.Button("GetField"))
            tfield.GetField();
        if (GUILayout.Button("GetNeighbour"))
            tfield.GetNeighbour();
    }

}
