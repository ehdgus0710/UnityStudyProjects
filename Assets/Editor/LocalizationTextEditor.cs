using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(LocalizationText))]
public class LocalizationTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var text = (LocalizationText)target;
        var newId = EditorGUILayout.TextField("Text ID", text.stringId);
        var newLanguage = (Languages)EditorGUILayout.EnumPopup("Language", text.editorLanguage);
        // text.stringId
        if(newId != text.stringId || newLanguage != text.editorLanguage)
        {
            text.stringId = newId;
            text.editorLanguage = newLanguage;
            text.OnChangeLanguage(text.editorLanguage);
            // 내용 갱신
            EditorUtility.SetDirty(text);
        }

    }
}
