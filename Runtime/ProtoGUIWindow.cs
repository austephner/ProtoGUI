using System;
using ProtoGUI.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace ProtoGUI
{
    public class ProtoGUIWindow : MonoBehaviour
    {
        private const float DRAG_AREA_HEIGHT = 30;
        
        #region Settings & Public Fields
        
        [Header("General")] [SerializeField] [RandomizableInt(int.MaxValue)]
        private int _windowId = 7316;

        public string windowTitle = "Window";

        [SerializeField] 
        public bool 
            canDrag, 
            showMinimizeButton, 
            showCloseButton, 
            startOpen, 
            startMinimized, 
            showInWindowManager, 
            showToolbar;

        [Header("Sizing")] [SerializeField] 
        private SizingModeType _horizontalSizingMode = SizingModeType.Screen;
        
        [EnableIf(nameof(_horizontalSizingMode), (int) SizingModeType.Custom)]
        public float width;
        
        [EnableIf(nameof(_horizontalSizingMode), (int) SizingModeType.Multiply)]
        public float widthScreenMultiplier = 1.0f;

        [SerializeField] 
        private SizingModeType _verticalSizingMode = SizingModeType.Screen;

        [EnableIf(nameof(_verticalSizingMode), (int) SizingModeType.Custom)]
        public float height;

        [EnableIf(nameof(_verticalSizingMode), (int) SizingModeType.Multiply)]
        public float heightScreenMultiplier = 1.0f;

        [Header("Positioning")]
        public WindowPositionType startPosition = WindowPositionType.TopLeftCorner;
        
        [EnableIf("startPosition", (int) WindowPositionType.Coordinates)]
        public float startX, startY;
        
        [Header("Misc")]
        public UnityEvent onGuiWindowCall;
        
        [TextArea] [SerializeField] 
        private string _notes;
        
        #endregion
        
        #region Properties

        public Rect rect => _rect;

        public Rect minimizedRect => _minimizedRect;
        
        public DateTime lastShown { get; private set; } = DateTime.Now;
        
        public DateTime lastHidden { get; private set; } = DateTime.Now;

        public bool minimized
        {
            get => _minimized;
            set
            {
                _minimized = value;
                OnMinimizedChanged();
            }
        }

        public bool show
        {
            get => _show;
            set
            {
                _show = value;

                if (value)
                {
                    lastShown = DateTime.Now;
                }
                else
                {
                    lastHidden = DateTime.Now;
                }
                
                OnShowChanged();
            }
        }

        public int windowId => _windowId;
        
        #endregion
        
        #region Private Fields

        private Rect _rect, _minimizedRect;

        private Vector3 _lastMousePosition;
        
        private bool _show, _minimized, _startCalled;
        
        #endregion
        
        #region Unity Events

        private void OnEnable()
        {
            ProtoGUIWindowManager.Register(this);
        }

        private void OnDisable()
        {
            ProtoGUIWindowManager.Unregister(this);
        }

        protected virtual void Start()
        {
            _rect = new Rect();
            UpdateSizingModes();
            AlignToPosition(startPosition);
            show = startOpen;
            minimized = startMinimized;
            _startCalled = true;
        }
        
        private void Update()
        {
            if (!_startCalled)
            {
                throw new Exception("Start() wasn't called. Please ensure Start() is called with base.Start()");
            }
            
            UpdateSizingModes();
            _rect.y = Mathf.Clamp(_rect.y, 0, Screen.height - DRAG_AREA_HEIGHT);
            OnUpdate();
        }
        
        private void OnGUI()
        {
            if (!show) return;
            
            if (!minimized)
            {
                _rect = GUI.Window(_windowId, _rect, Window, windowTitle);
            }
            else
            {
                GUI.Window(_windowId, _minimizedRect, Window, windowTitle);
            }
        }

        private void Window(int id)
        {
            if (showToolbar)
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                DrawToolbar();
                GUILayout.FlexibleSpace();
                if (!minimized)
                {
                    if (showMinimizeButton && GUILayout.Button(GetMinimizeLabel(), GUILayout.ExpandWidth(false)))
                    {
                        minimized = true;
                    }
                }
                else
                {
                    if (showCloseButton && GUILayout.Button(GetMaximizeLabel(), GUILayout.ExpandWidth(false)))
                    {
                        minimized = false;
                    }
                }

                if (GUILayout.Button(GetCloseLabel(), GUILayout.ExpandWidth(false)))
                {
                    show = false;
                }

                GUILayout.EndHorizontal();
            }

            if (minimized)
            {
                return;
            }
            
            DrawContent();
            onGuiWindowCall?.Invoke();

            if (canDrag)
            {
                GUI.DragWindow(new Rect(0, 0, _rect.width, DRAG_AREA_HEIGHT));
            }
        }
        
        #endregion
        
        #region Public Utilities

        public void AlignToPosition(WindowPositionType windowPosition)
        {
            switch (windowPosition)
            {
                case WindowPositionType.Coordinates:
                    _rect.x = startX;
                    _rect.y = startY;
                    break;
                case WindowPositionType.BottomLeftCorner:
                    _rect.x = 0;
                    _rect.y = Screen.height - _rect.height;
                    break;
                case WindowPositionType.BottomRightCorner:
                    _rect.x = Screen.width - _rect.width;
                    _rect.y = Screen.height - _rect.height;
                    break;
                case WindowPositionType.TopLeftCorner:
                    _rect.x = 0;
                    _rect.y = 0;
                    break;
                case WindowPositionType.TopRightCorner:
                    _rect.x = Screen.width - _rect.width;
                    _rect.y = 0;
                    break;
                case WindowPositionType.Middle:
                    _rect.x = (Screen.width / 2.0f) - (_rect.width / 2.0f);
                    _rect.y = (Screen.height / 2.0f) - (_rect.height / 2.0f);
                    break;
            }
        }
        
        public void SetMinimizedRect(Rect rect)
        {
            _minimizedRect = rect;
        }
        
        public bool ContainsMouse() => _rect.Contains(Input.mousePosition);
        
        public void DrawHorizontalFlexibleLabel(string label) 
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(label);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        public void DrawVerticalFlexibleLabel(string label)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(label);
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        public void DrawFullyFlexibleLabel(string label)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(label);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        
        #endregion

        #region Protected Utilities

        protected void UpdateSizingModes()
        {
            _rect.width = _horizontalSizingMode == SizingModeType.Custom ? width : _horizontalSizingMode == SizingModeType.Multiply ? Screen.width * widthScreenMultiplier : Screen.width;
            _rect.height = _verticalSizingMode == SizingModeType.Custom ? height : _verticalSizingMode == SizingModeType.Multiply ? Screen.height * heightScreenMultiplier : Screen.height;
        }

        #endregion

        #region Inheritable Methods

        protected virtual string GetMinimizeLabel() => "Minimize";

        protected virtual string GetMaximizeLabel() => "Maximize";
        
        protected virtual string GetCloseLabel() => "Close";
        
        protected virtual void DrawToolbar() { }

        protected virtual void DrawContent() { }

        protected virtual void OnUpdate() { }

        protected virtual void OnShowChanged() { }

        protected virtual void OnMinimizedChanged() { }

        #endregion

        #region Supporting Types
        
        public enum WindowPositionType
        {
            Coordinates,
            BottomLeftCorner,
            BottomRightCorner,
            TopLeftCorner,
            TopRightCorner,
            Middle
        }

        public enum SizingModeType
        {
            Custom,
            Screen,
            Multiply
        }

        #endregion
    }
}