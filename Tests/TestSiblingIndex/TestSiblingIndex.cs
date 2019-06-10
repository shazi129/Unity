using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSiblingIndex : MonoBehaviour {

    [SerializeField]
    Transform layout;

    [SerializeField]
    GameObject buttonPrefab;

	// Use this for initialization
	void Start ()
    {
	    for (int i = 0; i < 10; i++)
        {
            int r = UnityEngine.Random.Range(1, 100);
            GameObject newButton = GameObject.Instantiate(buttonPrefab);
            Text t = newButton.GetComponentInChildren<Text>();

            t.text = r.ToString();
            newButton.name = t.text;

            
            newButton.transform.SetParent(layout.transform, false);
            //newButton.transform.SetSiblingIndex(r);

            newButton.GetComponent<Button>().onClick.AddListener(onButtonClick);
        }

        onButtonClick();

    }

    private void onButtonClick()
    {
        int childCount = layout.childCount;
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < childCount; i++)
        {
            Transform child = layout.GetChild(i);
            children.Add(layout.GetChild(i));
            Debug.Log("" + child.GetSiblingIndex());
        }

        children.Sort((Transform t1, Transform t2) => 
        {
            int num1 = int.Parse(t1.gameObject.name);
            int num2 = int.Parse(t2.gameObject.name);
            if (num1 < num2)
            {
                return -1;
            }
            else if (num2 == num1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        });

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}
