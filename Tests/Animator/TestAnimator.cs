using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAnimator : MonoBehaviour {

    public Animator animator;

    public Button aniType0;
    public Button aniType1;
    public Button aniType2;

    private List<Button> btnList;

    // Use this for initialization
    void Start () {

        btnList = new List<Button>();
        btnList.Add(aniType0);
        btnList.Add(aniType1);
        btnList.Add(aniType2);

        aniType0.onClick.AddListener(() =>
        {
            showAniType(0);
        });

        aniType1.onClick.AddListener(() =>
        {
            showAniType(1);
        });

        aniType2.onClick.AddListener(() =>
        {
            showAniType(2);
        });
    }

    void showAniType(int type)
    {
        for (int i = 0; i < btnList.Count; i++)
        {
            btnList[i].interactable = true;
        }
        btnList[type].interactable = false;

        animator.SetInteger("AnimationType", type);
    }
}
