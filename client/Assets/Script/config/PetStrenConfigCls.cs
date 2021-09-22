using System.Collections.Generic;
using System;
public class PetStrenConfigElm : BaseConfigElm
{
      public int id;
      public int lv;
      public int[][] attr;
      public int[][] cost;
}
public class PetStrenConfigCls : BaseConfigCls
{
    public Dictionary<string, object[]> data { get; set; }
    public Dictionary<string, PetStrenConfigElm> parsedData = new Dictionary<string, PetStrenConfigElm>();
    public string[] fixed_keys { get; set; }
    public void Parse()
    {
        foreach (var item in data)
        {
            var dataArr = item.Value;
            var newObj = new PetStrenConfigElm();
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
    public PetStrenConfigElm Get(string key)
    {
        parsedData.TryGetValue(key, out var result);
        return result;
    }
}