using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSIXEntityVisualizer
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideAutoLoad(Microsoft.VisualStudio.VSConstants.UICONTEXT.NoSolution_string)]
    public sealed class VisualizerPackage : Package
    {
        /// <summary>
        /// VisualizerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "9d7575cb-5d55-474c-8c8c-8d8139f1a7d1";

        private const string EntityframeworkvisualizerDll = "EntityFrameworkVisualizer.dll";

        private string _sourceFileFullName;
        private string _destinationFileFullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizerPackage"/> class.
        /// </summary>
        // ReSharper disable once EmptyConstructor
        public VisualizerPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // The Visualizer dll is in the same folder as the package because its project is added as reference to this project,
            // so it is included inside the .vsix file. We only need to deploy it to the correct destination folder.
            var sourceFolderFullName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Get the destination folder for visualizers
            var shell = GetService(typeof(SVsShell)) as IVsShell;
            object documentsFolderFullNameObject;
            // ReSharper disable once PossibleNullReferenceException
            shell.GetProperty((int)__VSSPROPID2.VSSPROPID_VisualStudioDir, out documentsFolderFullNameObject);

            var documentsFolderFullName = documentsFolderFullNameObject.ToString();
            var destinationFolderFullName = Path.Combine(documentsFolderFullName, "Visualizers");

            // ReSharper disable once AssignNullToNotNullAttribute
            _sourceFileFullName = Path.Combine(sourceFolderFullName, EntityframeworkvisualizerDll);
            _destinationFileFullName = Path.Combine(destinationFolderFullName, EntityframeworkvisualizerDll);

            try
            {
                CopyFileIfNewerVersion(_sourceFileFullName, _destinationFileFullName);
            }
            catch
            {

            }
        }

        private static void CopyFileIfNewerVersion(string sourceFileFullName, string destinationFileFullName)
        {
            bool copy = false;

            if (File.Exists(destinationFileFullName))
            {
                var sourceFileVersionInfo = FileVersionInfo.GetVersionInfo(sourceFileFullName);
                var destinationFileVersionInfo = FileVersionInfo.GetVersionInfo(destinationFileFullName);

                if (sourceFileVersionInfo.FileMajorPart > destinationFileVersionInfo.FileMajorPart)
                {
                    copy = true;
                }
                else if (sourceFileVersionInfo.FileMajorPart == destinationFileVersionInfo.FileMajorPart
                         && sourceFileVersionInfo.FileMinorPart > destinationFileVersionInfo.FileMinorPart)
                {
                    copy = true;
                }
            }
            else
            {
                // First time
                copy = true;
            }

            if (copy)
            {
                File.Copy(sourceFileFullName, destinationFileFullName, true);
            }
        }
    }
}
