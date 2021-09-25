# ProtoGUI
#### Summary
Collection of useful classes and functionality that allows for rapid UI-window prototyping with IMGUI. Includes an extensible window system that mimics OS-like behaviour. It's very easy to setup and implement!

# Usage
### Inheriting From `ProtoGUIWindow`
1. Create a new script
2. Import the `ProtoGUI` namespace.
3. Inherit from `ProtoGUIWindow` instead of `MonoBehaviour`
4. Override `DrawContent` to begin using `GUILayout` code. 

```c#
using UnityEngine;

namespace ProtoGUI.Examples
{
    public class ExampleProtoGUIWindow : ProtoGUIWindow
    {
        protected override void DrawContent()
        {
            GUILayout.FlexibleSpace();

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Hello World");
                GUILayout.FlexibleSpace();
            }

            GUILayout.FlexibleSpace();
        }
    }
}
```

4. In the inspector, attach the script to a game object. 
5. Configure the window as you wish!

![ExampleWindow](https://i.imgur.com/eGxf46Z.gif)

**Note** that not all settings work during runtime when changed. This will be improved as time goes on. Most changes however would require `Start()` to be called again or the editor to be stopped/started.

### Using the Window Manager
The window manager allows for certain windows to appear minimized and otherwise accessible from a small menu. 
1. Add the `ProtoGUIWindowManager` to a game object. 
2. Configure as needed. 
3. Ensure any `ProtoGUIWindow` implementations have set "Show in Window Manager" to `true` if you'd like for them to appear in the window manager.

![Window Manager](https://i.imgur.com/24HYKm9.gif)

### Drawing the Toolbar
1. Enable the "Show Toolbar" setting for the selected window. 
2. Override `DrawToolbar` in the script. 
3. Add your custom `GUILayout` code. 

![ExampleWindow2](https://i.imgur.com/w4MhklM.png)

1. Enabling "Show Toolbar" will draw the entire toolbar. This setting has to be on in order for the "Close" and "Minimize" buttons to show, regardless of whether or not those fields are enabled. 
2. The "Minimize" and "Close" buttons. These can be enabled/disabled through the inspector. 
3. The custom `GUILayout` content. It's spaced by with a call to `GUILayout.FlexibleSpace()` by default, so all code will be left-aligned.

```c#
// Snippet from the above example image...

protected override void DrawToolbar()
{
    if (GUILayout.Button("Refresh"))
    {
        RefreshContent();
    }

    if (GUILayout.Button("Go Up"))
    {
        GoUpDirectory();
    }
    
    base.DrawToolbar();
}
```

# Example
This API comes with a simple folder-browser implementation (see the above section for a screenshot). 

```c#
using System.IO;
using System.Linq;
using UnityEngine;

namespace ProtoGUI.Examples
{
    public class ProtoFolderBrowserGUIWindow : ProtoGUIWindow
    {
        private Vector2 _scroll;

        private string _dir;

        private string[] _content;

        protected override void Start()
        {
            // ALWAYS CALL base.Start()
            
            base.Start();

            _dir = Application.dataPath.Replace("/", "\\"); 
            RefreshContent();
        }

        protected override void DrawToolbar()
        {
            if (GUILayout.Button("Refresh"))
            {
                RefreshContent();
            }

            if (GUILayout.Button("Go Up"))
            {
                GoUpDirectory();
            }
            
            base.DrawToolbar();
        }

        protected override void DrawContent()
        {
            var dir = GUILayout.TextField(_dir).Replace("\\", "/").Replace("//", "/");

            if (dir != _dir)
            {
                _dir = dir;
                RefreshContent();
            }
            
            if (!Directory.Exists(_dir))
            {
                GUILayout.Label("Directory doesn't exist.");
                _content = new string[0];
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                _scroll = GUILayout.BeginScrollView(_scroll, GUIStyle.none, GUI.skin.verticalScrollbar);
                
                foreach (var content in _content)
                {
                    var lastInPath = content.Replace("\\", "/").Split('/').Last();
                    
                    if (GUILayout.Button(lastInPath))
                    {
                        _dir = Path.Combine(_dir, lastInPath).Replace("\\", "/");
                        RefreshContent();
                    }
                }
                
                GUILayout.EndScrollView();
                
                GUILayout.FlexibleSpace();
            }
        }

        private void RefreshContent()
        {
            try
            {
                _content = Directory.GetDirectories(_dir);
            }
            catch
            {
                Debug.LogError($"Invalid directory: {_dir}");
                _content = new string[0];
            }
        }

        private void GoUpDirectory()
        {
            var modifiedDir = _dir.Replace("\\", "/").Replace("//", "/");

            if (modifiedDir.EndsWith("/"))
            {
                modifiedDir = modifiedDir.Remove(modifiedDir.Length - 1, 1);
            }
            
            var splitDir = modifiedDir.Split('/');

            _dir = string.Join("/", splitDir, 0, splitDir.Length - 1);

            if (!_dir.EndsWith("/"))
            {
                _dir += "/";
            }
            
            RefreshContent();
        }
    }
}
```