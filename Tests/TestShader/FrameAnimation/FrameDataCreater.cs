﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class FrameData : ScriptableObject
{
    public List<Vector4> data = new List<Vector4>();
}

public class FrameDataCreater : MonoBehaviour
{
    public string path = "Assets/Tests/TestShader/FrameAnimation";
    public string frameDataName = "frame_data";

    public SpriteAtlas spriteAtlas; 

    [ContextMenu("create frame data")]
    public void createFrameData()
    {
        FrameData frameData = ScriptableObject.CreateInstance<FrameData>();
        for (int i = 0; i < spriteAtlas.spriteCount; i++)
        {
            Rect spriteRect = spriteAtlas.GetSprite("frame_data_" + i).rect;
            frameData.data.Add(new Vector4(spriteRect.x, spriteRect.y, spriteRect.width, spriteRect.height));
        }

        Debug.Log(path);
        AssetDatabase.CreateAsset(frameData, path + "/"+ frameDataName + ".asset");
        AssetDatabase.SaveAssets();
    }
}
