using System;

namespace SlimeLab.Systems
{
    public class VersionChecker
    {
        private string _currentVersion;

        public VersionChecker(string currentVersion)
        {
            _currentVersion = currentVersion;
        }

        public bool IsCompatible(string saveVersion)
        {
            if (string.IsNullOrEmpty(saveVersion))
            {
                return false;
            }

            if (!TryParseVersion(saveVersion, out int saveMajor, out int saveMinor, out int savePatch))
            {
                return false;
            }

            if (!TryParseVersion(_currentVersion, out int currentMajor, out int currentMinor, out int currentPatch))
            {
                return false;
            }

            // Major version must match for compatibility
            return saveMajor == currentMajor;
        }

        private bool TryParseVersion(string version, out int major, out int minor, out int patch)
        {
            major = 0;
            minor = 0;
            patch = 0;

            if (string.IsNullOrEmpty(version))
            {
                return false;
            }

            string[] parts = version.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out major))
            {
                return false;
            }

            if (!int.TryParse(parts[1], out minor))
            {
                return false;
            }

            if (!int.TryParse(parts[2], out patch))
            {
                return false;
            }

            return true;
        }
    }
}
