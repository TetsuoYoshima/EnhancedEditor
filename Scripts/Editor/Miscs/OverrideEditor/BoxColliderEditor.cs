// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using UnityEditor;
using UnityEngine;

using Object = UnityEngine.Object;

namespace EnhancedEditor.Editor {
    /// <summary>
    /// Custom <see cref="BoxCollider"/> editor, adding save and load utilities.
    /// </summary>
    [CustomEditor(typeof(BoxCollider), true), CanEditMultipleObjects]
    public sealed class BoxColliderEditor : UnityObjectEditor {
        /// <summary>
        /// Serializable <see cref="BoxCollider"/> data.
        /// </summary>
        [Serializable]
        private sealed class Data : PlayModeObjectData {
            #region Content
            [SerializeField] public Vector3 Center  = Vector3.zero;
            [SerializeField] public Vector3 Size    = Vector3.one;
            [SerializeField] public bool IsTrigger  = false;

            // -------------------------------------------
            // Constructor(s)
            // -------------------------------------------

            public Data() : base() { }

            // -------------------------------------------
            // Utility
            // -------------------------------------------

            public override void Save(Object _object) {

                if (_object is BoxCollider _collider) {

                    IsTrigger = _collider.isTrigger;
                    Center    = _collider.center;
                    Size      = _collider.size;
                }

                base.Save(_object);
            }

            public override bool Load(Object _object) {

                if (_object is BoxCollider _collider) {

                    _collider.isTrigger = IsTrigger;
                    _collider.center    = Center;
                    _collider.size  = Size;

                    return true;
                }

                return false;
            }
            #endregion
        }

        #region Editor Content
        private static readonly GUIContent resetCenterGUI = new GUIContent(" Adjust Center", "Adjust this collider position and reset its center.");
        private static readonly Type editorType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.BoxColliderEditor");
        private static readonly Data data = new Data();

        private UnityEditor.Editor colliderEditor = null;

        protected override bool CanSaveData {
            get { return true; }
        }

        // -----------------------

        protected override void OnEnable() {
            base.OnEnable();
            colliderEditor = CreateEditor(serializedObject.targetObjects, editorType);
        }

        public override void OnInspectorGUI() {

            // Save load properties.
            SaveLoadButtonGUILayout();

            // Adjust center button.
            if ((target is BoxCollider _collider) && (_collider.center != Vector3.zero)) {

                Rect position = EditorGUILayout.GetControlRect(true, 20f);
                position.xMin = position.xMax - SaveValueButtonWidth;

                if (EnhancedEditorGUI.IconDropShadowButton(position, resetCenterGUI)) {
                    Object[] _targets = targets;

                    for (int i = _targets.Length; i-- > 0;) {
                        if (_targets[i] is BoxCollider _box) {
                            SceneDesignerUtility.AdjustBoxCenter(_box);
                        }
                    }
                }
            }

            colliderEditor.OnInspectorGUI();
        }

        protected override void OnDisable() {
            base.OnDisable();

            // Avoid memory leak.
            DestroyImmediate(colliderEditor);
        }

        // -------------------------------------------
        // Callback
        // -------------------------------------------

        protected override PlayModeObjectData SaveData(Object _object) {
            data.Save(_object);
            return data;
        }
        #endregion
    }
}
