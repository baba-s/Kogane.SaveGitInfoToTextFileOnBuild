using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Kogane.Internal
{
    internal sealed class SaveGitInfoToTextFileOnBuild : CompletableProcessBuildWithReportBase
    {
        private static readonly string DIRECTORY_NAME = $"Assets/{nameof( SaveGitInfoToTextFileOnBuild )}/Resources";

        protected override void OnStart( BuildReport report )
        {
            // リリースビルドにテキストファイルが含まれないように
            // ビルド開始時に削除しています
            Refresh();

#if KOGANE_DISABLE_SAVE_GIT_INFO_TO_TEXT_FILE_ON_BUILD
#else
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
            File.WriteAllText( path, result );
            AssetDatabase.ImportAsset( path );
#endif
        }

#if KOGANE_DISABLE_SAVE_GIT_INFO_TO_TEXT_FILE_ON_BUILD
#else
        protected override void OnComplete()
        {
            Refresh();
        }
#endif

        private static void Refresh()
        {
            var directoryName = Path.GetDirectoryName( DIRECTORY_NAME );
            if ( !AssetDatabase.IsValidFolder( directoryName ) ) return;
            AssetDatabase.DeleteAsset( directoryName );
        }
    }
}