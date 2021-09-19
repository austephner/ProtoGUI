using System;
using System.Collections.Generic;
using System.Linq;
using ProtoGUI.Attributes;
using UnityEngine;

namespace ProtoGUI
{
    public class ProtoGUIWindowManager : MonoBehaviour
    {
        #region Settings

        [SerializeField] 
        private bool _dontDestroyOnLoad = true;
        
        [SerializeField] [RandomizableInt(int.MaxValue)]
        private int _managerWindowId = 812356;
        
        [SerializeField] 
        private bool _hideWhenNoWindows = true;
        
        [SerializeField] 
        private float 
            _defaultWindowWidth = 150,
            _minimizedWindowHeight = 55;

        #endregion

        #region Properties

        public static List<ProtoGUIWindow> windows = new List<ProtoGUIWindow>();

        public static ProtoGUIWindowManager instance { get; private set; }

        #endregion

        #region Private Fields

        private bool _windowsExpanded = false;

        private string _filter = "";

        private Vector2 _scrollArea;

        #endregion

        #region Unity Events

        private void OnEnable()
        {
            if (instance && instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            instance = this;
        }

        private void OnDisable()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
        
        private void Update()
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            var minimizedWindows = GetMinimizedWindows();
            var count = minimizedWindows.Count + 1;
            
            var windowWidth = 
                count * _defaultWindowWidth > screenWidth
                    ? (float)screenWidth / count
                    : _defaultWindowWidth;
            
            for (int i = 0; i < minimizedWindows.Count; i++)
            {
                var rect = minimizedWindows[i].minimizedRect;

                rect.x = (i + 1) * windowWidth;
                rect.y = screenHeight - _minimizedWindowHeight;
                rect.width = windowWidth;
                rect.height = _minimizedWindowHeight;
                
                minimizedWindows[i].SetMinimizedRect(rect);
            }
        }
        
        private void OnGUI()
        {
            if (_hideWhenNoWindows && 
                (windows == null ||
                windows.Count == 0 || 
                !windows.Any(window => window.showInWindowManager)))
            {
                return;
            }
            
            var rect = new Rect()
            {
                x = 0,
                width = _defaultWindowWidth
            };

            if (_windowsExpanded)
            {
                rect.height = Screen.height;
                rect.y = 0;
            }
            else
            {
                rect.height = _minimizedWindowHeight;
                rect.y = Screen.height - _minimizedWindowHeight;
            }

            GUI.Window(_managerWindowId, rect, Window, "Windows");
        }
        
        private void Window(int id)
        {
            if (_windowsExpanded)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Filter", GUILayout.ExpandWidth(false));
                _filter = GUILayout.TextField(_filter);
                GUILayout.EndHorizontal();

                _scrollArea = GUILayout.BeginScrollView(_scrollArea, GUIStyle.none, GUI.skin.verticalScrollbar);
                GUILayout.BeginVertical(GUI.skin.box);

                var modifiedFilter = _filter.ToLower().Replace(" ", "");
                
                foreach (var window in windows)
                {
                    if (!window.showInWindowManager)
                    {
                        continue;
                    }
                    
                    if (!string.IsNullOrWhiteSpace(modifiedFilter) && 
                        window.windowTitle.ToLower().Replace(" ", "") != modifiedFilter)
                    {
                        continue;
                    }
                    
                    GUI.enabled = !window.show;

                    if (GUILayout.Button(window.windowTitle))
                    {
                        window.show = true;
                        _windowsExpanded = false;
                    }
                    
                    GUI.enabled = true;
                }
                
                GUILayout.EndVertical();
                GUILayout.EndScrollView();

                if (GUILayout.Button("Minimize"))
                {
                    _windowsExpanded = false;
                }
            }
            else if (GUILayout.Button("Show All"))
            {
                _windowsExpanded = true;
            }
        }

        #endregion

        #region Private Utilities

        private List<ProtoGUIWindow> GetMinimizedWindows()
        {
            var result = new List<ProtoGUIWindow>();

            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].minimized && windows[i].show)
                {
                    result.Add(windows[i]);
                }
            }

            return result;
        }

        #endregion

        #region Public Static Utilities

        public static void Register(ProtoGUIWindow prototypeWindow)
        {
            windows.Add(prototypeWindow);
        }

        public static void Unregister(ProtoGUIWindow prototypeWindow)
        {
            windows.Remove(prototypeWindow);
        }

        #endregion
    }
}