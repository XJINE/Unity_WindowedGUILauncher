using UnityEngine;
using XGUI;

public class Sample : MonoBehaviour
{
    private WindowedGUILauncher _guiLauncher = new ();

    private FlexWindow _window      = new ("Window");
    private IntGUI     _intGUI      = new ("INT GUI",   0, 100);
    private FloatGUI   _floatGUI    = new ("Float GUI", 0, 100);
    private StringGUI  _stringGUI_A = new ("String GUI A");
    private StringGUI  _stringGUI_B = new ("String GUI B");
    private StringGUI  _stringGUI_C = new ("String GUI C");

    public int    intValue     = 0;
    public float  floatValue   = 0;
    public string stringValueA = "String Value A";
    public string stringValueB = "String Value B";
    public string stringValueC = "String Value C";

    private void Awake()
    {
        _guiLauncher.Add("Int Window", () =>
        {
            intValue = _intGUI.Show(intValue);
        });
        _guiLauncher.Add("Float Window", () =>
        {
            floatValue = _floatGUI.Show(floatValue);
        });
        _guiLauncher.Add("Parent/String Window A", () =>
        {
            stringValueA = _stringGUI_A.Show(stringValueA);
        });
        _guiLauncher.Add("Parent/Child/String Window B", () =>
        {
            stringValueB = _stringGUI_B.Show(stringValueB);
        });
        _guiLauncher.Add("Parent/Child/String Window C", () =>
        {
            stringValueC = _stringGUI_C.Show(stringValueC);
        });
    }

    private void OnGUI()
    {
        _window.Show(() =>
        {
            _guiLauncher.ShowMenu();
        });

        _guiLauncher.ShowWindows();
    }
}