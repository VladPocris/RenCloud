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
        private float widthVideo = 0f;
        private readonly List<(Image thumbnail, float position)> allThumbnailsWithPositions = new List<(Image, float)>();
        private List<RectangleF> allVideoBounds = new List<RectangleF>();
        private RectangleF selectedVideoBounds = RectangleF.Empty;
        private List<RectangleF> allAudioSegments = new List<RectangleF>();
        private RectangleF selectedAudioBounds = RectangleF.Empty;
        private List<RectangleF> allAudioAmplitudeBars = new List<RectangleF>();
        private Dictionary<RectangleF, RectangleF> videoToAudioMapping = new Dictionary<RectangleF, RectangleF>();
        private bool isDraggingTracker = false;
        private float trackerXPosition = 0f;
        private float currentPlaybackTime = 0f;
        private float pixelsPerSecond = 50f;
        private float pixelsPerMillisecond;
        private Timer autoScrollTimer;
        private Timer playbackTimer;
        private int autoScrollDirection = 0;
        private bool isUpdatingUI = false;
        private List<VideoSegment> fullVideo = new List<VideoSegment>();
        private List<VideoSegment> fullAudio = new List<VideoSegment>();
        private int segmentsVideoCount = 0;
        private int segmentsAudioCount = 0;
        private string previewFile = "";
        private bool wasPlayingBeforeDrag = false;
        private float lastUpdatedPosition = -1;
        private const float updateThreshold = 5.0f;


        class VideoSegment
        {
            public string FilePath { get; set; }
            public int Id { get; set; }
            public float StartTime { get; set; }
            public float EndTime { get; set; }
            public float TimeLinePosition { get; set; }
        }

        class AudioSegment
        {
            public string FilePath { get; set; }
            public int Id { get; set; }
            public float StartTime { get; set; }
            public float EndTime { get; set; }
            public float TimeLinePosition { get; set; }
        }

        private async void GeneratePreview()
        {
            string outputPath = Path.Combine(Path.GetTempPath(), "preview.mp4");
            StringBuilder ffmpegCmd = new StringBuilder();
            List<string> filterComplex = new List<string>();
            int fileIndex = 0;

            // Common settings for resolution, framerate, audio format, and SAR
            int targetWidth = 640;  // Example width
            int targetHeight = 360; // Example height
            int targetFps = 30;     // Target frames per second
            string audioFormat = "fltp"; // Floating-point audio samples
            int audioSampleRate = 44100; // Example sample rate
            int audioChannels = 2;       // Stereo audio

            foreach (var segment in fullVideo)
            {
                ffmpegCmd.AppendFormat("-i \"{0}\" ", segment.FilePath);
                // Apply filters to standardize video and audio streams, including setting SAR to 1:1
                filterComplex.Add($"[{fileIndex}:v]trim=start={segment.StartTime}:end={segment.EndTime},setpts=PTS-STARTPTS,scale={targetWidth}:{targetHeight},setsar=1,fps={targetFps}[v{fileIndex}]; ");
                filterComplex.Add($"[{fileIndex}:a]atrim=start={segment.StartTime}:end={segment.EndTime},asetpts=PTS-STARTPTS,aformat=sample_fmts={audioFormat}:sample_rates={audioSampleRate}:channel_layouts=stereo[a{fileIndex}]; ");
                fileIndex++;
            }

            // Concatenate video and audio streams with ensured compatibility
            if (fileIndex > 0)
            {
                ffmpegCmd.Append("-filter_complex \"");
                foreach (var filter in filterComplex)
                {
                    ffmpegCmd.Append(filter);
                }

                // Link video and audio streams to the concat filter
                string videoInputs = string.Join("", Enumerable.Range(0, fileIndex).Select(i => $"[v{i}]"));
                string audioInputs = string.Join("", Enumerable.Range(0, fileIndex).Select(i => $"[a{i}]"));
                ffmpegCmd.Append($"{videoInputs}concat=n={fileIndex}:v=1:a=0[outv]; ");
                ffmpegCmd.Append($"{audioInputs}concat=n={fileIndex}:v=0:a=1[outa]\"");
                ffmpegCmd.Append(" -map \"[outv]\" -map \"[outa]\" ");
            }

            ffmpegCmd.AppendFormat("-y \"{0}\"", outputPath);

            Process ffmpegProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCmd.ToString(),
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            ffmpegProcess.Start();
            string errorOutput = ffmpegProcess.StandardError.ReadToEnd();
            ffmpegProcess.WaitForExit();

            previewFile = outputPath;
            PreviewBox.settings.autoStart = false;

            if (ffmpegProcess.ExitCode != 0)
            {
                MessageBox.Show("Failed to create preview.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("FFmpeg error output: " + errorOutput);
            }
            PreviewBox.URL = outputPath;
            PlayPreview();
            await Task.Delay(890);
            PausePreview();
        }


        private void PlayPreview()
        {
            Debug.WriteLine("PlayPreview called from: " + new StackTrace().GetFrame(1).GetMethod().Name);
            PreviewBox.Ctlcontrols.play();
            playbackTimer.Start();
        }

        private void PausePreview()
        {
            Debug.WriteLine("PausePreview called from: " + new StackTrace().GetFrame(1).GetMethod().Name);
            PreviewBox.Ctlcontrols.pause();
            playbackTimer.Stop();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            PausePreview();
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            PlayPreview();
        }










        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingTracker)
            {
                currentPlaybackTime = (float)(PreviewBox.Ctlcontrols.currentPosition * 1000);
                trackerXPosition = currentPlaybackTime * pixelsPerMillisecond;
                UpdatePlaybackLabel(currentPlaybackTime);
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }
        }










        private void OnScroll(object sender, ScrollEventArgs e)
        {
            panel8.Invalidate();
            EditingRuller.Invalidate();
            VideoTrack.Invalidate();
            AudioTrack.Invalidate();
            VideoTrackPlaceholder.Invalidate();
            AudioTrackPlaceholder.Invalidate();
        }
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
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWindowUpdate(IntPtr hWnd);
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

        private static void SetDoubleBuffered(Control control)
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
            pixelsPerMillisecond = pixelsPerSecond / 1000f;
            autoScrollTimer = new Timer { Interval = 1 };
            autoScrollTimer.Tick += AutoScrollTimer_Tick;
            playbackTimer = new Timer { Interval = 100 };
            playbackTimer.Tick += PlaybackTimer_Tick;
            PreviewBox.uiMode = "none";
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
            EditingRuller.MouseDown += EditingRuller_MouseDown;
            EditingRuller.MouseMove += EditingRuller_MouseMove;
            EditingRuller.MouseUp += EditingRuller_MouseUp;
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
        private void EditingRuller_MouseDown(object sender, MouseEventArgs e)
        {
            wasPlayingBeforeDrag = PreviewBox.playState == WMPLib.WMPPlayState.wmppsPlaying;


            isDraggingTracker = true;
            PausePreview();

            if (Math.Abs(e.X - trackerXPosition) <= 5)
            {
                isDraggingTracker = true;
            }
            else
            {
                // If not clicking directly on the tracker, move the tracker to the mouse position
                trackerXPosition = Math.Max(0, Math.Min(e.X, widthVideo));
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
                PreviewBox.Ctlcontrols.currentPosition = currentPlaybackTime / 1000.0;
                UpdatePlaybackLabel(currentPlaybackTime);
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }
        }

        private void EditingRuller_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                int visibleStart = panel8.HorizontalScroll.Value;
                int visibleEnd = visibleStart + panel8.ClientRectangle.Width;
                trackerXPosition = Math.Max(visibleStart, Math.Min(e.X, Math.Min(widthVideo, visibleEnd)));
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
                PreviewBox.Ctlcontrols.currentPosition = currentPlaybackTime / 1000.0;

                // Ensure the player updates the frame display
                PreviewBox.Ctlcontrols.pause();
                ((WMPLib.IWMPControls2)PreviewBox.Ctlcontrols).step(1);

                UpdatePlaybackLabel(currentPlaybackTime);
                if (!isUpdatingUI)
                {
                    isUpdatingUI = true;
                    EditingRuller.Invalidate();
                    VideoTrack.Invalidate();
                    AudioTrack.Invalidate();
                    isUpdatingUI = false;
                }

                ManageAutoScroll(visibleStart, visibleEnd);
            }
        }


        private void EditingRuller_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                isDraggingTracker = false;
                autoScrollTimer.Stop();
                autoScrollDirection = 0;
                if (wasPlayingBeforeDrag)
                {
                    PlayPreview();
                }
                else
                {
                    PausePreview();
                }
            }
        }

        private void ManageAutoScroll(int visibleStart, int visibleEnd)
        {

            if (trackerXPosition == visibleEnd && visibleEnd < widthVideo)
            {
                autoScrollDirection = 1;
                autoScrollTimer.Start();
            }

            else if (trackerXPosition == visibleStart && visibleStart > 0)
            {
                autoScrollDirection = -1;
                autoScrollTimer.Start();
            }
            else
            {
                autoScrollDirection = 0;
                autoScrollTimer.Stop();
            }
        }


        private void AutoScrollTimer_Tick(object sender, EventArgs e)
        {
            int scrollSpeed = 40;

            if (autoScrollDirection == 1)
            {
                if (panel8.HorizontalScroll.Value < panel8.HorizontalScroll.Maximum)
                {
                    int newValue = Math.Min(panel8.HorizontalScroll.Value + scrollSpeed, panel8.HorizontalScroll.Maximum);
                    if (panel8.HorizontalScroll.Value != newValue)
                    {
                        panel8.HorizontalScroll.Value = newValue;
                        if (!isUpdatingUI)
                        {
                            isUpdatingUI = true;
                            EditingRuller.Invalidate();
                            VideoTrack.Invalidate();
                            AudioTrack.Invalidate();
                            isUpdatingUI = false;
                        }
                    }
                }
                else
                {
                    autoScrollDirection = 0;
                    autoScrollTimer.Stop();
                }
            }
            else if (autoScrollDirection == -1)
            {
                if (panel8.HorizontalScroll.Value > panel8.HorizontalScroll.Minimum)
                {
                    int newValue = Math.Max(panel8.HorizontalScroll.Value - scrollSpeed, panel8.HorizontalScroll.Minimum);
                    if (panel8.HorizontalScroll.Value != newValue)
                    {
                        panel8.HorizontalScroll.Value = newValue;
                        if (!isUpdatingUI)
                        {
                            isUpdatingUI = true;
                            EditingRuller.Invalidate();
                            VideoTrack.Invalidate();
                            AudioTrack.Invalidate();
                            isUpdatingUI = false;
                        }
                    }
                }
                else
                {
                    autoScrollDirection = 0;
                    autoScrollTimer.Stop();
                }
            }
            else
            {
                autoScrollTimer.Stop();
            }
        }
        private void UpdatePlaybackLabel(float playbackTime)
        {
            int minutes = (int)(playbackTime / 60000);
            int seconds = (int)((playbackTime % 60000) / 1000);
            int milliseconds = (int)(playbackTime % 1000);
            string timeFormatted = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
            TimeStamp.Text = timeFormatted;
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

            g.Clear(Color.Gray);

            using (Pen majorTickPen = new Pen(Color.Black))
            using (Pen minorTickPen = new Pen(Color.Black))
            using (Brush textBrush = new SolidBrush(Color.White))
            using (Pen trackerPen = new Pen(Color.Blue, 2))
            using (Brush handleBrush = new SolidBrush(Color.Blue))
            using (Brush tooltipBrush = new SolidBrush(Color.FromArgb(200, 50, 50, 50)))
            using (Brush tooltipTextBrush = new SolidBrush(Color.White))
            using (Font tooltipFont = new Font("Arial", 8, FontStyle.Bold))
            {
                for (int x = 0; x < panelWidth; x += (int)pixelsPerSecond)
                {
                    g.DrawLine(majorTickPen, x, 0, x, tickHeightMajor);
                    int seconds = x / (int)pixelsPerSecond;
                    g.DrawString(seconds.ToString(), this.Font, textBrush, x + 2, tickHeightMajor);

                    for (int i = 1; i < 5; i++)
                    {
                        int minorX = x + (i * (int)pixelsPerSecond / 5);
                        g.DrawLine(minorTickPen, minorX, 0, minorX, tickHeightMinor);
                    }
                }
                g.DrawLine(trackerPen, trackerXPosition, 0, trackerXPosition, EditingRuller.Height);
                float arrowWidth = 10f;
                float arrowHeight = 12f;
                PointF[] arrowPoints = new PointF[]
                {
            new PointF(trackerXPosition, 0),
            new PointF(trackerXPosition - arrowWidth / 2, arrowHeight),
            new PointF(trackerXPosition + arrowWidth / 2, arrowHeight)
                };
                g.FillPolygon(handleBrush, arrowPoints);
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

            segmentsVideoCount++;
            fullVideo.Add(new VideoSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsVideoCount, TimeLinePosition = videoStartPosition/pixelsPerSecond});

            segmentsAudioCount++;
            fullAudio.Add(new VideoSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsAudioCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

            foreach (VideoSegment segment in fullVideo)
            {
                Console.WriteLine($"Position: " + segment.Id + " | FilePath: " + segment.FilePath + " | Start time: " + segment.StartTime + " | End Time: " + segment.EndTime + " | TimeLine Position: " + segment.TimeLinePosition);
            }

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

            GeneratePreview();
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
            using (Pen trackerPen = new Pen(Color.Blue, 2))
            {
                g.DrawLine(trackerPen, trackerXPosition, 0, trackerXPosition, VideoTrack.Height);
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
        public List<Image> ExtractVideoThumbnails(string videoFilePath, double intervalSeconds = 4.0, int maxDegreeOfParallelism = 8)
        {
            if (!File.Exists(videoFilePath))
            {
                throw new FileNotFoundException("Video file not found: " + videoFilePath);
            }
            string tempDir = Path.Combine(Path.GetTempPath(), "VideoThumbnails");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
            double videoDuration = GetVideoDuration(videoFilePath, ffmpegPath);
            if (videoDuration <= 0)
            {
                throw new InvalidOperationException("Failed to retrieve video duration.");
            }
            List<Image> thumbnails = new List<Image>();
            int thumbnailCount = (int)(videoDuration / intervalSeconds);
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            Parallel.For(0, thumbnailCount, parallelOptions, i =>
            {
                double timestamp = i * intervalSeconds;
                string outputFile = Path.Combine(tempDir, $"thumbnail_{i:D3}.jpg");
                string ffmpegCommand = $"-ss {timestamp} -i \"{videoFilePath}\" -vframes 1 -q:v 2 " +
                                       $"-threads {Environment.ProcessorCount} " +
                                       $"\"{outputFile}\"";
                try
                {
                    Console.WriteLine($"Running FFmpeg command for thumbnail {i}: {ffmpegCommand}");
                    Process ffmpegProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = ffmpegPath,
                            Arguments = ffmpegCommand,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardError = true
                        }
                    };
                    ffmpegProcess.Start();
                    string errorOutput = ffmpegProcess.StandardError.ReadToEnd();
                    ffmpegProcess.WaitForExit();
                    if (ffmpegProcess.ExitCode != 0)
                    {
                        Console.WriteLine($"FFmpeg process failed for thumbnail {i} with exit code {ffmpegProcess.ExitCode}. Details: {errorOutput}");
                    }
                    if (File.Exists(outputFile))
                    {
                        using (var tempImage = Image.FromFile(outputFile))
                        {
                            lock (thumbnails)
                            {
                                thumbnails.Add(new Bitmap(tempImage));
                            }
                        }
                        File.Delete(outputFile); // Cleanup
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error extracting thumbnail at {timestamp} seconds: {ex.Message}");
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
                    amplitudes.Add(amplitude);
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
            using (Pen trackerPen = new Pen(Color.Blue, 2))
            {
                g.DrawLine(trackerPen, trackerXPosition, 0, trackerXPosition, AudioTrack.Height);
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
    }
}
