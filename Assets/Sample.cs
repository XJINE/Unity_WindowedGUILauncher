using UnityEngine;
using XGUI;

public class Sample : MonoBehaviour
{
    private WindowedGUILauncher _guiLauncher = new ();

    private IntGUI    _intGUI    = new ("INT GUI",   0, 100);
    private FloatGUI  _floatGUI  = new ("Float GUI", 0, 100);
    private StringGUI _stringGUI = new ("String GUI");

    public int    intValue    = 0;
    public float  floatValue  = 0;
    public string stringValue = "String Value";

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
        _guiLauncher.Add("String Window", () =>
        {
            stringValue = _stringGUI.Show(stringValue);
        });
    }

    private void OnGUI()
    {
        _guiLauncher.Show();
    }
}