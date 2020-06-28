//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace NTween
{
    public interface ITransformTween 
    {
        Transform GetTarget();
        void SetTarget(Transform target);
    }
}