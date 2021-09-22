import { EventCenter, Log, Timer, TSMain, UnityEngine } from "csharp";
import { ECSManager } from "../core/ecs/base/ECSManager";
import { RoleCom } from "./Com/RoleCom";
import { Sigleton } from "../core/Sigleton";
import { VoCom } from "./Com/VoCom";

export class World extends Sigleton {
  static getInstance(): World {
    return super.getInstance(World);
  }

  public ani: UnityEngine.Animator;
  public roleEId: number = 0;
  public role: UnityEngine.GameObject;

  public nowInCube: number = 0;

  public async init() {
    //创建实体
    let entity = ECSManager.getInstance().createEntity();
    this.roleEId = entity.id;

    //为实体添加组件
    let com: RoleCom = entity.addCom(RoleCom);
    let vocom = entity.addCom(VoCom) as VoCom;

    //修改组件内容
    vocom.ani = this.ani;
    vocom.gameObject = this.role;

    //获取组件并修改组件内容
    let en = ECSManager.getInstance().getEntity(this.roleEId);
    let vo = en.getCom(VoCom) as VoCom;
    vo.isMove = false;
  }
}
