using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    [FilePath( "ProjectSettings/Kogane/SaveGitInfoToTextFileOnBuildSetting.asset", FilePathAttribute.Location.ProjectFolder )]
    internal sealed class SaveGitInfoToTextFileOnBuildSetting : ScriptableSingleton<SaveGitInfoToTextFileOnBuildSetting>
    {
        private const string DEFAULT_FILE_NAME = "git.txt";

        private const string DEFAULT_TEMPLATE = @"ブランチ名：#BRANCH_NAME#
コミットハッシュ：#SHORT_COMMIT_HASH#

コミットログ：
#COMMIT_LOG#";

        private const int    DEFAULT_COMMIT_LOG_COUNT        = 10;
        private const bool   DEFAULT_COMMIT_LOG_IS_NO_MERGES = false;
        private const string DEFAULT_COMMIT_LOG_FORMAT       = "    %cd %h %cn %s";

        [SerializeField]                                         private string m_fileName            = DEFAULT_FILE_NAME;
        [SerializeField][TextArea( minLines: 10, maxLines: 10 )] private string m_template            = DEFAULT_TEMPLATE;
        [SerializeField]                                         private int    m_commitLogCount      = DEFAULT_COMMIT_LOG_COUNT;
        [SerializeField]                                         private bool   m_commitLogIsNoMerges = DEFAULT_COMMIT_LOG_IS_NO_MERGES;
        [SerializeField]                                         private string m_commitLogFormat     = DEFAULT_COMMIT_LOG_FORMAT;

        public string FileName            => m_fileName;
        public string Template            => m_template;
        public int    CommitLogCount      => m_commitLogCount;
        public bool   CommitLogIsNoMerges => m_commitLogIsNoMerges;
        public string CommitLogFormat     => m_commitLogFormat;

        public void Save()
        {
            Save( true );
        }

        public void ResetToDefault()
        {
            m_fileName            = DEFAULT_FILE_NAME;
            m_template            = DEFAULT_TEMPLATE;
            m_commitLogCount      = DEFAULT_COMMIT_LOG_COUNT;
            m_commitLogIsNoMerges = DEFAULT_COMMIT_LOG_IS_NO_MERGES;
            m_commitLogFormat     = DEFAULT_COMMIT_LOG_FORMAT;
        }
    }
}