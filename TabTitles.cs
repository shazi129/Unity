using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class TabTitles : MonoBehaviour {

    public enum TAB_LAYOUT
    {
        Vertical, Horizontal
    }

    public bool canEdit = false;

    //布局
    public TAB_LAYOUT tabLayout = TAB_LAYOUT.Horizontal;
    public List<string> tabNameList;

    //tab样式
    public GameObject selectTabPrefab;
    public GameObject unselectTabPrefab;

    public int focusTabIndex { set; get; }
    public Action<int> tabSelectedAction { get; set; }
    public Action initAction { get; set; }

    [SerializeField]
    private List<GameObject> _tabGroups;

    [SerializeField]
    private List<Button> _selectTabs;

    [SerializeField]
    private List<Button> _unselectTabs;

    #region create Tab Titles
    [ContextMenu("CreateTabTitles")]
    public void createTabTitles()
    {
        if (canEdit)
        {
            createTabGroup(0, unselectTabPrefab);
            createTabGroup(1, selectTabPrefab);
            refreshSize();
        }
    }

    [ContextMenu("RemoveTabTitles")]
    public void removeTabTitles()
    {
        if (canEdit)
        {
            for (int i = 0; i < _tabGroups.Count; i++)
            {
#if UNITY_EDITOR
                DestroyImmediate(_tabGroups[i]);
#else
            Destroy(_tabGroups[i]);
#endif
            }
            _tabGroups.Clear();
            _selectTabs.Clear();
            _unselectTabs.Clear();
        }
    }

    [ContextMenu("CreateTabTitles", true)]
    private bool canCreateTabTitles()
    {
        return canEdit;
    }
    [ContextMenu("RemoveTabTitles", true)]
    private bool canRemoveTabTitles()
    {
        return canEdit;
    }

    private RectTransform create2DGameObject()
    {
        GameObject go = new GameObject();
        RectTransform rectTransform = go.AddComponent<RectTransform>();
        return rectTransform;
    }

    private void createTabGroup(int tabType, GameObject tabPrefab)
    {
        //创建
        RectTransform rectTransform = create2DGameObject();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);

        GameObject tabGroup = rectTransform.gameObject;
        tabGroup.name = tabType == 0 ? "UnselectedTabGroup" : "SelectedTabGroup";
        tabGroup.transform.SetParent(this.transform, false);

        //标签布局
        if (tabLayout == TAB_LAYOUT.Horizontal)
        {
            HorizontalLayoutGroup hLayout = tabGroup.AddComponent<HorizontalLayoutGroup>();
            hLayout.childForceExpandHeight = false;
            hLayout.childForceExpandWidth = false;
            hLayout.childAlignment = TextAnchor.MiddleLeft;
        }
        else
        {
            VerticalLayoutGroup vLayout = tabGroup.AddComponent<VerticalLayoutGroup>();
            vLayout.childForceExpandHeight = false;
            vLayout.childForceExpandWidth = false;
            vLayout.childAlignment = TextAnchor.UpperCenter;
        }

        //创建tab标签
        createTabs(tabPrefab, tabGroup, tabType);

        //保存引用
        _tabGroups.Add(tabGroup);

    }

    private void createTabs(GameObject tabPrefab, GameObject tabGroup, int tabType)
    {
        if (tabNameList == null)
        {
            Debug.LogWarning("TabTitles::createTabs, name was not set");
            return;
        }
        for (int i = 0; i < tabNameList.Count; i++)
        {
            //创建一个tab容器,方便布局
            RectTransform tabContainer = create2DGameObject();
            tabContainer.gameObject.name = "Tab_Container" + (i + 1);
            tabContainer.sizeDelta = (tabPrefab.transform as RectTransform).sizeDelta;
            tabContainer.SetParent(tabGroup.transform, false);

            //创建
            GameObject tabGO = GameObject.Instantiate(tabPrefab);
            tabGO.name = "Tab_" + (i + 1);
            tabGO.SetActive(true);
            tabGO.transform.SetParent(tabContainer, false);

            List<Button> tabRefList = tabType == 0 ? _unselectTabs : _selectTabs;
            tabRefList.Add(tabGO.GetComponent<Button>());

            //tab名称
            if (!string.IsNullOrEmpty(tabNameList[i]))
            {
                Transform tabNameTrans = tabGO.transform.Find("TabName");
                if (tabNameTrans != null)
                {
                    Text tabNameText = tabNameTrans.GetComponent<Text>();
                    if (tabNameText != null)
                    {
                        tabNameText.text = tabNameList[i];
                    }
                }
            }
        }
    }


    private void refreshSize()
    {
        if (_selectTabs != null && _selectTabs.Count > 0)
        {
            Vector2 itemSize = (_selectTabs[0].transform as RectTransform).sizeDelta;

            RectTransform selfTransform = transform as RectTransform;
            if (tabLayout == TAB_LAYOUT.Horizontal)
            {
                selfTransform.sizeDelta = new Vector2(_selectTabs.Count * itemSize.x, itemSize.y);
            }
            else
            {
                selfTransform.sizeDelta = new Vector2(itemSize.x, _selectTabs.Count * itemSize.y);
            }
        }
    }
    #endregion

    void Start()
    {
        //未选中的按钮是可以点的
        for (int i = 0; i < _unselectTabs.Count; i++)
        {
            if (_unselectTabs[i] != null)
            {
                GameObject tabGO = _unselectTabs[i].gameObject;
                _unselectTabs[i].onClick.AddListener(() => { onTabBtnClick(tabGO); });
            }
        }

        if (initAction != null)
        {
            initAction();
        }
        else
        {
            setTabFocus(0);
        }
    }

    private void onTabBtnClick(GameObject obj)
    {
        for (int i = 0; i < _unselectTabs.Count; i++)
        {
            if (obj == _unselectTabs[i].gameObject)
            {
                setTabFocus(i);
            }
        }
    }

    public void setTabFocus(int tabIndex)
    {
        focusTabIndex = tabIndex;
        for (int i = 0; i < _selectTabs.Count; i++)
        {
            if (_selectTabs[i] != null)
            {
                _selectTabs[i].gameObject.SetActive(i == tabIndex);
            }
            
        }

        if (tabSelectedAction != null)
        {
            tabSelectedAction(tabIndex);
        }
    }
}
