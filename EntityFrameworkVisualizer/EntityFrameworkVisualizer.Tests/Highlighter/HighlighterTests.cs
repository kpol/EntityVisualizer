using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkVisualizer.Highlighter;
using EntityFrameworkVisualizer.Highlighter.Template;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EntityFrameworkVisualizer.Tests.Highlighter
{
    /// <summary>
    /// Tests for <see cref="SqlHighlighter"/>.
    /// </summary>
    [TestClass]
    public class SqlHighlighterTests
    {
        /// <summary>
        /// Parsing test.
        /// </summary>
        [TestMethod]
        public void Parse_SqlFrom()
        {
            const string input = @"SELECT * FROM @T";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));
            CheckTokens(
                input,
                res,
               new[]
                {
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword)
                });
        }

        /// <summary>
        /// Parsing test.
        /// </summary>
        [TestMethod]
        public void Parse_SqlJoin()
        {
            const string input = @"SELECT * FROM @T1 t1 JOIN @T2 t2 ON t2.Id = t1.Id";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));
            CheckTokens(
                input,
                res,
                new[]
                {
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("JOIN", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("ON", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign)
                });
        }

        /// <summary>
        /// Parsing test.
        /// </summary>
        [TestMethod]
        public void Parse_SqlJoinWhereFunctions()
        {
            const string input = @"SELECT * FROM @T1 t1 JOIN @T2 t2 ON t2.Id = t1.Id WHERE t1.[DateTime] = GETDATE()";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));
            CheckTokens(
                input,
                res,
                new[]
                {
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("JOIN", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("ON", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("WHERE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("GETDATE", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign)
                });
        }

        /// <summary>
        /// Parsing test.
        /// </summary>
        [TestMethod]
        public void Parse_SqlString()
        {
            const string input = @"SELECT * FROM @T WHERE [Name] = 'Name'";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));
            CheckTokens(
                input,
                res,
                new[]
                {
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("WHERE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("'Name'", SqlTokenType.String)
                });
        }

        /// <summary>
        /// Parsing test.
        /// </summary>
        [TestMethod]
        public void Parse_ComplexSql()
        {
            const string input = @"SELECT TOP (5) 
                [Filter2].[SKU] AS [SKU], 
                [Filter2].[Description] AS [Description], 
                [Filter2].[Price] AS [Price]
                FROM ( SELECT [Extent1].[SKU] AS [SKU], [Extent1].[Description] AS [Description], [Extent1].[Price] AS [Price], row_number() OVER (ORDER BY [Extent1].[Price] ASC) AS [row_number]
                    FROM [Chapter2].[Item] AS [Extent1]
                    WHERE ( NOT EXISTS (SELECT 
                        1 AS [C1]
                        FROM   [Chapter2].[OrderItem] AS [Extent2]
                        INNER JOIN [Chapter2].[Order] AS [Extent3] ON [Extent2].[OrderId] = [Extent3].[OrderId]
                        LEFT OUTER JOIN [Chapter2].[Order] AS [Extent4] ON [Extent2].[OrderId] = [Extent4].[OrderId]
                        WHERE ([Extent1].[SKU] = [Extent2].[SKU]) AND (( NOT ([Extent3].[OrderDate] < (SysDateTime()))) OR (CASE WHEN ([Extent4].[OrderDate] < (SysDateTime())) THEN cast(1 as bit) WHEN ( NOT ([Extent4].[OrderDate] < (SysDateTime()))) THEN cast(0 as bit) END IS NULL))
                    )) AND ([Extent1].[Description] LIKE 'A%')
                )  AS [Filter2]
                WHERE [Filter2].[row_number] > 1
                ORDER BY [Filter2].[Price] ASC";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));
            CheckTokens(
                input,
                res,
                new[]
                {
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("TOP", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("row_number", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("OVER", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("ORDER", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("BY", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("ASC", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("WHERE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("NOT", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("EXISTS", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("SELECT", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("FROM", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("INNER", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("JOIN", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("ON", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("LEFT", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("OUTER", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("JOIN", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("ON", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("WHERE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AND", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("NOT", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("<", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("SysDateTime", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("OR", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("CASE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("WHEN", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("<", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("SysDateTime", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("THEN", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("cast", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("as", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("bit", SqlTokenType.DataType),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("WHEN", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("NOT", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("<", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("SysDateTime", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("THEN", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("cast", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("as", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("bit", SqlTokenType.DataType),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("END", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("IS", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("NULL", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AND", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("LIKE", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("'A%'", SqlTokenType.String),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("AS", SqlTokenType.Keyword),

                    new Tuple<string, SqlTokenType>("WHERE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(">", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("ORDER", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("BY", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("ASC", SqlTokenType.Keyword)
                });
        }

        /// <summary>
        /// Parse source code with comments.
        /// </summary>
        [TestMethod]
        public void Parse_SourceCodeWithComments()
        {
            const string input = @"insert [dbo].[Post]([Title], [Text])
values (@0, @1)
select [PostID], [Date]
from [dbo].[Post]
where @@ROWCOUNT > 0 and [PostID] = scope_identity()
-- @0 = New post
-- @1 = First post

--------------------insert------------------------------
[dbo].[uspCreateUser]

-- Name = Kirill Polishchuk
";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));

            CheckTokens(
                input,
                res,
                new[]
                {
                    new Tuple<string, SqlTokenType>("insert", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("values", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("select", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("from", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("where", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("@@ROWCOUNT", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>(">", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("and", SqlTokenType.Operator),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("scope_identity", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("-- @0 = New post", SqlTokenType.Comment),

                    new Tuple<string, SqlTokenType>("-- @1 = First post", SqlTokenType.Comment),

                    new Tuple<string, SqlTokenType>("--------------------insert------------------------------", SqlTokenType.Comment),

                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("-- Name = Kirill Polishchuk", SqlTokenType.Comment)
                });
        }

        /// <summary>
        /// Parse stored procedures.
        /// </summary>
        [TestMethod]
        public void Parse_StoredProcedures()
        {
            const string input = @"DECLARE @0 nvarchar(max) = N'New post';
DECLARE @1 nvarchar(max) = 'First post';

EXEC [dbo].[usp_CreateUser] @Name = 'Kirill Polishchuk', @BirthDate = '1987-05-14 00:00:00.0000000', @Image = 0x0102FF";

            SqlHighlighter sqlHighlighter = new SqlHighlighter();

            var res = sqlHighlighter.Parse(input).ToList();
            var res2 = sqlHighlighter.Parse(input.ToLower());

            Assert.IsTrue(res.SequenceEqual(res2));

            CheckTokens(
                input,
                res,
                new[]
                {
                    new Tuple<string, SqlTokenType>("DECLARE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("nvarchar", SqlTokenType.DataType),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("max", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("N'New post'", SqlTokenType.String),
                    new Tuple<string, SqlTokenType>(";", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("DECLARE", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>("nvarchar", SqlTokenType.DataType),
                    new Tuple<string, SqlTokenType>("(", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("max", SqlTokenType.Function),
                    new Tuple<string, SqlTokenType>(")", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("'First post'", SqlTokenType.String),
                    new Tuple<string, SqlTokenType>(";", SqlTokenType.Sign),

                    new Tuple<string, SqlTokenType>("EXEC", SqlTokenType.Keyword),
                    new Tuple<string, SqlTokenType>(".", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("'Kirill Polishchuk'", SqlTokenType.String),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("'1987-05-14 00:00:00.0000000'", SqlTokenType.String),
                    new Tuple<string, SqlTokenType>(",", SqlTokenType.Sign),
                    new Tuple<string, SqlTokenType>("=", SqlTokenType.Sign),
                });
        }

        /// <summary>
        /// Checks tokens.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <param name="inputSequence">Input tokens.</param>
        /// <param name="expectedSequence">Expected tokens.</param>
        private static void CheckTokens(string input, IList<SqlToken> inputSequence, IList<Tuple<string, SqlTokenType>> expectedSequence)
        {
            using (var enumerator = inputSequence.GetEnumerator())
            {
                using (var enumerator2 = expectedSequence.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (!enumerator2.MoveNext())
                        {
                            Assert.Fail("Expected count: {0}; Actual count: {1}", expectedSequence.Count(), inputSequence.Count());
                        }

                        // ReSharper disable once PossibleNullReferenceException
                        Assert.AreEqual(enumerator2.Current.Item1, input.Substring(enumerator.Current.Index, enumerator.Current.Length));
                        Assert.AreEqual(enumerator2.Current.Item2, enumerator.Current.SqlTokenType);
                    }

                    if (enumerator2.MoveNext())
                    {
                        Assert.Fail("Expected count: {0}; Actual count: {1}", expectedSequence.Count(), inputSequence.Count());
                    }
                }
            }
        }
    }
}