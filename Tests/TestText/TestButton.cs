using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(onClick1);
        b.onClick.AddListener(onClick2);
    }

    private void onClick1()
    {
        Debug.Log("onClick1");
    }

    private void onClick2()
    {
        Debug.Log("onClick2");
    }
}
