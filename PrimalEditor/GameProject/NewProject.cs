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
        private readonly string _TemplatePath = @"..\..\PrimalEditor\ProjectTemplates"; // do not hard code this (it may be else where)

        private string _ProjectName = "NewProject"; // name of the project
        public string ProjectName
        {
            get => _ProjectName;
            set // check if the name is a different value, set a name for it (bound property)
            {
                if(_ProjectName != value)
                {
                    _ProjectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        private string _projectpath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\PrimalProject\"; // name of the project
        public string ProjectPath
        {
            get => _projectpath;
            set // check if the name is a different value, set a name for it (bound property)
            {
                if (_projectpath != value)
                {
                    _projectpath = value;
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _ProjectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> projectTemplates { get; }
        
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
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // TODO: log error in output file.
            }
        }
    }
}
