using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Canvas c = gameObject.GetComponentInParent<Canvas>();
        if (c != null)
        {
            Debug.Log(c.gameObject.name);
        }
        else
        {
            Debug.Log("cannot find canvas in parent");
        }
    }

}
