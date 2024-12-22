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
        //VARIABLES & OBJECTS//
        //this.PreviewBox.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "VlcLibs"));//
        private string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "ffmpeg", "bin", "ffmpeg.exe");
        private string outputPath;

        public UserInterfaceForm()
        {
            //INITIALIZATIONS//
            InitializeComponent();
            InitializeRoundCorners();
            InitializeDragFunctionality();
            InitializeAutoScrollTimer();
            InitializePlayBackTimer();
            InitializePlayBackStopWatch();
            InitializingDoubleBuffersForComponents();
            //VARIABLES&ADJUSTMENTS//
            pixelsPerMillisecond = pixelsPerSecond / 1000f;
        }

        private void UserInterfaceForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            AttachEditingRullerMouseEvents();
            AttachEditingRullerPaintEvent();
            AttachFormDraggingToComponents();
            AttachPlaceHolderPaintEvents();
            ApplyRoundCorners();
        }

        //----------------------------------------------------------------------------------------//
        //-----------------------------//UX-UI Interactions//-------------------------------------//
        //----------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private Corners applyCorners;
        private DragFunctionality dragFunctionality;
        private bool isActive = false;

        ////FEATURES & EVENT HANDLERS & INTERACTIONS////

        ///
        /// Set round corners to the form. 
        /// 
        public void ApplyRoundCorners()
        {
            applyCorners.AttributesRoundCorners(this, isActive);
        }

        ///
        /// Enabling form dragging for specified components.
        /// 
        public void AttachFormDraggingToComponents()
        {
            dragFunctionality.AttachDraggingEvent(panel2, this);
            dragFunctionality.AttachDraggingEvent(panel3, this);
            dragFunctionality.AttachDraggingEvent(pictureBox1, this);
        }

        ///
        /// Initializing round corners to be applied.
        ///
        public void InitializeRoundCorners()
        {
            applyCorners = new Corners();
        }

        ///
        /// Initializing form dragging.
        ///
        public void InitializeDragFunctionality()
        {
            dragFunctionality = new DragFunctionality();
        }

        ///
        /// Initializing playbackStopwatch.
        ///
        public void InitializePlayBackStopWatch()
        {
            playbackStopwatch = new Stopwatch();
        }

        ///
        /// Initializing autoScrollTimer.
        ///
        public void InitializeAutoScrollTimer()
        {
            autoScrollTimer = new Timer();
            autoScrollTimer.Interval = 1;
            autoScrollTimer.Tick += new EventHandler(AutoScrollTimer_Tick);
            autoScrollTimer.Tick += new EventHandler(AutoScrollTimer_Tick);
            autoScrollTimer.Enabled = false;
        }

        ///
        /// Initializing playbackTimer.
        ///
        public void InitializePlayBackTimer()
        {
            playbackTimer = new Timer();
            playbackTimer.Interval = 1;
            playbackTimer.Tick += new EventHandler(PlaybackTimer_Tick);
        }

        ///
        /// Adjusts corner styling when the form is activated.
        ///
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            isActive = false;
            applyCorners.AttributesRoundCorners(this, isActive);
        }

        ///
        /// Adjusts corner styling when the form is deactivated.
        ///
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isActive = true;
            applyCorners.AttributesRoundCorners(this, isActive);
        }

        ///
        /// Starts video playback, restarts timers, and sets the 'wasPlayingBeforeDrag' flag.
        ///
        private void PlayPreview()
        {
            Debug.WriteLine("PlayPreview called from: " + new StackTrace().GetFrame(1).GetMethod().Name);
            UpdatePlaybackLabel(PreviewBox.Time);
            if (!PreviewBox.IsPlaying)
            {
                PreviewBox.Play();
                playbackTimer.Start();
                playbackStopwatch.Restart();
            }
        }

        ///
        /// Pauses video playback, stops timers, and updates playback label.
        /// 
        private void PausePreview()
        {
            Debug.WriteLine("PausePreview called from: " + new StackTrace().GetFrame(1).GetMethod().Name);
            UpdatePlaybackLabel(PreviewBox.Time);
            if (PreviewBox.IsPlaying)
            {
                PreviewBox.Pause();
                playbackTimer.Stop();
                playbackStopwatch.Stop();
            }
        }

        ///
        /// Called when the pause button is clicked; pauses the preview.
        ///
        private void PauseButton_Click(object sender, EventArgs e)
        {
            PausePreview();
        }

        ///
        /// Called when the play button is clicked; starts playing the preview.
        ///
        private void PlayButton_Click(object sender, EventArgs e)
        {
            PlayPreview();
        }

        ///
        /// Splits a selected video or audio segment at the current tracker position.
        ///
        private void Split_Click(object sender, EventArgs e)
        {
            float trackerPosition = trackerXPosition;

            if (selectedVideoBounds != RectangleF.Empty)
            {
                if (trackerPosition < selectedVideoBounds.Left || trackerPosition > selectedVideoBounds.Right)
                {
                    MessageBox.Show("Tracker is outside the selected video segment range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                float splitTimeMilliseconds = (trackerPosition - selectedVideoBounds.Left) / pixelsPerMillisecond;

                RectangleF firstSegment = new RectangleF(
                    selectedVideoBounds.Left,
                    selectedVideoBounds.Top,
                    trackerPosition - selectedVideoBounds.Left,
                    selectedVideoBounds.Height
                );

                RectangleF secondSegment = new RectangleF(
                    trackerPosition,
                    selectedVideoBounds.Top,
                    selectedVideoBounds.Right - trackerPosition,
                    selectedVideoBounds.Height
                );

                allVideoBounds.Remove(selectedVideoBounds);
                allVideoBounds.Add(firstSegment);
                allVideoBounds.Add(secondSegment);

                var originalVideo = fullVideoRender.FirstOrDefault(v => v.TimeLinePosition == selectedVideoBounds.Left / pixelsPerSecond);
                if (originalVideo != null)
                {
                    fullVideoRender.Remove(originalVideo);

                    float firstSegmentStartTime = originalVideo.StartTime;
                    float firstSegmentEndTime = originalVideo.StartTime + (splitTimeMilliseconds / 1000.0f);
                    float secondSegmentStartTime = firstSegmentEndTime;
                    float secondSegmentEndTime = originalVideo.EndTime;

                    fullVideoRender.Add(new VideoRenderSegment
                    {
                        FilePath = originalVideo.FilePath,
                        StartTime = firstSegmentStartTime,
                        EndTime = firstSegmentEndTime,
                        TimeLinePosition = firstSegment.Left / pixelsPerSecond,
                        Id = originalVideo.Id
                    });

                    fullVideoRender.Add(new VideoRenderSegment
                    {
                        FilePath = originalVideo.FilePath,
                        StartTime = secondSegmentStartTime,
                        EndTime = secondSegmentEndTime,
                        TimeLinePosition = secondSegment.Left / pixelsPerSecond,
                        Id = ++segmentsVideoCount
                    });
                }

                foreach (VideoRenderSegment segment in fullVideoRender)
                {
                    Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2}");
                }
                VideoTrack.Invalidate();
            }
            else if (selectedAudioBounds != RectangleF.Empty)
            {
                if (trackerPosition < selectedAudioBounds.Left || trackerPosition > selectedAudioBounds.Right)
                {
                    MessageBox.Show("Tracker is outside the selected audio segment range.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                float splitTimeMilliseconds = (trackerPosition - selectedAudioBounds.Left) / pixelsPerMillisecond;

                RectangleF firstSegment = new RectangleF(
                    selectedAudioBounds.Left,
                    selectedAudioBounds.Top,
                    trackerPosition - selectedAudioBounds.Left,
                    selectedAudioBounds.Height
                );

                RectangleF secondSegment = new RectangleF(
                    trackerPosition,
                    selectedAudioBounds.Top,
                    selectedAudioBounds.Right - trackerPosition,
                    selectedAudioBounds.Height
                );

                allAudioSegments.Remove(selectedAudioBounds);
                allAudioSegments.Add(firstSegment);
                allAudioSegments.Add(secondSegment);

                var originalAudio = fullAudioRender.FirstOrDefault(a => a.TimeLinePosition == selectedAudioBounds.Left / pixelsPerSecond);
                if (originalAudio != null)
                {
                    fullAudioRender.Remove(originalAudio);
                    float firstSegmentStartTime = originalAudio.StartTime;
                    float firstSegmentEndTime = originalAudio.StartTime + (splitTimeMilliseconds / 1000.0f);
                    float secondSegmentStartTime = firstSegmentEndTime;
                    float secondSegmentEndTime = originalAudio.EndTime;

                    fullAudioRender.Add(new AudioRenderSegment
                    {
                        FilePath = originalAudio.FilePath,
                        StartTime = firstSegmentStartTime,
                        EndTime = firstSegmentEndTime,
                        TimeLinePosition = firstSegment.Left / pixelsPerSecond,
                        Id = originalAudio.Id
                    });

                    fullAudioRender.Add(new AudioRenderSegment
                    {
                        FilePath = originalAudio.FilePath,
                        StartTime = secondSegmentStartTime,
                        EndTime = secondSegmentEndTime,
                        TimeLinePosition = secondSegment.Left / pixelsPerSecond,
                        Id = ++segmentsAudioCount
                    });
                }
                foreach (AudioRenderSegment segment in fullAudioRender)
                {
                    Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2}");
                }
                AudioTrack.Invalidate();
            }
            else
            {
                MessageBox.Show("No segment selected. Please select a video or audio segment to split.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///
        /// Exits the application when the close button is clicked.
        ///
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        ///
        /// Updates the tracker's position based on VLC time or interpolated system time, ensuring smooth playback.
        ///
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

        //----------------------------------------------------------------------------------------//
        //-----------------------------//RULLER HANDLING//----------------------------------------//
        //----------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private Timer autoScrollTimer;
        private Timer playbackTimer;
        private Stopwatch playbackStopwatch;
        private int autoScrollDirection = 0;
        private long lastKnownVlcTime = 0;
        private float trackerXPosition = 0f;
        private float currentPlaybackTime = 0f;
        private float pixelsPerSecond = 50f;
        private float pixelsPerMillisecond;
        private float widthVideo = 0f;
        private bool wasPlayingBeforeDrag = false;
        private bool isDraggingTracker = false;

        ////PAINT & EVENT HANDLERS////

        ///
        /// Attaching ruller paint.
        ///
        public void AttachEditingRullerPaintEvent()
        {
            EditingRuller.Paint += EditingRuller_Paint;
        }

        ///
        /// Attach the ruller's mouse events.
        /// 
        public void AttachEditingRullerMouseEvents()
        {
            EditingRuller.MouseDown += EditingRuller_MouseDown;
            EditingRuller.MouseMove += EditingRuller_MouseMove;
            EditingRuller.MouseUp += EditingRuller_MouseUp;
        }

        ///
        /// Updates the tracker's position based on VLC time or interpolated system time, ensuring smooth playback.
        ///
        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingTracker)
            {
                long elapsedSinceLastKnown = playbackStopwatch.ElapsedMilliseconds;
                long interpolatedTime = lastKnownVlcTime + elapsedSinceLastKnown;
                float newPosition = interpolatedTime * pixelsPerMillisecond;
                float maxPosition = widthVideo;
                trackerXPosition = Math.Min(newPosition, maxPosition);
                EnsureTrackerVisible(trackerXPosition);
                UpdatePlaybackLabel(interpolatedTime);
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }
            long currentVlcTime = PreviewBox.Time;

            if (currentVlcTime != lastKnownVlcTime)
            {
                lastKnownVlcTime = currentVlcTime;
                playbackStopwatch.Restart();
            }
        }

        ///
        /// Scrolls the timeline horizontally if the tracker goes beyond the visible area.
        ///
        private void EnsureTrackerVisible(float trackerXPosition)
        {
            int visibleStart = panel8.HorizontalScroll.Value;
            int visibleEnd = visibleStart + panel8.ClientRectangle.Width;
            const int padding = 50;

            if (trackerXPosition < visibleStart + padding)
            {
                panel8.HorizontalScroll.Value = Math.Max(0, (int)(trackerXPosition - padding));
            }
            else if (trackerXPosition > visibleEnd - padding)
            {
                panel8.HorizontalScroll.Value = Math.Min(panel8.HorizontalScroll.Maximum,
                                                         (int)(trackerXPosition - panel8.ClientRectangle.Width + padding));
            }
        }

        ///
        /// Occurs when the user presses the mouse button on the timeline ruler; sets up tracker drag state.
        ///
        private void EditingRuller_MouseDown(object sender, MouseEventArgs e)
        {
            isDraggingTracker = true;
            wasPlayingBeforeDrag = PreviewBox.IsPlaying;
            if (wasPlayingBeforeDrag)
            {
                PausePreview();
            }
            else
            {
                PlayPreview();
                PausePreview();
            }
        }

        ///
        /// Occurs when the user moves the mouse on the timeline while dragging the tracker.
        ///
        private void EditingRuller_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                int visibleStart = panel8.HorizontalScroll.Value;
                int visibleEnd = visibleStart + panel8.ClientRectangle.Width;
                float bufferPixels = 0.1f * pixelsPerMillisecond;
                float maxXPosition = widthVideo - bufferPixels;
                float proposedX = Math.Max(0.1f, Math.Min(e.X, maxXPosition));

                trackerXPosition = proposedX;
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
                PreviewBox.Time = (long)(currentPlaybackTime);

                if (!wasPlayingBeforeDrag)
                {
                    PausePreview();
                }

                UpdatePlaybackLabel(currentPlaybackTime);

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

                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }
        }

        ///
        /// Occurs when the user releases the mouse button on the timeline; finalizes tracker movement.
        ///
        private void EditingRuller_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                trackerXPosition = Math.Max(0, Math.Min(e.X, widthVideo));
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
                PreviewBox.Time = (long)currentPlaybackTime;
                UpdatePlaybackLabel(currentPlaybackTime);
                isDraggingTracker = false;
                autoScrollTimer.Stop();
                autoScrollDirection = 0;
                lastKnownVlcTime = (long)currentPlaybackTime;
                playbackStopwatch.Reset();
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

        ///
        /// Paints the ruler, draws major/minor ticks, and renders the tracker arrow on the timeline.
        ///
        private void EditingRuller_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Gray);
            int panelWidth = EditingRuller.Width;
            float tickHeightMajor = 6;
            float tickHeightMinor = 3;

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

        ///
        /// Occurs at each auto-scroll timer tick, moves the scroll position if needed.
        ///
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

        //---------------------------------------------------------------------------------------//
        //-----------------------------//VIDEO HANDLING//----------------------------------------//
        //---------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private readonly List<(Image thumbnail, float position)> allThumbnailsWithPositions = new List<(Image, float)>();
        private List<VideoRenderSegment> fullVideoRender = new List<VideoRenderSegment>();
        private List<RectangleF> allVideoBounds = new List<RectangleF>();
        private RectangleF selectedVideoBounds = RectangleF.Empty;
        private int segmentsVideoCount = 0;

        class VideoRenderSegment
        {
            public string FilePath { get; set; }
            public int Id { get; set; }
            public float StartTime { get; set; }
            public float EndTime { get; set; }
            public float TimeLinePosition { get; set; }
        }

        ////PAINT & EVENT HANDLERS////

        ///
        /// Paint event for the placeholder panel under video track (decorative pattern).
        ///
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

        ///
        /// Paint event handler for the actual video track; draws thumbnails, segment borders, and tracker line.
        ///
        private void VideoTrack_PaintHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using (Brush backgroundBrush = new SolidBrush(Color.Gray))
            {
                g.FillRectangle(backgroundBrush, 0, 0, widthVideo, VideoTrack.Height);
            }
            using (Brush transparentBrush = new SolidBrush(Color.Transparent))
            {
                g.FillRectangle(transparentBrush, widthVideo, 0, VideoTrack.Width - widthVideo, VideoTrack.Height);
            }
            foreach (var (thumbnail, position) in allThumbnailsWithPositions)
            {
                float thumbnailHeight = VideoTrack.Height - 20f;
                float thumbnailY = 10f;
                g.DrawImage(thumbnail, position, thumbnailY, 100, thumbnailHeight);
            }
            using (Brush videoBrush = new SolidBrush(Color.Transparent))
            using (Pen borderPen = new Pen(Color.Green, 2))
            using (Pen selectedPen = new Pen(Color.Red, 3))
            {
                foreach (var bounds in allVideoBounds)
                {
                    g.FillRectangle(videoBrush, bounds);
                    g.DrawRectangle(bounds == selectedVideoBounds ? selectedPen : borderPen, Rectangle.Round(bounds));
                }
            }
            using (Pen trackerPen = new Pen(Color.Blue, 2))
            {
                g.DrawLine(trackerPen, trackerXPosition, 0, trackerXPosition, VideoTrack.Height);
            }
        }

        ///
        /// MouseDown handler for the video track; marks the selected region if clicked inside a segment.
        ///
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

        ///
        /// Calculates the duration of a video using FFmpeg.
        ///
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

        ///
        /// Extracts video thumbnails at regular intervals using FFmpeg; returns a list of images.
        ///
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
                        File.Delete(outputFile);
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

        ///
        /// Generates a preview video using FFmpeg by concatenating/trimming segments.
        ///
        private async Task GeneratePreview(int targetSizeMB = 30)
        {
            List<string> filterComplexVideo = new List<string>();
            List<string> filterComplexAudio = new List<string>();
            double totalDurationSeconds = fullVideoRender.Sum(segment => segment.EndTime - segment.StartTime);
            int fileIndex = 0;
            int targetWidth = 320;
            int targetHeight = 180;
            int targetFps = 15;
            int audioBitrateKbps = 128;
            int totalBitrate = (int)((targetSizeMB * 8192) / totalDurationSeconds) - audioBitrateKbps;
            string videoFormat = "yuv420p";
            string videoBitrate = totalBitrate.ToString() + "k";
            string previewDirectory = Path.Combine(Path.GetTempPath(), "VideoPreviews");

            if (!Directory.Exists(previewDirectory))
            {
                Directory.CreateDirectory(previewDirectory);
            }
            outputPath = Path.Combine(previewDirectory, $"preview_{DateTime.Now:yyyyMMddHHmmssfff}.mp4");
            StringBuilder ffmpegCmd = new StringBuilder("-y ");

            foreach (var segment in fullVideoRender)
            {
                ffmpegCmd.AppendFormat("-hwaccel cuda -c:v h264_cuvid -i \"{0}\" ", segment.FilePath);
                filterComplexVideo.Add($"[{fileIndex}:v]trim=start={segment.StartTime}:end={segment.EndTime},setpts=PTS-STARTPTS,scale={targetWidth}:{targetHeight},fps={targetFps},setsar=1,format={videoFormat}[v{fileIndex}];");
                filterComplexAudio.Add($"[{fileIndex}:a]atrim=start={segment.StartTime}:end={segment.EndTime},asetpts=PTS-STARTPTS,aformat=sample_fmts=fltp:sample_rates=44100:channel_layouts=stereo[a{fileIndex}];");
                fileIndex++;
            }

            if (fileIndex > 0)
            {
                string videoFilter = string.Join("", Enumerable.Range(0, fileIndex).Select(i => $"[v{i}]"));
                string audioFilter = string.Join("", Enumerable.Range(0, fileIndex).Select(i => $"[a{i}]"));
                string filterComplex = $"{string.Join("", filterComplexVideo)}{videoFilter}concat=n={fileIndex}:v=1:a=0[outv];{string.Join("", filterComplexAudio)}{audioFilter}concat=n={fileIndex}:v=0:a=1[outa]";
                ffmpegCmd.Append($"-filter_complex \"{filterComplex}\" ");
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

        ///
        /// Asynchronously adds a video to the timeline, generates thumbnails/audio bars, and triggers preview generation.
        ///
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

            if ((widthVideo + newWidth) / pixelsPerSecond > 600) // 600 seconds = 10 minutes
            {
                MessageBox.Show("Adding this video will exceed the 10-minute limit. Please adjust your timeline.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = true;
                return; // Exit without modifying the timeline
            }

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
                fullVideoRender.Add(new VideoRenderSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsVideoCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

                segmentsAudioCount++;
                fullAudioRender.Add(new AudioRenderSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsAudioCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

                VideoTrackPlaceholder.Location = new Point(Math.Max((int)Math.Round(widthVideo), VideoTrackPlaceholder.Location.X), VideoTrackPlaceholder.Location.Y);
                AudioTrackPlaceholder.Location = new Point(Math.Max((int)Math.Round(widthVideo), AudioTrackPlaceholder.Location.X), AudioTrackPlaceholder.Location.Y);

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

        //---------------------------------------------------------------------------------------//
        //-----------------------------//AUDIO HANDLING//----------------------------------------//
        //---------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private RectangleF selectedAudioBounds = RectangleF.Empty;
        private Dictionary<RectangleF, RectangleF> videoToAudioMapping = new Dictionary<RectangleF, RectangleF>();
        private List<RectangleF> allAudioSegments = new List<RectangleF>();
        private List<RectangleF> allAudioAmplitudeBars = new List<RectangleF>();
        private List<AudioRenderSegment> fullAudioRender = new List<AudioRenderSegment>();
        private int segmentsAudioCount = 0;
        class AudioRenderSegment
        {
            public string FilePath { get; set; }
            public int Id { get; set; }
            public float StartTime { get; set; }
            public float EndTime { get; set; }
            public float TimeLinePosition { get; set; }
        }

        ////PAINT & EVENT HANDLERS////

        ///
        /// Paint event for the placeholder panel under audio track (decorative pattern).
        ///
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

        ///
        /// Paint event handler for the audio track; draws amplitude bars, segment borders, and the tracker line.
        ///
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

        ///
        /// MouseDown handler for the audio track; marks the selected region if clicked inside a segment.
        ///
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

        ///
        /// Extracts audio amplitude data from a video file for waveform visualization.
        ///
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

        //----------------------------------------------------------------------------------------//
        //-------------------------------//HELP METHODS//-----------------------------------------//
        //----------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private bool isUpdatingUI = false;


        ////FUNCTIONS////

        ///
        /// Attaches placeholder events.
        /// 
        public void AttachPlaceHolderPaintEvents()
        {
            VideoTrackPlaceholder.Paint += VideoTrackPlaceholder_Paint_1;
            AudioTrackPlaceholder.Paint += AudioTrackPlaceholder_Paint;
        }

        ///
        /// Sets double buffered value to true on specified components.
        ///
        public void InitializingDoubleBuffersForComponents()
        {
            SetDoubleBuffered(EditingRuller);
            SetDoubleBuffered(VideoTrackPlaceholder);
            SetDoubleBuffered(AudioTrackPlaceholder);
            SetDoubleBuffered(VideoTrack);
            SetDoubleBuffered(AudioTrack);
            SetDoubleBuffered(panel8);
            SetDoubleBuffered(this);
        }

        ///
        /// Updates the playback label to display the current time in mm:ss:ms format, ensuring the displayed time does not exceed the total duration.
        ///
        private void UpdatePlaybackLabel(float playbackTime)
        {
            long videoDurationMs = (long)(fullVideoRender.Sum(segment => segment.EndTime - segment.StartTime) * 1000);
            playbackTime = Math.Min(playbackTime, videoDurationMs);

            int minutes = (int)(playbackTime / 60000);
            int seconds = (int)((playbackTime % 60000) / 1000);
            int milliseconds = (int)(playbackTime % 1000);
            string timeFormatted = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";

            TimeStamp.Text = timeFormatted;
        }

        ///
        /// Handles timeline scrolling and invalidates the relevant panels.
        ///
        private void OnScroll(object sender, ScrollEventArgs e)
        {
            panel8.Invalidate();
            EditingRuller.Invalidate();
            VideoTrack.Invalidate();
            AudioTrack.Invalidate();
            VideoTrackPlaceholder.Invalidate();
            AudioTrackPlaceholder.Invalidate();
        }

        ///
        /// Uses LockWindowUpdate for smoother scrolling (double buffering at scroll).
        ///
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

        ///
        /// Enables WS_EX_COMPOSITED for reducing flicker in WinForms.
        ///
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

        ///
        /// Sets the DoubleBuffered property at runtime to reduce flickering.
        ///
        private static void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
    }
}
