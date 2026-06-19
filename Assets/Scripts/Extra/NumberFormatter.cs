using UnityEngine;

public static class NumberFormatter
{
    public static string FormatNumber(double number)
    {
        if (number < 1000)
            return number.ToString("F0");
        string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };
        int suffixIndex = 0;
        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }
        return number.ToString("F2") + suffixes[suffixIndex];
    }
}
