
class BaseModel<T> : Singleton<T> where T : Singleton<T>, new()
{
    // public class ClassObjectPool<T> : IPoolClaer where T : class, new()


}