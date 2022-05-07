using PrimalEditor.Utiltiies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PrimalEditor.GameProject
{
    [DataContract]
    public class ProjectData
    {
        // data we save below
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectPath { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        public string FullPath { get => $"{ProjectPath}{ProjectName}{Project.Extension}"; }
        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }
    }

    [DataContract]
    public class ProjectDataList
    {
        // list of project data
        [DataMember]
        public List<ProjectData>Projects { get; set; }
    }

    class OpenProject  // remembers where we create project
    {
        private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\PrimalEditor\";
        private static readonly string _projectDataPath;
        private static readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();
        public static ReadOnlyObservableCollection<ProjectData> Projects { get; } // read only property set in constructor
        
        private static void ReadProjectData()
        {
            if(File.Exists(_projectDataPath)) // check file exists
            {
                var projects = Serializer.FromFile<ProjectDataList>(_projectDataPath).Projects.OrderByDescending(x => x.Date);
                _projects.Clear(); // clears so we can set/replace with new one
                foreach(var project in projects)
                {
                    if(File.Exists(project.FullPath))
                    {
                        // if does not exist, project is missing or deleted, do not add.
                        project.Icon = File.ReadAllBytes($@"{project.ProjectPath}\.Primal\Icon.png");
                        project.Screenshot = File.ReadAllBytes($@"{project.ProjectPath}\.Primal\Screenshot.png");
                        _projects.Add(project);
                    }
                }    
            }
        }

        private static void WriteProjectData()
        {
            // orders by date of acess
            var projects = _projects.OrderBy(x => x.Date).ToList();
            Serializer.ToFile(new ProjectDataList() { Projects = projects }, _projectDataPath);
        }

        public static Project Open(ProjectData data)
        {
            ReadProjectData(); // read data in constructor and there are multiple instance of the editor and it saves correctly
            var project = _projects.FirstOrDefault(x => x.FullPath == data.FullPath); // check if is in the correct project
            if(project != null) // check if project exists 
            {
                project.Date = DateTime.Now; //update the date acceessed
            }
            else  // new project
            {
                project = data;
                project.Date = DateTime.Now;
                _projects.Add(project);
            }
            WriteProjectData();

            return Project.Load(project.FullPath);
        }

        static OpenProject()
        {
           try
            {
                if (!Directory.Exists(_applicationDataPath)) Directory.CreateDirectory(_applicationDataPath); // check if directory exists, if not make it
                _projectDataPath = $@"{_applicationDataPath}ProjectData.xml"; // get full path
                Projects = new ReadOnlyObservableCollection<ProjectData>(_projects); // set projects
                ReadProjectData();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // TODO: log error
            }
        }
    }
}
