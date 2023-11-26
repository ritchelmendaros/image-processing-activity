using System.Diagnostics;
using System.Windows.Forms;

namespace ImageProcessingAct
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        Bitmap processed;
        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) { }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFileOpened();
            loaded = new Bitmap(openFileDialog1.FileName);
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
            label2.Text = "Copy";
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFileOpened();
            loaded = new Bitmap(openFileDialog1.FileName);
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);
            int grey;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    grey = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    processed.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            }
            pictureBox2.Image = processed;
            label2.Text = "Greyscale";
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFileOpened();
            loaded = new Bitmap(openFileDialog1.FileName);
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }
            pictureBox2.Image = processed;
            label2.Text = "Invert";
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFileOpened();
            loaded = new Bitmap(openFileDialog1.FileName);
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    int tr = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int tg = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int tb = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    tr = Math.Min(255, Math.Max(0, tr));
                    tg = Math.Min(255, Math.Max(0, tg));
                    tb = Math.Min(255, Math.Max(0, tb));

                    processed.SetPixel(x, y, Color.FromArgb(tr, tg, tb));
                }
            }
            pictureBox2.Image = processed;
            label2.Text = "Sepia";
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFileOpened();
            loaded = new Bitmap(openFileDialog1.FileName);
            int[] histogram = new int[256];

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int grey = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    histogram[grey]++;
                }
            }
            int maxCount = histogram.Max();
            List<int> normalizedHistogram = histogram.Select(value => (int)(value * 100.0 / maxCount)).ToList();

            DrawHistogram(normalizedHistogram, pictureBox2);
        }
        private void DrawHistogram(List<int> histogram, PictureBox pictureBox)
        {
            Bitmap histogramBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(histogramBitmap))
            {
                g.Clear(Color.White);

                int barWidth = pictureBox.Width / histogram.Count;

                for (int i = 0; i < histogram.Count; i++)
                {
                    int barHeight = histogram[i] * 3;
                    g.FillRectangle(Brushes.Black, i * barWidth, pictureBox.Height - barHeight, barWidth, barHeight);
                }
            }

            pictureBox.Image = histogramBitmap;
            label2.Text = "Histogram";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckFileOpened();
            if (processed != null)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Bitmap Image|*.bmp|JPEG Image|*.jpg|PNG Image|*.png";
                saveDialog.Title = "Save Processed Image";
                saveDialog.ShowDialog();

                if (saveDialog.FileName != "")
                {
                    processed.Save(saveDialog.FileName);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CheckFileOpened()
        {
            if (loaded == null)
            {
                MessageBox.Show("Please load an image first.", "Warning!", MessageBoxButtons.OK);
            }
        }
    }
}