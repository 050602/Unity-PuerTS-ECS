import { UnityEngine } from "csharp";
import { ECSComponent } from "../../core/ecs/base/ECSComponent";


export class RoleCom extends ECSComponent {
    public get name(): string {
        return RoleCom.ComName;
    }
    public static ComName: string = "RoleCom";
    /**
     * 组件的所有者实体 ID
     */
    public entityId = 0;

    public roleId: number = 0;//角色ID
    public modelId: number = 0;//模型ID
    
    recycleToObjectPool() {
        super.recycleToObjectPool();
    }



}