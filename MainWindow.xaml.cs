using IDEaHTest1.Custom.UserControl;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IDEaHTest1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private const string EMPTY = "";

        private List<FileTab> _fileTabs = new List<FileTab>();
        private FileTab _chosenTab;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void RemoveFileTab(FileTab tab)
        {
            tab.UpdateMainWindow(this);
            _fileTabs.Remove(tab);
        }

        public void DisableTextEditor()
        {
            if (_fileTabs.Count == 0)
            {
                textEditor.Text = EMPTY;
                TurnTextEditor(false);

            }
        }

        private void AddFileTab()
        {
            FileTab newFile = new FileTab();
            newFile.UpdateMainWindow(this);

            _fileTabs.Add(newFile);
            Tabs.Children.Add(newFile);

            newFile.SetChosen();

            TurnTextEditor(true);
        }

        private void AddFileTab(string filePath, string content)
        {
            FileTab newFile = new FileTab(filePath, content, GetOnlyFileName(filePath));
            newFile.UpdateMainWindow(this);

            _fileTabs.Add(newFile);
            Tabs.Children.Add(newFile);

            newFile.SetChosen();

            TurnTextEditor(true);
        }

        public string GetOnlyFileName(string filePath)
        {
            return System.IO.Path.GetFileName(filePath);
        }

        public void ChooseNewTab(FileTab newChosen)
        {
            _chosenTab = newChosen;
            textEditor.Text = newChosen.GetFileText();
        }

        private void UpdateChosensData(string filePath) 
        {
            _chosenTab.SetFilePath(filePath);
            //_chosenTab.SetTabName(GetOnlyFileName(filePath));
        }

        private void TurnTextEditor(bool off)
        {
            textEditor.IsEnabled = off;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            AddFileTab();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true) 
            { 
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);

                AddFileTab(filePath, fileContent);
            }
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (textEditor.IsEnabled)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, textEditor.Text);

                    UpdateChosensData(saveFileDialog.FileName);
                }
            }
            
        }

        private void textEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            _chosenTab.GetChanges(textEditor.Text);
        }
    }
}