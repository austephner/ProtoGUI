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