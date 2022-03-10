# ProtoGUI [v1.1.0]
#### Summary
Collection of useful classes and `GUILayout` functionality that allows for rapid UI-window prototyping with IMGUI. Includes an extensible window system that mimics OS-like behaviour. It's very easy to setup and implement!

#### Features
- Simply inherit from `ProtoGUIWindow` instead of `MonoBehaviour` then override `DrawContent()`
- Useful `GUILayout` styled functions that can be used anywhere
- Window system allows for minimizing, closing, re-opening individual windows like an operating system
- Windows are highly configurable

#### Preview of Latest Features
![Example](https://i.imgur.com/0L6oYts.png)

#### Window Functionality Example
![ExampleWindow](https://i.imgur.com/PmPCRHN.gif)

# Usage
## Creating New `ProtoGUIWindow` Implementations
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

## Using the Window Manager
The window manager allows for certain windows to appear minimized and otherwise accessible from a small menu. See the image at the top of this page for an example.
1. Add the `ProtoGUIWindowManager` component to a game object. 
2. Configure as needed. 
3. Ensure any `ProtoGUIWindow` implementations have set "Show in Window Manager" to `true` if you'd like for them to appear in the window manager.
4. Start the game. 

**Note** that the window manager's own window may not appear at all if there aren't any windows in the scene configured to appear as an option inside of it.

## Using `ProtoGUILayout`
The ProtoGUI layout class contains functionality similar to `GUILayout` but with more short hand practices and easy to use functions for rapid IMGUI development. Note that you **don't need to use `ProtoGUIWindow` to access the `ProtoGUILayout` functions - they'll work anywhere that `GUILayout` works!**

### General Containers
#### `DrawFullyFlexibleContent`
- Takes an `Action` which is wrapped in both a horizontal and vertical section, surrounded by calls to `GUILayout.FlexibleSpace()`
- Useful for centering content within a container or window.

```c#
ProtoGUILayout.DrawFullyFlexibleContent(() => GUILayout.Label("I'm a centered label!"));

ProtoGUILayout.DrawFullyFlexibleContent(() => 
  {
    if (GUILayout.Button("I'm a centered button!")) 
    {
        Debug.Log("Clicked, good job!");
    }
  });
```
- Variations:
  - `DrawHorizontallyFlexibleContent`
  - `DrawVerticallyFlexibleContent`

#### `DrawVerticalScrollArea`
- Shorthand method for drawing a vertical scroll area. 
- Prevents horizontal scroll bar from appearing.

### Special Containers
#### `DrawIconContentBox`
- Draws a box with the specified background color, icon, and content
- Useful for tips, notes, errors, warnings, etc. 
- Variations:
  - `DrawInfoBox`
  - `DrawWarningBox`
  - `DrawErrorBox`

```c#
ProtoGUILayout.DrawInfoBox(() =>
{
    GUILayout.Label("This is an important information update! You can use these in your prototyping GUI to call out important facts or pieces of information that are necessary to the user/player. Put whatever type of content you like inside!");
    
    if (GUILayout.Button("Even Buttons!")) { }
});

ProtoGUILayout.DrawWarningBox(() =>
{
    GUILayout.Label("Warning! You haven't starred this repository yet! Better hop on that!");
});

ProtoGUILayout.DrawErrorBox(() =>
{
    GUILayout.Label("There was (no) problem with drawing this box... but you could easily put an error here to signify there might've been something wrong with the project!");
});
```

![Special Containers Example](https://i.imgur.com/YvVhaRE.png)

_Icons in above screenshot are included in the packages resource folder for free._

### Editable Fields
This section is rather small but will be expanded upon in the future. There is a ton of code I haven't committed/pushed yet because its usage is being tested in other projects.

#### `DrawHorizontalSliderField`
- Draws a slider field with a label width of `150`
- Allows for a `min` and `max` to clamp the value between

```c#
var myFloat = 0.5f; 

myFloat = ProtoGUILayout.DrawHorizontalSliderField("Normal Slider", myFloat, -1.0f, 1.0f);;
```

![Normal Slider Example](https://i.imgur.com/73mKgLv.png)

#### `DrawFancySliderField`
- Draws a slider field with a label and several other labels to better visualize the current slider's value and limitations
- Allows for a `min` and `max`
- Optional parameter `showValueText` can enable/disable a label that shows the slider's value real time
- Optional parameter `valueFormatting` defaults to `"0.000"` which will format the value in the label
- Optional parameter `showMinText` will show the value of `min` to the left of the slider
- Optional parameter `showMaxText` will show the value of `max` to the right of the slider

```c#
var myFloat = 0.5f; 

myFloat = ProtoGUILayout.DrawFancyHorizontalSlider("Fancy Slider", myFloat, -1.0f, 1.0f);
```

![Fancy Slider Example](https://i.imgur.com/NwnrpJu.png)

#### `DrawHorizontalEnumSelector<T>`
- Draws one of two types of selection systems for a series of enum values:
  - Next/Prev button selector
  - Selection Grid

```c#
public enum ExampleEnum
{
    EnumValueA,
    EnumValueB,
    EnumValueC,
    EnumValueD,
}

var myEnumValue = ExampleEnum.EnumValueA;

myEnumValue = ProtoGUILayout.DrawHorizontalEnumSelector("Enum Selector", myEnumValue);

myEnumValue = ProtoGUILayout.DrawHorizontalEnumSelectionGrid("Enum Grid", myEnumValue);
```

![Enum Example](https://i.imgur.com/A2LAV7g.png)

### Drawing Menus
#### `DrawVerticalMenuOptions`
- Takes an array of `ProtoMenuOption` objects which have metadata to describe that menu option's appearance and behaviour
- Overloaded implementation can include a style to apply to the main layout group

```c#
ProtoGUILayout.DrawVerticalMenuOptions(
  new ProtoMenuButton()
  {
      text = "Button A"
  },
  new ProtoMenuButton()
  {
      text = "Button B (Red)",
      color = Color.red
  },
  new ProtoMenuButton()
  {
      text = "Button C (Disable, Green)",
      color = Color.green,
      enabled = false
  });
```

![Menu Example](https://i.imgur.com/yVMQU0p.png)

# Example Window
This API comes with a simple folder-browser implementation. 

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