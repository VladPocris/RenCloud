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
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Forms;
using static RenCloud.Program;

namespace RenCloud
{
    public partial class UserInterfaceForm : Form
    {
        //VARIABLES & OBJECTS//
        //this.PreviewBox.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "VlcLibs"));//
        private string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "ffmpeg", "bin", "ffmpeg.exe");
        private string outputPath = Path.Combine(Path.GetTempPath(), "VideoPreviews");

        public UserInterfaceForm()
        {
            //INITIALIZATIONS//
            InitializeComponent();
            InitializeRoundCorners();
            InitializeDragFunctionality();
            InitializeAutoScrollTimer();
            InitializePlayBackTimer();
            InitializePlayBackStopWatch();
            InitializeMouseEvents();
            InitializingDoubleBuffersForComponents();
            ClearingTempPaths();

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
        private int segmentsToLeft = 0;
        private bool isActive = false;
        private bool isDraggingSegment = false;
        private bool swapping = false;
        private bool rightMv = false;
        private Point initialMousePosition;
        private RectangleF initialSegmentBounds;
        private List<(Image thumbnail, float position)> tempPositions = new List<(Image, float)>();
        private List<(Image thumbnail, float position)> draggedThumbnails = new List<(Image, float)>();
        private List<(Image thumbnail, float position)> draggedThumbnailsInitialPosition = new List<(Image, float)>();
        private List<(RectangleF bar, int id)> tempPositionsBars = new List<(RectangleF bar, int id)>();
        private List<(RectangleF bar, int id)> draggedBars = new List<(RectangleF bar, int id)>();
        private List<(RectangleF bar, int id)> draggedBarsInitialPosition = new List<(RectangleF bar, int id)>();

        ////FEATURES & EVENT HANDLERS & INTERACTIONS////

        ///
        /// MouseDown handler for the audio track; marks the selected region if clicked inside a segment and storing necessary info for dragging selected segments.
        ///
        private void AudioTrack_MouseDownHandler(object sender, MouseEventArgs e)
        {
            selectedVideoBounds = Rectangle.Empty;
            selectedAudioBounds = Rectangle.Empty;
            draggedBars.Clear();
            draggedBarsInitialPosition.Clear();

            foreach (var bar in allAudioAmplitudeBars)
            {
                tempPositionsBars.Add(bar);
            }
            foreach (var bounds in allAudioSegments)
            {
                if (bounds.Contains(e.Location))
                {
                    selectedAudioBounds = bounds;
                    isDraggingSegment = true;
                    initialMousePosition = e.Location;
                    initialSegmentBounds = bounds;

                    foreach (var (bar, id) in allAudioAmplitudeBars)
                    {
                        if (bar.X >= bounds.Left && bar.X < bounds.Right)
                        {
                            draggedBars.Add((bar, id));
                            draggedBarsInitialPosition.Add((bar, id));
                        }
                    }
                    break;
                }
            }
            segmentsToLeft = allAudioSegments.Count(segment => segment.Right <= selectedAudioBounds.Left);
            AudioTrack.Invalidate();
        }


        ///
        /// MouseMove handler for the audio track; moves the selected segment and images based on user movement.
        ///
        private void AudioTrack_MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (isDraggingSegment && selectedAudioBounds != RectangleF.Empty)
            {
                float offsetX = e.Location.X - initialMousePosition.X;

                float newSegmentLeft = Math.Max(0, initialSegmentBounds.Left + offsetX);
                newSegmentLeft = Math.Min(AudioTrack.Width - initialSegmentBounds.Width, newSegmentLeft);
                RectangleF updatedBounds = new RectangleF(
                    newSegmentLeft,
                    initialSegmentBounds.Top,
                    initialSegmentBounds.Width,
                    initialSegmentBounds.Height
                );

                int segmentIndex = allAudioSegments.IndexOf(selectedAudioBounds);
                allAudioSegments[segmentIndex] = updatedBounds;

                float segmentOffset = newSegmentLeft - selectedAudioBounds.Left;

                for (int i = 0; i < draggedBars.Count; i++)
                {
                    var (bar, id) = draggedBars[i];
                    var originalPosition = bar.X;
                    float newPosition = bar.X + segmentOffset;
                    draggedBars[i] = (new RectangleF(
                        newPosition,
                        bar.Top,
                        bar.Width,
                        bar.Height
                        ), id);

                    int globalIndex = allAudioAmplitudeBars.FindIndex(t => t.id == id && t.bar.X == originalPosition);
                    if (globalIndex != -1)
                    {
                        allAudioAmplitudeBars[globalIndex] = (new RectangleF(
                        newPosition,
                        bar.Top,
                        bar.Width,
                        bar.Height
                        ), id);
                    }
                }
                foreach (RectangleF otherSegment in allAudioSegments)
                {
                    if (otherSegment != updatedBounds)
                    {
                        float otherSegmentMidpoint = otherSegment.Left + (otherSegment.Width / 2);
                        if (updatedBounds.Left <= otherSegmentMidpoint && updatedBounds.Right >= otherSegmentMidpoint)
                        {
                            swapping = true;
                            break;
                        }
                    }
                }
                selectedAudioBounds = updatedBounds;
                AudioTrack.Invalidate();
            }
        }


        ///
        /// MouseUp handler for the audio track; on release specifies logic whether to move the segment or not and clearing information.
        /// 
        private void AudioTrack_MouseUpHandler(object sender, MouseEventArgs e)
        {
            int index = 0;
            if (isDraggingSegment)
            {
                int draggedIndex = allAudioSegments.IndexOf(selectedAudioBounds);
                RectangleF draggedSegment = selectedAudioBounds;

                if (draggedIndex != -1)
                {
                    int newIndex = DetermineNewAudioIndex(draggedIndex, draggedSegment);
                    bool segmentMoved = newIndex != -1 && newIndex != draggedIndex;

                    if (segmentMoved)
                    {
                        index = newIndex > draggedIndex ? newIndex - 1 : newIndex;
                        MoveAudioSegment(draggedIndex, newIndex);
                        NormalizeAudioSegmentPositions();
                        selectedAudioBounds = allAudioSegments[index];
                        SyncFullAudioRender();
                        UpdateBarPositions(index);
                        draggedSegment = selectedAudioBounds;
                        if (rightMv)
                        {
                            UpdateBarsForRightDraggingFix();
                            rightMv = false;
                        }
                        else
                        {
                            if (selectedAudioBounds.Left < 0.1f) {
                                UpdateBarsForLeftDraggingFix(index + 1); 
                            }
                        }
                    }
                    else
                    {
                        RevertSegmentAndBars();
                        ResetDraggingState();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Dragged segment index not found.");
                }
                ResetDraggingState();
            }
            AudioTrack.Invalidate();
            GeneratePreview();
        }

        ///
        /// Making sure it's not only moved visually but from data perspective also.
        ///
        private void MoveAudioSegment(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex) return;

            RectangleF segment = allAudioSegments[oldIndex];
            allAudioSegments.RemoveAt(oldIndex);

            if (newIndex > oldIndex) newIndex--;

            allAudioSegments.Insert(newIndex, segment);

            var segmentData = fullAudioRender[oldIndex];
            fullAudioRender.RemoveAt(oldIndex);

            fullAudioRender.Insert(newIndex, segmentData);

            AudioTrack.Invalidate();
        }

        ///
        /// Function to normalize segments when their positions are changed.
        ///
        private void NormalizeAudioSegmentPositions()
        {
            int debug = 0;
            float currentLeft = 0;

            for (int i = 0; i < allAudioSegments.Count; i++)
            {
                RectangleF segment = allAudioSegments[i];
                if (Math.Abs(segment.Left - currentLeft) > 0.1f)
                {
                    allAudioSegments[i] = new RectangleF(
                        currentLeft,
                        segment.Top,
                        segment.Width,
                        segment.Height
                    );
                    if (initialSegmentBounds.Left > currentLeft)
                    {
                        if(currentLeft != 0)
                        {
                            UpdateBarsForLeftDragging();
                        }
                    }
                    else
                    {
                        if (debug < 1)
                        {
                            UpdateBarsForRightDragging(segment, currentLeft);
                        }
                        debug++;
                    }
                    if (i < fullAudioRender.Count)
                    {
                        fullAudioRender[i].TimeLinePosition = currentLeft / pixelsPerSecond;
                    }
                }
                currentLeft += segment.Width;
            }
            AudioTrack.Invalidate();
        }

        ///
        /// MouseDown handler for the video track; marks the selected region if clicked inside a segment and storing necessary info for dragging selected segments.
        ///
        private void VideoTrack_MouseDownHandler(object sender, MouseEventArgs e)
        {
            selectedVideoBounds = Rectangle.Empty;
            selectedAudioBounds = Rectangle.Empty;
            draggedThumbnails.Clear();
            draggedThumbnailsInitialPosition.Clear();

            foreach (var (thumbnail, position) in allThumbnailsWithPositions)
            {
                tempPositions.Add((thumbnail, position));
            }
            foreach (var bounds in allVideoBounds)
            {
                if (bounds.Contains(e.Location))
                {
                    selectedVideoBounds = bounds;
                    isDraggingSegment = true;
                    initialMousePosition = e.Location;
                    initialSegmentBounds = bounds;

                    foreach (var (thumbnail, position) in allThumbnailsWithPositions)
                    {
                        if (position >= bounds.Left && position < bounds.Right)
                        {
                            draggedThumbnails.Add((thumbnail, position));
                            draggedThumbnailsInitialPosition.Add((thumbnail, position));
                        }
                    }
                    break;
                }
            }
            VideoTrack.Invalidate();
        }

        ///
        /// MouseMove handler for the video track; moves the selected segment and images based on user movement.
        ///
        private void VideoTrack_MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (isDraggingSegment && selectedVideoBounds != RectangleF.Empty)
            {
                float offsetX = e.Location.X - initialMousePosition.X;

                float newSegmentLeft = Math.Max(0, initialSegmentBounds.Left + offsetX);
                newSegmentLeft = Math.Min(VideoTrack.Width - initialSegmentBounds.Width, newSegmentLeft);
                RectangleF updatedBounds = new RectangleF(
                    newSegmentLeft,
                    initialSegmentBounds.Top,
                    initialSegmentBounds.Width,
                    initialSegmentBounds.Height
                );

                int segmentIndex = allVideoBounds.IndexOf(selectedVideoBounds);
                allVideoBounds[segmentIndex] = updatedBounds;

                float segmentOffset = newSegmentLeft - selectedVideoBounds.Left;

                for (int i = 0; i < draggedThumbnails.Count; i++)
                {
                    var (thumbnail, originalPosition) = draggedThumbnails[i];
                    float newPosition = originalPosition + segmentOffset;
                    draggedThumbnails[i] = (thumbnail, newPosition);

                    int globalIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail && t.position == originalPosition);
                    if (globalIndex != -1)
                    {
                        allThumbnailsWithPositions[globalIndex] = (thumbnail, newPosition);
                    }
                }
                foreach (RectangleF otherSegment in allVideoBounds)
                {
                    if (otherSegment != updatedBounds)
                    {
                        float otherSegmentMidpoint = otherSegment.Left + (otherSegment.Width / 2);
                        if (updatedBounds.Left <= otherSegmentMidpoint && updatedBounds.Right >= otherSegmentMidpoint)
                        {
                            swapping = true;
                            break;
                        }
                    }
                }
                selectedVideoBounds = updatedBounds;
                VideoTrack.Invalidate();
            }
        }

        ///
        /// MouseUp handler for the video track; on release specifies logic whether to move the segment or not and clearing information.
        /// 
        private void VideoTrack_MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (isDraggingSegment)
            {
                int draggedIndex = allVideoBounds.IndexOf(selectedVideoBounds);
                RectangleF draggedSegment = selectedVideoBounds;
                int index = 0;

                if (draggedIndex != -1)
                {
                    int newIndex = DetermineNewIndex(draggedIndex, draggedSegment);
                    bool segmentMoved = newIndex != -1 && newIndex != draggedIndex;

                    if (segmentMoved)
                    {
                        index = newIndex > draggedIndex ? newIndex - 1 : newIndex;
                        MoveSegment(draggedIndex, newIndex);
                        NormalizeSegmentPositions();
                        SyncFullVideoRender();
                        UpdateThumbnailPositions(index);
                        selectedVideoBounds = allVideoBounds[index];
                        draggedSegment = selectedVideoBounds;
                        if (rightMv)
                        {
                            FixImages(draggedSegment);
                            rightMv = false;
                        }
                        else
                        {
                            if (selectedVideoBounds.Left < 0.01f)
                            {
                                UpdateThumbnailsForLeftDraggingFix(index + 1);
                            }
                        }
                    }
                    else
                    {
                        RevertSegmentAndThumbnails();
                        ResetDraggingState();
                        VideoTrack.Invalidate();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Dragged segment index not found.");
                }

                ResetDraggingState();
            }
            else
            {
                Console.WriteLine("MouseUp detected without dragging.");
            }
            VideoTrack.Invalidate();
            GeneratePreview();
        }

        ///
        ///  Making sure it's not only moved visually but from data perspective also.
        ///
        private void MoveSegment(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex) return;

            RectangleF segment = allVideoBounds[oldIndex];
            allVideoBounds.RemoveAt(oldIndex);

            if (newIndex > oldIndex) newIndex--;

            allVideoBounds.Insert(newIndex, segment);

            var segmentData = fullVideoRender[oldIndex];
            fullVideoRender.RemoveAt(oldIndex);

            if (newIndex > oldIndex) newIndex--;

            fullVideoRender.Insert(newIndex, segmentData);

            VideoTrack.Invalidate();
        }

        ///
        /// Function to normalize segments when their positions are changed.
        ///
        private void NormalizeSegmentPositions()
        {
            float currentLeft = 0;

            for (int i = 0; i < allVideoBounds.Count; i++)
            {
                RectangleF segment = allVideoBounds[i];

                if (Math.Abs(segment.Left - currentLeft) > 0.1f)
                {

                    allVideoBounds[i] = new RectangleF(
                        currentLeft,
                        segment.Top,
                        segment.Width,
                        segment.Height
                    );

                    if (initialSegmentBounds.Left > currentLeft)
                    {
                        UpdateThumbnailsForLeftDragging();
                    }
                    else
                    {
                        UpdateThumbnailsForRightDragging(segment, currentLeft);
                    }

                    if (i < fullVideoRender.Count)
                    {
                        fullVideoRender[i].TimeLinePosition = currentLeft / pixelsPerSecond;
                    }
                }

                currentLeft += segment.Width;
            }

            VideoTrack.Invalidate();
        }

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
            autoScrollTimer.Interval = 2;
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
            UpdatePlaybackLabel(PreviewBox.Time);
            if (!PreviewBox.IsPlaying && PreviewBox.GetCurrentMedia() != null)
            {
                PreviewBox.Play();
                playbackTimer.Start();
                playbackStopwatch.Restart();
                playbackStopwatch.Stop();
            }
        }

        ///
        /// Pauses video playback, stops timers, and updates playback label.
        /// 
        private void PausePreview()
        {
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
            float epsilon = 0.01f;

            if (selectedVideoBounds == RectangleF.Empty && selectedAudioBounds == RectangleF.Empty)
            {
                MessageBox.Show("No segment selected. Please select a video or audio segment to split.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            float trackerPosition = trackerXPosition;

            if (selectedVideoBounds != RectangleF.Empty)
            {
                if (trackerPosition < selectedVideoBounds.Left || trackerPosition > selectedVideoBounds.Right)
                {
                    MessageBox.Show("Tracker is outside the selected video segment range.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                float oldStartTime = selectedVideoBounds.Left / pixelsPerSecond;
                float splitOffset = trackerPosition - selectedVideoBounds.Left;

                var originalVideo = fullVideoRender.FirstOrDefault(v =>
                    Math.Abs(v.TimeLinePosition - oldStartTime) < epsilon);

                if (originalVideo == null)
                {
                    MessageBox.Show("Could not find the original video segment for the selected bounds.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (splitOffset <= 0 || splitOffset >= selectedVideoBounds.Width)
                {
                    MessageBox.Show("Cannot split at the boundary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RectangleF firstSegment = new RectangleF(selectedVideoBounds.Left, selectedVideoBounds.Top, splitOffset, selectedVideoBounds.Height);
                RectangleF secondSegment = new RectangleF(trackerPosition, selectedVideoBounds.Top, selectedVideoBounds.Right - trackerPosition, selectedVideoBounds.Height);

                int index = allVideoBounds.IndexOf(selectedVideoBounds);
                allVideoBounds.RemoveAt(index);
                allVideoBounds.Insert(index, firstSegment);
                allVideoBounds.Insert(index + 1, secondSegment);

                fullVideoRender.RemoveAt(index);
                fullVideoRender.Insert(index, new VideoRenderSegment
                {
                    FilePath = originalVideo.FilePath,
                    StartTime = originalVideo.StartTime,
                    EndTime = originalVideo.StartTime + (splitOffset / pixelsPerSecond),
                    TimeLinePosition = firstSegment.Left / pixelsPerSecond,
                    Id = ++segmentsVideoCount
                });
                fullVideoRender.Insert(index + 1, new VideoRenderSegment
                {
                    FilePath = originalVideo.FilePath,
                    StartTime = originalVideo.StartTime + (splitOffset / pixelsPerSecond),
                    EndTime = originalVideo.EndTime,
                    TimeLinePosition = secondSegment.Left / pixelsPerSecond,
                    Id = ++segmentsVideoCount
                });

                selectedVideoBounds = firstSegment;

                NormalizeSegmentPositions();
                VideoTrack.Invalidate();
            }

            if (selectedAudioBounds != RectangleF.Empty)
            {
                if (trackerPosition < selectedAudioBounds.Left || trackerPosition > selectedAudioBounds.Right)
                {
                    MessageBox.Show("Tracker is outside the selected audio segment range.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                float oldStartTime = selectedAudioBounds.Left / pixelsPerSecond;
                float splitOffset = trackerPosition - selectedAudioBounds.Left;

                var originalAudio = fullAudioRender.FirstOrDefault(a =>
                    Math.Abs((a.TimeLinePosition * pixelsPerSecond) - selectedAudioBounds.Left) < epsilon);

                if (originalAudio == null)
                {
                    MessageBox.Show("Could not find the original audio segment for the selected bounds.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (splitOffset <= 0 || splitOffset >= selectedAudioBounds.Width)
                {
                    MessageBox.Show("Cannot split at the boundary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                RectangleF firstSegment = new RectangleF(selectedAudioBounds.Left, selectedAudioBounds.Top, splitOffset, selectedAudioBounds.Height);
                RectangleF secondSegment = new RectangleF(trackerPosition, selectedAudioBounds.Top, selectedAudioBounds.Right - trackerPosition, selectedAudioBounds.Height);

                int index = allAudioSegments.IndexOf(selectedAudioBounds);
                allAudioSegments.RemoveAt(index);
                allAudioSegments.Insert(index, firstSegment);
                allAudioSegments.Insert(index + 1, secondSegment);

                fullAudioRender.RemoveAt(index);
                fullAudioRender.Insert(index, new AudioRenderSegment
                {
                    FilePath = originalAudio.FilePath,
                    StartTime = originalAudio.StartTime,
                    EndTime = originalAudio.StartTime + (splitOffset / pixelsPerSecond),
                    TimeLinePosition = firstSegment.Left / pixelsPerSecond,
                    Id = ++segmentsAudioCount
                });
                fullAudioRender.Insert(index + 1, new AudioRenderSegment
                {
                    FilePath = originalAudio.FilePath,
                    StartTime = originalAudio.StartTime + (splitOffset / pixelsPerSecond),
                    EndTime = originalAudio.EndTime,
                    TimeLinePosition = secondSegment.Left / pixelsPerSecond,
                    Id = ++segmentsAudioCount
                });

                selectedAudioBounds = firstSegment;

                NormalizeAudioSegmentPositions();
                NormalizeSegmentPositions();
                AudioTrack.Invalidate();
            }
        }



        ///
        /// Removes selected segment from timeline.
        ///
        private async void RemoveSegment_Click(object sender, EventArgs e)
        {
            if (selectedVideoBounds == RectangleF.Empty && selectedAudioBounds == RectangleF.Empty)
            {
                MessageBox.Show("No segment selected. Please select a video or audio segment to remove.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("Before Removal:");
            foreach (var segment in fullVideoRender)
            {
                Console.WriteLine($"Video Segment: ID={segment.Id}, TimeLinePosition={segment.TimeLinePosition}, Start={segment.StartTime}, End={segment.EndTime}");
            }

            float removedWidthVideo = 0;
            float removedWidthAudio = 0;

            bool videoChanged = false;
            bool audioChanged = false;
            if (selectedVideoBounds != RectangleF.Empty)
            {
                videoChanged = true;
                removedWidthVideo = selectedVideoBounds.Width;

                allVideoBounds.Remove(selectedVideoBounds);
                for (int i = 0; i < allVideoBounds.Count; i++)
                {
                    var bounds = allVideoBounds[i];
                    if (bounds.Left > selectedVideoBounds.Left)
                    {
                        RectangleF newBounds = new RectangleF(bounds.X - removedWidthVideo, bounds.Y, bounds.Width, bounds.Height);
                        allVideoBounds[i] = newBounds;
                    }
                }

                for (int i = allThumbnailsWithPositions.Count - 1; i >= 0; i--)
                {
                    var (thumbnail, position) = allThumbnailsWithPositions[i];
                    if (position >= selectedVideoBounds.Left && position < selectedVideoBounds.Right)
                    {
                        allThumbnailsWithPositions.RemoveAt(i);
                    }
                    else if (position >= selectedVideoBounds.Right)
                    {
                        allThumbnailsWithPositions[i] = (thumbnail, position - removedWidthVideo);
                    }
                }

                float oldTimeLinePos = selectedVideoBounds.Left / pixelsPerSecond;
                var videoSegmentToRemove = fullVideoRender.FirstOrDefault(v => Math.Abs(v.TimeLinePosition - oldTimeLinePos) < 0.1);
                if (videoSegmentToRemove != null)
                {
                    fullVideoRender.Remove(videoSegmentToRemove);
                }

                foreach (var segment in fullVideoRender)
                {
                    if (segment.TimeLinePosition > oldTimeLinePos)
                    {
                        segment.TimeLinePosition -= removedWidthVideo / pixelsPerSecond;
                    }
                }

                selectedVideoBounds = RectangleF.Empty;
            }
            if (selectedAudioBounds != RectangleF.Empty)
            {
                audioChanged = true;
                removedWidthAudio = selectedAudioBounds.Width;

                allAudioSegments.Remove(selectedAudioBounds);
                for (int i = 0; i < allAudioSegments.Count; i++)
                {
                    var bounds = allAudioSegments[i];
                    if (bounds.Left > selectedAudioBounds.Left)
                    {
                        RectangleF newBounds = new RectangleF(bounds.X - removedWidthAudio, bounds.Y, bounds.Width, bounds.Height);
                        allAudioSegments[i] = newBounds;
                    }
                }

                for (int i = allAudioAmplitudeBars.Count - 1; i >= 0; i--)
                {
                    var bar = allAudioAmplitudeBars[i].bar;
                    if (bar.Left >= selectedAudioBounds.Left && bar.Right <= selectedAudioBounds.Right)
                    {
                        allAudioAmplitudeBars.RemoveAt(i);
                    }
                    else if (bar.Left >= selectedAudioBounds.Right)
                    {
                        RectangleF newBar = new RectangleF(bar.X - removedWidthAudio, bar.Y, bar.Width, bar.Height);
                        allAudioAmplitudeBars[i] = (newBar, i);
                    }
                }

                float oldTimeLinePos = selectedAudioBounds.Left / pixelsPerSecond;
                var audioSegmentToRemove = fullAudioRender.FirstOrDefault(a => Math.Abs(a.TimeLinePosition - oldTimeLinePos) < 0.1);
                if (audioSegmentToRemove != null)
                {
                    fullAudioRender.Remove(audioSegmentToRemove);
                }

                foreach (var segment in fullAudioRender)
                {
                    if (segment.TimeLinePosition > oldTimeLinePos)
                    {
                        segment.TimeLinePosition -= removedWidthAudio / pixelsPerSecond;
                    }
                }

                selectedAudioBounds = RectangleF.Empty;
            }
            float videoWidth = allVideoBounds.Any() ? allVideoBounds.Max(b => b.Right) : 0;
            float audioWidth = allAudioSegments.Any() ? allAudioSegments.Max(b => b.Right) : 0;
            int widthPlaceholderDefault = 733;
            this.Invoke(new Action(() =>
            {
                if (videoChanged)
                {
                    widthVideo = videoWidth;
                    if (videoWidth < audioWidth)
                    {
                        AudioTrackPlaceholder.Width = widthPlaceholderDefault;
                        VideoTrackPlaceholder.Width = (int)(AudioTrackPlaceholder.Width + (widthAudio - widthVideo + selectedVideoBounds.Width));
                        VideoTrack.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
                        AudioTrack.Width = VideoTrack.Width;
                    }
                    else
                    {
                        VideoTrackPlaceholder.Width = widthPlaceholderDefault;
                        AudioTrackPlaceholder.Width = (int)(AudioTrackPlaceholder.Width - selectedVideoBounds.Width);
                        VideoTrack.Width = (int)(audioWidth + VideoTrackPlaceholder.Width);
                        AudioTrack.Width = VideoTrack.Width;
                    }
                    VideoTrackPlaceholder.Location = new Point((int)Math.Round(widthVideo), VideoTrackPlaceholder.Location.Y);
                    VideoTrackPlaceholder.BackColor = Color.FromArgb(67, 38, 88);
                    EditingRuller.Width = VideoTrack.Width;
                    VideoTrack.Invalidate();
                    VideoTrackPlaceholder.Invalidate();
                }

                if (audioChanged)
                {
                    widthAudio = audioWidth;
                    if (audioWidth < videoWidth)
                    {
                        VideoTrackPlaceholder.Width = widthPlaceholderDefault;
                        AudioTrackPlaceholder.Width = (int)(VideoTrackPlaceholder.Width + (widthVideo - widthAudio + selectedAudioBounds.Width));
                        AudioTrack.Width = (int)Math.Ceiling(widthAudio + AudioTrackPlaceholder.Width);
                        VideoTrack.Width = AudioTrack.Width;
                    }
                    else
                    {
                        AudioTrackPlaceholder.Width = widthPlaceholderDefault;
                        VideoTrackPlaceholder.Width = (int)(VideoTrackPlaceholder.Width - selectedAudioBounds.Width);
                        AudioTrack.Width = (int)(audioWidth + AudioTrackPlaceholder.Width);
                        VideoTrack.Width = AudioTrack.Width;
                    }
                    AudioTrackPlaceholder.Location = new Point((int)Math.Round(widthAudio), AudioTrackPlaceholder.Location.Y);
                    AudioTrackPlaceholder.BackColor = Color.FromArgb(67, 38, 88);
                    EditingRuller.Width = AudioTrack.Width;
                    AudioTrack.Invalidate();
                    AudioTrackPlaceholder.Invalidate();
                }
                EditingRuller.Invalidate();
            }));

            Console.WriteLine("After Removal:");
            foreach (var segment in fullVideoRender)
            {
                Console.WriteLine($"Video Segment: ID={segment.Id}, TimeLinePosition={segment.TimeLinePosition}, Start={segment.StartTime}, End={segment.EndTime}");
            }

            SyncMediaTracks();
            await GeneratePreview();
        }

        ///
        /// Syncs media and audio tracks with placeholders for successfull generation of preview and/or render. 
        ///
        private void SyncMediaTracks()
        {
            string blackScreenVideoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "EmptyMedia", "BlackScreenVideo.mp4");
            string silentAudioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "EmptyMedia", "SilentAudio.mp3");

            fullVideoRender.RemoveAll(v => v.FilePath == blackScreenVideoPath);
            fullAudioRender.RemoveAll(a => a.FilePath == silentAudioPath);

            float totalVideoDuration = fullVideoRender.Sum(v => v.EndTime - v.StartTime);
            float totalAudioDuration = fullAudioRender.Sum(a => a.EndTime - a.StartTime);

            if (totalVideoDuration < totalAudioDuration)
            {
                float blackScreenDuration = totalAudioDuration - totalVideoDuration;
                fullVideoRender.Add(new VideoRenderSegment
                {
                    FilePath = blackScreenVideoPath,
                    StartTime = 0,
                    EndTime = blackScreenDuration,
                    TimeLinePosition = totalVideoDuration,
                    Id = ++segmentsVideoCount
                });
            }
            else if (totalAudioDuration < totalVideoDuration)
            {
                float silentAudioDuration = totalVideoDuration - totalAudioDuration;

                fullAudioRender.Add(new AudioRenderSegment
                {
                    FilePath = silentAudioPath,
                    StartTime = 0,
                    EndTime = silentAudioDuration,
                    TimeLinePosition = totalAudioDuration,
                    Id = ++segmentsAudioCount
                });
            }

            float runningTime = 0f;
            foreach (var segment in fullAudioRender)
            {
                segment.TimeLinePosition = runningTime;
                runningTime += segment.EndTime - segment.StartTime;
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
        private const int seekUpdateThreshold = 50;
        private long lastKnownVlcTime = 0;
        private float trackerXPosition = 0f;
        private float currentPlaybackTime = 0f;
        private float pixelsPerSecond = 50f;
        private float pixelsPerMillisecond;
        private long lastSeekUpdate = 0;
        private bool wasPlayingBeforeDrag = false;
        private bool isDraggingTracker = false;
        private bool isUpdatingPreview = false;

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
                float maxPosition = Math.Max(widthVideo, widthAudio);
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
                float maxXPosition = Math.Max(widthVideo, widthAudio) - bufferPixels;
                float proposedX = Math.Max(0.1f, Math.Min(e.X, maxXPosition));

                trackerXPosition = proposedX;
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;

                long now = Environment.TickCount;
                if (now - lastSeekUpdate > seekUpdateThreshold)
                {
                    lastSeekUpdate = now;

                    if (!isUpdatingPreview)
                    {
                        isUpdatingPreview = true;

                        try
                        {
                            PreviewBox.Time = (long)currentPlaybackTime;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to update preview: {ex.Message}");
                        }
                        finally
                        {
                            isUpdatingPreview = false;
                        }
                    }
                }

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

                AudioTrack.Invalidate();
                VideoTrack.Invalidate();
                EditingRuller.Invalidate();
            }
        }


        ///
        /// Occurs when the user releases the mouse button on the timeline; finalizes tracker movement.
        ///
        private void EditingRuller_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingTracker)
            {
                trackerXPosition = Math.Max(0, Math.Min(e.X, Math.Max(widthVideo, widthAudio)));
                currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;

                try
                {
                    PreviewBox.Time = (long)currentPlaybackTime; // Direct update
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to update preview on mouse up: {ex.Message}");
                }

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
            AudioTrack.Invalidate();
            VideoTrack.Invalidate();
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
        private float widthVideo = 0f;
        private float widthAudio = 0f;
        private float widthEditing = 0f;

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
        ///  Initialize mouse events for video track.
        ///
        public void InitializeMouseEvents()
        {
            VideoTrack.MouseDown += VideoTrack_MouseDownHandler;
            VideoTrack.MouseMove += VideoTrack_MouseMoveHandler;
            VideoTrack.MouseUp += VideoTrack_MouseUpHandler;
            AudioTrack.MouseDown += AudioTrack_MouseDownHandler;
            AudioTrack.MouseMove += AudioTrack_MouseMoveHandler;
            AudioTrack.MouseUp += AudioTrack_MouseUpHandler;
        }
        ///
        /// Paint event for the placeholder panel under video track (decorative pattern).
        ///
        private void VideoTrackPlaceholder_Paint_1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.FromArgb(67, 38, 88));

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
            using (Pen borderPen = new Pen(Color.Green, 2))
            {
                foreach (var bounds in allVideoBounds)
                {
                    if (bounds != selectedVideoBounds)
                    {
                        g.DrawRectangle(borderPen, Rectangle.Round(bounds));
                    }
                }
            }
            foreach (var (thumbnail, position) in allThumbnailsWithPositions)
            {
                if (!draggedThumbnails.Any(t => t.thumbnail == thumbnail))
                {
                    float thumbnailHeight = VideoTrack.Height - 20f;
                    float thumbnailY = 10f;
                    g.DrawImage(thumbnail, position, thumbnailY, 100, thumbnailHeight);
                }
            }
            if (selectedVideoBounds != RectangleF.Empty)
            {
                using (Pen selectedPen = new Pen(Color.Red, 3))
                using (Brush overlayBrush = new SolidBrush(Color.FromArgb(128, Color.Purple)))
                {
                    g.FillRectangle(overlayBrush, selectedVideoBounds);
                    g.DrawRectangle(selectedPen, Rectangle.Round(selectedVideoBounds));
                }

                foreach (var (thumbnail, position) in draggedThumbnails)
                {
                    float thumbnailHeight = VideoTrack.Height - 20f;
                    float thumbnailY = 10f;
                    g.DrawImage(thumbnail, position, thumbnailY, 100, thumbnailHeight);
                }
            }
            using (Pen trackerPen = new Pen(Color.Blue, 2))
            {
                g.DrawLine(trackerPen, trackerXPosition, 0, trackerXPosition, VideoTrack.Height);
            }
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
        private async Task GeneratePreview()
        {
            string outputDirectory = outputPath;
            string tempDirectory = Path.Combine(outputPath, "tempBuild");
            int fps = 15;
            int width = 472;
            int height = 404;

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string tempVideoPath = Path.Combine(tempDirectory, $"temp_video_{timestamp}.mp4");
            string tempAudioPath = Path.Combine(tempDirectory, $"temp_audio_{timestamp}.mp4");
            string finalOutputPath = Path.Combine(outputDirectory, $"preview_{timestamp}.mp4");

            StringBuilder videoCmd = new StringBuilder("-y ");
            List<string> videoFilters = new List<string>();
            int videoIndex = 0;

            foreach (var segment in fullVideoRender)
            {
                videoCmd.AppendFormat("-i \"{0}\" -an ", segment.FilePath);
                videoFilters.Add($"[{videoIndex}:v]trim=start={segment.StartTime}:end={segment.EndTime},setpts=PTS-STARTPTS,scale={width}:{height},fps={fps},setsar=1,format=yuv420p[v{videoIndex}];");
                videoIndex++;
            }

            string videoInputs = string.Join("", Enumerable.Range(0, videoIndex).Select(i => $"[v{i}]"));
            videoFilters.Add($"{videoInputs}concat=n={videoIndex}:v=1[outv]");
            videoCmd.Append($"-filter_complex \"{string.Join(" ", videoFilters)}\" -map \"[outv]\" -an -c:v libx264 -crf 25 -preset ultrafast -b:v 8000k \"{tempVideoPath}\"");

            StringBuilder audioCmd = new StringBuilder("-y ");
            List<string> audioFilters = new List<string>();
            int audioIndex = 0;

            foreach (var segment in fullAudioRender)
            {
                audioCmd.AppendFormat("-i \"{0}\" ", segment.FilePath);
                audioFilters.Add($"[{audioIndex}:a]atrim=start={segment.StartTime}:end={segment.EndTime},asetpts=PTS-STARTPTS[a{audioIndex}];");
                audioIndex++;
            }

            string audioInputs = string.Join("", Enumerable.Range(0, audioIndex).Select(i => $"[a{i}]"));
            audioFilters.Add($"{audioInputs}concat=n={audioIndex}:v=0:a=1[outa]");
            audioCmd.Append($"-filter_complex \"{string.Join(" ", audioFilters)}\" -map \"[outa]\" -vn -c:a aac -b:a 192k \"{tempAudioPath}\"");

            StringBuilder mergeCmd = new StringBuilder("-y ");
            mergeCmd.AppendFormat("-i \"{0}\" -i \"{1}\" -c:v copy -c:a copy \"{2}\"", tempVideoPath, tempAudioPath, finalOutputPath);

            var videoTask = RunFFmpegCommand(videoCmd.ToString(), "Video Generation");
            var audioTask = RunFFmpegCommand(audioCmd.ToString(), "Audio Generation");
            await Task.WhenAll(videoTask, audioTask);
            await RunFFmpegCommand(mergeCmd.ToString(), "Final Merging");
            if (Directory.Exists(tempDirectory))
            {
                File.Delete(tempVideoPath);
                File.Delete(tempAudioPath);
                Console.WriteLine($"Deleted the output directory: {tempDirectory}");
            }
            this.Invoke(new Action(() =>
            {
                var mediaOptions = new string[] { "input-repeat=65535" };
                PreviewBox.SetMedia(new FileInfo(finalOutputPath), mediaOptions);
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

            if ((widthVideo + newWidth) / pixelsPerSecond > 600)
            {
                MessageBox.Show("Adding this video will exceed the 10-minute limit. Please adjust your timeline.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = true;
                return;
            }

            this.Invoke(new Action(() =>
            {
                widthVideo += newWidth;
                widthAudio += newWidth;
                widthEditing = Math.Max(widthVideo, widthAudio);
                VideoTrack.Width = (int)Math.Ceiling(widthVideo + VideoTrackPlaceholder.Width);
                AudioTrack.Width = (int)Math.Ceiling(widthAudio + AudioTrackPlaceholder.Width);
                EditingRuller.Width = Math.Max(VideoTrack.Width, AudioTrack.Width);
                EditingRuller.Invalidate();
                VideoTrackPlaceholder.Invalidate();
                AudioTrackPlaceholder.Invalidate();
            }));

            List<Image> videoThumbnails = await Task.Run(() => ExtractVideoThumbnails(filePath));
            List<float> audioAmplitudes = await Task.Run(() => GetAudioAmplitudeData(filePath));

            int thumbnailCount = videoThumbnails.Count;
            float intervalInPixels = 4.0f * pixelsPerSecond;
            float videoStartPosition = widthVideo - newWidth;
            float audioStartPosition = widthAudio - newWidth;
            float thumbnailWidth = 100f;

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
                var audioBounds = new RectangleF(audioStartPosition, 0f, newWidth, AudioTrack.Height - 2f);
                allVideoBounds.Add(videoBounds);
                videoToAudioMapping[videoBounds] = audioBounds;
                allAudioSegments.Add(audioBounds);

                int amplitudeCount = audioAmplitudes.Count;
                int totalBars = (int)(videoDuration * barsPerSecond);

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
                    allAudioAmplitudeBars.Add((new RectangleF(barXPosition, barYPosition, barWidth, amplitudeHeight), i));
                }


                segmentsVideoCount++;
                fullVideoRender.Add(new VideoRenderSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsVideoCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

                segmentsAudioCount++;
                fullAudioRender.Add(new AudioRenderSegment { StartTime = 0, EndTime = videoDuration, FilePath = filePath, Id = segmentsAudioCount, TimeLinePosition = videoStartPosition / pixelsPerSecond });

                VideoTrackPlaceholder.Location = new Point(Math.Max((int)Math.Round(widthVideo), VideoTrackPlaceholder.Location.X), VideoTrackPlaceholder.Location.Y);
                AudioTrackPlaceholder.Location = new Point(Math.Max((int)Math.Round(widthAudio), AudioTrackPlaceholder.Location.X), AudioTrackPlaceholder.Location.Y);

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
            foreach (VideoRenderSegment segment in fullVideoRender)
            {
                Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2}");
            }
            SyncMediaTracks();
            GeneratePreview();
        }

        //---------------------------------------------------------------------------------------//
        //-----------------------------//AUDIO HANDLING//----------------------------------------//
        //---------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private RectangleF selectedAudioBounds = RectangleF.Empty;
        private Dictionary<RectangleF, RectangleF> videoToAudioMapping = new Dictionary<RectangleF, RectangleF>();
        private List<RectangleF> allAudioSegments = new List<RectangleF>();
        private List<(RectangleF bar, int id)> allAudioAmplitudeBars = new List<(RectangleF bar, int id)>();
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
                g.FillRectangle(backgroundBrush, 0, 0, widthAudio, AudioTrack.Height);
            }
            using (Brush barBrush = new SolidBrush(Color.LightGreen))
            {
                foreach ((RectangleF bar, int id) in allAudioAmplitudeBars)
                {
                    g.FillRectangle(barBrush, bar);
                }
            }
            using (Pen borderPen = new Pen(Color.Green, 2))
            using (Pen selectedPen = new Pen(Color.Red, 2))
            {
                foreach (var bounds in allAudioSegments)
                {
                    if (bounds == selectedAudioBounds)
                    {
                        using (Brush overlayBrush = new SolidBrush(Color.FromArgb(128, Color.Purple)))
                        {
                            g.FillRectangle(overlayBrush, bounds);
                        }
                        g.DrawRectangle(selectedPen, Rectangle.Round(bounds));
                    }
                    else
                    {
                        g.DrawRectangle(borderPen, Rectangle.Round(bounds));
                    }
                }
            }
            using (Brush transparentBrush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
            {
                g.FillRectangle(transparentBrush, widthAudio, 0, AudioTrack.Width - widthAudio, AudioTrack.Height);
            }
            using (Pen trackerPen = new Pen(Color.Blue, 2))
            {
                g.DrawLine(trackerPen, trackerXPosition, 0, trackerXPosition, AudioTrack.Height);
            }
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
                    float amplitude = sample == short.MinValue ? 1.0f : Math.Abs(sample) / 32768f;
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
        /// Called on initialization to ensure temp folders/files are removed if application not closed properly.
        ///
        public void ClearingTempPaths()
        {
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
                Console.WriteLine($"Deleted the output directory: {outputPath}");
            }
            else
            {
                Console.WriteLine($"Output directory does not exist: {outputPath}");
            }
        }

        ///
        /// Run ffmpeg generated commands on call.
        ///
        private async Task RunFFmpegCommand(string ffmpegArguments, string description)
        {
            Console.WriteLine($"Running FFmpeg {description} command: {ffmpegArguments}");
            await Task.Run(() =>
            {
                using (Process ffmpegProcess = new Process())
                {
                    ffmpegProcess.StartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = ffmpegArguments,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    ffmpegProcess.Start();
                    string errorOutput = ffmpegProcess.StandardError.ReadToEnd();
                    ffmpegProcess.WaitForExit();

                    if (ffmpegProcess.ExitCode != 0)
                    {
                        throw new Exception($"FFmpeg {description} failed. See error: {errorOutput}");
                    }
                }
            });
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
        /// Fix Image positioning bug on right drag.
        ///
        private void FixImages(RectangleF selectedSegment)
        {
            float segmentEndPoint = selectedSegment.Left + selectedSegment.Width;
            List<(Image thumbnail, float originalPosition)> updates = new List<(Image thumbnail, float originalPosition)>();
            foreach (var (thumbnail, originalPosition) in tempPositions)
            {
                float currentPosition = allThumbnailsWithPositions.FirstOrDefault(t => t.thumbnail == thumbnail).position;
                if (originalPosition > segmentEndPoint && originalPosition != currentPosition)
                {
                    updates.Add((thumbnail, originalPosition));
                }
            }

            foreach (var (thumbnail, originalPosition) in updates)
            {
                int thumbnailIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                if (thumbnailIndex != -1)
                {
                    allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, originalPosition);
                }
            }

            VideoTrack.Invalidate();
        }

        ///
        /// Fix Bars positioning bug on right drag.
        ///
        private void UpdateBarsForRightDraggingFix()
        {
            bool moving = true;
            var draggedBarsIds = draggedBars.Select(t => t.id).ToList();
            var currentBounds = selectedAudioBounds;
            var nextBounds = allAudioSegments
                .Where(segment => segment.Right <= currentBounds.Left + 1)
                .OrderByDescending(segment => segment.Right)
                .FirstOrDefault();

            var updatedBars = new List<(RectangleF bar, int id)>();

            int remainingSegmentsCount = allAudioSegments
                .Count(segment => segment.Right <= currentBounds.Left)
                - segmentsToLeft;

            while (moving)
            {
                if (remainingSegmentsCount >= 1)
                {

                    float leftEdge, rightEdge;
                    if (remainingSegmentsCount >= 2 && nextBounds != null && nextBounds != default && nextBounds != currentBounds)
                    {
                        leftEdge = Math.Min(currentBounds.Left, nextBounds.Left);
                        rightEdge = Math.Max(currentBounds.Right, nextBounds.Right);
                    }
                    else
                    {
                        leftEdge = currentBounds.Left;
                        rightEdge = currentBounds.Right;
                    }

                    foreach (var (bar, id) in allAudioAmplitudeBars)
                    {
                        if (!draggedBarsIds.Contains(id) &&
                            bar.X >= leftEdge &&
                            bar.X <= rightEdge)
                        {
                            updatedBars.Add((
                                new RectangleF(
                                    bar.X - currentBounds.Width - 1,
                                    bar.Top,
                                    bar.Width,
                                    bar.Height
                                ),
                                id
                            ));
                        }
                    }

                    for (int i = 0; i < allAudioAmplitudeBars.Count; i++)
                    {
                        var (bar, id) = allAudioAmplitudeBars[i];
                        var updatedBar = updatedBars.FirstOrDefault(t => t.id == id);
                        if (updatedBar != default)
                        {
                            allAudioAmplitudeBars[i] = updatedBar;
                        }
                    }

                    var nextSegmentToLeft = allAudioSegments
                        .Where(segment => segment.Right <= currentBounds.Left + 1)
                        .OrderByDescending(segment => segment.Right)
                        .FirstOrDefault();

                    if (nextSegmentToLeft == currentBounds || nextSegmentToLeft == default)
                    {
                        moving = false;
                    }
                    else
                    {
                        currentBounds = nextSegmentToLeft;
                        remainingSegmentsCount = allAudioSegments
                            .Count(segment => segment.Right <= currentBounds.Left)
                            - segmentsToLeft;

                        draggedBarsIds = draggedBarsIds
                            .Union(updatedBars.Select(t => t.id))
                            .ToList();

                        nextBounds = allAudioSegments
                            .Where(segment => segment.Right <= currentBounds.Left + 1)
                            .OrderByDescending(segment => segment.Right)
                            .FirstOrDefault();
                    }

                    updatedBars.Clear();
                }
                else
                {
                    moving = false;
                }
            }

            AudioTrack.Invalidate();
        }

        ///
        /// When moving the segments this function help to identify if the user drags the selected segment over a segment located left from its position or right in order to determine the appropriate logic to proceed.
        ///
        private int DetermineNewIndex(int draggedIndex, RectangleF draggedSegment)
        {
            int newIndex = draggedIndex;

            for (int i = 0; i < allVideoBounds.Count; i++)
            {
                if (i == draggedIndex) continue;

                RectangleF currentSegment = allVideoBounds[i];
                float currentSegmentMiddle = currentSegment.Left + currentSegment.Width / 2;

                if (draggedSegment.Left < currentSegmentMiddle && draggedIndex > i)
                {
                    newIndex = Math.Min(newIndex, i);
                }

                if (draggedSegment.Right > currentSegmentMiddle && draggedIndex < i)
                {
                    newIndex = Math.Max(newIndex, i + 1);
                }
            }
            VideoTrack.Invalidate();
            return newIndex;
        }

        ///
        /// When moving the segments this function help to identify if the user drags the selected segment over a segment located left from its position or right in order to determine the appropriate logic to proceed.
        ///
        private int DetermineNewAudioIndex(int draggedIndex, RectangleF draggedSegment)
        {
            int newIndex = draggedIndex;

            for (int i = 0; i < allAudioSegments.Count; i++)
            {
                if (i == draggedIndex) continue;

                RectangleF currentSegment = allAudioSegments[i];
                float currentSegmentMiddle = currentSegment.Left + currentSegment.Width / 2;
                if (draggedSegment.Left < currentSegmentMiddle && draggedIndex > i)
                {
                    newIndex = Math.Min(newIndex, i);
                }
                if (draggedSegment.Right > currentSegmentMiddle && draggedIndex < i)
                {
                    newIndex = Math.Max(newIndex, i + 1);
                }
            }
            return newIndex;
        }

        ///
        /// Function to sync data to pass to PreviewGeneration function.
        ///
        private void SyncFullVideoRender()
        {
            for (int i = 0; i < allVideoBounds.Count; i++)
            {
                if (i < fullVideoRender.Count)
                {
                    fullVideoRender[i].TimeLinePosition = allVideoBounds[i].Left / pixelsPerSecond;
                }
                else
                {
                    Console.WriteLine($"Warning: Missing render data for segment {i}");
                }
            }
            VideoTrack.Invalidate();
        }

        ///
        /// Function to sync data to pass to PreviewGeneration function.
        ///
        private void SyncFullAudioRender()
        {
            for (int i = 0; i < allAudioSegments.Count; i++)
            {
                if (i < fullAudioRender.Count)
                {
                    fullAudioRender[i].TimeLinePosition = allAudioSegments[i].Left / pixelsPerSecond;
                }
                else
                {
                    Console.WriteLine($"Warning: Missing render data for segment {i}");
                }
            }
        }

        ///
        /// Reseting dragging state.
        ///
        private void ResetDraggingState()
        {
            isDraggingSegment = false;
            initialMousePosition = Point.Empty;
            draggedThumbnails.Clear();
            draggedThumbnailsInitialPosition.Clear();
            draggedBars.Clear();
            draggedBarsInitialPosition.Clear();
            tempPositions.Clear();
            tempPositionsBars.Clear();
            AudioTrack.Invalidate();
            VideoTrack.Invalidate();
        }

        ///
        /// If no swap should occur just return the segment and it's position at the original place.
        ///
        private void RevertSegmentAndThumbnails()
        {
            int segmentIndex = allVideoBounds.IndexOf(selectedVideoBounds);
            if (segmentIndex != -1)
            {
                allVideoBounds[segmentIndex] = initialSegmentBounds;
            }
            for (int i = 0; i < draggedThumbnailsInitialPosition.Count; i++)
            {
                var (thumbnail, initialPosition) = draggedThumbnailsInitialPosition[i];
                int thumbnailIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                if (thumbnailIndex != -1)
                {
                    allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, initialPosition);
                }
            }
            selectedVideoBounds = initialSegmentBounds;
        }

        ///
        /// If no swap should occur just return the segment and it's position at the original place.
        ///
        private void RevertSegmentAndBars()
        {
            int segmentIndex = allAudioSegments.IndexOf(selectedAudioBounds);
            if (segmentIndex != -1)
            {
                allAudioSegments[segmentIndex] = initialSegmentBounds;
            }
            for (int i = 0; i < draggedBarsInitialPosition.Count; i++)
            {
                var (bar, id) = draggedBarsInitialPosition[i];
                int barIndex = allAudioAmplitudeBars.FindIndex(t => t.id == id);
                if (barIndex != -1)
                {
                    allAudioAmplitudeBars[barIndex] = (new RectangleF(
                        bar.X,
                        bar.Top,
                        bar.Width,
                        bar.Height
                        ), id);
                }
            }
            selectedAudioBounds = initialSegmentBounds;
        }

        /// 
        /// Update the selected thumbnails of the segment to the right position.
        /// 
        private void UpdateThumbnailPositions(int segmentIndex)
        {
            RectangleF newSegmentBounds = allVideoBounds[segmentIndex];
            float offset = newSegmentBounds.Left - initialSegmentBounds.Left;

            foreach (var (thumbnail, initialPosition) in draggedThumbnailsInitialPosition)
            {
                if (initialPosition >= initialSegmentBounds.Left && initialPosition < initialSegmentBounds.Right)
                {
                    float newPosition = initialPosition + offset;
                    int index = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                    if (index != -1)
                    {
                        allThumbnailsWithPositions[index] = (thumbnail, newPosition);
                    }
                    int draggedIndex = draggedThumbnails.FindIndex(t => t.thumbnail == thumbnail);
                    if (draggedIndex != -1)
                    {
                        draggedThumbnails[draggedIndex] = (thumbnail, newPosition);
                    }
                }
            }
        }

        /// 
        /// Update the selected bars of the segment to the right position.
        /// 
        private void UpdateBarPositions(int segmentIndex)
        {
            RectangleF newSegmentBounds = allAudioSegments[segmentIndex];
            float offset = newSegmentBounds.Left - initialSegmentBounds.Left;

            foreach (var (bar, id) in draggedBarsInitialPosition)
            {
                if (bar.X >= initialSegmentBounds.Left && bar.X < initialSegmentBounds.Right)
                {
                    float newPosition = bar.X + offset;
                    int index = allAudioAmplitudeBars.FindIndex(t => t.id == id);
                    if (index != -1)
                    {
                        allAudioAmplitudeBars[index] = (new RectangleF(
                            newPosition,
                            bar.Top,
                            bar.Width,
                            bar.Height
                            ), id);
                    }
                    int draggedIndex = draggedBars.FindIndex(t => t.id == id);
                    if (draggedIndex != -1)
                    {
                        draggedBars[draggedIndex] = (new RectangleF(
                            newPosition,
                            bar.Top,
                            bar.Width,
                            bar.Height
                            ), id);
                    }
                }
            }
        }

        /// 
        /// Right drag handle of thumbnails positions.
        /// 
        private void UpdateThumbnailsForRightDragging(RectangleF segment, float newLeft)
        {
            float offset = newLeft - segment.Left;

            foreach (var (thumbnail, originalPosition) in allThumbnailsWithPositions.ToList())
            {
                if (originalPosition >= segment.Left && originalPosition < segment.Right)
                {
                    float newThumbnailPosition = originalPosition + offset;

                    int thumbnailIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                    if (thumbnailIndex != -1)
                    {
                        allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, newThumbnailPosition);
                    }
                }
            }

            rightMv = true;
            VideoTrack.Invalidate();
        }

        /// 
        /// Right drag handle of bars positions.
        /// 
        private void UpdateBarsForRightDragging(RectangleF oldSegmentRect, float newLeft)
        {
            float offset = newLeft - oldSegmentRect.Left;
            var draggedTemp = draggedBars.Select(bar => bar.id).ToList();

            var initialBars = allAudioAmplitudeBars.ToList();

            for (int i = 0; i < allAudioAmplitudeBars.Count; i++)
            {
                var (barRect, id) = allAudioAmplitudeBars[i];

                if (barRect.X >= oldSegmentRect.Left)
                {
                    float newBarX = barRect.X + offset;
                    allAudioAmplitudeBars[i] = (
                        new RectangleF(newBarX, barRect.Top, barRect.Width, barRect.Height),
                        id
                    );
                }
            }

            foreach (var (initialBar, id) in initialBars)
            {
                if (initialBar.X > oldSegmentRect.Right || (initialBar.X < oldSegmentRect.Right && initialBar.X >= oldSegmentRect.Left && draggedTemp.Contains(id)))
                {
                    int barIndex = allAudioAmplitudeBars.FindIndex(t => t.id == id);
                    if (barIndex != -1)
                    {
                        var currentBar = allAudioAmplitudeBars[barIndex].bar;
                        allAudioAmplitudeBars[barIndex] = (
                            new RectangleF(
                                initialBar.X,
                                currentBar.Top,
                                currentBar.Width,
                                currentBar.Height
                            ),
                            id
                        );
                    }
                }
            }

            rightMv = true;
            AudioTrack.Invalidate();
        }

        /// 
        /// Left drag handle of thumbnails positions.
        /// 
        private void UpdateThumbnailsForLeftDragging()
        {
            float currentLeft = 0;

            var updatedThumbnails = new List<(Image thumbnail, float newPosition)>();

            for (int i = 0; i < allVideoBounds.Count; i++)
            {
                RectangleF segment = allVideoBounds[i];

                if (Math.Abs(segment.Left - currentLeft) > 0.1f)
                {

                    allVideoBounds[i] = new RectangleF(
                        currentLeft,
                        segment.Top,
                        segment.Width,
                        segment.Height
                    );

                    foreach (var (thumbnail, originalPosition) in allThumbnailsWithPositions.ToList())
                    {
                        if (originalPosition >= segment.Left && originalPosition < segment.Right)
                        {
                            float relativeOffset = originalPosition - segment.Left;
                            float newThumbnailPosition = currentLeft + relativeOffset;
                            updatedThumbnails.Add((thumbnail, newThumbnailPosition));
                        }
                    }

                    if (i < fullVideoRender.Count)
                    {
                        fullVideoRender[i].TimeLinePosition = currentLeft / pixelsPerSecond;
                    }
                }

                currentLeft += segment.Width;
            }

            foreach (var (thumbnail, newPosition) in updatedThumbnails)
            {
                int thumbnailIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                if (thumbnailIndex != -1)
                {
                    allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, newPosition);
                }
            }
            VideoTrack.Invalidate();
        }

        /// 
        /// Left drag handle of bars positions.
        /// 
        private void UpdateBarsForLeftDragging()
        {
            float currentLeft = 0;

            var updatedBars = new List<(RectangleF bar, int id)>();

            for (int i = 0; i < allAudioSegments.Count; i++)
            {
                RectangleF segment = allAudioSegments[i];

                if (Math.Abs(segment.Left - currentLeft) > 0.1f)
                {

                    allAudioSegments[i] = new RectangleF(
                        currentLeft,
                        segment.Top,
                        segment.Width,
                        segment.Height
                    );

                    foreach (var (bar, id) in allAudioAmplitudeBars.ToList())
                    {
                        if (bar.X >= segment.Left && bar.X < segment.Right)
                        {
                            float relativeOffset = bar.X - segment.Left;
                            float newBarPosition = currentLeft + relativeOffset;
                            updatedBars.Add((new RectangleF(
                                newBarPosition,
                                bar.Top,
                                bar.Width,
                                bar.Height
                                ), id));
                        }
                    }

                    if (i < fullAudioRender.Count)
                    {
                        fullAudioRender[i].TimeLinePosition = currentLeft / pixelsPerSecond;
                    }
                }

                currentLeft += segment.Width;
            }

            foreach (var (bar, id) in updatedBars)
            {
                int barIndex = allAudioAmplitudeBars.FindIndex(t => t.id == id);
                if (barIndex != -1)
                {
                    allAudioAmplitudeBars[barIndex] = (new RectangleF(
                        bar.X,
                        bar.Top,
                        bar.Width,
                        bar.Height
                        ), id);
                }
            }
            NormalizeAudioSegmentPositions();
            AudioTrack.Invalidate();
        }

        ///
        /// Function for fixing bugs on left drag (bug - when segment is dragged at position 0 on video track the swap of thumbnails properly doesn't happen).
        ///
        private void UpdateThumbnailsForLeftDraggingFix(int nextBoundsIndex)
        {
            var draggedThumbnailsIds = draggedThumbnails.Select(t => t.thumbnail).ToList();
            var currentBounds = selectedVideoBounds;

            var updatedBars = new List<(Image thumbnail, float position)>();

            var nextBounds = allVideoBounds[nextBoundsIndex];

            var leftEdge = Math.Min(currentBounds.Left, nextBounds.Left);
            var rightEdge = Math.Max(currentBounds.Right, nextBounds.Right);

            foreach (var (thumbnail, position) in allThumbnailsWithPositions)
            {
                if (!draggedThumbnailsIds.Contains(thumbnail) && position >= leftEdge && position < rightEdge)
                {
                    updatedBars.Add((
                        thumbnail,
                        position + currentBounds.Width
                    ));
                }
            }

            for (int i = 0; i < allThumbnailsWithPositions.Count; i++)
            {
                var (thumbnail, position) = allThumbnailsWithPositions[i];
                var updatedBar = updatedBars.FirstOrDefault(t => t.thumbnail == thumbnail);
                if (updatedBar != default)
                {
                    allThumbnailsWithPositions[i] = updatedBar;
                }
            }

            updatedBars.Clear();

            VideoTrack.Invalidate();
        }

        ///
        /// Function for fixing bugs on left drag (bug - when segment is dragged at position 0 on audio track the swap doesn't happen).
        ///
        private void UpdateBarsForLeftDraggingFix(int nextBoundsIndex)
        {
            var draggedBarsIds = draggedBars.Select(t => t.id).ToList();
            var currentBounds = selectedAudioBounds;

            var updatedBars = new List<(RectangleF bar, int id)>();

            var nextBounds = allAudioSegments[nextBoundsIndex];

            var leftEdge = Math.Min(currentBounds.Left, nextBounds.Left);
            var rightEdge = Math.Max(currentBounds.Right, nextBounds.Right);

            foreach (var (bar, id) in allAudioAmplitudeBars)
            {
                if (!draggedBarsIds.Contains(id) && bar.X >= leftEdge && bar.X <= rightEdge)
                {
                    updatedBars.Add((
                        new RectangleF(
                            bar.X + currentBounds.Width,
                            bar.Top,
                            bar.Width,
                            bar.Height
                        ),
                        id
                    ));
                }
            }

            for (int i = 0; i < allAudioAmplitudeBars.Count; i++)
            {
                var (bar, id) = allAudioAmplitudeBars[i];
                var updatedBar = updatedBars.FirstOrDefault(t => t.id == id);
                if (updatedBar != default)
                {
                    allAudioAmplitudeBars[i] = updatedBar;
                }
            }

            updatedBars.Clear();

            AudioTrack.Invalidate();
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

        private void TestGen_Click(object sender, EventArgs e)
        {
            GeneratePreview();
        }

        private void Debug_Click(object sender, EventArgs e)
        {
            Console.WriteLine("\nVIDEO\n");
            foreach (VideoRenderSegment segment in fullVideoRender)
            {
                Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2} | Path: {segment.FilePath} | TimeLinePosition: {segment.TimeLinePosition}");
            }
            Console.WriteLine("\nAUDIO\n");
            foreach (AudioRenderSegment segment in fullAudioRender)
            {
                Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2} | Path: {segment.FilePath} | TimeLinePosition: {segment.TimeLinePosition}");
            }
            Console.WriteLine($"Editing Ruller Width: {EditingRuller.Width}\nVideo Track Width: {VideoTrack.Width}\nAudio Track Width: {AudioTrack.Width}");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x000F) // WM_PAINT
            {
                Console.WriteLine($"WM_PAINT at {DateTime.Now}");
            }
            base.WndProc(ref m);
        }
    }
}
