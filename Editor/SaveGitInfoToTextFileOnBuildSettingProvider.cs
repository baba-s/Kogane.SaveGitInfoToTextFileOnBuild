using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
    internal sealed class SaveGitInfoToTextFileOnBuildSettingProvider : SettingsProvider
    {
        private const string PATH = "Kogane/Save Git Info To Text File On Build";

        private Editor m_editor;

        private SaveGitInfoToTextFileOnBuildSettingProvider
        (
            string              path,
            SettingsScope       scopes,
            IEnumerable<string> keywords = null
        ) : base( path, scopes, keywords )
        {
        }

        public override void OnActivate( string searchContext, VisualElement rootElement )
        {
            var instance = SaveGitInfoToTextFileOnBuildSetting.instance;

            instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;

            Editor.CreateCachedEditor( instance, null, ref m_editor );
        }

        public override void OnGUI( string searchContext )
        {
            using var changeCheckScope = new EditorGUI.ChangeCheckScope();

            m_editor.OnInspectorGUI();

            EditorGUILayout.Space();

            if ( GUILayout.Button( "Reset to Default" ) )
            {
                Undo.RecordObject( SaveGitInfoToTextFileOnBuildSetting.instance, "Reset to Default" );
                SaveGitInfoToTextFileOnBuildSetting.instance.ResetToDefault();
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox( "Template で使用できるタグ", MessageType.Info );

            EditorGUILayout.TextArea
            (
                @"#BRANCH_NAME#：ブランチ名
#COMMIT_HASH#：コミットハッシュ
#SHORT_COMMIT_HASH#：コミットハッシュ（短縮版）
#COMMIT_LOG#：コミットログ"
            );

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox( "Commit Log Format で使用できるタグ", MessageType.Info );

            EditorGUILayout.TextArea
            (
                @"%h：コミットハッシュ（短縮版）
%cd：日付
%cn：コミットユーザー
%s：コミットコメント"
            );

            if ( !changeCheckScope.changed ) return;

            SaveGitInfoToTextFileOnBuildSetting.instance.Save();
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingProvider()
        {
            return new SaveGitInfoToTextFileOnBuildSettingProvider
            (
                path: PATH,
                scopes: SettingsScope.Project
            );
        }
    }
}