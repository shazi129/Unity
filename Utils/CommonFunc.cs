using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunc
{
    /// <summary>
    /// 获取单个数字的汉字表示
    /// </summary>
    /// <param name="num">数字</param>
    /// <param name="isOrdinal">是否是序数，例如2序数表示"二"，非序数表示"两"</param>
    /// <returns></returns>
    public static string getSingleCharacterNum(int num, bool isOrdinal = false)
    {
        if (num == 0)
        {
            return "零";
        }
        else if (num == 1)
        {
            return "一";
        }
        else if (num == 2)
        {
            if (isOrdinal == false)
                return "两";
            else
                return "二";
        }
        else if (num == 3)
        {
            return "三";
        }
        else if (num == 4)
        {
            return "四";
        }
        else if (num == 5)
        {
            return "五";
        }
        else if (num == 6)
        {
            return "六";
        }
        else if (num == 7)
        {
            return "七";
        }
        else if (num == 8)
        {
            return "八";
        }
        else if (num == 9)
        {
            return "九";
        }
        else
        {
            return "十";
        }
    }

    public static string getCharaterUnitName(int count)
    {
        List<string> units = new List<string>() { "", "十", "百", "千", "万" };
        if (count >= units.Count)
        {
            count = units.Count - 1;
        }
        if (count < 0)
        {
            count = 0;
        }
        return units[count];
    }

    public static string getCharacterNum(int num)
    {
        List<int> numbers = new List<int>();
        while (num > 0)
        {
            numbers.Add(num % 10);
            num = num / 10;
        }

        string str = "";

        for (int i = 0; i < numbers.Count; i++)
        {
            //末尾的0不输出
            if (numbers[i] == 0 && str == "")
            {
                continue;
            }
            //11的时候只展示十一
            else if (i == 1 && numbers.Count == 2 && numbers[i] == 1)
            {
                str = getCharaterUnitName(i) + str;
            }
            //字段中间的0，例如101， 2002
            else if (numbers[i] == 0 && str != "")
            {
                if (numbers[i-1] != 0)
                {
                    str = getSingleCharacterNum(numbers[i], true) + str;
                }
            }
            else
            {
                str = getSingleCharacterNum(numbers[i], true) + getCharaterUnitName(i) + str;
            }
        }
        return str;
    }

    public static ulong getCurTime()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (ulong)(ts.TotalSeconds * 1000);
    }
}
