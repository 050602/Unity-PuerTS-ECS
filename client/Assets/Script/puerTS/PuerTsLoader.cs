public class PuerTsLoader : Puerts.ILoader
{
    public bool FileExists(string filepath)
    {
        var fullpath = TSMgr.TSPath + filepath + TSMgr.TSEnd;
        return TSMgr.Instance.checkScript(fullpath);
    }

    public string ReadFile(string filepath, out string debugpath)
    {
        var fullpath = TSMgr.TSPath + filepath + TSMgr.TSEnd;
        debugpath = fullpath;//此处用于debug时对文件解析
        return TSMgr.Instance.getScript(fullpath);
    }
}