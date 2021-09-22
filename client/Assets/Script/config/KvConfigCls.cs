using System;
public class KvConfigCls : BaseConfigCls
{
      public object game_name { get; set; }
      public object m_game_name;
      public object camera_scale { get; set; }
      public object m_camera_scale;
      public object hero_max { get; set; }
      public object m_hero_max;
      public object battle_hero_num { get; set; }
      public object m_battle_hero_num;
      public object battle_money_get { get; set; }
      public object m_battle_money_get;
    public void Parse()
    {
        var props = GetType().GetProperties();
        for (int n = props.Length - 1; n >= 0; n--)
        {
            var prop = props[n];
            if (!prop.Name.Contains("m_"))
            {
                var mKeyName = "m_" + prop.Name;
                var obj = prop.GetValue(this);
                var type = GetFieldType(mKeyName);
                if (type == typeof(int[]))
                {
                    SetFieldValue(mKeyName, ConvertToIntArray(obj));
                }
                else if (type == typeof(int[][]))
                {
                    SetFieldValue(mKeyName, ConvertToIntArray2D(obj));
                }
                else if (type == typeof(Int32))
                {
                    SetFieldValue(mKeyName, Convert.ToInt32(obj));
                }
                else
                {
                    SetFieldValue(mKeyName, obj);
                }
            }
        }
    }
}