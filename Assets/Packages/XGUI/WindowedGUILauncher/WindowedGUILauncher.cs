using System;
using System.Collections.Generic;
using System.Linq;

namespace XGUI
{
    public class WindowedGUILauncher
    {
        public class WindowedGUIAction
        {
            public BoolGUI    Menu      { get; private set; }
            public FlexWindow Window    { get; private set; }
            public Action     GUIAction { get; private set; }

            public WindowedGUIAction(string itemName, Action guiAction)
            {
                Menu      = new BoolGUI   (itemName);
                Window    = new FlexWindow(itemName) { IsVisible = false };
                GUIAction = guiAction;
            }
        }

        public List<WindowedGUIAction> WindowedGUIActions { get; private set; } = new();

        #region Method

        public void Add(string itemName, Action guiAction)
        {
            WindowedGUIActions.Add(new WindowedGUIAction(itemName, guiAction));
        }

        public void Insert(int index, string itemName, Action guiAction)
        {
            WindowedGUIActions.Insert(index, new WindowedGUIAction(itemName, guiAction));
        }

        public bool Remove(string itemName)
        {
            return WindowedGUIActions.Remove(WindowedGUIActions.First(action => action.Menu.Title == itemName));
        }

        // CAUTION:
        // If GUI(Layout).Window is called inside another Window, the following error occurs.
        // > GUI Error: You called GUI.Window inside a another window's function.
        // > Ensure to call it in a OnGUI code path.
        // So separate following functions to call the Window function outside another one.

        public void ShowMenu()
        {
            foreach (var windowedGUI in WindowedGUIActions)
            {
                windowedGUI.Window.IsVisible = windowedGUI.Menu.Show(windowedGUI.Window.IsVisible);
            }
        }

        public void ShowWindows()
        {
            foreach (var windowedGUI in WindowedGUIActions)
            {
                windowedGUI.Window.Show(windowedGUI.GUIAction);
            }
        }

        #endregion Method
    }
}