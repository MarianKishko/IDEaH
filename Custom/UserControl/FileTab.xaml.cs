using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IDEaHTest1.Custom.UserControl
{
    /// <summary>
    /// Interaktionslogik für FileTab.xaml
    /// </summary>
    
    public partial class FileTab
    {
        private MainWindow _main;
        private string _filePath = "";
        
        private List<string> _changes = new List<string>();
       
        private string _fileName; 

        public string FilePath => _filePath;

        public FileTab()
        {
            InitializeComponent();

            _filePath = "";
            _changes.Add("");
            _fileName = "New File";
            SetTabsName();
        }

        public FileTab(string filePath, string content, string fileName)
        {
            InitializeComponent();

            _filePath = filePath;
            _changes.Add(content);
            _fileName = fileName;
            SetTabsName();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            _main.RemoveFileTab(this);
            _main.DisableTextEditor();
            _main.Tabs.Children.Remove(this);
            
            e.Handled = true;
        }


        public void UpdateMainWindow(MainWindow newVersion)
        {
            _main = newVersion;
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            SetChosen();
        }

        public void SetFilePath(string filePath)
        {
            _filePath = filePath;
        }

        private void SetTabsName()
        {
            Name.Text = _fileName;
        }

        public string GetFileText()
        {
            return _changes.Last();
        }

        public void GetChanges(string newChange)
        {
            _changes.Add(newChange);
        }

        public void SetChosen()
        {
            _main.ChooseNewTab(this);
        }
    }
}
