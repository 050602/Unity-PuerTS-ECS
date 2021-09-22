import { ECSComponent } from "./ECSComponent";

//系统 会监听 组件，当某个实体拥有指定组件时，系统才会执行逻辑  ，系统没有监听到任何组件时，系统不进入update
//同一类型的系统只会同时存在一个
export abstract class ECSSystem {
    public static SystemName: string = "ECSSystem";
    /**
   * 系统的类名 在每个Com里面应该有个静态属性，为该组件的类名， get name要返回该静态属性
   */
    public abstract get name(): string;
    public abstract update(dt: number): void;
    
    //当正式监视一个实体时 ，即该实体拥有本系统需要的所有组件后，加入到系统逻辑
    public abstract onWatchEntity(id:number): void;
    //当停止监听一个实体时
    public abstract onStopWatchEntity(id:number): void;
    public abstract bindTypes: string[];//绑定的组件类型，当有实体添加对应的组件时，会把对应组件绑定到System
    // public abstract components: Map<string, ECSComponent>;
    public abstract entitys: Map<number, boolean>;//储存索引了的实体
}
