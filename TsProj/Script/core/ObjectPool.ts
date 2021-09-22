
/**
* 对象池
* @author An
*/

export interface ObjectPoolIT {
    recycleToObjectPool: Function
}

export class ObjectPool {
    private static pool: Map<string, any[]> = new Map<string, any[]>();
    private static useCount: number = 0;//new过的对象数量
    private static reCount: number = 0;//回收过的对象数量
    private static reUsrCount: number = 0;//重新被使用的对象数量

    /**
     * 指定类名及类型 获取一个对象
     * @param className 类名
     * @param T 类型
     */
    public static getObjectByClass(className: string, T: any): any {
        let arr = ObjectPool.pool.get(className);
        if (!arr || !arr.length) {
            let t = new T();
            this.useCount++;
            // gzalog(className,"new的对象数量",this.useCount);
            return t;
        }

        let t = arr.shift();
        this.reUsrCount++;
        return t;
    }

    /**
     * 指定类名 获取一个对象 若没有该类名的对象，返回Null
     * @param className 类名
     * @param T 类型
     */
    public static getObject(className: string): any {
        let arr = ObjectPool.pool.get(className);
        if (!arr || !arr.length) {
            return null;
        }

        let t = arr.shift();
        this.reUsrCount++;
        return t;
    }

    public static getPoolLength(className: string): number {
        let arr = ObjectPool.pool.get(className);
        return arr.length;
    }

    /**
     * 把对象回收到对象池，如果被回收的对象有recoverToObjcetPool方法，会自动调用该方法，可以在该方法里进行重置操作
     * @param className 类名
     * @param obj 要回收的对象
     */
    public static recover(className: string, obj: any): void {
        let arr = ObjectPool.pool.get(className);
        if (!arr) {
            arr = [];
        }

        if (obj.recycleToObjectPool) {
            obj.recycleToObjectPool();
        }

        arr.push(obj);
        this.reCount++;
        this.pool.set(className, arr);
    }

    /**
     * 清空指定对象池
     * @param className 
     */
    public static clearPool(className: string): void {
        ObjectPool.pool.delete(className);
        // ObjectPool.checkAllObject();
    }

    public static checkAllObject(): void {
        // gzalog("===================对象池内对象================");
        // ObjectPool.pool.forEach((value: any[], key: string) => {
        //     gzalog(key + ":" + value.length);
        // });

        // gzalog("new过的对象数量" + ObjectPool.useCount);
        // gzalog("回收过的对象数量" + ObjectPool.reCount);
        // gzalog("重新被使用的对象数" + ObjectPool.reUsrCount);
        // gzalog("===================对象池数据end================");
    }

}