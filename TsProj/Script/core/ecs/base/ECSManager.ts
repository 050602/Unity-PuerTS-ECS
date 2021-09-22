import { Log } from "csharp";
import { ObjectPool } from "../../ObjectPool";
import { Sigleton } from "../../Sigleton";
import { TimerTS } from "../../TimerTS";
import { ECSComponent } from "./ECSComponent";
import { ECSEntity } from "./ECSEntity";
import { ECSSystem } from "./ECSSystem";
import { gzaLog } from "../../../utils/LogTS";


export class ECSManager extends Sigleton {
    static getInstance(): ECSManager {
        return super.getInstance(ECSManager);
    }

    public uid: number = 1;
    public systems: Map<string, ECSSystem>;
    public entitys: Map<number, ECSEntity>;

    private runningSystems: Map<string, ECSSystem>;

    //所有System都需要在此注册
    register() {
        // this.systems.set(RoleSystem.SystemName, new RoleSystem);
    }

    initInstance() {
        this.entitys = new Map();
        this.systems = new Map();
        this.runningSystems = new Map();
        this.uid = 1;
        TimerTS.getInstance().loop(this, this.update);

        this.register();

    }

    public update(dt: number) {
        this.runningSystems.forEach(value => {
            value.update(dt);
        });
    }

    public getEntity(uid: number): ECSEntity {
        return this.entitys.get(uid);
    }

    public getSystem(type: any): ECSSystem {
        return this.systems.get(type.SystemName);
    }

    public getUid(): number {
        return this.uid++;
    }


    public getSystemByName(name: string): ECSSystem {
        return this.systems.get(name);
    }

    public createEntity(): ECSEntity {
        let ent: ECSEntity = ObjectPool.getObjectByClass("ECSEntity", ECSEntity);
        ent.id = this.uid++;

        // Log.gzaLog("createEntity", ent.id);
        this.entitys.set(ent.id, ent);
        return ent;
    }

    public recoverEntity(ent: ECSEntity) {
        let id = ent.id;
        ObjectPool.recover("ECSEntity", ent);
        this.entitys.delete(id);
    }

    //添加组件时，要为监听该组件的system添加索引 能来到这里，说明这个实体还没绑定过这个组件
    public addCom(com: ECSComponent) {
        let entity = this.entitys.get(com.entityId);

        if (entity) {
            for (let value of this.systems.values()) {
                //只有当实体拥有该system所需要的全部组件时，才激活对应系统
                let canActvie = true;

                for (let i = 0; i < value.bindTypes.length; i++) {
                    // gzaLog(value.name,com.entityId,value.bindTypes[i], entity.hasCom(value.name));
                    if (!entity.hasCom(value.bindTypes[i])) {
                        canActvie = false;
                        continue;
                    }
                }

                if (canActvie) {
                    if (!value.entitys.has(com.entityId)) {
                        value.entitys.set(com.entityId, true);
                        value.onWatchEntity(com.entityId);
                        if (!this.runningSystems.has(value.name)) {
                            this.runningSystems.set(value.name, value);
                        }
                    }
                }
            }
        }

    }


    //删除组件时，要把组件从对应system的索引移除
    public removeCom(com: ECSComponent) {
        let entity = this.entitys.get(com.entityId);
        if (entity) {
            for (let value of this.systems.values()) {
                if (!value.entitys.has(com.entityId)) {
                    continue;
                }

                //缺少任何一个，都需要把它从系统干掉
                if (value.bindTypes.indexOf(com.name) != -1) {
                    // gzaLog("从系统中移除", com.name, com.entityId, value.name);
                    value.entitys.delete(com.entityId);
                    if (value.entitys.size <= 0) {
                        this.runningSystems.delete(value.name);
                    }
                }

            }
        }
    }


}
