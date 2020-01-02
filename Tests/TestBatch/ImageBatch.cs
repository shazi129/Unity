using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBatch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IdObjectMap objMap = GetComponent<IdObjectMap>();

        Debug.Log("transform.childCount:" + transform.childCount);

        for (int i = 0; i < transform.childCount; i++)
        {
            Sprite s = objMap.getObject(i) as Sprite;
            if ( s != null)
            {
                Image image = transform.GetChild(i).GetComponent<Image>();
                image.sprite = s;
            }
            else
            {
                Debug.Log("========i:" + i);
            }
        }
    }

}
