import { Log } from "csharp";

let isDebug = true;
if (isDebug)
    Log.setDeBug();
export let gzaLog = (...data: any[]) => {
    if (!isDebug) {
        return;
    }
    Log.log(...data);
}

export let errLog = (...data: any[]) => {
    if (!isDebug) {
        return;
    }
    Log.logError(...data);
}