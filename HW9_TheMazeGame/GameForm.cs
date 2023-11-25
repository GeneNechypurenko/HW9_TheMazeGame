using Microsoft.VisualBasic.Devices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace HW9_TheMazeGame
{
    public partial class GameForm : Form
    {
        private int[,] map2d;
        private int rows;
        private int cols;
        private int playerPosX;
        private int playerPosY;
        public GameForm(int[,] map2d)
        {
            this.map2d = map2d;
            rows = map2d.GetLength(0);
            cols = map2d.GetLength(1);
            playerPosX = rows / 2;
            playerPosY = cols / 2;

            Load += (s, e) =>
            {
                Paint += GameForm_Paint;
                KeyDown += GameForm_KeyDown;
                DoubleBuffered = true;
            };

            InitializeComponent();
        }
        private void GameForm_KeyDown(object? sender, KeyEventArgs e)
        {
            int x = playerPosX;
            int y = playerPosY;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        y = Math.Max(0, playerPosY - 1);
                        break;
                    }
                case Keys.Down:
                    {
                        y = Math.Min(rows - 1, playerPosY + 1);
                        break;
                    }
                case Keys.Left:
                    {
                        x = Math.Max(0, playerPosX - 1);
                        break;
                    }
                case Keys.Right:
                    {
                        x = Math.Min(rows - 1, playerPosX + 1);
                        break;
                    }
            }

            if (map2d[y, x] == 0)
            {
                playerPosX = x;
                playerPosY = y;
                Invalidate();
            }
            if (playerPosX == map2d.GetLength(1) - 1 || playerPosY == map2d.GetLength(0) - 1)
            {
                MessageBox.Show("Yeah boy!");
                this.Close();
            }
        }
        private void GameForm_Paint(object? sender, PaintEventArgs e)
        {
            ClientSize = new Size(rows * cols, rows * cols);

            Graphics g = e.Graphics;

            for (int i = 0; i < map2d.GetLength(0); ++i)
            {
                for (int j = 0; j < map2d.GetLength(1); j++)
                {
                    Brush brush = map2d[i, j] == 1 ? Brushes.Gold : Brushes.MediumPurple;
                    g.FillRectangle(brush, j * cols, i * rows, rows, cols);
                }
            }
            g.FillEllipse(Brushes.Crimson, playerPosX * rows, playerPosY * cols, rows, cols);
        }

    }
}
