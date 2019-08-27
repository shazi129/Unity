using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TEST_TYPE
{
    REST = 0,
    SHOW_ALGORITHM_POS,
    SHOW_EDITOR_POS,
    EDGE_TEST,
    DRAW_LINE_TEST,
}

public class HexagonalGrid : MonoBehaviour {

    [SerializeField]
    List<HorizontalLayoutGroup> lines = new List<HorizontalLayoutGroup>();

    [SerializeField]
    GameObject hexagonalPrefab;

    [SerializeField]
    Dropdown testOption;

    [SerializeField]
    InputField param1Input;

    [SerializeField]
    Text outPut;

    List<List<Hexanonal>> hexagonals = new List<List<Hexanonal>>();


    // Use this for initialization
    void Start () {
		
        //初始化六边形
        for (int i = 0; i < lines.Count; i++)
        {
            List<Hexanonal> row = new List<Hexanonal>();

            for (int j = 0; j < 12; j++)
            {
                GameObject go = GameObject.Instantiate(hexagonalPrefab);
                Hexanonal logic = go.GetComponent<Hexanonal>();
                logic.setPosition(new Vector2() { x = j, y = i });
                logic.setController(this);
                row.Add(logic);

                go.transform.SetParent(lines[i].transform, false);
            }

            hexagonals.Add(row);
        }

        //初始化测试项目
        testOption.ClearOptions();
        List<Dropdown.OptionData> optionalData = new List<Dropdown.OptionData>();
        optionalData.Add(new Dropdown.OptionData("重置"));
        optionalData.Add(new Dropdown.OptionData("展示算法坐标"));
        optionalData.Add(new Dropdown.OptionData("展示编辑坐标"));
        optionalData.Add(new Dropdown.OptionData("范围"));
        optionalData.Add(new Dropdown.OptionData("画线"));
        testOption.AddOptions(optionalData);
        testOption.onValueChanged.AddListener(onTestOptionChange) ;
    }

    private int _testOptionValue = 0;
    private void onTestOptionChange(int arg0)
    {
        Debug.Log("onTestOptionChange:" + arg0);
        _testOptionValue = arg0;

        //一些不用点就能触发的功能
        switch(_testOptionValue)
        {
            case (int)TEST_TYPE.REST:
                {
                    for (int i = 0; i < hexagonals.Count; i++)
                    {
                        List<Hexanonal> row = hexagonals[i];
                        for (int j = 0; j < row.Count; j++)
                        {
                            row[j].reset();
                        }
                    }
                }
                break;
            case (int)TEST_TYPE.SHOW_EDITOR_POS:
                {
                    for (int i = 0; i < hexagonals.Count; i++)
                    {
                        List<Hexanonal> row = hexagonals[i];
                        for (int j = 0; j < row.Count; j++)
                        {
                            row[j].resetPosition();
                        }
                    }
                }
                break;
            case (int)TEST_TYPE.SHOW_ALGORITHM_POS:
                {
                    for (int i = 0; i < hexagonals.Count; i++)
                    {
                        for (int j = 0; j < hexagonals[i].Count; j++)
                        {
                            Vector2 algorithmPos = HexagonalAlgorithm.positionToAlgorithm(new Vector2(j, i));
                            hexagonals[i][j].setText(algorithmPos.x + "  " + algorithmPos.y);
                        }
                    }
                }
                break;
        }
    }
    
    private void clearSelected()
    {
        for (int i = 0; i < hexagonals.Count; i++)
        {
            for (int j = 0; j < hexagonals[i].Count; j++)
            {
                hexagonals[i][j].setSelected(false);
            }
        }
    }

    private void positionToAlgorithm()
    {
        for (int i = 0; i < hexagonals.Count; i++)
        {
            List<Hexanonal> row = hexagonals[i];
            for (int j = 0; j < row.Count; j++)
            {
                Vector2 p = row[j].getPosition();
                row[j].setPosition(HexagonalAlgorithm.positionToAlgorithm(p));
            }
        }
    }

    private void positionToEditor()
    {
        for (int i = 0; i < hexagonals.Count; i++)
        {
            List<Hexanonal> row = hexagonals[i];
            for (int j = 0; j < row.Count; j++)
            {
                Vector2 p = row[j].getPosition();
                row[j].setPosition(HexagonalAlgorithm.positionToEditor(p));
            }
        }
    }

    List<Vector2> _lineData = new List<Vector2>(); //划线时用到的数据

    public void onHexagonalClick(Vector2 position)
    {
        switch(_testOptionValue)
        {
            case (int)TEST_TYPE.EDGE_TEST: //求相邻点
                {
                    //范围
                    if (string.IsNullOrEmpty(param1Input.text))
                    {
                        showNeighbours(position, 1);
                    }
                    else
                    {
                        showNeighbours(position, int.Parse(param1Input.text));
                    }
                    
                }
                break;
            case (int)TEST_TYPE.DRAW_LINE_TEST:
                {
                    if (_lineData.Count == 0) //第一个点
                    {
                        hexagonals[(int)position.y][(int)position.x].setSelected(true);
                        _lineData.Add(HexagonalAlgorithm.positionToAlgorithm(position));
                    }
                    else if (_lineData.Count == 1) //第二个点
                    {
                        _lineData.Add(HexagonalAlgorithm.positionToAlgorithm(position));
                        List<Vector2> line = HexagonalAlgorithm.drawLine(_lineData[0], _lineData[1]);
                        for (int i = 0; i < line.Count; i++)
                        {
                            line[i] = HexagonalAlgorithm.positionToEditor(line[i]);
                        }
                        setSelected(line, true);

                        outPut.text = "距离：" + HexagonalAlgorithm.distance(_lineData[0], _lineData[1]);
                    }
                    else
                    {
                        clearSelected();
                        _lineData.Clear();
                        hexagonals[(int)position.y][(int)position.x].setSelected(true);
                        _lineData.Add(HexagonalAlgorithm.positionToAlgorithm(position));
                    }
                }
                break;
        }
    }

    //
    public void showNeighbours(Vector2 position, int Area)
    {
        clearSelected();

        Vector2 algorithmPosition = HexagonalAlgorithm.positionToAlgorithm(position);
        List<Vector2> neigbours = HexagonalAlgorithm.edge(algorithmPosition, Area);

        List<Vector2> editorPositions = new List<Vector2>();
        for (int i = 0; i< neigbours.Count; i++)
        {
            Vector2 editorPosition = HexagonalAlgorithm.positionToEditor(neigbours[i]);
            Debug.Log("x:" + neigbours[i].x + ", y:" + neigbours[i].y + " to editor: x:" + editorPosition.x + ", y:" + editorPosition.y);
            editorPositions.Add(editorPosition);
        }
        setSelected(editorPositions, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="positions">算法坐标系中的坐标</param>
    /// <param name="value"></param>
    public void setSelected(List<Vector2> positions, bool value)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Vector2 position = positions[i];
            if (0 <= position.y && position.y < hexagonals.Count)
            {
                List<Hexanonal> row = hexagonals[(int)position.y];
                if (0 <= position.x && position.x < row.Count)
                {
                    row[(int)position.x].setSelected(value);
                }
            }
        }
    }
}
