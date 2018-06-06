using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EntityFrameworkVisualizer.Highlighter.Template;

namespace EntityFrameworkVisualizer.Highlighter
{
    /// <summary>
    /// SQL source code highlighter.
    /// </summary>
    public class SqlHighlighter
    {
        /// <summary>
        /// Parses SQL source code string.
        /// </summary>
        /// <param name="input">SQL source code string.</param>
        /// <returns>Returns parsed tokens.</returns>
        public IEnumerable<SqlToken> Parse(string input)
        {
            foreach (Match match in Regex.Matches(input, RegexHelper.SqlPattern))
            {
                if (match.Groups["continue"].Success)
                {
                    continue;
                }

                var sqlToken = GetSqlToken(match);

                if (sqlToken.HasValue)
                {
                    yield return sqlToken.Value;
                }
            }
        }

        /// <summary>
        /// Gets <see cref="SqlToken"/> from <see cref="Match"/>.
        /// </summary>
        /// <param name="match">Match token.</param>
        /// <returns><see cref="SqlToken"/>.</returns>
        private static SqlToken? GetSqlToken(Match match)
        {
            foreach (SqlTokenType item in Enum.GetValues(typeof(SqlTokenType)))
            {
                if (match.Groups[item.ToString()].Success)
                {
                    return new SqlToken 
                    { 
                        Index = match.Groups[item.ToString()].Index, 
                        Length = match.Groups[item.ToString()].Length, 
                        SqlTokenType = item 
                    };
                }
            }

            return null;
        }
    }
}