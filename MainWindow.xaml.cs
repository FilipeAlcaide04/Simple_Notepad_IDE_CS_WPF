using BlocoDeNotasWPF;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace BlocoDeNotasWPF
{
    public partial class MainWindow : Window
    {
        private double originalWidth;
        private double originalHeight;
        private double originalLeft;
        private double originalTop;

        public MainWindow()
        {
            InitializeComponent();

            //Used to test the location of the resources
            //var resourceNames = typeof(MainWindow).Assembly.GetManifestResourceNames();
            //MessageBox.Show(string.Join("\n", resourceNames));
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true // Permite a seleção de múltiplos ficheiros
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    string fileContent = File.ReadAllText(filePath);

                    TabItem newTab = new TabItem
                    {
                        Header = System.IO.Path.GetFileName(filePath) + "  ",
                        Tag = filePath
                    };

                    Grid grid = new Grid();
                    TextEditor textEditor = new TextEditor
                    {
                        Text = fileContent,
                        ShowLineNumbers = true,
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E2E2E")),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"))
                    };

                    // Set custom syntax highlighting for C/C++
                    if (Path.GetExtension(filePath).Equals(".c", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(filePath).Equals(".cpp", StringComparison.OrdinalIgnoreCase) ||
                        Path.GetExtension(filePath).Equals(".cs", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            using (Stream stream = typeof(MainWindow).Assembly.GetManifestResourceStream("Project_Filipe_Alcaide.CustomCpp-Mode.xshd"))
                            using (XmlReader reader = XmlReader.Create(stream))
                            {
                                textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Erro ao carregar o arquivo de realce de sintaxe: {ex.Message}");
                        }
                    }

                    grid.Children.Add(textEditor);
                    newTab.Content = grid;

                    tabControl.Items.Add(newTab);
                }

                // Seleciona a primeira aba ou outra lógica para definir a aba selecionada
                if (tabControl.Items.Count > 0)
                {
                    tabControl.SelectedIndex = 0;
                }
            }
        }

        private void OpenFileExplorer(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe");
        }

        // Slower Version of CreatFile


        //private void CreateNewFile_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog
        //    {
        //        Filter = "Text files (*.txt)|*.txt|C files (*.c)|*.c|C++ files (*.cpp)|*.cpp|All files (*.*)|*.*"
        //    };

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        string filePath = saveFileDialog.FileName;

        //        // Cria e guarda o novo ficheiro
        //        File.WriteAllText(filePath, string.Empty);

        //        TabItem newTab = new TabItem
        //        {
        //            Header = System.IO.Path.GetFileName(filePath) + "  ",
        //            Tag = filePath
        //        };

        //        Grid grid = new Grid();
        //        TextEditor textEditor = new TextEditor
        //        {
        //            Text = string.Empty,
        //            ShowLineNumbers = true,
        //            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E2E2E")),
        //            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"))
        //        };

        //        if (Path.GetExtension(filePath).Equals(".c", StringComparison.OrdinalIgnoreCase) ||
        //            Path.GetExtension(filePath).Equals(".cpp", StringComparison.OrdinalIgnoreCase) ||
        //            Path.GetExtension(filePath).Equals(".cs", StringComparison.OrdinalIgnoreCase))
        //        {
        //            try
        //            {
        //                using (Stream stream = typeof(MainWindow).Assembly.GetManifestResourceStream("Project_Filipe_Alcaide.CustomCpp-Mode.xshd"))
        //                using (XmlReader reader = XmlReader.Create(stream))
        //                {
        //                    textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Erro ao carregar o arquivo de realce de sintaxe: {ex.Message}");
        //            }
        //        }

        //        grid.Children.Add(textEditor);
        //        newTab.Content = grid;
        //        tabControl.Items.Add(newTab);
        //        tabControl.SelectedItem = newTab;
        //    }
        //}

        private void CreateNewFile_Click(object sender, RoutedEventArgs e)
        {
            TabItem newTab = new TabItem
            {
                Header = "New File  ",
                Tag = "New File  "
            };

            Grid grid = new Grid();
            TextEditor textEditor = new TextEditor
            {
                Text = "",
                ShowLineNumbers = true,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E2E2E")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("white"))
            };

            try
            {
                using (Stream stream = typeof(MainWindow).Assembly.GetManifestResourceStream("Project_Filipe_Alcaide.CustomCpp-Mode.xshd"))
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o arquivo de realce de sintaxe: {ex.Message}");
            }

            grid.Children.Add(textEditor);
            newTab.Content = grid;

            tabControl.Items.Add(newTab);


            // Seleciona a primeira aba ou outra lógica para definir a aba selecionada
            if (tabControl.Items.Count > 0)
            {
                tabControl.SelectedIndex = 0;
            }

        }



        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab &&
                selectedTab.Content is Grid grid &&
                grid.Children[0] is TextEditor textEditor)
            {
                string filePath = selectedTab.Tag as string;
                if (filePath == null || !File.Exists(filePath))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        filePath = saveFileDialog.FileName;
                        selectedTab.Tag = filePath;
                        selectedTab.Header = Path.GetFileName(filePath);
                    }
                }

                if (filePath != null)
                {
                    string fileContent = textEditor.Text;
                    File.WriteAllText(filePath, fileContent);
                }
            }
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab &&
                selectedTab.Content is Grid grid &&
                grid.Children[0] is TextEditor textEditor)
            {
                // Initialize SaveFileDialog and set filters
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt|C files (*.c)|*.c|C++ files (*.cpp)|*.cpp|C# files (*.cs)|*.cs",
                    FilterIndex = 1, // Default to C files
                    DefaultExt = ".txt" // Default file extension
                };

                // Show SaveFileDialog and check if user clicked Save
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    selectedTab.Tag = filePath;
                    selectedTab.Header = Path.GetFileName(filePath) + "  ";

                    // Ensure that the file has one of the supported extensions
                    string fileExtension = Path.GetExtension(filePath).ToLower();
                    if (fileExtension != ".c" && fileExtension != ".cpp" && fileExtension != ".cs" && fileExtension != ".txt")
                    {
                        MessageBox.Show("Unsupported file type. Please choose a valid file extension.");
                        return;
                    }

                    // Write content to the file
                    string fileContent = textEditor.Text;
                    File.WriteAllText(filePath, fileContent);
                }
            }
        }


        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab &&
                selectedTab.Content is Grid grid &&
                grid.Children[0] is TextEditor textEditor)
            {
                textEditor.Undo();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Verifica se a janela já está maximizada
            if (this.Width == SystemParameters.WorkArea.Width && this.Height == SystemParameters.WorkArea.Height)
            {
                // Restaura as dimensões e posição original
                this.WindowState = WindowState.Normal;
                this.Width = originalWidth;
                this.Height = originalHeight;
                this.Left = originalLeft;
                this.Top = originalTop;
            }
            else
            {
                // Salva as dimensões e posição original
                originalWidth = this.Width;
                originalHeight = this.Height;
                originalLeft = this.Left;
                originalTop = this.Top;

                // Ajusta o tamanho e posição para ocupar a área de trabalho excluindo a barra de tarefas
                this.WindowState = WindowState.Normal;
                this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;
                this.Left = SystemParameters.WorkArea.Left;
                this.Top = SystemParameters.WorkArea.Top;
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab &&
                selectedTab.Content is Grid grid &&
                grid.Children[0] is TextEditor textEditor)
            {
                textEditor.Redo();
            }
        }

        private void cima_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in tabControl.Items)
            {
                if (item is TabItem tabItem &&
                    tabItem.Content is Grid grid &&
                    grid.Children[0] is TextEditor textEditor)
                {
                    if (int.TryParse(tamanhot.Text, out int x) && x < 50)
                    {
                        x += 1;
                        tamanhot.Text = x.ToString();
                        textEditor.FontSize += 1;
                    }
                }
            }
        }

        private void baixo_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in tabControl.Items)
            {
                if (item is TabItem tabItem &&
                    tabItem.Content is Grid grid &&
                    grid.Children[0] is TextEditor textEditor)
                {
                    if (int.TryParse(tamanhot.Text, out int x) && x > 10)
                    {
                        x -= 1;
                        tamanhot.Text = x.ToString();
                        textEditor.FontSize -= 1;
                    }
                }
            }
        }

       private void Execute_File(object sender, RoutedEventArgs e)
{
    // Abre o OpenFileDialog para selecionar o ficheiro a ser executado
    OpenFileDialog openFileDialog = new OpenFileDialog
    {
        Filter = "Ficheiros C e C++ (*.c;*.cpp)|*.c;*.cpp|Todos os ficheiros (*.*)|*.*"
    };

    if (openFileDialog.ShowDialog() == true)
    {
        // Obtém o dia da semana atual para identificar o eze criado
        DayOfWeek wk = DateTime.Today.DayOfWeek;
        string dia = wk.ToString();

        // Obtém o caminho do ficheiro e a sua extensão
        string filePath = openFileDialog.FileName;
        string fileName = Path.GetFileName(filePath);
        string fileDirectory = Path.GetDirectoryName(filePath);
        string fileExtension = Path.GetExtension(filePath);

        // Verifica se o ficheiro selecionado tem a extensão correta
        if (fileExtension != ".c" && fileExtension != ".cpp")
        {
            MessageBox.Show("Por favor, selecione um ficheiro C ou C++ válido.");
            return;
        }

        // Constrói os caminhos completos para o executável de saída
        string outputExe = Path.Combine(fileDirectory, $"{dia}.exe");

        // Prepara os comandos a serem executados
        string compileCommand = fileExtension == ".c"
            ? $"gcc -o \"{outputExe}\" \"{filePath}\""
            : $"g++ -o \"{outputExe}\" \"{filePath}\"";
        string executeCommand = $"cmd.exe /K \"\"{outputExe}\" && pause\"";

        // Cria as informações de início do processo para compilação
        ProcessStartInfo compileStartInfo = new ProcessStartInfo("cmd.exe", "/C " + compileCommand)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            // Inicia o processo de compilação
            using (Process compileProcess = Process.Start(compileStartInfo))
            {
                // Lê a saída e o erro do processo
                string output = compileProcess.StandardOutput.ReadToEnd();
                string error = compileProcess.StandardError.ReadToEnd();

                // Verifica se a compilação foi bem sucedida
                if (File.Exists(outputExe))
                {
                    // Cria as informações de início do processo para execução
                    ProcessStartInfo executeStartInfo = new ProcessStartInfo("cmd.exe", "/K " + executeCommand)
                    {
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };

                    // Inicia o processo de execução
                    using (Process executeProcess = Process.Start(executeStartInfo))
                    {
                        executeProcess.WaitForExit();
                    }

                    // Apaga o ficheiro executável após a execução
                    try
                    {
                        File.Delete(outputExe);
                        // Usado para testes
                        // MessageBox.Show($"Executável {outputExe} Apagado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred due to the process not terminating properly: " + ex.Message);
                        File.Delete(outputExe);
                    }
                }
                else
                {
                    // Se a compilação falhar, mostra a mensagem de erro
                    MessageBox.Show("Compilation Failed Error:\n" + error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }
} 

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            if (sender is Button closeButton)
            {
                var tabItem = closeButton.DataContext as TabItem;
                if (tabItem != null)
                {
                    tabControl.Items.Remove(tabItem);
                }
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void OpenTerminal(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("cmd.exe");
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show a message to the user)
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
