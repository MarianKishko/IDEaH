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
    
    public static class FileExtensions
    {
        public static readonly FileExtension Text = new FileExtension(".txt", "Text");
        
        public static readonly FileExtension C = new FileExtension(".c", "C");
        public static readonly FileExtension Python = new FileExtension(".py", "Python");
        public static readonly FileExtension Java = new FileExtension(".java", "Java");
        public static readonly FileExtension JS = new FileExtension(".js", "JS");

        // Their names are backwards to solve the bug that makes TypeOfFile Lable show them in reverse (the symbols are the first characters, then C is the last)
        public static readonly FileExtension CSharp = new FileExtension(".cs", "#C");
        public static readonly FileExtension Cpp = new FileExtension(".cpp", "++C");
    }

    public partial class MainWindow : Window
    {
        private FileExtension[] _extensions =
        [
            FileExtensions.Text,
            FileExtensions.CSharp,
            FileExtensions.Cpp,
            FileExtensions.C,
            FileExtensions.Python,
            FileExtensions.Java,
            FileExtensions.JS
            ];

        private const string EMPTY = "";

        private List<FileTab> _fileTabs = new List<FileTab>();
        private FileTab _chosenTab;

        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyUp += SaveChanges;
        }

        public void RemoveFileTab(FileTab tab)
        {
            tab.UpdateMainWindow(this);
            _fileTabs.Remove(tab);

            if (_fileTabs.Count > 1) 
            {
                _chosenTab = _fileTabs.Last();
                _chosenTab.SetChosen();
            }
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
            //ChangeTypeOfFileLableText(FileExtensions.Text.Name);

            TurnTextEditor(true);
        }

        private void AddFileTab(string filePath, string content)
        {
            FileTab newFile = new FileTab(filePath, content, GetOnlyFileName(filePath));
            newFile.UpdateMainWindow(this);

            _fileTabs.Add(newFile);
            Tabs.Children.Add(newFile);

            newFile.SetChosen();

            //ChangeTypeOfFileLableText(GetFileExtensionName());

            TurnTextEditor(true);
        }

        public string GetOnlyFileName(string filePath)
        {
            return System.IO.Path.GetFileName(filePath);
        }

        public void ChooseNewTab(FileTab newChosen)
        {
            _chosenTab = newChosen;
            ChangeText(newChosen.GetFileText());

            if (newChosen.FilePath == "")
                ChangeTypeOfFileLableText(FileExtensions.Text.Name);
            else
                ChangeTypeOfFileLableText(GetFileExtensionName());
        }

        private void UpdateChosensData(string filePath) 
        {
            _chosenTab.SetFilePath(filePath);
            _chosenTab.SetNewName(GetOnlyFileName(filePath));
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
                if (_chosenTab.IsNew())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                    //saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        File.WriteAllText(saveFileDialog.FileName, textEditor.Text);

                        ChangeTypeOfFileLableText(GetFileExtensionName());

                        UpdateChosensData(saveFileDialog.FileName);
                    }
                }
                else
                    File.WriteAllText(_chosenTab.FilePath, textEditor.Text);
            }
        }

        private void SaveChanges(object sender, KeyEventArgs e)
        {
            if (!IsControlKey(e.Key))
            {
                _chosenTab.GetChanges(textEditor.Text);
            }
        }

        private bool IsControlKey(Key key)
        {
            return key == Key.Tab ||
                      key == Key.Escape ||
                      key == Key.LeftShift || key == Key.RightShift ||
                      key == Key.LeftCtrl || key == Key.RightCtrl ||
                      key == Key.LeftAlt || key == Key.RightAlt ||
                      (key >= Key.F1 && key <= Key.F24);
        }

        public void ChangeText(string content)
        {
            textEditor.Text = content;
        }

        private void Undo(object sender, RoutedEventArgs e)
        {
            if (_chosenTab.IsMinLimitAchived())
                _chosenTab.Undo();
        }

        private void Redo(object sender, RoutedEventArgs e)
        {
            if (_chosenTab.IsMaxLimitAchived())
                _chosenTab.Redo();
        }

        private string GetFileExtensionName()
        {
            string extension = System.IO.Path.GetExtension(_chosenTab.FilePath);
            string extensionName = "";

            for (int i = 0; i < _extensions.Length; i++) 
                if (extension == _extensions[i].Extension)
                {
                    extensionName = _extensions[i].Name;
                    break;
                }

            return extensionName;
        }

        private void ChangeTypeOfFileLableText(string newType)
        {
            TypeOfFile.Content = newType;
        }

        private int BinarySearch(int[] array, int target, int low, int high)
        {
            while (low <= high)
            {
                int mid = low + (high - low) / 2;

                if (array[mid] == target)
                    return mid;

                if (array[mid] < target)
                    low = mid + 1;
                else
                    high = mid - 1;
            }
            return -1;
        }

        private void textEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int caretPosition = textEditor.SelectionStart;
            int x = textEditor.GetLineIndexFromCharacterIndex(caretPosition);
            int y = caretPosition - textEditor.GetLineIndexFromCharacterIndex(x);

            CountOfLns.Content = "Ln: " + x;
            CountOfChs.Content = "Ch: " + y;
        }
    }

    
}