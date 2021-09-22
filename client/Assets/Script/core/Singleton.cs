public class Singleton<T> where T : Singleton<T>, new()
{
    // private static readonly T instance = new T();

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.initInstance();
            }
            return _instance;
        }
    }

    public virtual void initInstance()
    {
    }
    public virtual void destroyInstance()
    {
    }
}