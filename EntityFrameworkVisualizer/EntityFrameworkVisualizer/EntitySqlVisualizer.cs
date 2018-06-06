using System;
using System.Drawing;
using System.IO;
using EntityFrameworkVisualizer.Highlighter;
using EntityFrameworkVisualizer.Highlighter.Template;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace EntityFrameworkVisualizer
{
    /// <summary>
    /// Entity SQL visualizer.
    /// </summary>
    public class EntitySqlVisualizer : DialogDebuggerVisualizer
    {
        /// <summary>
        /// Shows visualizer.
        /// </summary>
        /// <param name="windowService">Windows service.</param>
        /// <param name="objectProvider">Object provider.</param>
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var mainForm = new MainForm();

            using (var sr = new StreamReader(objectProvider.GetData()))
            {
                var sql = sr.ReadToEnd();

                SqlHighlighter sqlHighlighter = new SqlHighlighter();
                var input = sql.Replace(Environment.NewLine, "\r");
                mainForm.richTextBox.Text = input;
                var matches = sqlHighlighter.Parse(input);
                foreach (var item in matches)
                {
                    Color color = GetColor(item.SqlTokenType);

                    mainForm.richTextBox.Select(item.Index, item.Length);
                    mainForm.richTextBox.SelectionColor = color;
                }
            }

            windowService.ShowDialog(mainForm);
        }

        /// <summary>
        /// Gets color.
        /// </summary>
        /// <param name="sqlTokenType">SqlTokenType.</param>
        /// <returns>Color.</returns>
        private static Color GetColor(SqlTokenType sqlTokenType)
        {
            Color color = Color.Black;

            switch (sqlTokenType)
            {
                case SqlTokenType.String:
                    color = Color.Red;
                    break;

                case SqlTokenType.Keyword:
                case SqlTokenType.DataType:
                    color = Color.Blue;
                    break;

                case SqlTokenType.Operator:
                case SqlTokenType.Sign:
                    color = Color.Gray;
                    break;

                case SqlTokenType.Function:
                    color = Color.Magenta;
                    break;

                case SqlTokenType.Comment:
                    color = Color.Green;
                    break;
            }

            return color;
        }
    }
}