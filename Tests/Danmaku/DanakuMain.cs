using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanakuMain : MonoBehaviour
{
    [SerializeField]
    DanmakuPanel danmakuPanel;

    [SerializeField]
    InputField danmakuMsgInput;

    [SerializeField]
    InputField danmakuSizeInput;

    [SerializeField]
    Button sendBtn;

    [SerializeField]
    Button batchSendBtn;

    // Start is called before the first frame update
    void Start()
    {
        sendBtn.interactable = false;

        sendBtn.onClick.AddListener(onSendBtnClick);
        danmakuMsgInput.onValueChanged.AddListener(onInputTextChanged);

        batchSendBtn.onClick.AddListener(onBatchSenderBtnClick);
    }

    private void onBatchSenderBtnClick()
    {
        List<DanmakuItemData> itemDataList = new List<DanmakuItemData>();
        for (int i = 0; i < 50; i++)
        {
            int fontSize = UnityEngine.Random.Range(10, 50);
            string msg = "这是" + fontSize + "号字";

            DanmakuItemData itemData = new DanmakuItemData();
            itemData.msg = msg;
            itemData.fontSize = fontSize;
            if (fontSize % 3 == 0)
            {
                itemData.fontColor = Color.white;
            }
            else if (fontSize % 7 == 0)
            {
                itemData.fontColor = Color.black;
            }
            else if (fontSize % 11 == 0)
            {
                itemData.fontColor = Color.yellow;
            }

            itemDataList.Add(itemData);
        }

        Debug.Log(danmakuPanel.positionDataToString());
        danmakuPanel.createItems(itemDataList);
    }

    private void onInputTextChanged(string msg)
    {
        sendBtn.interactable = !string.IsNullOrEmpty(msg);
    }

    private void onSendBtnClick()
    {
        string msg = danmakuMsgInput.text;

        int fontSize = 30;
        string sizeStr = danmakuSizeInput.text;
        try
        {
            fontSize = Int32.Parse(sizeStr);
        }
        catch (Exception)
        {
            fontSize = 30;
        }

        List<DanmakuItemData> itemDataList = new List<DanmakuItemData>();
        itemDataList.Add(new DanmakuItemData() { msg = msg, fontSize = fontSize });
        danmakuPanel.createItems(itemDataList);
    }
}
