/**
 * 组件
 */
export abstract class ECSComponent {
  /**
   * 组件的类名 在每个Com里面应该有个静态属性，为该组件的类名， get name要返回该静态属性
   */
  public abstract get name(): string;
  public static ComName: string = "ECSComponent";

  /**
   * 组件的所有者实体 ID
   */
  public abstract entityId: number;

  recycleToObjectPool() {
    this.entityId = 0;
  }
}
