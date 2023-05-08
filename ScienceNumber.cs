using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceNumber
{
    public static string ConvertFromScience(string scienceNum)
    {
        string result = "";
        int sciencePos = scienceNum.IndexOf('e');
        
        int power = int.Parse(scienceNum.Substring(sciencePos + 1));
        int num = int.Parse(scienceNum.Substring(0, sciencePos));
        
        result = (num * Math.Pow(10, power)).ToString();
        return result;
    }

    public static string ConvertToScience(double scienceNum)
    {
        string result = string.Format("{0:E1}", scienceNum);

        result = result.Replace("E", "e");
        result = result.Replace("+", "");

        if (result[4] == '0')
        {
            result = result.Remove(4, 1);
            if (result[4] == '0')
            {
                result = result.Remove(4, 1);
            }
        }
        
        return result;
    }
}
