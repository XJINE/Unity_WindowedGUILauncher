using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XGUI
{
    public class WindowedGUILauncher
    {
        public class WindowedGUIAction
        {
            public BoolGUI             Menu        { get; }
            public FlexWindow          Window      { get; }
            public WindowedGUILauncher SubLauncher { get; }
            public Action              GUIAction   { get; }

            public WindowedGUIAction(string itemName)
            {
                Menu        = new BoolGUI   (itemName);
                Window      = new FlexWindow(itemName){ IsVisible = false };
                SubLauncher = new WindowedGUILauncher();
                GUIAction   = () =>
                {
                    SubLauncher.ShowMenu();
                };
            }

            public WindowedGUIAction(string itemName, Action guiAction)
            {
                Menu      = new BoolGUI   (itemName);
                Window    = new FlexWindow(itemName) { IsVisible = false };
                GUIAction = guiAction;
            }

            public void ShowMenu()
            {
                Window.IsVisible = Menu.Show(Window.IsVisible);

                if (Menu.Updated && Window.IsVisible)
                {
                    var mousePos   = Input.mousePosition;
                        mousePos.y = Screen.height - mousePos.y; // Flip-Y
                        mousePos  += Vector3.one * 10;           // Small offset

                    Window.Position = mousePos;
                }
            }

            public void ShowWindow()
            {
                Window.Show(GUIAction);
            }
        }

        public List<WindowedGUIAction> WindowedGUIActions { get; } = new();

        #region Method

        public void Add(string itemName, Action guiAction)
        {
            var splitNames = SplitItemName(itemName);

            if (splitNames.Length == 1)
            {
                WindowedGUIActions.Add(new WindowedGUIAction(itemName, guiAction));
            }
            else
            {
                var subLauncherTitle = splitNames[0];

                var subLauncherAction = WindowedGUIActions.FirstOrDefault
                    (windowedGUIAction => windowedGUIAction.Window.Title == subLauncherTitle);

                if (subLauncherAction == null)
                {
                    subLauncherAction = new WindowedGUIAction(subLauncherTitle);
                    WindowedGUIActions.Add(subLauncherAction);
                }

                subLauncherAction.SubLauncher.Add(splitNames[1], guiAction);
            }
        }

        public void Insert(int index, string itemName, Action guiAction)
        {
            var splitNames = SplitItemName(itemName);

            if (splitNames.Length == 1)
            {
                WindowedGUIActions.Insert(index, new WindowedGUIAction(itemName, guiAction));
            }
            else
            {
                var subLauncherTitle = splitNames[0];

                var subLauncherAction = WindowedGUIActions.FirstOrDefault
                                        (windowedGUIAction => windowedGUIAction.Window.Title == subLauncherTitle);

                if (subLauncherAction == null)
                {
                    subLauncherAction = new WindowedGUIAction(subLauncherTitle);
                    WindowedGUIActions.Add(subLauncherAction);
                }

                subLauncherAction.SubLauncher.Insert(index, splitNames[1], guiAction);
            }
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
                windowedGUI.ShowMenu();
            }
        }

        public void ShowWindows()
        {
            foreach (var windowedGUI in WindowedGUIActions)
            {
                windowedGUI.ShowWindow();
                windowedGUI.SubLauncher?.ShowWindows();
            }
        }

        private static string[] SplitItemName(string itemName)
        {
            var slashIndex = itemName.IndexOf('/');

            if (slashIndex != -1)
            {
                var beforeSlash = itemName[..slashIndex];
                var afterSlash  = itemName[(slashIndex + 1)..];

                return new []{ beforeSlash, afterSlash };
            }
            else
            {
                return new []{ itemName };
            }
        }

        #endregion Method
    }
}