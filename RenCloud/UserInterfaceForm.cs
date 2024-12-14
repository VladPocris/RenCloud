using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RenCloud.Program;

namespace RenCloud
{
    public partial class UserInterfaceForm : Form
    {

        //Variables&Objects
        private bool isActive = false;
        private Corners applyCorners;
        private DragFunctionality dragFunctionality;
        string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "ffmpeg", "bin", "ffmpeg.exe");
        string tempDir = Path.Combine(Path.GetTempPath(), "VideoThumbnails");

        //ROUNDCORNERS LOGIC//
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            isActive = false;
            applyCorners.AttributesRoundCorners(this, isActive);
        }
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isActive = true;
            applyCorners.AttributesRoundCorners(this, isActive);
        }
        public UserInterfaceForm()
        {
            InitializeComponent();
            //APPLY ROUND CORNERS//
            applyCorners = new Corners();
            //APPLY DRAGGING FUNCTIONALITY//
            dragFunctionality = new DragFunctionality();
            AudioTrack.Scroll += (sender, e) =>
            {
                // Get the current scroll position of the AudioTrack
                int newPosition = AudioTrack.HorizontalScroll.Value;

                // Ensure the newPosition is within the valid range
                newPosition = Math.Max(AudioTrack.HorizontalScroll.Minimum, Math.Min(newPosition, AudioTrack.HorizontalScroll.Maximum));

                // Set the new position for VideoTrack and EditingRuller, ensuring the values are within valid range
                VideoTrack.HorizontalScroll.Value = Math.Max(VideoTrack.HorizontalScroll.Minimum, Math.Min(newPosition, VideoTrack.HorizontalScroll.Maximum));
                EditingRuller.HorizontalScroll.Value = Math.Max(EditingRuller.HorizontalScroll.Minimum, Math.Min(newPosition, EditingRuller.HorizontalScroll.Maximum));
            };
        }

        private void UserInterfaceForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            applyCorners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(panel2, this);
            dragFunctionality.AttachDraggingEvent(panel3, this);
            dragFunctionality.AttachDraggingEvent(pictureBox1, this);
            EditingRuller.Paint += EditingRuller_Paint;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axTimelineControl1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EditingRuller_Paint(object sender, PaintEventArgs e)
        {
            // Graphics object to draw on the panel (Ruller)
            Graphics g = e.Graphics;
            int panelWidth = EditingRuller.Width;
            float tickHeightMajor = 6;
            float tickHeightMinor = 3;
            int secondsInterval = 1;
            int pixelsPerSecond = 50;
            g.Clear(Color.Gray);

            for (int x = 0; x < panelWidth; x += pixelsPerSecond)
            {
                g.DrawLine(Pens.Black, x, 0, x, tickHeightMajor);
                int seconds = x / pixelsPerSecond;
                g.DrawString(seconds.ToString(), this.Font, Brushes.White, x + 2, tickHeightMajor);
                for (int i = 1; i < 5; i++)
                {
                    int minorX = x + (i * pixelsPerSecond / 5);
                    g.DrawLine(Pens.Black, minorX, 0, minorX, tickHeightMinor);
                }
            }
        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Gray);
            int stripHeight = 30;
            int spacing = 20;
            int frameWidth = 20;
            int frameHeight = 20;
            int frameSpacing = 5;
            int panelWidth = VideoPlaceholder.Width;
            int panelHeight = VideoPlaceholder.Height;
            for (int y = 0; y < panelHeight; y += stripHeight + spacing)
            {
                Rectangle stripRect = new Rectangle(0, y, panelWidth, stripHeight);
                using (Brush stripBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
                {
                    g.FillRectangle(stripBrush, stripRect);
                }
                for (int x = 0; x < panelWidth; x += frameWidth + frameSpacing)
                {
                    Color frameColor = Color.LightGreen;
                    Rectangle frameRect = new Rectangle(x, y + (stripHeight - frameHeight) / 2, frameWidth, frameHeight);
                    using (Brush frameBrush = new SolidBrush(frameColor))
                    {
                        g.FillRectangle(frameBrush, frameRect);
                    }
                    using (Pen framePen = new Pen(Color.Black, 1))
                    {
                        g.DrawRectangle(framePen, frameRect);
                    }
                }
                using (Pen outlinePen = new Pen(Color.DarkGray, 1))
                {
                    g.DrawRectangle(outlinePen, stripRect);
                }
            }
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Gray);
            int tapeWidth = 30;
            int spacing = 20;
            int barWidth = 5;
            int barSpacing = 3;
            int maxBarHeight = 15;
            Random rand = new Random();
            int panelWidth = AudioPlaceholder.Width;
            int panelHeight = AudioPlaceholder.Height;
            for (int y = 0; y < panelHeight; y += tapeWidth + spacing)
            {
                Rectangle tapeRect = new Rectangle(0, y, panelWidth, tapeWidth);
                using (Brush tapeBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
                {
                    g.FillRectangle(tapeBrush, tapeRect);
                }
                for (int x = 0; x < panelWidth; x += barWidth + barSpacing)
                {
                    int barHeight = rand.Next(5, maxBarHeight);
                    int barY = y + (tapeWidth / 2) - (barHeight / 2);
                    Rectangle barRect = new Rectangle(x, barY, barWidth, barHeight);
                    using (Brush barBrush = new SolidBrush(Color.LightGreen))
                    {
                        g.FillRectangle(barBrush, barRect);
                    }
                }
                using (Pen outlinePen = new Pen(Color.DarkGray, 1))
                {
                    g.DrawRectangle(outlinePen, tapeRect);
                }
            }
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files|*.mp4;*.avi;*.mov;*.wmv;*.mkv",
                Title = "Select a Video File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                Console.WriteLine(GetVideoDuration(filePath, ffmpegPath));
                AddVideoToTimeline(filePath);
            }
        }

        private void AddVideoToTimeline(string filePath)
        {

            EditingRuller.Paint += (sender, e) =>
            {
                EditingRuller.Width = (int)GetVideoDuration(filePath, ffmpegPath);
            };
            EditingRuller.Invalidate();

            VideoTrack.Paint += (sender, e) =>
            {
                VideoTrack.Width = (int)GetVideoDuration(filePath, ffmpegPath);
            };
            VideoTrack.Invalidate();

            AudioTrack.Paint += (sender, e) =>
            {
                AudioTrack.Width = (int)GetVideoDuration(filePath, ffmpegPath);
            };
            AudioTrack.Invalidate();
            //panel8.Paint += (sender, e) =>
            //{
            //    Graphics g = e.Graphics;
            //    g.Clear(Color.Gray);

            //    int x = 0;
            //    foreach (var thumbnail in videoThumbnails)
            //    {
            //        g.DrawImage(thumbnail, x, 0, 100, panel8.Height);
            //        x += 110;
            //    }
            //};
            //panel8.Invalidate();
            //panel9.Paint += (sender, e) =>
            //{
            //    Graphics g = e.Graphics;
            //    g.Clear(Color.Gray);

            //    int x = 0;
            //    foreach (var height in audioWaveform)
            //    {
            //        g.FillRectangle(Brushes.LightGreen, x, panel9.Height / 2 - height / 2, 2, height);
            //        x += 4;
            //    }
            //};
            //panel9.Invalidate();
            //panel8.Width += videoThumbnails.Count * 110;
            //panel9.Width += audioWaveform.Count * 4;
        }

        private void VideoTrack_Paint(object sender, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        private double GetVideoDuration(string videoFilePath, string ffmpegPath)
        {
            string ffmpegCommand = $"-i \"{videoFilePath}\"";
            Process ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCommand,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpegProcess.Start();
            string ffmpegOutput = ffmpegProcess.StandardError.ReadToEnd();
            ffmpegProcess.WaitForExit();

            string durationPattern = @"Duration:\s(?<hours>\d+):(?<minutes>\d+):(?<seconds>\d+\.\d+)";
            var match = System.Text.RegularExpressions.Regex.Match(ffmpegOutput, durationPattern);
            if (match.Success)
            {
                int hours = int.Parse(match.Groups["hours"].Value);
                int minutes = int.Parse(match.Groups["minutes"].Value);
                double seconds = double.Parse(match.Groups["seconds"].Value);

                return hours * 3600 + minutes * 60 + seconds;
            }

            return 0;
        }

        public List<Image> ExtractVideoThumbnails(string videoFilePath)
        {
            Directory.CreateDirectory(tempDir);

            if (!File.Exists(ffmpegPath))
            {
                throw new FileNotFoundException("FFmpeg executable not found at the specified location." + ffmpegPath);
            }

            double videoDuration = GetVideoDuration(videoFilePath, ffmpegPath);

            if (videoDuration <= 0)
            {
                throw new InvalidOperationException("Failed to retrieve video duration.");
            }
            double interval = videoDuration / 10.0;
            string filter = $"select='not(mod(t,{interval}))',scale=100:100";
            string outputPattern = Path.Combine(tempDir, "thumbnail%03d.png");
            string ffmpegCommand = $"-i \"{videoFilePath}\" -vf \"{filter}\" -vsync vfr \"{outputPattern}\"";

            Process ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCommand,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            ffmpegProcess.Start();
            ffmpegProcess.WaitForExit();

            List<Image> thumbnails = new List<Image>();
            foreach (var file in Directory.GetFiles(tempDir, "thumbnail*.png"))
            {
                using (var tempImage = Image.FromFile(file))
                {
                    thumbnails.Add(new Bitmap(tempImage));
                    Console.WriteLine("file added.");
                }
            }

            foreach (var file in Directory.GetFiles(tempDir))
            {
                File.Delete(file);
            }
            Directory.Delete(tempDir);

            return thumbnails;
        }

        //private List<int> ExtractAudioWaveform(string filePath)
        //{
        //    List<int> waveform = new List<int>();
        //    Random rand = new Random();
        //    for (int i = 0; i < 100; i++)
        //    {
        //        waveform.Add(rand.Next(10, 50));
        //    }

        //    return waveform;
        //}

    }
}
