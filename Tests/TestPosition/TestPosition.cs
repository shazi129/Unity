using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPosition : MonoBehaviour {

    [SerializeField]
    Button buttonPrefab;

    [SerializeField]
    HorizontalLayoutGroup layout;

    [SerializeField]
    Canvas otherCanvas;

    // Use this for initialization
    void Start ()
    {
        buttonPrefab.onClick.AddListener(onClick);

        for (int i = 0; i < 5; i++)
        {
            GameObject obj = GameObject.Instantiate(buttonPrefab.gameObject);
            obj.transform.SetParent(layout.transform, false);
            
            Debug.Log("localPosition in layout:" + obj.transform.localPosition.ToString());

        }
    }

    private void onClick()
    {
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            Vector3 localPosition = layout.transform.GetChild(i).localPosition;
            Debug.Log("localPosition in layout:" + localPosition.ToString());

            Vector3 worldPosition = layout.transform.TransformPoint(localPosition);
            Debug.Log("worldPosition:" + worldPosition.ToString());

            Vector3 localPosition1 = otherCanvas.transform.InverseTransformPoint(worldPosition);
            Debug.Log("localPosition1:" + localPosition1.ToString());

            GameObject obj = GameObject.Instantiate(buttonPrefab.gameObject);
            obj.transform.SetParent(otherCanvas.transform, false);
            obj.transform.localPosition = localPosition1;
        }
    }
}
