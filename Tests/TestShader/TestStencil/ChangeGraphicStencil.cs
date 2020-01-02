using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChangeGraphicStencil : MonoBehaviour
{
    [SerializeField]
    int stencilRef = 20;

    [SerializeField]
    StencilOp stencilOp;

    [SerializeField]
    CompareFunction stencilComp;

    // Start is called before the first frame update
    void Start()
    {
        Graphic[] m = GetComponentsInChildren<Graphic>();
        

        for (int i = 0; i < m.Length; i++)
        {
            Material a = GameObject.Instantiate(m[i].material);
            a.SetInt("_Stencil", stencilRef);
            a.SetInt("_StencilComp", (int)stencilComp);
            a.SetInt("_StencilOp", (int)stencilOp);

            m[i].material = a;
        }
    }

}
