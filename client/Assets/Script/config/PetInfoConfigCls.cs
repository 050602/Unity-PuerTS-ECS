using System.Collections.Generic;
using System;
public class PetInfoConfigElm : BaseConfigElm
{
      public int id;
      public int type;
      public string name;
      public int color;
      public int[][] attr;
      public int[][] extra_attr;
      public int rwd_time;
      public int[][] time_rwd;
}
public class PetInfoConfigCls : BaseConfigCls
{
    public Dictionary<string, object[]> data { get; set; }
    public Dictionary<string, PetInfoConfigElm> parsedData = new Dictionary<string, PetInfoConfigElm>();
    public string[] fixed_keys { get; set; }
    public void Parse()
    {
        foreach (var item in data)
        {
            var dataArr = item.Value;
            var newObj = new PetInfoConfigElm();
            for (int n = dataArr.Length - 1; n >= 0; n--)
            {
                var obj = dataArr[n];
                if (obj.GetType() == typeof(String) && (string)obj == "")
                {
                    continue;
                }
                var keyName = fixed_keys[n];
                var type = newObj.GetFieldType(keyName);
                if (type == typeof(int[]))
                {
                    newObj.SetFieldValue(keyName, ConvertToIntArray(obj));
                }
                else if (type == typeof(int[][]))
                {
                    newObj.SetFieldValue(keyName, ConvertToIntArray2D(obj));
                }
                else if (type == typeof(Int32))
                {
                    newObj.SetFieldValue(keyName, Convert.ToInt32(obj));
                }
                else
                {
                    newObj.SetFieldValue(keyName, obj);
                }
            }
            parsedData.Add(item.Key, newObj);
        }
    }
    public PetInfoConfigElm Get(string key)
    {
        parsedData.TryGetValue(key, out var result);
        return result;
    }
}