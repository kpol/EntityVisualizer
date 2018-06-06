using System;
using System.Data;
using System.Linq;

namespace EntityFrameworkVisualizer.VisualizerObjectSources.CommandFormatters
{
    public static class SqlCommandFormatter
    {
        /// <summary>
        /// Format parameter.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="parameterType">Parameter type.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>String representation.</returns>
        public static string DeclareParameter(string parameterName, Type parameterType, object value)
        {
            const string formatString = "DECLARE @{0} {1} = {2};";

            var sqlType = GetSqlType(parameterType);
            var typeString = GetSqlTypeString(sqlType);
            var sqlValue = FormatValue(sqlType, value);

            return string.Format(formatString, parameterName, typeString, sqlValue);
        }

        /// <summary>
        /// Format value of command.
        /// </summary>
        /// <param name="sqlDbType"><see cref="SqlDbType"/>.</param>
        /// <param name="value">Value of command.</param>
        /// <returns>Formatting <paramref name="value"/>.</returns>
        public static string FormatValue(SqlDbType sqlDbType, object value)
        {
            var typesToQuote = new[]
            {
                SqlDbType.Char,
                SqlDbType.Date,
                SqlDbType.DateTime,
                SqlDbType.DateTime2,
                SqlDbType.DateTimeOffset,
                SqlDbType.NChar,
                SqlDbType.NText,
                SqlDbType.NVarChar,
                SqlDbType.SmallDateTime,
                SqlDbType.Text,
                SqlDbType.Time,
                SqlDbType.Timestamp,
                SqlDbType.UniqueIdentifier,
                SqlDbType.VarChar,
                SqlDbType.Xml
            };

            string resultValue = value.ToString();

            if (sqlDbType == SqlDbType.SmallDateTime)
            {
                resultValue = ((DateTime)value).ToString("yyyyMMdd");
            }
            else if (sqlDbType == SqlDbType.DateTime)
            {
                resultValue = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            }
            else if (sqlDbType == SqlDbType.Date)
            {
                resultValue = ((DateTime)value).ToString("yyyy-MM-dd");
            }
            else if (sqlDbType == SqlDbType.Time)
            {
                resultValue = ((DateTime)value).ToString("HH:mm:ss.fffffff");
            }
            else if (sqlDbType == SqlDbType.DateTime2)
            {
                resultValue = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fffffff");
            }
            else if (sqlDbType == SqlDbType.DateTimeOffset)
            {
                resultValue = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fffffffzzzz");
            }
            else if (value is byte[])
            {
                var array = (byte[])value;
                resultValue = "0x" + string.Concat(array.Select(b => b.ToString("X2")));
            }

            var unicode = new[]
            {
                SqlDbType.NChar,
                SqlDbType.NText,
                SqlDbType.NVarChar
            };

            var prefix = unicode.Contains(sqlDbType) ? "N" : string.Empty;

            return typesToQuote.Contains(sqlDbType) ? $"{prefix}'{resultValue}'" : resultValue;
        }

        /// <summary>
        /// Returns string representation of <see cref="SqlDbType"/>.
        /// </summary>
        /// <param name="sqlDbType"><see cref="SqlDbType"/> to format.</param>
        /// <returns>String representation of <see cref="SqlDbType"/>.</returns>
        public static string GetSqlTypeString(SqlDbType sqlDbType)
        {
            string result = null;
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                    result = "bigint";
                    break;

                case SqlDbType.Binary:
                    result = "varbinary(max)";
                    break;

                case SqlDbType.Bit:
                    result = "bit";
                    break;

                case SqlDbType.Char:
                    result = "char(max)";
                    break;

                case SqlDbType.Date:
                    result = "date";
                    break;

                case SqlDbType.DateTime:
                    result = "datetime";
                    break;

                case SqlDbType.DateTime2:
                    result = "datetime2";
                    break;

                case SqlDbType.DateTimeOffset:
                    result = "datetimeoffset";
                    break;

                case SqlDbType.Decimal:
                    result = "decimal";
                    break;

                case SqlDbType.Float:
                    result = "float";
                    break;

                case SqlDbType.VarBinary:
                case SqlDbType.Image:
                    result = "varbinary(max)";
                    break;

                case SqlDbType.Int:
                    result = "int";
                    break;

                case SqlDbType.Money:
                    result = "money";
                    break;

                case SqlDbType.NChar:
                    result = "nchar(max)";
                    break;

                case SqlDbType.NVarChar:
                case SqlDbType.NText:
                    result = "nvarchar(max)";
                    break;

                case SqlDbType.Real:
                    result = "real";
                    break;

                case SqlDbType.SmallDateTime:
                    result = "smalldatetime";
                    break;

                case SqlDbType.SmallInt:
                    result = "smallint";
                    break;

                case SqlDbType.SmallMoney:
                    result = "smallmoney";
                    break;

                case SqlDbType.VarChar:
                case SqlDbType.Text:
                    result = "varchar(max)";
                    break;

                case SqlDbType.Time:
                    result = "time";
                    break;

                case SqlDbType.Variant:
                    result = "sql_variant";
                    break;

                case SqlDbType.Timestamp:
                    result = "timestamp";
                    break;

                case SqlDbType.TinyInt:
                    result = "tinyint";
                    break;

                case SqlDbType.UniqueIdentifier:
                    result = "uniqueidentifier";
                    break;

                case SqlDbType.Xml:
                    result = "xml";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Returns <see cref="SqlDbType"/> from <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/>.</param>
        /// <returns><see cref="SqlDbType"/>.</returns>
        private static SqlDbType GetSqlType(Type type)
        {
            var result = SqlDbType.Int;

            if (type == typeof(string))
            {
                result = SqlDbType.NVarChar;
            }
            else if (type == typeof(bool))
            {
                result = SqlDbType.Bit;
            }
            else if (type == typeof(int))
            {
                result = SqlDbType.Int;
            }
            else if (type == typeof(long))
            {
                result = SqlDbType.BigInt;
            }
            else if (type == typeof(short))
            {
                result = SqlDbType.SmallInt;
            }
            else if (type == typeof(float))
            {
                result = SqlDbType.Real;
            }
            else if (type == typeof(decimal))
            {
                result = SqlDbType.Decimal;
            }
            else if (type == typeof(double))
            {
                result = SqlDbType.Float;
            }
            else if (type == typeof(byte[]))
            {
                result = SqlDbType.VarBinary;
            }
            else if (type == typeof(Guid))
            {
                result = SqlDbType.UniqueIdentifier;
            }
            else if (type == typeof(byte))
            {
                result = SqlDbType.TinyInt;
            }
            else if (type == typeof(DateTime))
            {
                result = SqlDbType.DateTime2;
            }

            return result;
        }
    }
}