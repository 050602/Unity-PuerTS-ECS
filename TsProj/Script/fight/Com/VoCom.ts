import { UnityEngine } from "csharp";
import { ECSComponent } from "../../core/ecs/base/ECSComponent";


export class VoCom extends ECSComponent {
    public get name(): string {
        return VoCom.ComName;
    }
    public static ComName: string = "VoCom";
    /**
     * 组件的所有者实体 ID
     */
    public entityId = 0;

    public gameObject: UnityEngine.GameObject;//持有的角色对象
    public ani: UnityEngine.Animator;//持有的动画
    // public state:number = 0;// 1表示处于攻击

    public isMove:boolean = false;


    recycleToObjectPool() {
        super.recycleToObjectPool();
        this.gameObject = null;
        this.ani = null;
    }

}