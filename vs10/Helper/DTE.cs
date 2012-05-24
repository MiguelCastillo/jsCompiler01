using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.IO;
using EnvDTE80;

namespace SoftGPL.vs10.Helper
{
    public class DTE
    {
        /// <summary>
        /// Static GUIDS for ProjectItem types.
        /// http://msdn.microsoft.com/en-us/library/z4bcch80(v=vs.80).aspx
        /// </summary>
        static class ProjectItemKind
        {
            public static string PhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
            public static string PhysicalFolder = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
            public static string VirtualFolder = "{6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}";
            public static string Subproject = "{EA6618E8-6E24-4528-94BE-6889FE16485C}";
        }


        private EnvDTE80.DTE2 _DTE = null;

        public DTE(EnvDTE80.DTE2 dte2)
        {
            _DTE = dte2;
        }


        public List<string> GetJavaScriptFilesFromSolution()
        {
            List<string> result = GetFilesFromProjects();
            return result;
        }


        private List<string> GetFilesFromProjects()
        {
            List<string> result = new List<string>();

            foreach ( Project project in _DTE.Solution.Projects )
            {
                List<string> files = FilesFromProjectItems(project.ProjectItems, "");
                result.AddRange(files);
            }

            return result;
        }


        private List<string> FilesFromProjectItems(ProjectItems projectItems, string path)
        {
            if (projectItems == null)
                return null;

            List<string> result = new List<string>();

            foreach (ProjectItem item in projectItems)
            {
                if (item.Kind == ProjectItemKind.PhysicalFile)
                {
                    if (item.Name.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase) == true)
                    {
                        result.Add(item.FileNames[0]);
                    }
                }
                else if (item.Kind == ProjectItemKind.PhysicalFolder || item.Kind == ProjectItemKind.VirtualFolder)
                {
                    List<string> files = FilesFromProjectItems(item.ProjectItems, path + "\\" + item.Name );
                    result.AddRange(files);
                }
            }

            return result;
        }


    }
}
