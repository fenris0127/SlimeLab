using System.Collections.Generic;

namespace SlimeLab.Systems
{
    public class ContentUnlockManager
    {
        private HashSet<string> _unlockedContent;

        public ContentUnlockManager()
        {
            _unlockedContent = new HashSet<string>();
        }

        public void UnlockContent(string contentID)
        {
            _unlockedContent.Add(contentID);
        }

        public bool IsContentUnlocked(string contentID)
        {
            return _unlockedContent.Contains(contentID);
        }

        public List<string> GetUnlockedContent()
        {
            return new List<string>(_unlockedContent);
        }

        public void LockContent(string contentID)
        {
            _unlockedContent.Remove(contentID);
        }
    }
}
