using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorStart {

    static EditorStart()
    {
        Debug.Log("Up and running");
        //EditorApplication.update += Update;
    }

    static void Update()
    {
        Debug.Log("Updating");
    }
}
