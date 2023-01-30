using System.Diagnostics;
using System.IO.Hashing;

namespace DiffExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<string, TreeNode> treePair = new Dictionary<string, TreeNode>();

        private string originPath
        {
            get
            {
                return originalFile.Text;
            }
        }

        private string targetPath
        {
            get
            {
                return targetFile.Text;
            }
        }

        private void selectOriginal_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    originalFile.Text = dialog.SelectedPath;
                }
            }
        }

        private void selectTarget_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    targetFile.Text = dialog.SelectedPath;
                }
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            if (originPath == "" || targetPath == "")
            {
                MessageBox.Show("원본과 대상 폴더를 선택해주세요.");
                return;
            }

            treePair.Clear();
            treeView1.Nodes.Add(createTree(new DirectoryInfo(targetPath)));
            treeView1.Nodes[0]?.Expand();

            Task.Run(async () =>
            {
                List<string> diffFiles = await FindDiffFiles(".");
                diffFiles = diffFiles.Select(x => Path.GetRelativePath(targetPath, x)).ToList();
                this.BeginInvoke(() =>
                {
                    using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                    {
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            foreach (string file in diffFiles)
                            {
                                string target = Path.Combine(dialog.SelectedPath, file);
                                string origin = Path.Combine(targetPath, file);
                                Directory.CreateDirectory(Path.GetDirectoryName(target));
                                File.Copy(origin, target, true);
                            }

                            Process.Start("explorer.exe", dialog.SelectedPath);
                        }
                    }
                });
            });
        }

        private TreeNode createTree(DirectoryInfo directory)
        {
            TreeNode newNode = new TreeNode(directory.Name);
            foreach (var subDirectory in directory.GetDirectories())
            {
                newNode.Nodes.Add(createTree(subDirectory));
            }
            foreach (var file in directory.GetFiles())
            {
                TreeNode fileNode = new TreeNode(file.Name);
                fileNode.Tag = file.FullName;
                treePair.Add(file.FullName, fileNode);
                newNode.Nodes.Add(fileNode);
            }
            return newNode;
        }

        private Task<List<string>> FindDiffFiles(string relativePath)
        {
            return Task.Run(() =>
            {
                List<string> diffFiles = new List<string>();
                DirectoryInfo originDir = new DirectoryInfo(Path.Combine(originPath, relativePath));
                DirectoryInfo targetDir = new DirectoryInfo(Path.Combine(targetPath, relativePath));

                List<Task> tasks = new();

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
                            } else
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
            updateNode(treePair[targetFilePath].Parent, Color.Orange);
        }

        private void setIsSame(string targetFilePath)
        {
            treePair[targetFilePath].BackColor = Color.Green;
        }

        private void updateNode(TreeNode node, Color color)
        {
            node.BackColor = color;
            if (node.Parent != null)
                updateNode(node.Parent, color);
        }

        private static async Task<bool> IsSameFile(FileInfo originFile, FileInfo targetFile)
        {
            if (originFile.Length != targetFile.Length)
            {
                return false;
            }

            Crc32 originHash = new Crc32();
            Task originHashTask = originHash.AppendAsync(originFile.OpenRead());

            Crc32 targetHash = new Crc32();
            Task targetHashTask = targetHash.AppendAsync(targetFile.OpenRead());

            await originHashTask;
            await targetHashTask;

            return originHash.GetCurrentHash().SequenceEqual(targetHash.GetCurrentHash());
        }
    }
}