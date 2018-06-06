using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace EntityFrameworkVisualizer.VisualizerObjectSources
{
    public class SimpleVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var writer = new StreamWriter(outgoingData);
            writer.WriteLine(target.ToString());
            writer.Flush();
        }
    }
}