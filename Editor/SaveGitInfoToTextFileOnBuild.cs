using System;
using System.IO;
using System.Text;
using Kogane.Internal;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Kogane
{
    public sealed class SaveGitInfoToTextFileOnBuild : CompletableProcessBuildWithReportBase
    {
        private static readonly string DIRECTORY_NAME = $"Assets/{nameof( SaveGitInfoToTextFileOnBuild )}/Resources";

        public static Func<bool> OnIsRelease { get; set; }

        private static bool IsRelease => OnIsRelease?.Invoke() ?? false;

        protected override void OnStart( BuildReport report )
        {
            // リリースビルドにテキストファイルが含まれないように
            // ビルド開始時に削除しています
            Refresh();

            if ( IsRelease ) return;

            var setting = SaveGitInfoToTextFileOnBuildSetting.instance;

            var commitLogOption = new CommitLogOption
            (
                count: setting.CommitLogCount,
                isNoMerges: setting.CommitLogIsNoMerges,
                format: setting.CommitLogFormat
            );

            var result = setting.Template
                    .Replace( "#BRANCH_NAME#", GitUtils.LoadBranchName() )
                    .Replace( "#COMMIT_HASH#", GitUtils.LoadCommitHash() )
                    .Replace( "#SHORT_COMMIT_HASH#", GitUtils.LoadShortCommitHash() )
                    .Replace( "#COMMIT_LOG#", GitUtils.LoadCommitLog( commitLogOption ) )
                ;

            Directory.CreateDirectory( DIRECTORY_NAME );
            var path = $"{DIRECTORY_NAME}/{setting.FileName}";
            File.WriteAllText( path, result, Encoding.UTF8 );
            AssetDatabase.ImportAsset( path );
        }

        protected override void OnComplete()
        {
            if ( IsRelease ) return;
            Refresh();
        }

        private static void Refresh()
        {
            var directoryName = Path.GetDirectoryName( DIRECTORY_NAME );
            if ( !AssetDatabase.IsValidFolder( directoryName ) ) return;
            AssetDatabase.DeleteAsset( directoryName );
        }
    }
}