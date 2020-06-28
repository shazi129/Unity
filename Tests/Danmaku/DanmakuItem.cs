using UnityEngine;
using UnityEngine.UI;

public class DanmakuItemData
{
    public string msg = "";
    public int fontSize = 30;
    public Color fontColor = Color.red;
}

public class DanmakuItem : MonoBehaviour
{
    [SerializeField]
    Text contentText;

    public void setContent(DanmakuItemData itemData)
    {
        contentText.text = itemData.msg;
        contentText.fontSize = itemData.fontSize;
        contentText.color = itemData.fontColor;

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentText.transform as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public int getHeight()
    {
        return (int)(transform as RectTransform).rect.height;
    }

    public int getWidth()
    {
        return (int)(transform as RectTransform).rect.width;
    }
}