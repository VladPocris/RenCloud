using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
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
        private string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "ffmpeg", "bin", "ffmpeg.exe");
        private string tempDir = Path.Combine(Path.GetTempPath(), "VideoThumbnails");
        private float newXPosition = 0f;
        private float widthVideo = 0f;
        float intervalInPixels = 0f;
        private readonly List<(Image thumbnail, float position)> allThumbnailsWithPositions = new List<(Image, float)>();
        private List<RectangleF> allVideoBounds = new List<RectangleF>();
        private RectangleF selectedVideoBounds = RectangleF.Empty;
        private List<RectangleF> allAudioAmplitudeBars = new List<RectangleF>();
        private Dictionary<RectangleF, RectangleF> videoToAudioMapping = new Dictionary<RectangleF, RectangleF>();
        private List<RectangleF> allAudioSegments = new List<RectangleF>();
        private RectangleF selectedAudioBounds = RectangleF.Empty;


        // Event handler for scroll to force full invalidation
        private void OnScroll(object sender, ScrollEventArgs e)
        {
            EditingRuller.Invalidate();
            VideoTrackPlaceholder.Invalidate();
            AudioTrackPlaceholder.Invalidate();
        }

        // LockWindowUpdate to prevent flickering during scroll
        private void OnScrollWithLock(object sender, ScrollEventArgs e)
        {
            if (e.Type == ScrollEventType.First)
            {
                LockWindowUpdate(this.Handle);
            }
            else
            {
                LockWindowUpdate(IntPtr.Zero);
                EditingRuller.Update();
                VideoTrackPlaceholder.Update();
                AudioTrackPlaceholder.Update();
                if (e.Type != ScrollEventType.Last)
                    LockWindowUpdate(this.Handle);
            }
        }

        // Pinvoke LockWindowUpdate method
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWindowUpdate(IntPtr hWnd);

        // Enable compositing for smooth rendering
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_COMPOSITED = 0x02000000;
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_COMPOSITED;
                return cp;
            }
        }

        // Utility method to enable double buffering
        private void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

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
            // Enable Double Buffering
            SetDoubleBuffered(EditingRuller);
            SetDoubleBuffered(VideoTrackPlaceholder);
            SetDoubleBuffered(AudioTrackPlaceholder);
            SetDoubleBuffered(VideoTrack);
            SetDoubleBuffered(AudioTrack);
            SetDoubleBuffered(panel8);
            this.DoubleBuffered = true;
        }

        private void UserInterfaceForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            applyCorners.AttributesRoundCorners(this, isActive);
            dragFunctionality.AttachDraggingEvent(panel2, this);
            dragFunctionality.AttachDraggingEvent(panel3, this);
            dragFunctionality.AttachDraggingEvent(pictureBox1, this);
            EditingRuller.Paint += EditingRuller_Paint;
            VideoTrackPlaceholder.Paint += VideoTrackPlaceholder_Paint_1;
            AudioTrackPlaceholder.Paint += AudioTrackPlaceholder_Paint;
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
            Graphics g = e.Graphics;
            int panelWidth = EditingRuller.Width;
            float tickHeightMajor = 6;
            float tickHeightMinor = 3;
            int pixelsPerSecond = 50;

            g.Clear(Color.Gray);

            using (Pen majorTickPen = new Pen(Color.Black))
            using (Pen minorTickPen = new Pen(Color.Black))
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                for (int x = 0; x < panelWidth; x += pixelsPerSecond)
                {
                    g.DrawLine(majorTickPen, x, 0, x, tickHeightMajor);
                    int seconds = x / pixelsPerSecond;
                    g.DrawString(seconds.ToString(), this.Font, textBrush, x + 2, tickHeightMajor);

                    for (int i = 1; i < 5; i++)
                    {
                        int minorX = x + (i * pixelsPerSecond / 5);
                        g.DrawLine(minorTickPen, minorX, 0, minorX, tickHeightMinor);
                    }
                }
            }
        }

        private void VideoTrackPlaceholder_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Gray);

            int stripHeight = 30;
            int spacing = 20;
            int frameWidth = 20;
            int frameHeight = 20;
            int frameSpacing = 5;
            int panelWidth = VideoTrackPlaceholder.Width;
            int panelHeight = VideoTrackPlaceholder.Height;

            using (Brush stripBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (Brush frameBrush = new SolidBrush(Color.LightGreen))
            using (Pen framePen = new Pen(Color.Black, 1))
            using (Pen outlinePen = new Pen(Color.DarkGray, 1))
            {
                for (int y = 0; y < panelHeight; y += stripHeight + spacing)
                {
                    Rectangle stripRect = new Rectangle(0, y, panelWidth, stripHeight);
                    g.FillRectangle(stripBrush, stripRect);

                    for (int x = 0; x < panelWidth; x += frameWidth + frameSpacing)
                    {
                        Rectangle frameRect = new Rectangle(x, y + (stripHeight - frameHeight) / 2, frameWidth, frameHeight);
                        g.FillRectangle(frameBrush, frameRect);
                        g.DrawRectangle(framePen, frameRect);
                    }
                    g.DrawRectangle(outlinePen, stripRect);
                }
            }
        }

        private void AudioTrackPlaceholder_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Gray);

            int tapeWidth = 30;
            int spacing = 20;
            int barWidth = 5;
            int barSpacing = 3;
            int maxBarHeight = 15;
            Random rand = new Random();

            int panelWidth = AudioTrackPlaceholder.Width;
            int panelHeight = AudioTrackPlaceholder.Height;

            using (Brush tapeBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
            using (Brush barBrush = new SolidBrush(Color.LightGreen))
            using (Pen outlinePen = new Pen(Color.DarkGray, 1))
            {
                for (int y = 0; y < panelHeight; y += tapeWidth + spacing)
                {
                    Rectangle tapeRect = new Rectangle(0, y, panelWidth, tapeWidth);
                    g.FillRectangle(tapeBrush, tapeRect);

                    for (int x = 0; x < panelWidth; x += barWidth + barSpacing)
                    {
                        int barHeight = rand.Next(5, maxBarHeight);
                        int barY = y + (tapeWidth / 2) - (barHeight / 2);
                        Rectangle barRect = new Rectangle(x, barY, barWidth, barHeight);
                        g.FillRectangle(barBrush, barRect);
                    }
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
            float videoDuration = (float)GetVideoDuration(filePath, ffmpegPath);
            float pixelsPerSecond = 50f;
            int barsPerSecond = 5;
            float barWidth = 8f;
            float barSpacing = 2f;
            float barWidthIncludingSpacing = barWidth + barSpacing;
            float newWidth = videoDuration * pixelsPerSecond;

            widthVideo += newWidth;
            EditingRuller.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
            VideoTrack.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
            AudioTrack.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);

            EditingRuller.Invalidate();
            VideoTrackPlaceholder.Invalidate();
            AudioTrackPlaceholder.Invalidate();

            List<Image> videoThumbnails = ExtractVideoThumbnails(filePath);
            int thumbnailCount = videoThumbnails.Count;
            float intervalInPixels = 4.0f * pixelsPerSecond;
            float videoStartPosition = widthVideo - newWidth;

            float thumbnailWidth = 100f;
            float thumbnailHeight = VideoTrack.Height - 20f;

            for (int i = 0; i < thumbnailCount; i++)
            {
                float position = videoStartPosition + intervalInPixels * (i + 1);
                if (position + thumbnailWidth > videoStartPosition + newWidth)
                    break;

                allThumbnailsWithPositions.Add((videoThumbnails[i], position));
            }

            var videoBounds = new RectangleF(videoStartPosition, 0f, newWidth, VideoTrack.Height - 2f);
            var audioBounds = new RectangleF(videoStartPosition, 0f, newWidth, AudioTrack.Height - 2f);

            allVideoBounds.Add(videoBounds);
            videoToAudioMapping[videoBounds] = audioBounds;
            allAudioSegments.Add(audioBounds);

            List<float> audioAmplitudes = GetAudioAmplitudeData(filePath);
            int amplitudeCount = audioAmplitudes.Count;

            int totalBars = (int)(videoDuration * barsPerSecond);

            int samplesPerSecond = 44100;
            int samplesPerBar = samplesPerSecond / barsPerSecond;

            float audioStartPosition = widthVideo - newWidth;

            for (int i = 0; i < totalBars; i++)
            {
                int startIdx = (int)((i / (float)totalBars) * amplitudeCount);
                int endIdx = (int)(((i + 1) / (float)totalBars) * amplitudeCount);

                if (startIdx >= amplitudeCount) break;

                float averageAmplitude = 0;
                for (int j = startIdx; j < endIdx; j++)
                {
                    averageAmplitude += audioAmplitudes[j];
                }

                averageAmplitude /= (endIdx - startIdx);

                float amplitudeHeight = averageAmplitude * AudioTrack.Height * 3;
                float barXPosition = audioStartPosition + (i * barWidthIncludingSpacing);
                float barYPosition = AudioTrack.Height - amplitudeHeight;

                if (barXPosition + barWidth > audioStartPosition + newWidth)
                    break;

                allAudioAmplitudeBars.Add(new RectangleF(barXPosition, barYPosition, barWidth, amplitudeHeight));

                Console.WriteLine($"Bar {i}: X = {barXPosition}, Height = {amplitudeHeight}");
            }


            // Assuming widthVideo is a float
            VideoTrackPlaceholder.Location = new Point((int)Math.Round(widthVideo), VideoTrackPlaceholder.Location.Y);
            AudioTrackPlaceholder.Location = new Point((int)Math.Round(widthVideo), AudioTrackPlaceholder.Location.Y);

            VideoTrack.Paint -= VideoTrack_PaintHandler;
            VideoTrack.Paint += VideoTrack_PaintHandler;
            VideoTrack.MouseDown -= VideoTrack_MouseDownHandler;
            VideoTrack.MouseDown += VideoTrack_MouseDownHandler;
            AudioTrack.Paint -= AudioTrack_PaintHandler;
            AudioTrack.Paint += AudioTrack_PaintHandler;
            AudioTrack.MouseDown -= AudioTrack_MouseDownHandler;
            AudioTrack.MouseDown += AudioTrack_MouseDownHandler;

            VideoTrack.Invalidate();
            AudioTrack.Invalidate();
        }

        private void VideoTrack_PaintHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Brush videoBrush = new SolidBrush(Color.Gray))
            using (Pen borderPen = new Pen(Color.Green, 2))
            using (Pen selectedPen = new Pen(Color.Red, 3))
            {
                foreach (var bounds in allVideoBounds)
                {
                    g.FillRectangle(videoBrush, bounds);
                    g.DrawRectangle(bounds == selectedVideoBounds ? selectedPen : borderPen, Rectangle.Round(bounds));
                }
            }

            foreach (var (thumbnail, position) in allThumbnailsWithPositions)
            {
                float thumbnailHeight = VideoTrack.Height - 20f;
                float thumbnailY = 10f;

                g.DrawImage(thumbnail, position, thumbnailY, 100, thumbnailHeight);
            }
        }

        private void VideoTrack_MouseDownHandler(object sender, MouseEventArgs e)
        {
            selectedVideoBounds = Rectangle.Empty;
            selectedAudioBounds = Rectangle.Empty;

            foreach (var bounds in allVideoBounds)
            {
                if (bounds.Contains(e.Location))
                {
                    selectedVideoBounds = bounds;
                    break;
                }
            }

            VideoTrack.Invalidate();
            AudioTrack.Invalidate();
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
                float seconds = float.Parse(match.Groups["seconds"].Value);

                return hours * 3600 + minutes * 60 + seconds;
            }

            return 0f;
        }
        public List<Image> ExtractVideoThumbnails(string videoFilePath)
        {
            Directory.CreateDirectory(tempDir);
            if (!File.Exists(ffmpegPath))
            {
                throw new FileNotFoundException("FFmpeg executable not found at the specified location: " + ffmpegPath);
            }
            double videoDuration = GetVideoDuration(videoFilePath, ffmpegPath);
            if (videoDuration <= 0)
            {
                throw new InvalidOperationException("Failed to retrieve video duration.");
            }
            double interval = 4.0;
            List<Image> thumbnails = new List<Image>();
            int thumbnailCount = (int)(videoDuration / interval);

            Parallel.For(1, thumbnailCount + 1, i =>
            {
                double timestamp = interval * i;
                string outputPath = Path.Combine(tempDir, $"thumbnail{i:D3}.png");
                string ffmpegCommand = $"-i \"{videoFilePath}\" -ss {timestamp} -vframes 1 -q:v 2 \"{outputPath}\"";

                try
                {
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

                    if (File.Exists(outputPath))
                    {
                        lock (thumbnails)
                        {
                            using (var tempImage = Image.FromFile(outputPath))
                            {
                                thumbnails.Add(new Bitmap(tempImage));
                            }
                        }
                        File.Delete(outputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating thumbnail for timestamp {timestamp}: {ex.Message}");
                }
            });

            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }

            return thumbnails;
        }

        private List<float> GetAudioAmplitudeData(string videoFilePath)
        {
            List<float> amplitudes = new List<float>();

            string ffmpegCommand = $"-i \"{videoFilePath}\" -vn -ac 1 -filter:a aresample=44100 -map 0:a -c:a pcm_s16le -f data -";

            Process ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCommand,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpegProcess.Start();

            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = ffmpegProcess.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i += 2)
                {
                    short sample = BitConverter.ToInt16(buffer, i);
                    float amplitude = Math.Abs(sample) / 32768f;
                    amplitudes.Add((float)(amplitude));
                }
            }

            ffmpegProcess.WaitForExit();

            Console.WriteLine($"Amplitude count: {amplitudes.Count}");

            return amplitudes;
        }

        private void AudioTrack_PaintHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Brush backgroundBrush = new SolidBrush(Color.Gray))
            {
                g.FillRectangle(backgroundBrush, 0, 0, widthVideo, AudioTrack.Height);
            }

            using (Brush barBrush = new SolidBrush(Color.LightGreen))
            {
                foreach (RectangleF bar in allAudioAmplitudeBars)
                {
                    g.FillRectangle(barBrush, bar);
                }
            }

            using (Pen borderPen = new Pen(Color.Green, 2))
            using (Pen selectedPen = new Pen(Color.Red, 2))
            {
                foreach (var bounds in allAudioSegments)
                {
                    g.DrawRectangle(bounds == selectedAudioBounds ? selectedPen : borderPen, Rectangle.Round(bounds));
                }
            }

            using (Brush transparentBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
            {
                g.FillRectangle(transparentBrush, widthVideo, 0, AudioTrack.Width - widthVideo, AudioTrack.Height);
            }
        }
        private void AudioTrack_MouseDownHandler(object sender, MouseEventArgs e)
        {
            selectedAudioBounds = Rectangle.Empty;
            selectedVideoBounds = Rectangle.Empty;

            foreach (var bounds in allAudioSegments)
            {
                if (bounds.Contains(e.Location))
                {
                    selectedAudioBounds = bounds;
                    break;
                }
            }

            AudioTrack.Invalidate();
            VideoTrack.Invalidate();
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
