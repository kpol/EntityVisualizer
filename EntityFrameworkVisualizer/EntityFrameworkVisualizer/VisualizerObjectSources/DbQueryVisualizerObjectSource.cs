using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EntityFrameworkVisualizer.VisualizerObjectSources.CommandFormatters;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace EntityFrameworkVisualizer.VisualizerObjectSources
{
    /// <summary>
    /// Visualizer for <see cref="IQueryable"/>.
    /// </summary>
    public class DbQueryVisualizerObjectSource : VisualizerObjectSource
    {
        /// <summary>
        /// Writes data to outgoing data stream.
        /// </summary>
        /// <param name="target">Object being visualized.</param>
        /// <param name="outgoingData">Outgoing data stream.</param>
        public override void GetData(object target, Stream outgoingData)
        {
            var queryObject = GetQueryFromQueryable((IQueryable)target);

            var stringBuilder = new StringBuilder();

            foreach (var item in queryObject.Parameters)
            {
                stringBuilder.AppendLine(SqlCommandFormatter.DeclareParameter(item.Name, item.ParameterType, item.Value));
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine(target.ToString());

            var writer = new StreamWriter(outgoingData);
            writer.WriteLine(stringBuilder.ToString());
            writer.Flush();
        }

        private static ObjectQuery GetQueryFromQueryable(IQueryable query)
        {
            var internalQueryField = query.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name.Equals("_internalQuery"));
            var internalQuery = internalQueryField?.GetValue(query);
            var objectQueryField = internalQuery?.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name.Equals("_objectQuery"));

            return objectQueryField?.GetValue(internalQuery) as ObjectQuery;
        }
    }
}