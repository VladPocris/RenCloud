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
using Vlc.DotNet.Forms;
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
        private bool wasPlayingBeforeDrag = false;
        private string outputPath;


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

        private async Task GeneratePreview(int targetSizeMB = 30)
        {
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VideoPreviews");
            if (!Directory.Exists(previewDirectory))
            {
                Directory.CreateDirectory(previewDirectory);
            }

            outputPath = Path.Combine(previewDirectory, $"preview_{DateTime.Now:yyyyMMddHHmmssfff}.mp4");
            StringBuilder ffmpegCmd = new StringBuilder("-y "); // Overwrite without asking

            int fileIndex = 0;
            List<string> filterComplexVideo = new List<string>();
            List<string> filterComplexAudio = new List<string>();

            // Common settings
            int targetWidth = 320;
            int targetHeight = 180;
            int targetFps = 15;
            int audioBitrateKbps = 128; // Audio bitrate in kbps
            string videoFormat = "yuv420p";

            // Calculate total video duration in seconds
            double totalDurationSeconds = fullVideo.Sum(segment => segment.EndTime - segment.StartTime);

            // Calculate target video bitrate
            int totalBitrate = (int)((targetSizeMB * 8192) / totalDurationSeconds) - audioBitrateKbps;
            string videoBitrate = totalBitrate.ToString() + "k";

            foreach (var segment in fullVideo)
            {
                // Add input-specific options for each file
                ffmpegCmd.AppendFormat("-hwaccel cuda -c:v h264_cuvid -i \"{0}\" ", segment.FilePath);

                // Add video and audio filters
                filterComplexVideo.Add($"[{fileIndex}:v]trim=start={segment.StartTime}:end={segment.EndTime},setpts=PTS-STARTPTS,scale={targetWidth}:{targetHeight},fps={targetFps},setsar=1,format={videoFormat}[v{fileIndex}];");
                filterComplexAudio.Add($"[{fileIndex}:a]atrim=start={segment.StartTime}:end={segment.EndTime},asetpts=PTS-STARTPTS,aformat=sample_fmts=fltp:sample_rates=44100:channel_layouts=stereo[a{fileIndex}];");
                fileIndex++;
            }

            // Add filter_complex and output-specific options
            if (fileIndex > 0)
            {
                string videoFilter = string.Join("", Enumerable.Range(0, fileIndex).Select(i => $"[v{i}]"));
                string audioFilter = string.Join("", Enumerable.Range(0, fileIndex).Select(i => $"[a{i}]"));
                string filterComplex = $"{string.Join("", filterComplexVideo)}{videoFilter}concat=n={fileIndex}:v=1:a=0[outv];{string.Join("", filterComplexAudio)}{audioFilter}concat=n={fileIndex}:v=0:a=1[outa]";
                ffmpegCmd.Append($"-filter_complex \"{filterComplex}\" ");

                // Output options
                ffmpegCmd.Append($"-b:v {videoBitrate} -preset ultrafast -b:a {audioBitrateKbps}k -c:a aac -crf 30 -threads 0 ");
                ffmpegCmd.Append($"-map \"[outv]\" -map \"[outa]\" \"{outputPath}\"");
            }
            else
            {
                MessageBox.Show("No video segments to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await Task.Run(() =>
            {
                using (Process ffmpegProcess = new Process())
                {
                    ffmpegProcess.StartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = ffmpegCmd.ToString(),
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    ffmpegProcess.Start();
                    string errorOutput = ffmpegProcess.StandardError.ReadToEnd();
                    ffmpegProcess.WaitForExit();

                    if (ffmpegProcess.ExitCode == 0)
                    {
                        this.Invoke(new Action(() =>
                        {
                            var mediaOptions = new string[] { "input-repeat=65535" };
                            PreviewBox.SetMedia(new FileInfo(outputPath), mediaOptions);
                            PreviewBox.Play();
                            // Delay to ensure media loads properly
                            Task.Delay(872).ContinueWith(t =>
                            {
                                if (!wasPlayingBeforeDrag)
                                {
                                    PreviewBox.Pause();
                                }
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }));
                    }
                    else
                    {
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show("Failed to create preview. See console output for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine($"ffmpeg error: {errorOutput}");
                        }));
                    }
                }
            });
        }










        private void PlayPreview()
        {
            Debug.WriteLine("PlayPreview called from: " + new StackTrace().GetFrame(1).GetMethod().Name);
            UpdatePlaybackLabel(PreviewBox.Time);
            PreviewBox.Play();
            playbackTimer.Start();
            playbackStopwatch.Restart();
            wasPlayingBeforeDrag = true;
        }
        private void PausePreview()
        {
            Debug.WriteLine("PausePreview called from: " + new StackTrace().GetFrame(1).GetMethod().Name);
            UpdatePlaybackLabel(PreviewBox.Time);
            PreviewBox.Pause();
            playbackTimer.Stop();
            playbackStopwatch.Stop();
            wasPlayingBeforeDrag = false;
        }
        private void PauseButton_Click(object sender, EventArgs e)
        {
            PausePreview();
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            PlayPreview();
        }







        private Stopwatch playbackStopwatch; // High-resolution stopwatch to track elapsed time
        private long lastKnownVlcTime = 0;

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingTracker)
            {
                long elapsedSinceLastKnown = playbackStopwatch.ElapsedMilliseconds;
                long interpolatedTime = lastKnownVlcTime + elapsedSinceLastKnown; // Predict current time

                // Calculate the new tracker position based on the interpolated time
                float newPosition = interpolatedTime * pixelsPerMillisecond;

                // Clamp the new position to not exceed the width of the video track
                float maxPosition = widthVideo; // Width of your video track in pixels
                trackerXPosition = Math.Min(newPosition, maxPosition);

                // Ensure the tracker stays visible by scrolling the panel
                EnsureTrackerVisible(trackerXPosition);

                UpdatePlaybackLabel(interpolatedTime); // Update UI with interpolated time

                // Invalidate UI components to reflect the tracker's new position
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }

            // Update last known time periodically from VLC, not every tick to avoid jumpy updates
            long currentVlcTime = PreviewBox.Time;
            if (currentVlcTime != lastKnownVlcTime)
            {
                lastKnownVlcTime = currentVlcTime;
                playbackStopwatch.Restart(); // Restart stopwatch when a new VLC time is obtained
            }
        }

        // Ensure the tracker is visible by scrolling the panel if necessary
        private void EnsureTrackerVisible(float trackerXPosition)
        {
            int visibleStart = panel8.HorizontalScroll.Value;
            int visibleEnd = visibleStart + panel8.ClientRectangle.Width;

            const int padding = 50; // Padding in pixels before scrolling

            if (trackerXPosition < visibleStart + padding)
            {
                // Scroll left
                panel8.HorizontalScroll.Value = Math.Max(0, (int)(trackerXPosition - padding));
            }
            else if (trackerXPosition > visibleEnd - padding)
            {
                // Scroll right
                panel8.HorizontalScroll.Value = Math.Min(panel8.HorizontalScroll.Maximum,
                                                         (int)(trackerXPosition - panel8.ClientRectangle.Width + padding));
            }
        }







        private void ScrollToPosition(float position)
        {
            int newValue = (int)Math.Max(0, Math.Min(position, panel8.HorizontalScroll.Maximum));

            if (panel8.HorizontalScroll.Value != newValue)
            {
                panel8.HorizontalScroll.Value = newValue;
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
            playbackStopwatch = new Stopwatch();
            autoScrollTimer = new Timer();
            autoScrollTimer.Interval = 1;
            autoScrollTimer.Tick += new EventHandler(AutoScrollTimer_Tick);
            autoScrollTimer.Enabled = false;
            playbackTimer = new Timer { Interval = 1 };
            playbackTimer.Tick += PlaybackTimer_Tick;
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
            autoScrollTimer.Tick += new EventHandler(AutoScrollTimer_Tick);
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
            isDraggingTracker = true;
            wasPlayingBeforeDrag = PreviewBox.IsPlaying;
            if (!wasPlayingBeforeDrag)
            {
                PausePreview();
            }
        }



        private void EditingRuller_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                int visibleStart = panel8.HorizontalScroll.Value;
                int visibleEnd = visibleStart + panel8.ClientRectangle.Width;

                // Buffer in pixels to ensure the tracker does not go beyond the video's logical bounds.
                float bufferPixels = 0.1f * pixelsPerMillisecond; // Assuming 0.1 ms is a meaningful buffer for your video's scale.

                // Apply buffer to tracker position to limit its range
                float maxXPosition = widthVideo - bufferPixels;
                // Clamp the tracker position with buffer limits
                float proposedX = Math.Max(0.1f, Math.Min(e.X, maxXPosition));

                trackerXPosition = proposedX; // This should now be properly constrained within the buffered range.
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
                PreviewBox.Time = (long)(currentPlaybackTime);

                UpdatePlaybackLabel(currentPlaybackTime);

                // Determine auto-scroll based on the tracker position
                if (trackerXPosition <= visibleStart + 50 && !autoScrollTimer.Enabled)
                {
                    autoScrollDirection = -1;
                    autoScrollTimer.Start();
                }
                else if (trackerXPosition >= visibleEnd - 50 && !autoScrollTimer.Enabled)
                {
                    autoScrollDirection = 1;
                    autoScrollTimer.Start();
                }
                else if (trackerXPosition > visibleStart + 50 && trackerXPosition < visibleEnd - 50)
                {
                    autoScrollDirection = 0;
                    autoScrollTimer.Stop();
                }

                // Directly invalidate necessary UI components
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }
        }












        private void EditingRuller_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                trackerXPosition = Math.Max(0, Math.Min(e.X, widthVideo));
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
                PreviewBox.Time = (long)currentPlaybackTime;
                UpdatePlaybackLabel(currentPlaybackTime);
                isDraggingTracker = false;
                if (!isUpdatingUI)
                {
                    isUpdatingUI = true;
                    EditingRuller.Invalidate();
                    VideoTrack.Invalidate();
                    AudioTrack.Invalidate();
                    isUpdatingUI = false;
                }
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


        private void AutoScrollTimer_Tick(object sender, EventArgs e)
        {
            int scrollIncrement = autoScrollDirection * 40;
            int newScrollValue = panel8.HorizontalScroll.Value + scrollIncrement;
            newScrollValue = Math.Max(0, Math.Min(newScrollValue, panel8.HorizontalScroll.Maximum));

            if (newScrollValue != panel8.HorizontalScroll.Value)
            {
                panel8.HorizontalScroll.Value = newScrollValue;
                trackerXPosition += scrollIncrement;

                int visibleStart = panel8.HorizontalScroll.Value;
                int visibleEnd = visibleStart + panel8.ClientRectangle.Width;

                if (!(trackerXPosition <= visibleStart + 50 || trackerXPosition >= visibleEnd - 50))
                {
                    autoScrollTimer.Stop();
                }

                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }
            else
            {
                autoScrollTimer.Stop();
            }
        }


        private void UpdatePlaybackLabel(float playbackTime)
        {
            long videoDurationMs = (long)(fullVideo.Sum(segment => segment.EndTime - segment.StartTime) * 1000);
            playbackTime = Math.Min(playbackTime, videoDurationMs); // Ensure time does not exceed video length

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
        private async Task AddVideoToTimeline(string filePath)
        {
            button4.Enabled = false;
            float videoDuration = (float)GetVideoDuration(filePath, ffmpegPath);
            float pixelsPerSecond = 50f;
            int barsPerSecond = 5;
            float barWidth = 8f;
            float barSpacing = 2f;
            float barWidthIncludingSpacing = barWidth + barSpacing;
            float newWidth = videoDuration * pixelsPerSecond;

            // UI update
            this.Invoke(new Action(() =>
            {
                widthVideo += newWidth;
                EditingRuller.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
                VideoTrack.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
                AudioTrack.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
                EditingRuller.Invalidate();
                VideoTrackPlaceholder.Invalidate();
                AudioTrackPlaceholder.Invalidate();
            }));

            List<Image> videoThumbnails = await Task.Run(() => ExtractVideoThumbnails(filePath));
            List<float> audioAmplitudes = await Task.Run(() => GetAudioAmplitudeData(filePath));

            int thumbnailCount = videoThumbnails.Count;
            float intervalInPixels = 4.0f * pixelsPerSecond;
            float videoStartPosition = widthVideo - newWidth;
            float thumbnailWidth = 100f;

            // UI update for thumbnails and audio bars
            this.Invoke(new Action(() =>
            {
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
                }

                videoThumbnails = null;


                segmentsVideoCount++;
                fullVideo.Add(new VideoSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsVideoCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

                segmentsAudioCount++;
                fullAudio.Add(new VideoSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsAudioCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

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
                EditingRuller.Invalidate();
            }));
            button4.Enabled = true;
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
            foreach (var bounds in from bounds in allVideoBounds
                                   where bounds.Contains(e.Location)
                                   select bounds)
            {
                selectedVideoBounds = bounds;
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
        public async Task<List<Image>> ExtractVideoThumbnails(string videoFilePath, double intervalSeconds = 4.0, int maxDegreeOfParallelism = 8)
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
                // Include scale parameter to resize the output image to 150x100
                string ffmpegCommand = $"-ss {timestamp} -i \"{videoFilePath}\" -vframes 1 -vf \"scale=150:100\" -q:v 2 " +
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
            foreach (var bounds in from bounds in allAudioSegments
                                   where bounds.Contains(e.Location)
                                   select bounds)
            {
                selectedAudioBounds = bounds;
            }

            AudioTrack.Invalidate();
            VideoTrack.Invalidate();
        }
    }
}
