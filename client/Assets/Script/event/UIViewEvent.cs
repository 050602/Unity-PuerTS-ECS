public class UIViewEvent
{
    public const string EVENT_ITEM_CHOOSE_UPDATE = "EVENT_ITEM_CHOOSE_UPDATE";//玩家信息详情展示通知
    public const string EVENT_CLOSE_CHALLENGESHOP_UPDATE = "EVENT_CLOSE_CHALLENGESHOP_UPDATE";//挑战商店关闭

    public const string VIEW_OPEN = "VIEW_OPEN";//当界面打开时 参数，UIID
    public const string VIEW_CLOSE = "VIEW_CLOSE";//当界面关闭时 参数UIID

    public const string VIEW_PRELOAD_OVER = "VIEW_PRELOAD_OVER";//当界面预加载完毕时 参数UIID

    public const string GAME_START_PROGRESS = "GAME_START_PROGRESS";//加载进度条进度更新

    public const string EVENT_UPDATE_BAG_LIST = "EVENT_UPDATE_BAG_LIST";//更新背包内容
}