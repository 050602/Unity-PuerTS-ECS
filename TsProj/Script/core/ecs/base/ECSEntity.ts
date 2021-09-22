import { Log, UnityEngine } from "csharp";
import { ObjectPool, ObjectPoolIT } from "../../ObjectPool";
import { ECSComponent } from "./ECSComponent";
import { ECSManager } from "./ECSManager";

export class ECSEntity implements ObjectPoolIT {
    //gameObject可以在Com持有

    recycleToObjectPool() {
        this.id = 0;

        for (let com of this.components.values()) {
            ECSManager.getInstance().removeCom(com);
            ObjectPool.recover(com.name, com);
        }

        this.components.clear();

    }

    public id: number;
    //ECS一般不会把Entity作为容器，但是在此处存储Component 减少manager储存的复杂度
    public readonly components: Map<string, ECSComponent> = new Map();

    public active:boolean = true;//是否处于激活状态

    public getCom(type: any) {
        return this.components.get(type.ComName);
    }
    public getComByName(name: string) {
        return this.components.get(name);
    }

    /**
     * @param type 类型
     */
    public addCom(type: any): any {
        if (this.components.has(type.ComName)) {
            Log.logError("该实体已拥有此组件:", type.ComName);
            return;
        }

        let com: ECSComponent = ObjectPool.getObjectByClass(type.ComName, type);
        com.entityId = this.id;
        this.components.set(com.name, com);
        ECSManager.getInstance().addCom(com);
        return com;
    }

    public removeCom(type: any) {
        let com = this.components.get(type.ComName);
        if (com) {
            ECSManager.getInstance().removeCom(com);
            ObjectPool.recover(type.ComName, com);
            this.components.delete(com.name);
        }
    }


    public hasCom(comName: string) {
        return this.components.has(comName);
    }


}
