using System;
public class BaseConfigElm
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
}