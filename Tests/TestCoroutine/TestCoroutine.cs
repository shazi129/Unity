using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCoroutine : MonoBehaviour
{
    private Coroutine _coroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        _coroutine = StartCoroutine(countDown());
        _coroutine = StartCoroutine(countDown());
    }

    int i = 0;

    private IEnumerator countDown()
    {
        while(true)
        {
            GetComponent<Text>().text = "" + i;
            i++;

            
            yield return new WaitForSeconds(1.0f);
        }

        yield return null;
    }

    private void Update()
    {
        if (i == 5)
        {
            Debug.Log("_coroutine is null: " + (_coroutine == null));
            StopCoroutine(_coroutine);
            Debug.Log("_coroutine is null: " + (_coroutine == null));
        }
        
    }
}
