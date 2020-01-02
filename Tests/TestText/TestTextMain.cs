using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTextMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);

            Debug.Log(transform.GetChild(i).gameObject.name + " SetActive");
        }

        Debug.Log(gameObject.name + " Start");

    }

    private void OnEnable()
    {
        //Debug.Log(gameObject.name + " OnEnable");
    }

    private void Update()
    {
        //Debug.Log(gameObject.name + " Update");
    }

}
