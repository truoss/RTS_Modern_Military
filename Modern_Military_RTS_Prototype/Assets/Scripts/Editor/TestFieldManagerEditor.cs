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
        if (GUILayout.Button("GetDiagonal"))
            tfield.GetDiagonal();
        if (GUILayout.Button("StartLinedrawing"))
            tfield.TestLinedrawing();
        if (GUILayout.Button("CoordinateRange"))
            tfield.TestCoordinateRange();
        if (GUILayout.Button("ReachableFields"))
            tfield.TestReachable();
        if (GUILayout.Button("ResetAllColor"))
            tfield.ResetAllColor();
    }

}
