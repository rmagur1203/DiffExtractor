using System.Diagnostics;
using System.IO.Hashing;

namespace DiffExtractor
{
    public partial class Form1 : Form
    {
        private long fileCount = -1;
        private long checkedFileCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private readonly Dictionary<string, TreeNode> treePair = new();

        private string originPath => originalFile.Text;

        private string targetPath => targetFile.Text;

        private void selectOriginal_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                originalFile.Text = dialog.SelectedPath;
            }
        }

        private void selectTarget_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                targetFile.Text = dialog.SelectedPath;
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            if (originPath == "" || targetPath == "")
            {
                _ = MessageBox.Show("������ ��� ������ �������ּ���.");
                return;
            }

            treePair.Clear();
            _ = treeView1.Nodes.Add(createTree(new DirectoryInfo(targetPath)));
            treeView1.Nodes[0]?.Expand();

            fileCount = 0;
            checkedFileCount = 0;


            _ = Task.Run(async () =>
            {
                fileCount = Math.Max(
                    Directory.GetFiles(originPath, "*.*", SearchOption.AllDirectories).Length,
                    Directory.GetFiles(targetPath, "*.*", SearchOption.AllDirectories).Length
                );
                List<string> diffFiles = await FindDiffFiles(".");
                diffFiles = diffFiles.Select(x => Path.GetRelativePath(targetPath, x)).ToList();

                _ = BeginInvoke(() =>
                {
                    using FolderBrowserDialog dialog = new();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string file in diffFiles)
                        {
                            string target = Path.Combine(dialog.SelectedPath, file);
                            string origin = Path.Combine(targetPath, file);
                            _ = Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                            File.Copy(origin, target, true);
                        }

                        _ = Process.Start("explorer.exe", dialog.SelectedPath);

                        fileCount = -1;
                    }
                });
            });
        }

        private TreeNode createTree(DirectoryInfo directory)
        {
            TreeNode newNode = new(directory.Name);
            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                _ = newNode.Nodes.Add(createTree(subDirectory));
            }
            foreach (FileInfo file in directory.GetFiles())
            {
                TreeNode fileNode = new(file.Name)
                {
                    Tag = file.FullName
                };
                treePair.Add(file.FullName, fileNode);
                _ = newNode.Nodes.Add(fileNode);
            }
            treePair.Add(directory.FullName, newNode);
            return newNode;
        }

        private Task<List<string>> FindDiffFiles(string relativePath)
        {
            return Task.Run(() =>
            {
                List<string> diffFiles = new();
                DirectoryInfo originDir = new(Path.Combine(originPath, relativePath));
                DirectoryInfo targetDir = new(Path.Combine(targetPath, relativePath));

                List<Task> tasks = new();

                if (!originDir.Exists)
                {
                    setIsDiff(targetDir.FullName);
                    IEnumerable<string> files = targetDir.GetFiles().Select(x => x.FullName);
                    foreach (string file in files)
                    {
                        setIsDiff(file);
                        diffFiles.Add(file);
                        checkedFileCount++;
                    }
                    diffFiles.AddRange(targetDir.GetDirectories().SelectMany(x => FindDiffFiles(Path.Combine(relativePath, x.Name)).Result));
                    return diffFiles;
                }


                foreach (DirectoryInfo subDir in targetDir.GetDirectories())
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        diffFiles.AddRange(await FindDiffFiles(Path.Combine(relativePath, subDir.Name)));
                    }));
                }

                foreach (FileInfo targetFile in targetDir.GetFiles())
                {
                    FileInfo? originFile = originDir.GetFiles().Where(f => f.Name == targetFile.Name).FirstOrDefault();
                    Console.WriteLine((originFile?.Name ?? "Not Found") + " : " + targetFile.Name);
                    checkedFileCount++;
                    if (originFile == null || originFile.Length != targetFile.Length)
                    {
                        setIsDiff(targetFile.FullName);
                        diffFiles.Add(targetFile.FullName);
                    }
                    else
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            if (!await IsSameFile(originFile, targetFile))
                            {
                                setIsDiff(targetFile.FullName);
                                diffFiles.Add(targetFile.FullName);
                            }
                            else
                            {
                                setIsSame(targetFile.FullName);
                            }
                        }));
                    }

                }

                Task.WaitAll(tasks.ToArray());

                return diffFiles;
            });
        }

        private void setIsDiff(string targetFilePath)
        {
            treePair[targetFilePath].BackColor = Color.Red;
            updateNode(treePair[targetFilePath].Parent, Color.Red);
        }

        private void setIsSame(string targetFilePath)
        {
            treePair[targetFilePath].BackColor = Color.LightGreen;
            updateNode(treePair[targetFilePath].Parent, Color.LightGreen);
        }

        private void updateNode(TreeNode node, Color color)
        {
            if (node.BackColor == default)
            {
                node.BackColor = color;
                if (node.Parent != null)
                {
                    updateNode(node.Parent, color);
                }
            }
            else if (node.BackColor != color)
            {
                node.BackColor = Color.Orange;
                if (node.Parent != null)
                {
                    updateNode(node.Parent, color);
                }
            }
        }

        private static async Task<bool> IsSameFile(FileInfo originFile, FileInfo targetFile)
        {
            if (originFile.Length != targetFile.Length)
            {
                return false;
            }

            Crc32 originHash = new();
            Task originHashTask = originHash.AppendAsync(originFile.OpenRead());

            Crc32 targetHash = new();
            Task targetHashTask = targetHash.AppendAsync(targetFile.OpenRead());

            await originHashTask;
            await targetHashTask;

            return originHash.GetCurrentHash().SequenceEqual(targetHash.GetCurrentHash());
        }

        private void updateProgressTimer_Tick(object sender, EventArgs e)
        {
            if (fileCount == -1)
            {
                progressLabel.Text = "waiting...";
                return;
            }
            if (fileCount == 0)
            {
                progressLabel.Text = "calculating files...";
                return;
            }
            progressLabel.Text = $"{checkedFileCount}/{fileCount}";
        }
    }
}