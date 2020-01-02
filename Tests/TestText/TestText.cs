using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour {

    void Awake()
    {
        //Debug.Log("Awake");
    }

    // Use this for initialization
    void Start ()
    {
        Debug.Log(gameObject.name + " Start");
    }

    private void OnEnable()
    {
        Debug.Log(gameObject.name + " OnEnable");

        //StartCoroutine(routine());
    }

    public void startRoutine()
    {
        StartCoroutine(routine());
    }

    IEnumerator routine()
    {
        yield return new WaitForSeconds(3.0f);

        print("routine end");

        yield return null;
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + "Update");
    }
}
