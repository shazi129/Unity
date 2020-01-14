using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PositionTextrue : MonoBehaviour
{
    public string textrueName = "position_texture";

    public int frameTexSize = 1024;

    public List<Sprite> frameSpriteList = new List<Sprite>();

    public int getPositionTextrueSize()
    {
        int count = frameSpriteList.Count - 1;
        int size = 0;
        while(count > 0)
        {
            count = count >> 1;
            size++;
        }
        return size;
    }

    [ContextMenu("create 1024 texture")]
    public void create1024Textrue()
    {
        int size = 1024;
        string filePath = Application.dataPath + "/Image/" + size + ".png";
        Texture2D texture = new Texture2D(size, size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float value = (float)(i * size + j) / (size * size);
                texture.SetPixel(i, j, new Color(value, value, value, value));
            }
        }

        Debug.Log(texture.GetPixel(100, 100).ToString());

        writeToPNG(filePath, texture);
    }

    [ContextMenu("read frame texture")]
    public void readFrameTextrue()
    {
        string filePath = Application.dataPath + "/Image/" + textrueName + ".png";
        Texture2D textrue = readFromPNG(filePath);
        if (textrue == null)
        {
            Debug.LogError("cannot read texture from:" + filePath);
            return;
        }

        string debugInfo = "read from png: \n";
        for (int i = 0; i < textrue.width; i++)
        {
            for (int j = 0; j < textrue.height; j++)
            {
                Color color = textrue.GetPixel(i, j);
                if (color == null)
                {
                    continue;
                }
                debugInfo = debugInfo + "(" + i + ", " + j + ")" + color.ToString() + "\n";
            }
        }
        Debug.Log(debugInfo);
    }

    [ContextMenu("create frame texture")]
    public void createTextrue()
    {
        if (frameSpriteList.Count <= 0)
        {
            Debug.LogError("sprite count error!!");
            return;
        }

        int pixelSize = 1;

        int positionTextureSize = getPositionTextrueSize();
        Texture2D positionTexture = new Texture2D(positionTextureSize * pixelSize, positionTextureSize * pixelSize);

        string debugInfo = "write to png: \n";

        for (int i = 0; i < positionTextureSize; i++)
        {
            for (int j = 0; j < positionTextureSize; j++)
            {
                int frameIndex = i * positionTextureSize + j;

                Color color = new Color(0, 0, 0, 0);
                if (frameIndex >= 0 && frameIndex < frameSpriteList.Count)
                {
                    Rect rect = frameSpriteList[frameIndex].rect;
                    color.r = rect.x / frameTexSize;
                    color.g = rect.y / frameTexSize;
                    color.b = rect.width / frameTexSize;
                    color.a = rect.height / frameTexSize;
                }

                positionTexture.SetPixel(i, j, color);
                debugInfo = debugInfo + "(" + i + ", " + j + ")" + color.ToString() + "\n";
            }
        }
        Debug.Log(debugInfo);

        string filePath = Application.dataPath + "/Image/" + textrueName + ".png";
        writeToPNG(filePath, positionTexture);
    }

    private void writePixel(Texture2D textrue, int x, int y, Color color, int pixelSize)
    {
        int startX = x * pixelSize;
        int startY = y * pixelSize;

        for (int i = 0; i < pixelSize; i++)
        {
            for (int j = 0; j < pixelSize; j++)
            {
                textrue.SetPixel(startX + i, startY + j, color);
            }
        }
    }


    private void writeToPNG(string filePath, Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        
        Debug.Log("out path:" + filePath);

        FileStream file = File.Open(filePath, FileMode.Create);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
    }

    public static Texture2D readFromPNG(string filePath)
    {
        Texture2D tex = null;
        if (File.Exists(filePath))
        {
            byte[]  fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(4, 4);
            tex.LoadImage(fileData);
        }
        return tex;
    }
}
