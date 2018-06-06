
using EntityFrameworkVisualizer.Highlighter.Template;

namespace EntityFrameworkVisualizer.Highlighter
{
    /// <summary>
    /// SQL token.
    /// </summary>
    public struct SqlToken
    {
        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets string length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets token type.
        /// </summary>
        public SqlTokenType SqlTokenType { get; set; }
    }
}