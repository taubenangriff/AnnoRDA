using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AnnoRDA.Loader
{
    public class RdaArchiveLoaderConfig
    {
        public IEnumerable<string> WhitelistPatterns { get; set; }
        public bool UseWhitelist { get; set; }
        public IEnumerable<string> BlacklistPatterns { get; set; }
        public bool UseBlacklist { get; set; }
        public bool PreferWhitelist { get; set; } = true;

        public bool UseRegexInsteadOfWildcard { get; set; } = false;

        public bool LoadZeroByteFiles { get; set; } = true;

        public RdaArchiveLoaderConfig(
            IEnumerable<string> whitelistPatterns,
            bool useWhitelist,
            IEnumerable<string> blacklistPatterns,
            bool useBlacklist,
            bool preferWhitelist = true)
        {
            WhitelistPatterns = whitelistPatterns;
            UseWhitelist = useWhitelist;
            BlacklistPatterns = blacklistPatterns;
            UseBlacklist = useBlacklist;
            PreferWhitelist = preferWhitelist;
        }

        public static RdaArchiveLoaderConfig Default => new RdaArchiveLoaderConfig(
                whitelistPatterns: Enumerable.Empty<string>(),
                useWhitelist: false,
                blacklistPatterns: Enumerable.Empty<string>(),
                useBlacklist: false
            );

        public bool ShouldLoadFilename(String filename) {

            var real_filename = Path.GetFileName(filename);
            bool whitelisted = WhitelistPatterns.Any(x => UseRegexInsteadOfWildcard ? Regex.IsMatch(real_filename, x) : FileSystemName.MatchesSimpleExpression(x, real_filename));
            bool blacklisted = BlacklistPatterns.Any(x => UseRegexInsteadOfWildcard ? Regex.IsMatch(real_filename, x) : FileSystemName.MatchesSimpleExpression(x, real_filename));

            if (UseWhitelist && UseBlacklist && whitelisted && blacklisted)
                return PreferWhitelist;

            return (!UseWhitelist || whitelisted) && (!UseBlacklist || !blacklisted);
        }
    }
}
