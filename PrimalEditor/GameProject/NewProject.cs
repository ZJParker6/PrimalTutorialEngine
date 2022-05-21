using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using PrimalEditor.Utiltiies;
using System.Collections.ObjectModel;

namespace PrimalEditor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }
        [DataMember]
        public List<string> Folders { get; set; }
        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }
        public string IconFilePath { get; set; }
        public string ScreenshotFilePath { get; set; }
        public string ProjectFilePath { get; set; }
    }

    class NewProject : ViewModelBase
    {
        // TODO: use a more apprioprate path (check installlation location)
        private readonly string _TemplatePath = @"..\..\PrimalEditor\ProjectTemplates\"; // do not hard code this (it may be else where)

        private string _ProjectName = "NewProject"; // name of the project
        public string ProjectName
        {
            get => _ProjectName;
            set // check if the name is a different value, set a name for it (bound property)
            {
                if(_ProjectName != value)
                {
                    _ProjectName = value;
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _projectpath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\PrimalProjects\"; // name of the project
        public string ProjectPath
        {
            get => _projectpath;
            set // check if the name is a different value, set a name for it (bound property)
            {
                if (_projectpath != value)
                {
                    _projectpath = value;
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private bool _isValid;
        public bool isValid
        {
            get => _isValid;
            set
            {
                if(_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(isValid));
                }
            }
        }

        private string _errorMsg;
        public string errorMsg
        {
            get => _errorMsg;
            set
            {
                if (_errorMsg != value)
                {
                    _errorMsg = value;
                    OnPropertyChanged(nameof(errorMsg));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _ProjectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> projectTemplates { get; }
        
        private bool ValidateProjectPath()
        {
            var path = ProjectPath; // store the path
            if (!Path.EndsInDirectorySeparator(path)) path += @"\"; // if missing slash add it
            path += $@"{ProjectName}\"; // add the project name to the path (full path)

            isValid = false;
            if (string.IsNullOrWhiteSpace(ProjectName.Trim()))
            {
                errorMsg = "Type in a project name.";
            }
            else if(ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                errorMsg = "Invalid character(s) used in project name.";
            }
            else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
            {
                errorMsg = "Select a valid project path.";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                errorMsg = "Invalid character(s) used in project path.";
            }
            else if(Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
            {
                errorMsg = "Selected project folder already exists and is not empty.";
            }
            else
            {
                errorMsg = "";
                isValid = true;
            }
            return isValid;
        }

        public string CreateProject(ProjectTemplate template)
        {
            ValidateProjectPath();
            if(!isValid)
            {
                return string.Empty;
            }
            // correctr if wrong.
         
            if (!Path.EndsInDirectorySeparator(ProjectPath)) ProjectPath += @"\"; // if missing slash add it
            var path = $@"{ProjectPath}{ProjectName}\"; // add the project name to the path (full path)

            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                foreach(var folder in template.Folders)
                {
                    // create subfolders for game
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), folder)));
                }
                var dirInfo = new DirectoryInfo(path + @".Primal\");
                dirInfo.Attributes |= FileAttributes.Hidden;
                File.Copy(template.IconFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Icon.png")));
                File.Copy(template.ScreenshotFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Screenshot.png")));

                // used to set up files
                // var project = new Project(ProjectName, path);
                // Serializer.ToFile(project, path + $"{ProjectName}" + Project.Extension);

                var projectXml = File.ReadAllText(template.ProjectFilePath); // read in file
                projectXml = string.Format(projectXml, ProjectName, ProjectPath);
                var projectPath = Path.GetFullPath(Path.Combine(path, $"{ProjectName}{Project.Extension}"));
                File.WriteAllText(projectPath, projectXml);
                return path;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log(MessageType.Error, $"Failed to create {ProjectName}");
                throw;
            }
        }

        public NewProject() // constructor
        {
            projectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_ProjectTemplates);

            try
            {
                var templatesFiles = Directory.GetFiles(_TemplatePath, "Template.xml", SearchOption.AllDirectories);
                Debug.Assert(templatesFiles.Any()); // need at least 1 template to create new project
                foreach (var file in templatesFiles)
                {
                    /* for writing to template file at the start. Do not use after first use.
                    var template = new ProjectTemplate()
                    {
                        ProjectType = "Empty Project",
                        ProjectFile = "Project.primal", // name will be changed later on from project
                        Folders = new List<string> { ".Primal", "Content", "GameCode" }
                    };
                    
                    Serializer.ToFile(template, file); // write project template to file (including serialization, using data contracts)
                    */

                    /* read in */
                    var template = Serializer.FromFile<ProjectTemplate>(file);
                    // fill in fields
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Icon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ScreenshotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), "Screenshot.png"));
                    template.Screenshot = File.ReadAllBytes(template.ScreenshotFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(file), template.ProjectFile));
                   
                    _ProjectTemplates.Add(template);

                    ValidateProjectPath();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Logger.Log(MessageType.Error, $"Failed to read project templates");
                throw;
            }
        }
    }
}
