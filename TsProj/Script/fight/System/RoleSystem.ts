import { Log, UnityEngine } from "csharp";
import { World } from "../World";
import { ECSComponent } from "../../core/ecs/base/ECSComponent";
import { ECSManager } from "../../core/ecs/base/ECSManager";
import { ECSSystem } from "../../core/ecs/base/ECSSystem";
import { RoleCom } from "../Com/RoleCom";
import { VoCom } from "../Com/VoCom";

//请注意，要在ECSManager的register里注册系统
export class RoleSystem extends ECSSystem {
    public get name(): string {
        return RoleSystem.SystemName;
    }
    public static SystemName: string = "RoleSystem";

    public bindTypes: string[] = [RoleCom.ComName, VoCom.ComName];//绑定的组件类型，当有实体添加对应的组件时，会把对应组件绑定到System
    public entitys: Map<number, boolean> = new Map();//储存索引了的实体


    constructor() {
        super();
    }

    public update(dt: number): void {
        //遍历执行组件方法
        for (let key of this.entitys.keys()) {
            let entity = ECSManager.getInstance().getEntity(key);
            let com: RoleCom = entity.getCom(RoleCom) as RoleCom;
        }

    }
    //当正式监视一个实体时 ，即该实体拥有本系统需要的所有组件后，加入到系统逻辑
    public onWatchEntity(id: number) {

    }



    public onStopWatchEntity(id: number) {

    }


}