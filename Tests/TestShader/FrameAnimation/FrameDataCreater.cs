using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FrameData : ScriptableObject
{
    public List<Vector4> data = new List<Vector4>();
}

public class FrameDataCreater : MonoBehaviour
{
    public string path = "Assets/Tests/TestShader/FrameAnimation";
    public string frameDataName = "frame_data";

    public Texture2D textrue; 

    [ContextMenu("create frame data")]
    public void createFrameData()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(textrue.name + "_0");
        Debug.Log(textrue.name + "  " + sprites.Length);

//         FrameData frameData = ScriptableObject.CreateInstance<FrameData>();
//         for (int i = 0; i < sprites.Length; i++)
//         {
//             Rect spriteRect = sprites[i].rect;
//             frameData.data.Add(new Vector4(spriteRect.x, spriteRect.y, spriteRect.width, spriteRect.height));
//         }
// 
//         Debug.Log(path);
//         AssetDatabase.CreateAsset(frameData, path + "/"+ frameDataName + ".asset");
//         AssetDatabase.SaveAssets();
    }
}
