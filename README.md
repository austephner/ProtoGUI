# ProtoGUI
#### Summary
Collection of useful classes and `GUILayout` functionality that allows for rapid UI-window prototyping with IMGUI. Includes an extensible window system that mimics OS-like behaviour. It's very easy to setup and implement!

#### Examples
![ExampleWindow](https://i.imgur.com/PmPCRHN.gif)

# Usage
### Creating New `ProtoGUIWindow` Implementations
1. Create a new script.
2. Import the `ProtoGUI` namespace.
3. Inherit from `ProtoGUIWindow` instead of `MonoBehaviour`
4. Override `DrawContent` to begin using `GUILayout` code.
5. Add the new script to a game object. 
6. Configure the window as needed from within the inspector.

```c#
using UnityEngine;

namespace ProtoGUI.Examples
{
    public class ExampleProtoGUIWindow : ProtoGUIWindow
    {
        protected override void DrawContent()
        {
            ProtoGUILayout.DrawFullyFlexibleContent("Hello World!");
        }
    }
}
```

**Note** that not all settings work during runtime when changed. This will be improved as time goes on. Most changes however would require `Start()` to be called again or the editor to be stopped/started.

### Using the Window Manager
The window manager allows for certain windows to appear minimized and otherwise accessible from a small menu. See the image at the top of this page for an example.
1. Add the `ProtoGUIWindowManager` component to a game object. 
2. Configure as needed. 
3. Ensure any `ProtoGUIWindow` implementations have set "Show in Window Manager" to `true` if you'd like for them to appear in the window manager.
4. Start the game. 

**Note** that the window manager's own window may not appear at all if there aren't any windows in the scene configured to appear as an option inside of it.

### Utilizing `ProtoGUILayout` Methods
The included `GUILayout` based class provides methods for some advanced field types. This class will be added to overtime. All functions are shown in the GIF at the top of this Readme, but are also listed here:
- `DrawVerticalMenuOptions()`
  - Uses an array of `ProtoMenuButton` objects to display clickable options
  - All options invoke an action and can be customized
- `DrawFullyFlexibleContent()`
- `DrawHorizontallyFlexibleContent()`
- `DrawVerticallyFlexibleContent()`
- `DrawHorizontalSliderField()`
- `DrawFancyHorizontalSliderField()`
  - Can be used to draw heavily labeled horizontal fields.
  - Labels can be customized to be shown or hidden, such as a "min" and "max" label.
- `DrawHorizontalEnumSelector<T>()`
  - Uses a left and right button to cycle through the given enum values.
- `DrawHorizontalEnumSelectionGrid()`
  - Uses `GUILayout.SelectionGrid()`
  - Automatically draws all the given enum values as selectable options.
- More coming soon!

# Example Window
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