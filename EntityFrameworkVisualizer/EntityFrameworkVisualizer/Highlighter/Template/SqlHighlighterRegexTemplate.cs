
namespace EntityFrameworkVisualizer.Highlighter.Template
{
    /// <summary>
    /// SQL token type. 
    /// </summary>
    public enum SqlTokenType
    {
        /// <summary>
        /// String.
        /// </summary>
        String,

        /// <summary>
        /// Comment.
        /// </summary>
        Comment,

        /// <summary>
        /// Keyword.
        /// </summary>
        Keyword,

        /// <summary>
        /// Operator.
        /// </summary>
        Operator,

        /// <summary>
        /// Function.
        /// </summary>
        Function,

        /// <summary>
        /// DataType.
        /// </summary>
        DataType,

        /// <summary>
        /// Sign.
        /// </summary>
        Sign
    }

    /// <summary>
    /// Regex helper.
    /// </summary>
    internal class RegexHelper
    {
        /// <summary>
        /// Gets SQL source code regex pattern..
        /// </summary>
        public static string SqlPattern =>
                                @"(?xsi)
    (?<continue>\[[^]]*\])
    |
    (@\w+)
    |
    (?<String>N?'(?:[^']|'')*')  # string
    |
    (?<Comment>--.*?)[\r\n$]   # comment
    |
    (?<Comment>/\*.*?\*/)      # multiline comment
    |
    (?<Keyword>\b(?:as|asc|by|case|declare|delete|desc|end|escape|exec|execute|from|full|go|group|insert|into|on|order|over|select|set|then|top|union|update|values|when|where)\b)     # keyword
    |
    (?<Operator>\b(?:all|and|any|between|cross|exists|in|inner|is|join|left|like|not|null|or|outer|right)\b)     # operator
    |
    (?<Function>(?<!\w)@@rowcount\b|\b(?:acos|ascii|asin|atan|atn2|cast|charindex|checksum|checksum_agg|convert|cos|cot|current_timestamp|current_user|datalength|dateadd|datediff|datename|datepart|degrees|difference|exp|getdate|getutcdate|host_name|isdate|isnumeric|log|log10|max|patindex|pi|quotename|radians|rand|replicate|row_number|scope_identity|sign|sin|soundex|space|sqrt|square|str|stuff|sysdatetime|tan|unicode|user_name)\b)     # function
    |
    (?<DataType>\b(?:bigint|binary|bit|char|cursor|date|datetime|datetime2|datetimeoffset|decimal|float|hierarchyid|image|int|money|nchar|ntext|numeric|nvarchar|real|smalldatetime|smallint|smallmoney|sql_variant|table|text|time|timestamp|tinyint|uniqueidentifier|varbinary|varchar|xml)\b)     # datatype
    |
    (?<Sign>[().,;<>=\-+/])  # sign
    ";
    }
}

