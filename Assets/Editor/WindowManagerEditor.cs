using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.IO;

[CustomEditor(typeof(WindowManager))]
public class WindowManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Generate Widnow Enum"))
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"public enum Windows");
            sb.AppendLine(@"{");
            var windowManager = (WindowManager)target;

            for(int i = 0; i < windowManager.Windows.Length; ++i)
            {
                sb.AppendLine($"\t{windowManager.Windows[i].name},");
            }

            sb.AppendLine(@"}");

            // 에디터에서 파일을 저장해주는 기능
            var path = EditorUtility.SaveFilePanel("Save", "", "Windows.cs", "cs");

            // using 키워드는 try catch 예외처리를 해주는 키워드
            using (var fs = new FileStream(path, FileMode.Create))
            {
                // 텍스쳐 파일을 쓰게 해주는 객체
                using (var writer = new StreamWriter(fs))
                {
                    // 해당 내용으로 파일을 쓰는 용도
                    writer.Write(sb.ToString());
                }
            }
            //if(path)

            // 정식 경로로 파일을 저장하는 것이 아니라 임의로 저장하는 것이기 때문에,
            // 수정한 내용을 에디터에게 알려줘야 함 (알려주지 않는 경우 정보가 갱신되지 않음)
            AssetDatabase.Refresh();
        }
    }
}
