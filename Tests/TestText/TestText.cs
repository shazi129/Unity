using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Text t = gameObject.GetComponent<Text>();
        if (t != null)
        {
            t.text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\nbbbbbbbbbbbbbbbbbbbbbb";
        }
	}
}
