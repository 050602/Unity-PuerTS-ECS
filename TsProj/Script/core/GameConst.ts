var frameRate: number = 60;
export const GameUpdateCD: number = 0.03;

//普通属性
export enum Attr {
    MoveSpeed = 1,
    AttackSpeed,
    Attack,
    AttackRange,
    Hp,
    MaxHp,
    FlySpeed = 7,//投射物飞行速度增益
    FlySpeedPercent = 8,//投射物飞行速度增益 万分比
}

//特殊属性
export enum AttrVar {

    ProjectilesMultipleCasting = 10001,//投射物多重施法概率 万分比属性  触发投射物时，会分裂出新的投射物
    ProjectilesAddNumber = 10002,//投射物多重施法增加的投射物个数，默认1个

}