import { FairyGUI } from "csharp";
import { MainUIView } from "./MainUIView";

export default class MainUIBinder
{
    public static BindAll():void
    {
        FairyGUI.UIObjectFactory.SetPackageItemExtension(MainUIView.URL, () => { return new MainUIView(); });
    }
}