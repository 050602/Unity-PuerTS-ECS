using System;
public class BaseConfigCls
{
    public Type GetFieldType(string name)
    {
        var field = GetType().GetField(name);
        return field.FieldType;
    }
    public void SetFieldValue(string name, object value)
    {
        var field = GetType().GetField(name);

        field.SetValue(this, value);
    }
    public int[] ConvertToIntArray(object obj)
    {
        var jArray = (Newtonsoft.Json.Linq.JArray)obj;
        int[] result = new int[jArray.Count];
        for (int c = 0; c < jArray.Count; c++)
        {
            result[c] = (int)jArray[c];
        }
        return result;
    }
    public int[][] ConvertToIntArray2D(object obj)
    {
        var jArray = (Newtonsoft.Json.Linq.JArray)obj;
        int[][] result = new int[jArray.Count][];
        for (int c = 0; c < jArray.Count; c++)
        {
            var innerJArray = (Newtonsoft.Json.Linq.JArray)jArray[c];
            int[] innerArr2 = new int[innerJArray.Count];
            for (int m = 0; m < innerJArray.Count; m++)
            {
                var val = innerJArray[m];
                innerArr2[m] = (int)innerJArray[m];
            }
            result[c] = innerArr2;
        }
        return result;
    }
}