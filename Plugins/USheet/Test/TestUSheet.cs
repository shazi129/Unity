﻿using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace USheet
{
    public class TestUSheet : MonoBehaviour
    {
        public SheetData data = null;
        public string dataPath = "Assets/Resource/Student.asset";

        public InputField inputField;
        public Text output;

        public Button getRow1Btn;
        public Button getRow2Btn;
        public Button modifyBtn;
        public Button indexOfBtn;
        


        // Use this for initialization
        void Start()
        {
            if (data == null)
            {
#if UNITY_EDITOR
                data = AssetDatabase.LoadAssetAtPath<SheetData>(dataPath);
                if (data == null)
                {
                    Debug.LogWarning("load sheet data error");
                    return;
                }
#endif
            }

            if (data == null)
            {
                Debug.LogWarning("sheet data is null");
                return;

            }

            if (getRow1Btn != null)
            {
                getRow1Btn.onClick.AddListener(onTestGetRowBtnClick);
            }
            if (getRow2Btn != null)
            {
                getRow2Btn.onClick.AddListener(onGetRow2BtnClick);
            }
            if (modifyBtn != null)
            {
                modifyBtn.onClick.AddListener(onModifyBtnClick);
            }
            if (indexOfBtn!= null)
            {
                indexOfBtn.onClick.AddListener(onIndexOfBtnClick);
            }

            output.text = "row:" + data.rowCount + ", col:" + data.columnCount;
        }

        private string formatRow(Dictionary<string, IGridData> rowData)
        {
            if (rowData == null) return "null";

            string outputText = "";
            foreach (var item in rowData)
            {
                outputText += string.Format("{0}:{1}\n", item.Key, item.Value.ToString());
            }
            return outputText;
        }

        private void onTestGetRowBtnClick()
        {
            if (data == null)
            {
                Debug.LogError("Sheet is null");
                return;
            }
            string rowStr = inputField.text;
            if (String.IsNullOrEmpty(rowStr))
            {
                output.text = "Input: row number";
                return;
            }
            Dictionary<string, IGridData> rowData = data.getRow(int.Parse(rowStr));
            output.text = formatRow(rowData);
        }

        private void onGetRow2BtnClick()
        {
            if (data == null)
            {
                Debug.LogError("Sheet is null");
                return;
            }
            string rowStr = inputField.text;
            if (String.IsNullOrEmpty(rowStr))
            {
                output.text = "Input: title|value (only string)";
                return;
            }
            string[] searchParams = rowStr.Split('|');
            if (searchParams.Length != 2)
            {
                output.text = "Input: title|value (only string)";
                return;
            }
            List<Dictionary<string, IGridData>> rows = data.getRows(searchParams[0], searchParams[1]);
            if (rows == null)
            {
                output.text = "No result";
                return;
            }

            string outputStr = "";
            for (int i = 0; i < rows.Count; i++)
            {
                outputStr += formatRow(rows[i]);
                outputStr += "-----------------------------------\n";
            }

            output.text = outputStr;
        }

        private void onModifyBtnClick()
        {
            string[] searchParams = inputField.text.Split('|');
            if (searchParams.Length != 3)
            {
                output.text = "Input: rowNo|colName|value (only string)";
                return;
            }
            int rowNo = int.Parse(searchParams[0]);
            string columnName = searchParams[1];
            string value = searchParams[2];

            data.modify(columnName, rowNo, new GridData<string>(value));

#if UNITY_EDITOR
            EditorUtility.SetDirty(data);
#endif
        }

        private void onIndexOfBtnClick()
        {
            string[] searchParams = inputField.text.Split('|');
            if (searchParams.Length != 3)
            {
                output.text = "Input: title|value|startIndex (only string)";
                return;
            }

            string title = searchParams[0];
            string value = searchParams[1];
            int startIndex = int.Parse(searchParams[2]);

            output.text = data.indexOf(title, value, startIndex).ToString();
        }
    }
}


