using System.Windows.Forms;

namespace HW9_TheMazeGame
{
    public partial class MainForm : Form
    {
        private readonly string mapPath = "Maps/";
        private readonly string fileExtension = ".txt";

        private List<string> maps;

        private int[,] map2d;
        public MainForm()
        {
            maps = new List<string>();
            LoadMaps();

            Load += (s, e) =>
            {
                startButton.Enabled = false;
                InitMapSelection();
                mapSelectionComboBox.SelectedIndexChanged += MapSelectionComboBox_SelectedIndexChanged;
                startButton.Click += (s, e) =>
                {
                    GameForm gameForm = new GameForm(map2d);
                    gameForm.ShowDialog(this);
                };
            };

            InitializeComponent();
        }

        private void MapSelectionComboBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string selectedMapName = mapSelectionComboBox.SelectedItem.ToString();
            string filePath = Path.Combine(mapPath, selectedMapName + fileExtension);
            string[] lines = File.ReadAllLines(filePath);

            map2d = new int[lines.Length, lines[0].Length];

            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        map2d[i, j] = lines[i][j] == '█' ? 1 : 0;
                    }
                }
                DisplayMap();
                startButton.Enabled = true;
                startButton.Text = "START YOUR MAZE RUN";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error file read: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayMap()
        {
            int rows = map2d.GetLength(0);
            int cols = map2d.GetLength(1);
            int cellWidth = mapPictureBox.Width / cols;
            int cellHeight = mapPictureBox.Height / rows;

            Bitmap mapImage = new Bitmap(mapPictureBox.Width, mapPictureBox.Height);

            using (Graphics g = Graphics.FromImage(mapImage))
            {
                g.Clear(Color.MediumPurple);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Brush brush = map2d[i, j] == 1 ? Brushes.Gold : Brushes.MediumPurple;
                        g.FillRectangle(brush, j * cellWidth, i * cellHeight, cellWidth, cellHeight);
                    }
                }
            }
            mapPictureBox.Image = mapImage;
        }

        private void InitMapSelection()
        {
            foreach (string map in maps)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(map);
                mapSelectionComboBox.Items.Add(fileNameWithoutExtension);
            }
        }

        private void LoadMaps()
        {
            string[] files = Directory.GetFiles(mapPath, "*.txt");
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                maps.Add(fileName);
            }
        }
    }
}
