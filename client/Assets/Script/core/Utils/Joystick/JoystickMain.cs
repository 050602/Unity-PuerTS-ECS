using UnityEngine;
using FairyGUI;

public class JoystickMain : MonoBehaviour
{
    GComponent _mainView;
    GTextField _text;
    JoystickModule _joystick;

    void Start()
    {
        Application.targetFrameRate = 60;
        Stage.inst.onKeyDown.Add(OnKeyDown);

        FairyGUI.UIPackage.AddPackage("Joy");
        _mainView = UIPackage.CreateObject("Joy","Joy") as GComponent;
        GRoot.inst.AddChildAt(_mainView,0);


        _joystick = new JoystickModule(_mainView);
        _joystick.onMove.Add(__joystickMove);
        _joystick.onEnd.Add(__joystickEnd);
    }

    void __joystickMove(EventContext context)
    {
        float degree = (float)context.data;
        // _text.text = "" + degree;
        TSMain.Instance.joyCallBack(degree);
    }

    void __joystickEnd()
    {
        // _text.text = "";
        TSMain.Instance.joyEndCallBack();
    }
    void OnKeyDown(EventContext context)
    {
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Application.Quit();
        }
    }
}