using System;
using UnityEngine;
using UnityEngine.UI;

public class TestAttribute : CustomBehaviour
{
    public GameObject prefab;

    [Bind(".")]
    public Canvas canvas;

    [Bind(".")]
    public RectTransform rectTransform;

    [Bind("TestBtn")]
    public Button testBtn;

    [Bind("TestInput")]
    public InputField testInput;

    
    void Start()
    {
        testBtn.onClick.AddListener(onTestBtnClick);
        testInput.gameObject.SetActive(false);
    }

    private void onTestBtnClick()
    {
        Debug.Log("onTestBtnClick");
    }
}
