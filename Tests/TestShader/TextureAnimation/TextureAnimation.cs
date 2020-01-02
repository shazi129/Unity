using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        for (int i = 0; i < transform.childCount; i++)
        {
            block.SetFloat("_VCount", i);
            Renderer s = transform.GetChild(i).GetComponent<Renderer>();
            s.SetPropertyBlock(block);
        }
    }
}
