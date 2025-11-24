// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EnhancedEditor.Editor {
    /// <summary>
    /// Base custom <see cref="Collider"/> editor, adding save and load utilities.
    /// </summary>
    public abstract class ColliderEditor : UnityObjectEditor {
        #region Editor Content
        private const string triggerPropertyName = "Is Trigger";
        private const string centerPropertyName  = "Center";

        protected static readonly GUIContent resetCenterGUI = new GUIContent(" Adjust Center", "Adjust this collider position and reset its center.");
        protected static readonly GUIContent editGUI        = new GUIContent("Edit Collider");

        // -----------------------

        protected override void OnEnable() {
            base.OnEnable();

            // Re-order properties
            List<SerializedProperty> _span = properties;
            for (int i = 0; i < _span.Count; i++) {

                SerializedProperty _property = _span[i];
                if (_property.displayName.Equals(triggerPropertyName, StringComparison.Ordinal)) {
                    _span.Move(i, 0);
                } else if (_property.displayName.Equals(centerPropertyName, StringComparison.Ordinal)) {
                    _span.Move(i, 2);
                }
            }
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.EditorToolbarForTarget(editGUI, this);
            EditorGUILayout.Space(5f);

            base.OnInspectorGUI();
        }
        #endregion
    }
}
