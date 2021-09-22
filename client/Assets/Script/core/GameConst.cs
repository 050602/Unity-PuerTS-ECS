
using UnityEngine;
public class GameConst
{
    public const float GameUpdateCD = 0.03f;
    public const int CD_SPEED = 1;//默认的CD转速
    public static Vector3 skill_effect_offset = new Vector3(0, 0, 0);//全局技能偏移
    public static Vector3 role_center_offset = new Vector3(0, 0, 0.3f);//角色偏移 用于追踪类型技能
    public const float rotation_offset = 0; // 默认特效旋转角度
    public const float role_scale = 1.4f; // 人物Scale
    public const float role_info_scale = 0.008f; // 人物信息scale
    public const int heroBagNums = 20; //武将背包格子数
    public const RoleDisplayType roleDisplayType = RoleDisplayType.SPINE;
    public const int sameHeroMaxNum = 2;// 同个英雄最多存在数量
    public static Vector3 fingerDragOffset = new Vector3(0, 50, 0);// 手指拖拽偏移
}
public enum EfferObjType
{
    UI = 0,
    V3D,
}
public enum RoleDisplayType
{
    SPINE = 0,
    MODEL,
}