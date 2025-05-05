using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

using static RenCloud.Program;
using System.Xml;
using System.Collections.ObjectModel;
using System.Threading;
using System.Drawing.Imaging;
using System.Globalization;

namespace RenCloud
{
    [ExcludeFromCodeCoverage]
    public partial class UserInterfaceForm : Form
    {
        //Exposing for tests

        //VARIABLES & OBJECTS//
        //this.PreviewBox.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "VlcLibs"));//
        private readonly string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "ffmpeg", "bin", "ffmpeg.exe");
        private readonly string outputPath = Path.Combine(Path.GetTempPath(), "VideoPreviews");
        private int ampID = 0;
        private readonly float tolerance = 0.0001f;
        private const string error = "Error";
        private Vlc.DotNet.Forms.VlcControl PreviewBox;

        public UserInterfaceForm(bool renderUIVLC)
        {
            //INITIALIZATIONS//
            InitializeComponent();
            VLCInitializer(renderUIVLC);
            InitializeDragFunctionality();
            InitializeAutoScrollTimer();
            InitializePlayBackTimer();
            InitializePlayBackStopWatch();
            InitializeMouseEvents();
            InitializeSettingsGroups();
            ToolTipDropdownPanel_File_Initialization();
            ToolTipDropdownPanel_ControlPanel_Initialization();
            ToolTipDropdownPanel_Help_Initialization();
            InitializingDoubleBuffersForComponents();
            PanelOrchestration(panel5);
            EditorPanelShow();
            ClearingTempPaths();

            trackerMoveTimer = new System.Windows.Forms.Timer
            {
                Interval = 2
            };
            trackerMoveTimer.Tick += TrackerMoveTimer_Tick;
            button8.MouseDown += button8_MouseDown;
            button8.MouseUp += button8_MouseUp;
            button9.MouseDown += button9_MouseDown;
            button9.MouseUp += button8_MouseUp;

            //VARIABLES&ADJUSTMENTS//
            pixelsPerMillisecond = pixelsPerSecond / 1000f;
        }

        [ExcludeFromCodeCoverage]
        public void VLCInitializer(bool renderUIVLC)
        {
            if (renderUIVLC)
            {
                PreviewBox_Initialization();
            }
        }

        private void UserInterfaceForm_Load(object sender, EventArgs e)
        {
            //ATTACHMENTS AND ON-LOAD FEATURES//
            AttachEditingRullerMouseEvents();
            AttachEditingRullerPaintEvent();
            AttachFormDraggingToComponents();
            AttachPlaceHolderPaintEvents();
            AttachToolBarEvents();
            AttachNavMenuEvents();
            Application.AddMessageFilter(new GlobalMouseClickFilter(
                this,
                new List<Label> { file_label, controlPanel_label, help_label },
                new List<Panel> { dropDownPanel1, dropDownPanel2, dropDownPanel3 },
                () => HideAllDropdowns()
            ));

            exportPathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "RenCloud_Outputs");
            exportPathTextBoxAdvanced.Text = exportPathTextBox.Text;

            VideoTrack.AllowDrop = true;
            AudioTrack.AllowDrop = true;
            VideoTrack.DragEnter += Media_DragEnter;
            AudioTrack.DragEnter += Media_DragEnter;
            VideoTrack.DragDrop += Media_DragDrop;
            AudioTrack.DragDrop += Media_DragDrop;

            LoadTemplates();
            DrawBorderDropDownPanels(dropDownPanel1);
            DrawBorderDropDownPanels(dropDownPanel2);
            DrawBorderDropDownPanels(dropDownPanel3);

            ApplyRoundCorners();
        }

        //----------------------------------------------------------------------------------------//
        //-----------------------------//UX-UI Interactions//-------------------------------------//
        //----------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private DragFunctionality dragFunctionality;
        private System.Windows.Forms.Label selectedLabel = null;
        private int segmentsToLeft = 0;
        private bool isActive = false;
        private bool isDraggingSegment = false;
        private bool rightMv = false;
        private Point initialMousePosition;
        private RectangleF initialSegmentBounds;
        private readonly Color _transparentColor = Color.FromArgb(0, 0, 0, 0);
        private readonly Color _hoverColor = Color.FromArgb(50, 100, 50, 150);
        private readonly Color _clickedColor = Color.FromArgb(100, 150, 100, 200);
        private readonly List<(Image thumbnail, float position)> tempPositions = new List<(Image, float)>();
        private readonly List<(Image thumbnail, float position, float width)> draggedThumbnails = new List<(Image, float, float)>();
        private readonly List<(Image thumbnail, float position)> draggedThumbnailsInitialPosition = new List<(Image, float)>();
        private readonly List<(RectangleF bar, int id)> tempPositionsBars = new List<(RectangleF bar, int id)>();
        private readonly List<(RectangleF bar, int id)> draggedBars = new List<(RectangleF bar, int id)>();
        private readonly List<(RectangleF bar, int id)> draggedBarsInitialPosition = new List<(RectangleF bar, int id)>();

        ////FEATURES & EVENT HANDLERS & INTERACTIONS////

        ///
        /// Hide panel for video editing and show render screen
        /// 
        public void button1_OnClick(object sender, EventArgs e)
        {
            button2.BackColor = Color.Navy;
            button1.BackColor = Color.MediumBlue;

            panel5.Hide();
            RenderPanel.Show();
            RenderPanel.BringToFront();
        }


        ///
        /// Hide panel for rendering and show video editing screen 
        ///
        public void button2_OnClick(object sender, EventArgs e)
        {
            button2.BackColor = Color.MediumBlue;
            button1.BackColor = Color.Navy;

            RenderPanel.Hide();
            panel5.Show();
            panel5.BringToFront();
        }

        ///
        /// On hover, if not selected, show the "hoverColor"
        ///
        private void Label_MouseEnter(object sender, EventArgs e)
        {
            var lbl = sender as System.Windows.Forms.Label;
            if (lbl != selectedLabel)
            {
                lbl.BackColor = _hoverColor;
            }
        }

        ///
        /// On mouse leave, if not selected, revert to transparent; if selected, keep hover color
        ///
        private void Label_MouseLeave(object sender, EventArgs e)
        {
            var lbl = sender as System.Windows.Forms.Label;
            if (lbl != selectedLabel)
            {
                lbl.BackColor = _transparentColor;
            }
            var fixedColor = Color.FromArgb(90, 25, 25, 40);
            option1Label.BackColor = fixedColor;
            option2Label.BackColor = fixedColor;
            option3Label.BackColor = fixedColor;
            option4Label.BackColor = fixedColor;
            option5Label.BackColor = fixedColor;
        }

        ///
        /// On mouse down, if not selected, show a more "clickedColor"
        ///
        private void Label_MouseDown(object sender, MouseEventArgs e)
        {
            var lbl = sender as System.Windows.Forms.Label;
            if (lbl != selectedLabel)
            {
                lbl.BackColor = _clickedColor;
            }
        }

        ///
        /// On mouse up, if not selected, revert to the hover color
        ///
        private void Label_MouseUp(object sender, MouseEventArgs e)
        {
            var lbl = sender as System.Windows.Forms.Label;
            if (lbl != selectedLabel)
            {
                lbl.BackColor = _hoverColor;
            }
        }

        ///
        /// Actual Click event: unselect the old label, select the new one
        ///
        private void Label_Click(object sender, EventArgs e)
        {
            var lbl = sender as System.Windows.Forms.Label;

            if (selectedLabel != null && selectedLabel != lbl)
            {
                selectedLabel.BackColor = _transparentColor;
            }

            selectedLabel = lbl;
            selectedLabel.BackColor = _hoverColor;
            var labelLocation = lbl.PointToScreen(Point.Empty);
            var formLocation = this.PointToScreen(Point.Empty);
            int labelXonForm = labelLocation.X - formLocation.X;
            int labelYonForm = labelLocation.Y - formLocation.Y;

            Panel currentPanel = null;

            if (lbl == file_label)
                currentPanel = dropDownPanel1;
            else if (lbl == controlPanel_label)
                currentPanel = dropDownPanel2;
            else if (lbl == help_label)
                currentPanel = dropDownPanel3;

            if (currentPanel != null)
            {
                int labelHeight = 30;
                int labelCount = currentPanel.Controls.Count;
                currentPanel.Height = labelHeight * labelCount;

                int x = labelXonForm + (lbl.Width / 2) - (currentPanel.Width / 2);
                int y = labelYonForm + lbl.Height;
                currentPanel.Location = new Point(x, y);
                currentPanel.Visible = true;
                currentPanel.BringToFront();
            }

            dropDownPanel1.Visible = currentPanel == dropDownPanel1;
            dropDownPanel2.Visible = currentPanel == dropDownPanel2;
            dropDownPanel3.Visible = currentPanel == dropDownPanel3;
        }

        ///
        /// Handler sthat closes the panel on click
        /// 
        private void HideAllDropdowns(object sender = null, MouseEventArgs e = null)
        {
            dropDownPanel1.Visible = false;
            dropDownPanel2.Visible = false;
            dropDownPanel3.Visible = false;

            if (selectedLabel != null)
            {
                selectedLabel.BackColor = _transparentColor;
                selectedLabel = null;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                HideAllDropdowns();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public class GlobalMouseClickFilter : IMessageFilter
        {
            private readonly Form _form;
            private readonly List<Label> _labels;
            private readonly List<Panel> _dropdowns;
            private readonly Action _hideAllDropdowns;

            public GlobalMouseClickFilter(Form form, List<Label> labels, List<Panel> dropdowns, Action hideAllDropdowns)
            {
                _form = form;
                _labels = labels;
                _dropdowns = dropdowns;
                _hideAllDropdowns = hideAllDropdowns;
            }

            public bool PreFilterMessage(ref Message m)
            {
                const int WM_LBUTTONDOWN = 0x0201;

                if (m.Msg == WM_LBUTTONDOWN)
                {
                    Point screenPoint = Control.MousePosition;
                    Point clientPoint = _form.PointToClient(screenPoint);
                    Control clickedControl = _form.GetChildAtPoint(clientPoint, GetChildAtPointSkip.Invisible);

                    bool clickedLabel = _labels.Contains(clickedControl as Label);

                    bool clickedDropdown = _dropdowns.Any(panel => panel.Visible && panel.Bounds.Contains(clientPoint));

                    if (!clickedLabel && !clickedDropdown)
                    {
                        _hideAllDropdowns();
                    }
                }

                return false;
            }
        }

        ///
        /// MouseDown handler for the audio track; marks the selected region if clicked inside a segment and storing necessary info for dragging selected segments.
        ///
        private void AudioTrack_MouseDownHandler(object sender, MouseEventArgs e)
        {
            selectedVideoBounds = Rectangle.Empty;
            draggedBars.Clear();
            draggedBarsInitialPosition.Clear();
            tempPositionsBars.AddRange(allAudioAmplitudeBars);
            var selectedSegment = allAudioSegments.FirstOrDefault(bounds => bounds.Contains(e.Location));
            if (selectedSegment.Width > 0)
            {
                selectedAudioBounds = selectedSegment;
                isDraggingSegment = true;
                initialMousePosition = e.Location;
                initialSegmentBounds = selectedSegment;

                var barsInBounds = allAudioAmplitudeBars
                    .Where(pair => pair.bar.X >= selectedSegment.Left && pair.bar.X < selectedSegment.Right)
                    .ToList();

                draggedBars.AddRange(barsInBounds);
                draggedBarsInitialPosition.AddRange(barsInBounds);
            }
            else
            {
                selectedAudioBounds = Rectangle.Empty;
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

                    int globalIndex = allAudioAmplitudeBars.FindIndex(t => t.id == id && Math.Abs(t.bar.X - originalPosition) < tolerance);
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
                selectedAudioBounds = updatedBounds;
                AudioTrack.Invalidate();
            }
        }

        ///
        /// MouseUp handler for the audio track; on release specifies logic whether to move the segment or not and clearing information.
        /// 
        private void AudioTrack_MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (isDraggingSegment)
            {
                int draggedIndex = allAudioSegments.IndexOf(selectedAudioBounds);
                RectangleF draggedSegment = selectedAudioBounds;

                if (draggedIndex != -1)
                {
                    int newIndex = DetermineNewAudioIndex(draggedIndex, draggedSegment);
                    bool segmentMoved = newIndex != -1 && newIndex != draggedIndex;
                    HandleSegmentSwap_AudioTrack(segmentMoved, newIndex, draggedIndex);
                }
                else
                {
                    Console.WriteLine("Error: Dragged segment index not found.");
                }
                ResetDraggingState();
            }
            AudioTrack.Invalidate();
        }

        ///
        /// Handles the movement of amplitude bars and segments across the timeline, deciding wether it's a right move or left.
        ///
        private void HandleSegmentSwap_AudioTrack(bool segmentMoved, int newIndex, int draggedIndex)
        {
            int index;
            if (segmentMoved)
            {
                index = newIndex > draggedIndex ? newIndex - 1 : newIndex;
                MoveAudioSegment(draggedIndex, newIndex);
                NormalizeAudioSegmentPositions();
                RectangleF tempBounds = selectedAudioBounds;
                selectedAudioBounds = allAudioSegments[index];
                SyncFullAudioRender();
                UpdateBarPositions(index);
                if (rightMv)
                {
                    HandleRightSwap_AudioTrack(index);
                }
                else
                {
                    HandleLeftSwap_AudioTrack(index, tempBounds);
                }
            }
            else
            {
                RevertSegmentAndBars();
                ResetDraggingState();
                return;
            }
            _ = GeneratePreview();
        }

        ///
        /// Handles the Right Move of the segments and amplitude bars, calling necessary methods to updated positions of bars and segments on the audio track.
        ///
        public void HandleRightSwap_AudioTrack(int index)
        {
            rightMv = false;
            if (Math.Abs(selectedAudioBounds.Left) < tolerance)
            {
                UpdateBarsForLeftDraggingFix(index);
            }
            else
            {
                UpdateBarsForRightDraggingFix();
            }
        }

        ///
        /// Handles the Left Move of the segments and amplitude bars, calling necessary methods to updated positions of bars and segments on the audio track.
        ///
        public void HandleLeftSwap_AudioTrack(int index, RectangleF tempBounds)
        {
            if (Math.Abs(tempBounds.Left) < tolerance)
            {
                UpdateBarsForLeftDraggingFix(index + 1);
            }
            else if (Math.Abs(selectedAudioBounds.Left) < tolerance)
            {
                UpdateBarsForLeftDraggingFix(index + 1);
            }
            else
            {
                UpdateBarsForLeftDraggingFix(index);
            }
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
        private int debug = 0;
        private void NormalizeAudioSegmentPositions()
        {
            float currentLeft = 0;

            for (int i = 0; i < allAudioSegments.Count; i++)
            {
                RectangleF segment = allAudioSegments[i];
                GapSequentialArrangement_AudioTrack(segment, currentLeft, i);
                currentLeft += segment.Width;
            }
            debug = 0;
            AudioTrack.Invalidate();
        }

        /// 
        /// The function makes sure there are no gaps between segments, keeping a full structured audio track format.
        /// 
        public void GapSequentialArrangement_AudioTrack(RectangleF segment, float currentLeft, int i)
        {
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
                    JustifyPositionLeft(currentLeft);
                }
                else
                {
                    JustifyPositionRight(currentLeft, segment);
                    debug++;
                }

                if (i < fullAudioRender.Count)
                {
                    fullAudioRender[i].TimeLinePosition = currentLeft / pixelsPerSecond;
                }
            }
        }

        ///
        /// Justify the segments position on left movement.
        ///
        public void JustifyPositionLeft(float currentLeft)
        {
            if (Math.Abs(currentLeft) > tolerance)
            {
                UpdateBarsForLeftDragging();
            }
        }

        ///
        /// Justify the segments position on right movement.
        ///
        public void JustifyPositionRight(float currentLeft, RectangleF segment)
        {
            if (debug < 1)
            {
                UpdateBarsForRightDragging(segment, currentLeft);
            }
        }


        ///
        /// MouseDown handler for the video track; marks the selected region if clicked inside a segment and storing necessary info for dragging selected segments.
        ///
        private void VideoTrack_MouseDownHandler(object sender, MouseEventArgs e)
        {
            selectedAudioBounds = Rectangle.Empty;
            draggedThumbnails.Clear();
            draggedThumbnailsInitialPosition.Clear();

            foreach (var (thumbnail, position, _) in allThumbnailsWithPositions)
            {
                tempPositions.Add((thumbnail, position));
            }
            var selectedSegment = allVideoBounds.FirstOrDefault(bounds => bounds.Contains(e.Location));

            if (selectedSegment.Width > 0)
            {
                selectedVideoBounds = selectedSegment;
                isDraggingSegment = true;
                initialMousePosition = e.Location;
                initialSegmentBounds = selectedSegment;

                var thumbnailsInBounds = allThumbnailsWithPositions
                    .Where(t => t.position >= selectedSegment.Left && t.position < selectedSegment.Right)
                    .ToList();

                if (thumbnailsInBounds.Any())
                {
                    draggedThumbnails.AddRange(thumbnailsInBounds);
                    draggedThumbnailsInitialPosition.AddRange(thumbnailsInBounds.Select(t => (t.thumbnail, t.position)));
                }
            }
            else
            {
                selectedVideoBounds = RectangleF.Empty;
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
                    var (thumbnail, originalPosition, width) = draggedThumbnails[i];
                    float newPosition = originalPosition + segmentOffset;
                    draggedThumbnails[i] = (thumbnail, newPosition, width);

                    int globalIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail && Math.Abs(t.position - originalPosition) < tolerance);
                    if (globalIndex != -1)
                    {
                        allThumbnailsWithPositions[globalIndex] = (thumbnail, newPosition, allThumbnailsWithPositions[globalIndex].thumbnailWidth);
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

                if (draggedIndex != -1)
                {
                    int newIndex = DetermineNewIndex(draggedIndex, draggedSegment);
                    bool segmentMoved = newIndex != -1 && newIndex != draggedIndex;

                    HandleSegmentSwap_VideoTrack(segmentMoved, newIndex, draggedIndex);
                }
                else
                {
                    Console.WriteLine("Error: Dragged segment index not found.");
                }

                ResetDraggingState();
            }
            VideoTrack.Invalidate();
        }

        ///
        /// Handles the movement of images and segments across the timeline, deciding wether it's a right move or left.
        ///
        private void HandleSegmentSwap_VideoTrack(bool segmentMoved, int newIndex, int draggedIndex)
        {
            if (segmentMoved)
            {
                int index = newIndex > draggedIndex ? newIndex - 1 : newIndex;
                MoveSegment(draggedIndex, newIndex);
                NormalizeSegmentPositions();
                SyncFullVideoRender();
                UpdateThumbnailPositions(index);
                var tempBounds = selectedVideoBounds;
                selectedVideoBounds = allVideoBounds[index];
                if (rightMv)
                {
                    HandleRightSwap_VideoTrack(index);
                    _ = GeneratePreview();
                    return;
                }
                HandleLeftSwap_VideoTrack(index, tempBounds);
            }
            else
            {
                RevertSegmentAndThumbnails();
                ResetDraggingState();
                return;
            }
            _ = GeneratePreview();
        }

        ///
        /// Handles the Right Move of the segments and thumbnails, calling necessary methods to updated positions of thumbnails and segments on the video track.
        ///
        public void HandleRightSwap_VideoTrack(int index)
        {
            rightMv = false;
            if (Math.Abs(selectedVideoBounds.Left) < tolerance)
            {
                Console.WriteLine("First Call");
                UpdateThumbnailsForLeftDraggingFix(index);
            }
            else
            {
                Console.WriteLine("Right Call");
                FixImages(selectedVideoBounds);
            }
        }

        ///
        /// Handles the Left Move of the segments and thumbnails, calling necessary methods to updated positions of thumbnails and segments on the video track.
        ///
        public void HandleLeftSwap_VideoTrack(int index, RectangleF tempBounds)
        {
            if (Math.Abs(tempBounds.Left) < tolerance)
            {
                Console.WriteLine("Second Call");
                UpdateThumbnailsForLeftDraggingFix(index + 1);
            }
            else
            {
                Console.WriteLine("Third Call");
                UpdateThumbnailsForLeftDraggingFix(index);
            }
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
            Program.Corners.AttributesRoundCorners(this, isActive);
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
            autoScrollTimer = new System.Windows.Forms.Timer
            {
                Interval = 2
            };
            autoScrollTimer.Tick += new EventHandler(AutoScrollTimer_Tick);
            autoScrollTimer.Tick += new EventHandler(AutoScrollTimer_Tick);
            autoScrollTimer.Enabled = false;
        }

        ///
        /// Initializing playbackTimer.
        ///
        public void InitializePlayBackTimer()
        {
            playbackTimer = new System.Windows.Forms.Timer
            {
                Interval = 1
            };
            playbackTimer.Tick += new EventHandler(PlaybackTimer_Tick);
        }

        ///
        /// Adjusts corner styling when the form is activated.
        ///
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            isActive = false;
            Program.Corners.AttributesRoundCorners(this, isActive);
        }

        ///
        /// Adjusts corner styling when the form is deactivated.
        ///
        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            isActive = true;
            Program.Corners.AttributesRoundCorners(this, isActive);
        }

        ///
        /// Starts video playback, restarts timers, and sets the 'wasPlayingBeforeDrag' flag.
        ///
        private void PlayPreview()
        {
            if (!PreviewBox.IsPlaying && PreviewBox.GetCurrentMedia() != null)
            {
                PreviewBox.Play();
                playbackStopwatch.Restart();
                playbackTimer.Start();

                suppressAutoScroll = true;

                UpdatePlaybackLabel(PreviewBox.Time);
            }
        }


        ///
        /// Pauses video playback, stops timers, and updates playback label.
        /// 
        private void PausePreview()
        {
            if (PreviewBox.IsPlaying)
            {
                PreviewBox.Pause();

                playbackTimer.Stop();
                playbackStopwatch.Stop();
            }

            currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;
            UpdatePlaybackLabel(PreviewBox.Time);
            EditingRuller.Invalidate();
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

            if (!SelectSegmentStatus()) return;

            if (selectedVideoBounds != RectangleF.Empty)
            {
                float oldStartTime = selectedVideoBounds.Left / pixelsPerSecond;
                var originalVideo = fullVideoRender.FirstOrDefault(v => Math.Abs(v.TimeLinePosition - oldStartTime) < tolerance);
                float splitOffset = trackerXPosition - selectedVideoBounds.Left;

                if (!VideoSegmentBoundaryCheck(originalVideo, splitOffset)) return;

                selectedVideoBounds = UpdateVideoDataSegments(splitOffset, originalVideo);

                PerformVideoSegmentSplit();

                NormalizeSegmentPositions();
                VideoTrack.Invalidate();
            }

            if (selectedAudioBounds != RectangleF.Empty)
            {
                float splitOffset = trackerXPosition - selectedAudioBounds.Left;
                var originalAudio = fullAudioRender.FirstOrDefault(a => Math.Abs(a.TimeLinePosition * pixelsPerSecond - selectedAudioBounds.Left) < tolerance);

                if (!AudioSegmentBoundaryCheck(splitOffset, originalAudio)) return;

                selectedAudioBounds = UpdateAudioDataSegments(splitOffset, originalAudio);

                PerformAudioSegmentSplit();

                NormalizeAudioSegmentPositions();
                NormalizeSegmentPositions();
                AudioTrack.Invalidate();
            }
        }

        ///
        /// Checks if any segment selected to proceed with next steps
        ///
        private bool SelectSegmentStatus()
        {
            if (selectedVideoBounds == RectangleF.Empty && selectedAudioBounds == RectangleF.Empty)
            {
                MessageBox.Show("No segment selected. Please select a video or audio segment to split.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        ///
        /// Checks if the user is trying to perform a split at the segments boundary of the video track or outside the selected segment
        ///
        private bool VideoSegmentBoundaryCheck(VideoRenderSegment originalVideo, float splitOffset)
        {
            if (trackerXPosition < selectedVideoBounds.Left || trackerXPosition > selectedVideoBounds.Right)
            {
                MessageBox.Show("Tracker is outside the selected video segment range.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (splitOffset <= 0 || splitOffset >= selectedVideoBounds.Width)
            {
                MessageBox.Show("Cannot split at the boundary.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (originalVideo == null)
            {
                MessageBox.Show("Could not find the original video segment for the selected bounds.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        ///
        /// Checks if the user is trying to perform a split at the segments boundary of the audio track or outside the selected segment
        ///
        private bool AudioSegmentBoundaryCheck(float splitOffset, AudioRenderSegment originalAudio)
        {
            if (trackerXPosition < selectedAudioBounds.Left || trackerXPosition > selectedAudioBounds.Right)
            {
                MessageBox.Show("Tracker is outside the selected audio segment range.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (originalAudio == null)
            {
                MessageBox.Show("Could not find the original audio segment for the selected bounds.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (splitOffset <= 0 || splitOffset >= selectedAudioBounds.Width)
            {
                MessageBox.Show("Cannot split at the boundary.",
                                error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        ///
        /// Performs the segment split for video track including images.
        ///
        private void PerformVideoSegmentSplit()
        {
            for (int i = 0; i < allThumbnailsWithPositions.Count; i++)
            {
                var (origImage, thumbX, origWidth) = allThumbnailsWithPositions[i];

                float thumbLeft = thumbX;
                float thumbRight = thumbX + origWidth;

                if (trackerXPosition > thumbLeft && trackerXPosition < thumbRight)
                {
                    float leftPartWidth = trackerXPosition - thumbLeft;
                    float rightPartWidth = thumbRight - trackerXPosition;

                    if (leftPartWidth < 1 || rightPartWidth < 1)
                        continue;
                    Image leftImage = CropImage(
                        origImage,
                        new Rectangle(0, 0, (int)leftPartWidth, origImage.Height)
                    );
                    Image rightImage = CropImage(
                        origImage,
                        new Rectangle((int)leftPartWidth, 0, (int)rightPartWidth, origImage.Height)
                    );
                    allThumbnailsWithPositions.RemoveAt(i);
                    allThumbnailsWithPositions.Insert(allThumbnailsWithPositions.Count - 1, (leftImage, thumbLeft - 3f, leftPartWidth));
                    allThumbnailsWithPositions.Insert(allThumbnailsWithPositions.Count, (rightImage, trackerXPosition + 3f, rightPartWidth));
                }
            }
        }

        ///
        ///
        ///
        private void PerformAudioSegmentSplit()
        {
            for (int i = 0; i < allAudioAmplitudeBars.Count; i++)
            {
                var (barRect, barId) = allAudioAmplitudeBars[i];

                if (barRect.Left < trackerXPosition && barRect.Right > trackerXPosition)
                {
                    float leftWidth = trackerXPosition - barRect.Left;
                    float rightWidth = barRect.Right - trackerXPosition;

                    if (leftWidth < 1f || rightWidth < 1f)
                        continue;

                    RectangleF firstBar = new RectangleF(barRect.Left - 2f, barRect.Y, leftWidth, barRect.Height);
                    allAudioAmplitudeBars[i] = (firstBar, barId);

                    RectangleF secondBar = new RectangleF(trackerXPosition + 2f, barRect.Y, rightWidth, barRect.Height);
                    int newBarId = ampID++;

                    allAudioAmplitudeBars.Insert(i + 1, (secondBar, newBarId));

                    ampID++;
                }
            }
        }

        ///
        /// This creates the new segment programatically and updates the data structure after performing the visual split of audio segment
        ///
        private RectangleF UpdateAudioDataSegments(float splitOffset, AudioRenderSegment originalAudio)
        {
            RectangleF firstSegment = new RectangleF(selectedAudioBounds.Left, selectedAudioBounds.Top, splitOffset, selectedAudioBounds.Height);
            RectangleF secondSegment = new RectangleF(trackerXPosition, selectedAudioBounds.Top, selectedAudioBounds.Right - trackerXPosition, selectedAudioBounds.Height);
            int oldAudioId = originalAudio.Id;
            int audioIndex = allAudioSegments.IndexOf(selectedAudioBounds);
            allAudioSegments.RemoveAt(audioIndex);
            allAudioSegments.Insert(audioIndex, firstSegment);
            allAudioSegments.Insert(audioIndex + 1, secondSegment);
            fullAudioRender.RemoveAt(audioIndex);
            var firstAudioRender = new AudioRenderSegment
            {
                FilePath = originalAudio.FilePath,
                StartTime = originalAudio.StartTime,
                EndTime = originalAudio.StartTime + (splitOffset / pixelsPerSecond),
                TimeLinePosition = firstSegment.Left / pixelsPerSecond,
                Id = oldAudioId
            };
            var secondAudioRender = new AudioRenderSegment
            {
                FilePath = originalAudio.FilePath,
                StartTime = originalAudio.StartTime + (splitOffset / pixelsPerSecond),
                EndTime = originalAudio.EndTime,
                TimeLinePosition = secondSegment.Left / pixelsPerSecond,
                Id = oldAudioId + 1
            };
            fullAudioRender.Insert(audioIndex, firstAudioRender);
            fullAudioRender.Insert(audioIndex + 1, secondAudioRender);
            for (int i = audioIndex + 2; i < fullAudioRender.Count; i++)
            {
                fullAudioRender[i].Id++;
            }

            return firstSegment;
        }

        ///
        /// This creates the new segment programatically and updates the data structure after performing the visual split of video segment
        ///
        private RectangleF UpdateVideoDataSegments(float splitOffset, VideoRenderSegment originalVideo)
        {
            RectangleF firstSegment = new RectangleF(
                    selectedVideoBounds.Left,
                    selectedVideoBounds.Top,
                    splitOffset,
                    selectedVideoBounds.Height
                );
            RectangleF secondSegment = new RectangleF(
                trackerXPosition,
                selectedVideoBounds.Top,
                selectedVideoBounds.Right - trackerXPosition,
                selectedVideoBounds.Height
            );

            int oldId = originalVideo.Id;
            int index = allVideoBounds.IndexOf(selectedVideoBounds);

            allVideoBounds.RemoveAt(index);
            allVideoBounds.Insert(index, firstSegment);
            allVideoBounds.Insert(index + 1, secondSegment);

            fullVideoRender.RemoveAt(index);
            var firstRenderSegment = new VideoRenderSegment
            {
                FilePath = originalVideo.FilePath,
                StartTime = originalVideo.StartTime,
                EndTime = originalVideo.StartTime + (splitOffset / pixelsPerSecond),
                TimeLinePosition = firstSegment.Left / pixelsPerSecond,
                Id = oldId
            };
            var secondRenderSegment = new VideoRenderSegment
            {
                FilePath = originalVideo.FilePath,
                StartTime = originalVideo.StartTime + (splitOffset / pixelsPerSecond),
                EndTime = originalVideo.EndTime,
                TimeLinePosition = secondSegment.Left / pixelsPerSecond,
                Id = oldId + 1
            };
            fullVideoRender.Insert(index, firstRenderSegment);
            fullVideoRender.Insert(index + 1, secondRenderSegment);

            for (int i = index + 2; i < fullVideoRender.Count; i++)
            {
                fullVideoRender[i].Id++;
            }

            return firstSegment;
        }

        ///
        /// Removes selected segment from timeline.
        ///
        private async void RemoveSegment_Click(object sender, EventArgs e)
        {
            if (!SelectSegmentStatus()) return;

            bool videoChanged = false;
            bool audioChanged = false;

            if (selectedVideoBounds != RectangleF.Empty)
            {
                videoChanged = true;

                PerformVideoSegmentRemoval(selectedVideoBounds.Width);

                selectedVideoBounds = RectangleF.Empty;
            }
            if (selectedAudioBounds != RectangleF.Empty)
            {
                audioChanged = true;

                PerformAudioSegmentRemoval(selectedAudioBounds.Width);

                selectedAudioBounds = RectangleF.Empty;
            }

            float videoWidth = allVideoBounds.Any() ? allVideoBounds.Max(b => b.Right) : 0;
            float audioWidth = allAudioSegments.Any() ? allAudioSegments.Max(b => b.Right) : 0;

            const int widthPlaceholderDefault = 733;

            this.Invoke(new Action(() =>
            {
                if (videoChanged)
                {
                    UpdateVideoTrackAfterRemoval(videoWidth, audioWidth, widthPlaceholderDefault);
                }

                if (audioChanged)
                {
                    UpdateAudioTrackAfterRemoval(audioWidth, videoWidth, widthPlaceholderDefault);
                }
                EditingRuller.Invalidate();
            }));

            SyncMediaTracks();
            await GeneratePreview();
        }

        ///
        /// Update audio track after removal
        ///
        public void UpdateAudioTrackAfterRemoval(float audioWidth, float videoWidth, int widthPlaceholderDefault)
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

        ///
        /// Update video track after removal
        ///
        public void UpdateVideoTrackAfterRemoval(float videoWidth, float audioWidth, int widthPlaceholderDefault)
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

        ///
        /// Remove the selected audio segment from data structure
        ///
        private void PerformAudioSegmentRemoval(float removedWidthAudio)
        {
            allAudioSegments.RemoveAll(segment => Math.Abs(segment.Left - selectedAudioBounds.Left) < tolerance && Math.Abs(segment.Right - selectedAudioBounds.Right) < tolerance);

            for (int i = 0; i < allAudioSegments.Count; i++)
            {
                var bounds = allAudioSegments[i];
                if (bounds.Left > selectedAudioBounds.Left)
                {
                    allAudioSegments[i] = new RectangleF(
                        bounds.X - removedWidthAudio,
                        bounds.Y,
                        bounds.Width,
                        bounds.Height
                    );
                }
            }

            for (int i = allAudioAmplitudeBars.Count - 1; i >= 0; i--)
            {
                var (bar, id) = allAudioAmplitudeBars[i];

                if (bar.Left >= selectedAudioBounds.Left && bar.Right <= selectedAudioBounds.Right)
                {
                    allAudioAmplitudeBars.RemoveAt(i);
                }
                else if (bar.Left > selectedAudioBounds.Left)
                {
                    RectangleF newBar = new RectangleF(
                        bar.X - removedWidthAudio,
                        bar.Y,
                        bar.Width,
                        bar.Height
                    );
                    allAudioAmplitudeBars[i] = (newBar, id);
                }
            }

            float oldTimeLinePos = selectedAudioBounds.Left / pixelsPerSecond;
            var audioSegmentToRemove = fullAudioRender.FirstOrDefault(a => Math.Abs(a.TimeLinePosition - oldTimeLinePos) < 0.1);
            if (audioSegmentToRemove != null)
            {
                fullAudioRender.Remove(audioSegmentToRemove);
            }

            float runningTime = 0f;
            foreach (var segment in fullAudioRender)
            {
                segment.TimeLinePosition = runningTime;
                runningTime += segment.EndTime - segment.StartTime;
            }

            widthAudio = allAudioSegments.Any() ? allAudioSegments.Max(segment => segment.Right) : 0;
        }


        ///
        /// Remove the selected video segment from data structure
        ///
        private void PerformVideoSegmentRemoval(float removedWidthVideo)
        {
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
                var (thumbnail, position, _) = allThumbnailsWithPositions[i];
                if (position >= selectedVideoBounds.Left && position < selectedVideoBounds.Right)
                {
                    allThumbnailsWithPositions.RemoveAt(i);
                }
                else if (position >= selectedVideoBounds.Right)
                {
                    allThumbnailsWithPositions[i] = (thumbnail, position - removedWidthVideo, allThumbnailsWithPositions[i].thumbnailWidth);
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
                    Id = fullAudioRender.Max(v => v.Id) + 1
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
        private void Button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        ///
        /// Test button for import functionality
        ///
        private void Button4_Click(object sender, EventArgs e)
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
                _ = AddVideoToPanel(filePath);
                _ = AddVideoToTimeline(filePath);
                button1.Enabled = true;
                importedNewVideo = true;
            }
        }

        ///
        /// Import media click event 
        ///
        private void option1Label_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files|*.mp4;*.avi;*.mov;*.wmv;*.mkv",
                Title = "Select a Video File"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                _ = AddVideoToPanel(filePath);
            }
        }

        ///
        /// Will extract only 1 thumbnail for metadata show
        ///
        private Image ExtractSingleThumbnail(string filePath)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "VideoThumbnailSingle");
            Directory.CreateDirectory(tempDir);

            string outputFile = Path.Combine(tempDir, $"thumb_{Guid.NewGuid():N}.jpg");

            string ffmpegArgs = $"-ss 00:00:01 -i \"{filePath}\" -vframes 1 -vf scale=100:70 -q:v 2 -y \"{outputFile}\"";

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = ffmpegPath,
                        Arguments = ffmpegArgs,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true
                    };

                    process.Start();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine($"FFmpeg failed: {stderr}");
                        return null;
                    }

                    if (File.Exists(outputFile))
                    {
                        if (File.Exists(outputFile))
                        {
                            Image copy;

                            using (var stream = new FileStream(outputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                copy = new Bitmap(stream);
                            }

                            try
                            {
                                File.Delete(outputFile);
                            }
                            catch (IOException ex)
                            {
                                Console.WriteLine($"[WARNING] Could not delete thumbnail: {ex.Message}");
                            }

                            return copy;
                        }

                    }
                    else
                    {
                        Console.WriteLine("FFmpeg ran but did not produce an image.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FFmpeg error: {ex.Message}");
            }

            return null;
        }

        //----------------------------------------------------------------------------------------//
        //-----------------------------//RULLER HANDLING//----------------------------------------//
        //----------------------------------------------------------------------------------------//

        ////VARIABLES & OBJECTS///
        private System.Windows.Forms.Timer autoScrollTimer;
        private System.Windows.Forms.Timer playbackTimer;
        private readonly Stopwatch seekTimer = Stopwatch.StartNew();
        private Stopwatch playbackStopwatch;
        private int autoScrollDirection = 0;
        private const int seekUpdateThreshold = 50;
        private long lastKnownVlcTime = 0;
        private float currentPlaybackTime = 0f;
        private readonly float pixelsPerSecond = 50f;
        private readonly float pixelsPerMillisecond;
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

        private float trackerXPosition = 0f;


        ///
        /// Updates the tracker's position based on VLC time or interpolated system time, ensuring smooth playback.
        ///
        private bool suppressAutoScroll = false;

        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingTracker)
            {
                long elapsedSinceLastKnown = playbackStopwatch.ElapsedMilliseconds;
                long interpolatedTime = lastKnownVlcTime + elapsedSinceLastKnown;
                float newPosition = interpolatedTime * pixelsPerMillisecond;
                float maxPosition = Math.Max(widthVideo, widthAudio);

                trackerXPosition = Math.Min(newPosition, maxPosition);

                if (!suppressAutoScroll)
                {
                    EnsureTrackerVisible(trackerXPosition);
                }
                else
                {
                    suppressAutoScroll = false;
                }

                UpdatePlaybackLabel(interpolatedTime);
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
            }

            long currentVlcTime = PreviewBox.Time;
            long videoLength = PreviewBox.Length;

            if (videoLength > 0 && currentVlcTime >= videoLength - 250)
            {
                PreviewBox.Pause();
                PreviewBox.Time = 0;

                lastKnownVlcTime = 0;
                trackerXPosition = 0;
                UpdatePlaybackLabel(0);

                if (wasPlayingBeforeDrag)
                {
                    playbackStopwatch.Restart();
                }
                else
                {
                    playbackStopwatch.Reset();
                    playbackStopwatch.Start();
                    PreviewBox.Play();
                }

                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();

                return;
            }

            if (currentVlcTime != lastKnownVlcTime)
            {
                lastKnownVlcTime = currentVlcTime;
                playbackStopwatch.Restart();
            }
        }

        private void UpdatePreviewBox()
        {
            try
            {
                if (PreviewBox.IsPlaying)
                {
                    PreviewBox.Stop();
                }

                if (File.Exists(lastOutputPath))
                {
                    PreviewBox.SetMedia(new Uri(lastOutputPath));
                    PreviewBox.Play();
                    PreviewBox.Time = lastKnownVlcTime;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

                long now = seekTimer.ElapsedMilliseconds;
                if (now - lastSeekUpdate >= seekUpdateThreshold)
                {
                    lastSeekUpdate = now;

                    if (!isUpdatingPreview)
                    {
                        isUpdatingPreview = true;

                        try
                        {
                            PreviewBox.Time = (long)currentPlaybackTime;
                            PreviewBox.Refresh();
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
                else
                {
                    Console.WriteLine("Update Problem");
                    Console.WriteLine($"{now - lastSeekUpdate}");
                }

                if (!wasPlayingBeforeDrag)
                {
                    PausePreview();
                }

                UpdatePlaybackLabel(currentPlaybackTime);

                AutoScrollTrack(visibleStart, visibleEnd);

                AudioTrack.Invalidate();
                VideoTrack.Invalidate();
                EditingRuller.Invalidate();
            }
        }

        ///
        /// AutoScroll functionality
        ///
        private void AutoScrollTrack(float visibleStart, float visibleEnd)
        {
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
        private List<(Image thumbnail, float position, float thumbnailWidth)> allThumbnailsWithPositions = new List<(Image, float, float)>();
        private List<VideoRenderSegment> fullVideoRender = new List<VideoRenderSegment>();
        private List<RectangleF> allVideoBounds = new List<RectangleF>();
        private RectangleF selectedVideoBounds = RectangleF.Empty;
        private int segmentsVideoCount = 0;
        private float widthVideo = 0f;
        private float widthAudio = 0f;

        public class VideoRenderSegment
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

            foreach (var (thumbImage, thumbPos, thumbWidth) in allThumbnailsWithPositions)
            {
                bool isDragged = draggedThumbnails.Any(dt => dt.thumbnail == thumbImage);
                if (!isDragged)
                {
                    float thumbnailHeight = VideoTrack.Height - 20f;
                    float thumbnailY = 10f;
                    g.DrawImage(thumbImage, thumbPos, thumbnailY, thumbWidth, thumbnailHeight);
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

                foreach (var (thumbImage, thumbPos, thumbWidth) in draggedThumbnails)
                {
                    float thumbnailHeight = VideoTrack.Height - 20f;
                    float thumbnailY = 10f;
                    g.DrawImage(thumbImage, thumbPos, thumbnailY, thumbWidth, thumbnailHeight);
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
            var match = Regex.Match(ffmpegOutput, durationPattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));
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
            var thumbnailArray = new Image[thumbnailCount];
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            Parallel.For(0, thumbnailCount, parallelOptions, i =>
            {
                double timestamp = i * intervalSeconds;
                string outputFile = Path.Combine(tempDir, $"thumbnail_{i:D3}.jpg");
                string ffmpegCommand = $"-ss {timestamp} -i \"{videoFilePath}\" -vframes 1 -vf \"scale=100:100\" -q:v 2 " +
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
                            thumbnailArray[i] = new Bitmap(tempImage);
                        }
                        File.Delete(outputFile);
                        File.Delete(outputFile);
                    }
                    thumbnails = new List<Image>(thumbnailArray);
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

        private const string defaultVal = "Default";

        private void InitPreviewCombos()
        {
            fpsComboBoxPreview.Items.Clear();
            widthComboBoxPreview.Items.Clear();
            heightComboBoxPreview.Items.Clear();

            fpsComboBoxPreview.Items.Add(defaultVal);
            fpsComboBoxPreview.Items.AddRange(new object[] { "15", "20", "25", "30", "35", "40", "45", "50", "55", "60", "120" });

            widthComboBoxPreview.Items.Add(defaultVal);
            widthComboBoxPreview.Items.AddRange(new object[] { "472", "708", "944", "1180", "1416", "1652", "1888", "1934" });

            heightComboBoxPreview.Items.Add(defaultVal);
            heightComboBoxPreview.Items.AddRange(new object[] { "414", "622", "828", "1036", "1242", "1450", "1656", "1696" });

            fpsComboBoxPreview.SelectedItem = defaultVal;
            widthComboBoxPreview.SelectedItem = defaultVal;
            heightComboBoxPreview.SelectedItem = defaultVal;

            initialisedCombs = true;
        }

        private bool generatingPreview = false;
        private bool initialisedCombs = false;
        private int fps = 15;
        private int widthPreview = 472;
        private int heightPreview = 414;
        private bool importedNewVideo = false;
        private string lastOutputPath = "";
        private string lastVideoCommand = "";
        private string lastAudioCommand = "";
        private string lastMergeCommand = "";
        private List<VideoRenderSegment> fullVideoRenderPreview = new List<VideoRenderSegment>();

        private static readonly object previewLock = new object();

        private async Task GeneratePreview()
        {
            Console.WriteLine("GeneratePreview Started - Thread ID: " + Thread.CurrentThread.ManagedThreadId);

            int localFps = 15;
            int localWidth = 472;
            int localHeight = 414;

            try
            {
                Invoke(new Action(() =>
                {
                    var fpsItem = fpsComboBoxPreview?.SelectedItem?.ToString();
                    var widthItem = widthComboBoxPreview?.SelectedItem?.ToString();
                    var heightItem = heightComboBoxPreview?.SelectedItem?.ToString();

                    localFps = string.IsNullOrWhiteSpace(fpsItem) || fpsItem == defaultVal ? 15 : int.Parse(fpsItem);
                    localWidth = string.IsNullOrWhiteSpace(widthItem) || widthItem == defaultVal ? 472 : int.Parse(widthItem);
                    localHeight = string.IsNullOrWhiteSpace(heightItem) || heightItem == defaultVal ? 414 : int.Parse(heightItem);
                }));

                if (!initialisedCombs)
                    InitPreviewCombos();

                lock (previewLock)
                {
                    if (generatingPreview)
                    {
                        Console.WriteLine("⚠️ Preview is already generating.");
                        return;
                    }

                    generatingPreview = true;
                }

                fps = localFps;
                widthPreview = localWidth;
                heightPreview = localHeight;

                string outputDirectory = outputPath;
                string tempDirectory = Path.Combine(outputDirectory, "tempBuild");
                Directory.CreateDirectory(outputDirectory);
                Directory.CreateDirectory(tempDirectory);

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string tempVideoPath = Path.Combine(tempDirectory, $"temp_video_{timestamp}.mp4");
                string tempAudioPath = Path.Combine(tempDirectory, $"temp_audio_{timestamp}.mp4");
                string finalOutputPath = Path.Combine(outputDirectory, $"preview_{timestamp}.mp4");

                bool useFastPreview = fastPreviewCheckBox.Checked;
                Console.WriteLine("⚡ Fast preview: " + useFastPreview + " | Imported new video: " + importedNewVideo);

                var segmentsToRender = BuildSegmentsForPreview(useFastPreview);

                var videoCmd = BuildVideoCommand(segmentsToRender, tempVideoPath);
                var audioCmd = BuildAudioCommand(fullAudioRender, tempAudioPath);
                var mergeCmd = BuildMergeCommand(tempVideoPath, tempAudioPath, finalOutputPath);

                lastVideoCommand = videoCmd;
                lastAudioCommand = audioCmd;
                lastMergeCommand = mergeCmd;

                Console.WriteLine("🎬 Running FFmpeg commands.");

                await Task.Run(async () =>
                {
                    await Task.WhenAll(
                        RunFFmpegCommand(videoCmd, "Video Gen"),
                        RunFFmpegCommand(audioCmd, "Audio Gen")
                    );
                    await RunFFmpegCommand(mergeCmd, "Final Merge");
                });

                _ = Task.Run(() => TempFolderRemovalHandling(tempDirectory, tempVideoPath, tempAudioPath));

                SavePreviewRenderResult(segmentsToRender, finalOutputPath);

                UpdatePreviewUI();

                Console.WriteLine("GeneratePreview Finished");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GeneratePreview: " + ex.ToString());
            }
            finally
            {
                generatingPreview = false;
            }
        }

        private static string BuildMergeCommand(string videoPath, string audioPath, string finalOutputPath)
        {
            StringBuilder cmd = new StringBuilder("-y ");
            cmd.Append($"-i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -c:a copy \"{finalOutputPath}\"");
            return cmd.ToString();
        }

        private List<VideoRenderSegment> BuildSegmentsForPreview(bool useFastPreview)
        {
            List<VideoRenderSegment> segments = new List<VideoRenderSegment>();
            foreach (var seg in fullVideoRender)
            {
                float duration = seg.EndTime - seg.StartTime;
                var previous = fullVideoRenderPreview.FirstOrDefault(p => p.Id == seg.Id);
                bool isBlack = Path.GetFileName(seg.FilePath)
                                  .Equals("BlackScreenVideo.mp4", StringComparison.OrdinalIgnoreCase);

                float baseStart = (!useFastPreview || importedNewVideo || previous == null)
                                    ? seg.StartTime
                                    : previous.StartTime;
                string filePath = (!useFastPreview || importedNewVideo || previous == null || isBlack)
                                    ? seg.FilePath
                                    : previous.FilePath;

                segments.Add(new VideoRenderSegment
                {
                    Id = seg.Id,
                    FilePath = filePath,
                    StartTime = baseStart,
                    EndTime = baseStart + duration,
                    TimeLinePosition = seg.TimeLinePosition
                });
            }
            return segments;
        }


        private string BuildVideoCommand(List<VideoRenderSegment> segments, string outputPath)
        {
            StringBuilder cmd = new StringBuilder("-y ");
            List<string> filters = new List<string>();
            int index = 0;

            foreach (var seg in segments)
            {
                cmd.Append($"-i \"{seg.FilePath}\" -an ");
                filters.Add($"[{index}:v]trim=start={seg.StartTime:F3}:end={seg.EndTime:F3},setpts=PTS-STARTPTS,scale={widthPreview}:{heightPreview},fps={fps},setsar=1,format=yuv420p[v{index}];");
                index++;
            }

            string concatInputs = string.Join("", Enumerable.Range(0, index).Select(i => $"[v{i}]"));
            filters.Add($"{concatInputs}concat=n={index}:v=1[outv]");
            cmd.Append($"-filter_complex \"{string.Join(" ", filters)}\" -map \"[outv]\" -an -c:v libx264 -crf 35 -preset ultrafast -b:v 4000k \"{outputPath}\"");

            return cmd.ToString();
        }

        private static string BuildAudioCommand(List<AudioRenderSegment> segments, string outputPath)
        {
            StringBuilder cmd = new StringBuilder("-y ");
            List<string> filters = new List<string>();
            int index = 0;

            foreach (var seg in segments)
            {
                cmd.Append($"-i \"{seg.FilePath}\" ");
                filters.Add($"[{index}:a]atrim=start={seg.StartTime:F3}:end={seg.EndTime:F3},asetpts=PTS-STARTPTS[a{index}];");
                index++;
            }

            string concatInputs = string.Join("", Enumerable.Range(0, index).Select(i => $"[a{i}]"));
            filters.Add($"{concatInputs}concat=n={index}:v=0:a=1[outa]");
            cmd.Append($"-filter_complex \"{string.Join(" ", filters)}\" -map \"[outa]\" -vn -c:a aac -b:a 192k \"{outputPath}\"");

            return cmd.ToString();
        }

        private void SavePreviewRenderResult(List<VideoRenderSegment> originalSegments, string finalPath)
        {
            fullVideoRenderPreview = new List<VideoRenderSegment>();
            float offset = 0;

            foreach (var seg in originalSegments)
            {
                float duration = seg.EndTime - seg.StartTime;
                fullVideoRenderPreview.Add(new VideoRenderSegment
                {
                    Id = seg.Id,
                    FilePath = finalPath,
                    StartTime = offset,
                    EndTime = offset + duration,
                    TimeLinePosition = seg.TimeLinePosition
                });
                offset += duration;
            }

            lastOutputPath = finalPath;
            importedNewVideo = false;
        }


        private void UpdatePreviewUI()
        {
            Invoke(new Action(async () =>
            {
                wasPlayingBeforeDrag = PreviewBox.IsPlaying;
                float savedTrackerX = trackerXPosition;
                long savedVlcTime = PreviewBox.Time;
                bool wasTimerRunning = playbackTimer.Enabled;

                playbackTimer.Stop();

                UpdatePreviewBox();

                while (PreviewBox.Length <= 0)
                    await Task.Delay(10);

                PreviewBox.Pause();
                PreviewBox.Time = savedVlcTime;
                trackerXPosition = savedTrackerX;
                lastKnownVlcTime = savedVlcTime;
                playbackStopwatch.Restart();

                UpdatePlaybackLabel(savedVlcTime);
                EditingRuller.Invalidate();
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();

                if (wasPlayingBeforeDrag) PreviewBox.Play();
                if (wasTimerRunning) playbackTimer.Start();
            }));
        }

        ///
        /// Remove temp folder
        ///
        private static void TempFolderRemovalHandling(string tempDirectory, string tempVideoPath, string tempAudioPath)
        {
            if (Directory.Exists(tempDirectory))
            {
                File.Delete(tempVideoPath);
                File.Delete(tempAudioPath);
                Console.WriteLine($"Deleted the output directory: {tempDirectory}");
            }
        }

        ///
        /// Asynchronously adds a video to the timeline, generates thumbnails/audio bars, and triggers preview generation.
        ///
        private async Task AddVideoToTimeline(string filePath)
        {
            button4.Enabled = false;
            float videoDuration = (float)GetVideoDuration(filePath, ffmpegPath);
            int barsPerSecond = 5;
            float barWidth = 8f;
            float barSpacing = 2f;
            float barWidthIncludingSpacing = barWidth + barSpacing;
            float newWidth = videoDuration * pixelsPerSecond;
            float widthEditing;

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
                CalculateNewThumbnailsPositions(thumbnailCount, videoStartPosition, intervalInPixels, thumbnailWidth, newWidth, videoThumbnails);

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
                    allAudioAmplitudeBars.Add((new RectangleF(barXPosition, barYPosition, barWidth, amplitudeHeight), ampID));
                    ampID++;
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
            button1.Enabled = true;
            foreach (VideoRenderSegment segment in fullVideoRender)
            {
                Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2}");
            }
            SyncMediaTracks();
            _ = GeneratePreview();
        }

        /// 
        /// Calculates thumbnail position to ensure each thumbnail is spaced evenly along the timeline.
        ///
        private void CalculateNewThumbnailsPositions(int thumbnailCount, float videoStartPosition, float intervalInPixels, float thumbnailWidth, float newWidth, List<Image> videoThumbnails)
        {
            float position;
            for (int i = 0; i < thumbnailCount; i++)
            {
                if (i == 0)
                {
                    position = videoStartPosition + intervalInPixels * (i) + 6;
                }
                else
                {
                    position = videoStartPosition + intervalInPixels * (i) - 50;
                }
                if (position + thumbnailWidth > videoStartPosition + newWidth)
                    break;

                allThumbnailsWithPositions.Add((videoThumbnails[i], position, 100f));
            }
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

        public class AudioRenderSegment
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
            var randomGenerator = RandomNumberGenerator.Create();
            byte[] data = new byte[16];

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
                        randomGenerator.GetBytes(data);
                        int rand = Math.Abs(BitConverter.ToInt32(data, 0) & int.MaxValue);
                        int barHeight = 5 + (rand % (maxBarHeight - 5));
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
                foreach ((RectangleF bar, _) in allAudioAmplitudeBars)
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
        private FlowLayoutPanel dropDownPanel1;
        private FlowLayoutPanel dropDownPanel2;
        private FlowLayoutPanel dropDownPanel3;
        private System.Windows.Forms.Label option1Label;
        private System.Windows.Forms.Label option2Label;
        private System.Windows.Forms.Label option3Label;
        private System.Windows.Forms.Label option4Label;
        private System.Windows.Forms.Label option5Label;

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
        /// Attach tool-bar events 
        /// 
        public void AttachToolBarEvents()
        {
            AttachHoverClickHandlers(file_label);
            AttachHoverClickHandlers(controlPanel_label);
            AttachHoverClickHandlers(help_label);
            AttachHoverClickHandlers(option1Label);
            AttachHoverClickHandlers(option2Label);
            AttachHoverClickHandlers(option3Label);
            AttachHoverClickHandlers(option4Label);
            AttachHoverClickHandlers(option5Label);
        }

        ///
        /// Attach main navigation menu event
        ///
        public void AttachNavMenuEvents()
        {
            button1.Click += button1_OnClick;
            button2.Click += button2_OnClick;
        }

        ///
        /// One-time hookup so each label reuses the event logic
        ///
        private void AttachHoverClickHandlers(System.Windows.Forms.Label label)
        {
            label.MouseEnter += Label_MouseEnter;
            label.MouseLeave += Label_MouseLeave;
            label.MouseDown += Label_MouseDown;
            label.MouseUp += Label_MouseUp;
            label.Click += Label_Click;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl != file_label && ctrl != controlPanel_label && ctrl != help_label &&
                    ctrl != dropDownPanel1 && ctrl != dropDownPanel2 && ctrl != dropDownPanel3)
                {
                    ctrl.MouseDown += (s, e) => HideAllDropdowns(s, e);
                }
            }

            this.MouseDown += (s, e) => HideAllDropdowns(s, e);
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
        }

        ///
        /// Run ffmpeg generated commands on call.
        ///
        private async Task RunFFmpegCommand(string ffmpegArguments, string description)
        {
            Console.WriteLine($"Running FFmpeg {description} command: {ffmpegArguments}");
            try
            {
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
                        _ = ffmpegProcess.StandardError.ReadToEnd();
                        ffmpegProcess.WaitForExit();
                    }
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"FFmpeg was canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Process error, {ex.Message}");
            }
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
            float segmentLeft = selectedSegment.Left;
            float segmentRight = selectedSegment.Right;

            var draggedThumbnailSet = new HashSet<Image>(
                draggedThumbnails.Select(t => t.thumbnail)
            );

            var updates = new List<(Image thumbnail, float newPosition)>();

            foreach (var (thumbnail, originalPosition) in tempPositions)
            {
                if (draggedThumbnailSet.Contains(thumbnail))
                    continue;

                int thumbIdx = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                if (thumbIdx == -1)
                    continue;

                var (_, currentPosition, thumbWidth) = allThumbnailsWithPositions[thumbIdx];

                bool wasMoved = Math.Abs(currentPosition - originalPosition) > 0.01f;
                if (!wasMoved)
                    continue;

                float originalLeft = originalPosition;
                float originalRight = originalPosition + thumbWidth;

                if (originalLeft > segmentRight)
                {
                    Console.WriteLine("Called Second (Thumbnail originally outside, revert)");
                    updates.Add((thumbnail, originalPosition));
                }
                else if (originalRight > segmentLeft && originalLeft < segmentRight)
                {
                    Console.WriteLine("Called Middle (Thumbnail was in segment, needs shift)");
                    float newPos = originalPosition - selectedSegment.Width;
                    updates.Add((thumbnail, newPos));
                }
            }
            foreach (var (thumbnail, newPosition) in updates)
            {
                int thumbIdx = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                if (thumbIdx != -1)
                {
                    var (tThumb, _, tWidth) = allThumbnailsWithPositions[thumbIdx];
                    allThumbnailsWithPositions[thumbIdx] = (tThumb, newPosition, tWidth);
                }
            }

            VideoTrack.Invalidate();
        }

        ///
        /// Fix Bars positioning bug on right drag.
        ///
        private int remainingSegmentsCount;
        bool moving = false;
        private void UpdateBarsForRightDraggingFix()
        {
            moving = true;
            var draggedBarsIds = draggedBars.Select(t => t.id).ToList();
            var currentBounds = selectedAudioBounds;
            var nextBounds = allAudioSegments
                .Where(segment => segment.Right <= currentBounds.Left)
                .OrderByDescending(segment => segment.Right)
                .FirstOrDefault();

            var updatedBars = new List<(RectangleF bar, int id)>();

            remainingSegmentsCount = allAudioSegments
            .Count(segment => segment.Right <= currentBounds.Left)
            - segmentsToLeft;

            while (moving)
            {
                if (remainingSegmentsCount >= 1)
                {
                    ProcessingSegmentsAndBarsPositions(ref nextBounds, currentBounds, ref draggedBarsIds, ref updatedBars);
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
        /// Applying necessary calculations for proper positioning of amplitude bars
        ///
        private void ProcessingSegmentsAndBarsPositions(ref RectangleF nextBounds, RectangleF currentBounds, ref List<int> draggedBarsIds, ref List<(RectangleF _, int id)> updatedBars)
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
                var (_, id) = allAudioAmplitudeBars[i];
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
                int prevRemainingSegments = remainingSegmentsCount;
                remainingSegmentsCount = allAudioSegments.Count(segment => segment.Right <= nextSegmentToLeft.Left) - segmentsToLeft;

                if (remainingSegmentsCount >= prevRemainingSegments)
                {
                    moving = false;
                }

                draggedBarsIds = draggedBarsIds
                    .Union(updatedBars.Select(t => t.id))
                    .ToList();

                nextBounds = allAudioSegments
                    .Where(segment => segment.Right <= nextSegmentToLeft.Left + 1)
                    .OrderByDescending(segment => segment.Right)
                    .FirstOrDefault();
            }
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
                    allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, initialPosition, allThumbnailsWithPositions[thumbnailIndex].thumbnailWidth);
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
                        allThumbnailsWithPositions[index] = (thumbnail, newPosition, allThumbnailsWithPositions[index].thumbnailWidth);
                    }
                    int draggedIndex = draggedThumbnails.FindIndex(t => t.thumbnail == thumbnail);
                    if (draggedIndex != -1)
                    {
                        draggedThumbnails[draggedIndex] = (thumbnail, newPosition, draggedThumbnails[draggedIndex].width);
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

            foreach (var (thumbnail, originalPosition, _) in allThumbnailsWithPositions.ToList())
            {
                if (originalPosition >= segment.Left && originalPosition < segment.Right)
                {
                    float newThumbnailPosition = originalPosition + offset;

                    int thumbnailIndex = allThumbnailsWithPositions.FindIndex(t => t.thumbnail == thumbnail);
                    if (thumbnailIndex != -1)
                    {
                        allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, newThumbnailPosition, allThumbnailsWithPositions[thumbnailIndex].thumbnailWidth);
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

                    foreach (var (thumbnail, originalPosition, _) in allThumbnailsWithPositions.ToList())
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
                    allThumbnailsWithPositions[thumbnailIndex] = (thumbnail, newPosition, allThumbnailsWithPositions[thumbnailIndex].thumbnailWidth);
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

            var updatedBars = new List<(Image thumbnail, float position, float width)>();

            var nextBounds = allVideoBounds[nextBoundsIndex];

            var leftEdge = Math.Min(currentBounds.Left, nextBounds.Left);
            var rightEdge = Math.Max(currentBounds.Right, nextBounds.Right);

            foreach (var (thumbnail, position, width) in allThumbnailsWithPositions)
            {
                if (!draggedThumbnailsIds.Contains(thumbnail) && position >= leftEdge && position < rightEdge)
                {
                    updatedBars.Add((
                        thumbnail,
                        position + currentBounds.Width,
                        width
                    ));
                }
            }

            for (int i = 0; i < allThumbnailsWithPositions.Count; i++)
            {
                var (thumbnail, _, _) = allThumbnailsWithPositions[i];
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
            Console.WriteLine("Called fix NOW");
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
                var (_, id) = allAudioAmplitudeBars[i];
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
        [SuppressMessage("SonarQube", "S3011", Justification = "Setting DoubleBuffered for performance improvement, will not impact security whatsoever.")]
        private static void SetDoubleBuffered(Control control)
        {
            if (SystemInformation.TerminalServerSession)
                return;

            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { true });
        }

        ///
        /// Generation Test btn
        ///
        private void TestGen_Click(object sender, EventArgs e)
        {
            _ = GeneratePreview();
        }

        ///
        /// Data debug btn
        ///
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

            Console.WriteLine("\nVIDEO PREVIEW\n");
            foreach (VideoRenderSegment segment in fullVideoRenderPreview)
            {
                Console.WriteLine($"ID: {segment.Id} | Start: {segment.StartTime:F2} | End: {segment.EndTime:F2} | Path: {segment.FilePath} | TimeLinePosition: {segment.TimeLinePosition}");
            }

            Console.WriteLine("\nFFMPEG COMMANDS\n");
            Console.WriteLine("\nVIDEO CMD:\n" + lastVideoCommand);
            Console.WriteLine("\nAUDIO CMD:\n" + lastAudioCommand);
            Console.WriteLine("\nMERGE CMD:\n" + lastMergeCommand);

            Console.WriteLine($"Start 1st thumbnail position: {allThumbnailsWithPositions[0].position}");
            Console.WriteLine($"Width video track: {widthVideo}");
        }

        ///////
        /////// Track paint/re-paint/invalidation for debug purposes 
        ///////
        ////protected override void WndProc(ref Message m)
        ////{
        ////    if (m.Msg == 0x000F)
        ////    {
        ////        Console.WriteLine($"WM_PAINT at {DateTime.Now}");
        ////    }
        ////    base.WndProc(ref m);
        ////}

        ///
        /// Fix for reassign vlc lib directory from .resx file serialization/Create programatically instead.
        ///
        private void PreviewBox_Initialization()
        {
            string vlcPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "VlcLibs");
            PreviewBox = new Vlc.DotNet.Forms.VlcControl();
            ((System.ComponentModel.ISupportInitialize)(PreviewBox)).BeginInit();
            PreviewBox.BackColor = System.Drawing.Color.Black;
            PreviewBox.Dock = System.Windows.Forms.DockStyle.Fill;
            PreviewBox.Location = new System.Drawing.Point(0, 0);
            PreviewBox.Margin = new System.Windows.Forms.Padding(0);
            PreviewBox.Name = "PreviewBox";
            PreviewBox.Size = new System.Drawing.Size(472, 404);
            PreviewBox.Spu = -1;
            PreviewBox.TabIndex = 0;
            PreviewBox.Text = "vlcControl1";
            PreviewBox.VlcMediaplayerOptions = null;
            PreviewBox.VlcLibDirectory = new DirectoryInfo(vlcPath);
            ((System.ComponentModel.ISupportInitialize)(PreviewBox)).EndInit();
            this.PreviewPanel.Controls.Add(this.PreviewBox);
        }

        ///
        /// Draws a simple purple border for dropDownPanels
        ///
        private static void DrawBorderDropDownPanels(FlowLayoutPanel dropDownPanel)
        {
            dropDownPanel.Padding = new Padding(3, 3, 3, 3);
            dropDownPanel.Paint += (s, e) =>
            {
                Control ctrl = s as Control;
                using (Pen pen = new Pen(Color.Purple, 2))
                {
                    Rectangle rect = new Rectangle(1, 1, ctrl.Width - 3, ctrl.Height - 3);
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            dropDownPanel.VisibleChanged += (s, e) =>
            {
                if (dropDownPanel.Visible)
                {
                    dropDownPanel.Refresh();
                }
            };
        }

        ///
        /// Initialize a hidden pannel that would act as options for clicked File label on tool tip bar
        ///
        private void ToolTipDropdownPanel_File_Initialization()
        {
            dropDownPanel1 = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Width = 85,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(90, 25, 25, 40),
                Visible = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            option1Label = new System.Windows.Forms.Label
            {
                Text = "Import Media",
                AutoSize = false,
                Width = 85,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(200, 150, 255),
                Margin = new Padding(0),
                Enabled = true,
                Cursor = Cursors.Hand
            };
            option2Label = new System.Windows.Forms.Label
            {
                Text = "Save",
                AutoSize = false,
                Width = 85,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(200, 150, 255),
                Margin = new Padding(0),
                Cursor = Cursors.Hand
            };
            option3Label = new System.Windows.Forms.Label
            {
                Text = "Save as",
                AutoSize = false,
                Width = 85,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(200, 150, 255),
                Margin = new Padding(0),
                Cursor = Cursors.Hand
            };

            dropDownPanel1.Controls.Add(option1Label);
            dropDownPanel1.Controls.Add(option2Label);
            dropDownPanel1.Controls.Add(option3Label);
            this.Controls.Add(dropDownPanel1);
            option1Label.Click += option1Label_Click;
            option3Label.Click += SaveAsToolTipMenuItem_Click;
            option2Label.Click += SaveToolTipMenuItem_Click;
        }

        ///
        /// Initialize a hidden pannel that would act as options for clicked ControlPanel label on tool tip bar
        ///
        private void ToolTipDropdownPanel_ControlPanel_Initialization()
        {
            dropDownPanel2 = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Width = 85,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(90, 25, 25, 40),
                Visible = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            option4Label = new System.Windows.Forms.Label
            {
                Text = "Open Panel",
                AutoSize = false,
                Width = 80,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(200, 150, 255),
                Margin = new Padding(0),
                Cursor = Cursors.Hand
            };

            dropDownPanel2.Controls.Add(option4Label);
            this.Controls.Add(dropDownPanel2);
            option4Label.Click += option4Label_Click;
        }

        ///
        /// Initialize a hidden pannel that would act as options for clicked Help label on tool tip bar
        ///
        private void ToolTipDropdownPanel_Help_Initialization()
        {
            dropDownPanel3 = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Width = 86,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.FromArgb(90, 25, 25, 40),
                Visible = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            option5Label = new System.Windows.Forms.Label
            {
                Text = "Open Project",
                AutoSize = false,
                Width = 80,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(200, 150, 255),
                Margin = new Padding(0),
                Cursor = Cursors.Hand
            };
            dropDownPanel3.Controls.Add(option5Label);
            this.Controls.Add(dropDownPanel3);
            option5Label.Click += openToolTipMenuItem_Click;
        }

        //
        // Helper to crop an Image to a given rectangle
        //
        private static Image CropImage(Image source, Rectangle cropArea)
        {
            if (cropArea.Width <= 0 || cropArea.Height <= 0 ||
                cropArea.Right > source.Width || cropArea.Bottom > source.Height)
            {
                return source;
            }

            Bitmap cropped = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.DrawImage(
                    source,
                    new Rectangle(0, 0, cropArea.Width, cropArea.Height),
                    cropArea,
                    GraphicsUnit.Pixel
                );
            }
            return cropped;
        }

        ///
        /// Editor won't register good enough the child-parenting of panels therefore I do it programatically.
        ///
        private void PanelOrchestration(Panel panel)
        {
            List<Panel> panels = new List<Panel> { panel6, panel11 };

            foreach (Panel p in panels)
            {

                Point screenPos = p.PointToScreen(Point.Empty);

                Point newLocalPos = panel.PointToClient(screenPos);

                panel.Controls.Add(p);

                p.Location = newLocalPos;
            }
        }

        ///
        /// Show Editor Panel
        ///
        private void EditorPanelShow()
        {
            panel5.Show();
            panel5.BringToFront();
        }

        ///
        /// Control panels setup 
        ///
        private void closePanel_Click(object sender, EventArgs e)
        {
            controlPanel.Hide();
        }

        private void option4Label_Click(object sender, EventArgs e)
        {
            controlPanel.Show();
        }

        private void videoTrackMove_Click(object sender, EventArgs e)
        {
            if (selectedVideoBounds == RectangleF.Empty && selectedAudioBounds == RectangleF.Empty)
            {
                selectedVideoBounds = allVideoBounds[0];
                selectedAudioBounds = RectangleF.Empty;
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
                return;
            }

            moveSegments(true);
        }

        private void audioTrackMove_Click(object sender, EventArgs e)
        {
            if (selectedAudioBounds == RectangleF.Empty && selectedVideoBounds == RectangleF.Empty)
            {
                selectedAudioBounds = allAudioSegments[0];
                selectedVideoBounds = RectangleF.Empty;
                VideoTrack.Invalidate();
                AudioTrack.Invalidate();
                return;
            }

            moveSegments(false);
        }

        public void moveSegments(bool isVideo)
        {

            float visibleLeft = panel8.HorizontalScroll.Value;

            if (isVideo)
            {
                selectedAudioBounds = Rectangle.Empty;
                for (int i = allVideoBounds.Count - 1; i >= 0; i--)
                {
                    RectangleF bounds = allVideoBounds[i];

                    if (bounds.Left <= visibleLeft && bounds.Right >= visibleLeft)
                    {
                        selectedVideoBounds = bounds;
                        break;
                    }
                }
            }
            else
            {
                selectedVideoBounds = Rectangle.Empty;
                for (int i = allAudioSegments.Count - 1; i >= 0; i--)
                {
                    RectangleF bounds = allAudioSegments[i];

                    if (bounds.Left <= visibleLeft && bounds.Right >= visibleLeft)
                    {
                        selectedAudioBounds = bounds;
                        break;
                    }
                }
            }

            VideoTrack.Invalidate();
            AudioTrack.Invalidate();
        }

        public void moveLeftRight(bool isVideo, bool direction)
        {
            List<RectangleF> segments = isVideo ? allVideoBounds : allAudioSegments;
            RectangleF current = isVideo ? selectedVideoBounds : selectedAudioBounds;

            if (current == RectangleF.Empty)
                return;

            int currentIndex = segments.FindIndex(s => s.Equals(current));
            if (currentIndex == -1)
                return;

            int newIndex = direction ? currentIndex + 1 : currentIndex - 1;

            if (newIndex < 0 || newIndex >= segments.Count)
                return;

            if (isVideo)
            {
                selectedVideoBounds = segments[newIndex];
            }
            else
            {
                selectedAudioBounds = segments[newIndex];
            }

            VideoTrack.Invalidate();
            AudioTrack.Invalidate();
        }

        private void leftMove_Click(object sender, EventArgs e)
        {
            if ((selectedVideoBounds == RectangleF.Empty) && (selectedAudioBounds == RectangleF.Empty))
            {
                SelectFirstVisibleVideo();
                return;
            }

            if (selectedVideoBounds != RectangleF.Empty)
            {
                moveLeftRight(true, false);
            }
            else if (selectedAudioBounds != RectangleF.Empty)
            {
                moveLeftRight(false, false);
            }
        }

        private void rightMove_Click(object sender, EventArgs e)
        {
            if ((selectedVideoBounds == RectangleF.Empty) && (selectedAudioBounds == RectangleF.Empty))
            {
                SelectFirstVisibleVideo();
                return;
            }

            if (selectedVideoBounds != RectangleF.Empty)
            {
                moveLeftRight(true, true);
            }
            else if (selectedAudioBounds != RectangleF.Empty)
            {
                moveLeftRight(false, true);
            }
        }

        private void SelectFirstVisibleVideo()
        {
            float visibleLeft = panel8.HorizontalScroll.Value;

            for (int i = allVideoBounds.Count - 1; i >= 0; i--)
            {
                RectangleF bounds = allVideoBounds[i];
                if (bounds.Left <= visibleLeft && bounds.Right >= visibleLeft)
                {
                    selectedVideoBounds = bounds;
                    VideoTrack.Invalidate();
                    AudioTrack.Invalidate();
                    break;
                }
            }
        }

        private const float TrackerMoveStep = 2f;
        private readonly System.Windows.Forms.Timer trackerMoveTimer;
        private int trackerMoveDirection = 0;

        private void TrackerMoveTimer_Tick(object sender, EventArgs e)
        {
            float maxPosition = Math.Max(widthVideo, widthAudio);
            float proposedX = trackerXPosition + trackerMoveDirection * TrackerMoveStep;
            float clampedX = Math.Max(0f, Math.Min(proposedX, maxPosition));

            trackerXPosition = clampedX;
            currentPlaybackTime = trackerXPosition / pixelsPerMillisecond;

            long now = seekTimer.ElapsedMilliseconds;
            if (now - lastSeekUpdate >= seekUpdateThreshold)
            {
                lastSeekUpdate = now;

                if (!isUpdatingPreview)
                {
                    isUpdatingPreview = true;

                    try
                    {
                        PreviewBox.Time = (long)currentPlaybackTime;
                        PreviewBox.Refresh();
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

            UpdatePlaybackLabel(currentPlaybackTime);
            EnsureTrackerVisible(trackerXPosition);

            AudioTrack.Invalidate();
            VideoTrack.Invalidate();
            EditingRuller.Invalidate();
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            trackerMoveDirection = -1;
            trackerMoveTimer.Start();
        }

        private void button8_MouseUp(object sender, MouseEventArgs e)
        {
            trackerMoveDirection = 0;
            trackerMoveTimer.Stop();
        }

        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            trackerMoveDirection = 1;
            trackerMoveTimer.Start();
        }

        private decimal originalRatio = 1;
        private bool isUpdatingValue = false;

        private void widthNumericAdvanced_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingValue || !enableProportionalScalingCheckBox.Checked)
                return;

            if (originalRatio == 0)
                return;

            isUpdatingValue = true;
            decimal newHeight = widthNumericAdvanced.Value / originalRatio;

            if (newHeight >= heightNumericAdvanced.Minimum && newHeight <= heightNumericAdvanced.Maximum)
            {
                heightNumericAdvanced.Value = Math.Round(newHeight, 2);
            }

            isUpdatingValue = false;
        }

        private void heightNumericAdvanced_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdatingValue || !enableProportionalScalingCheckBox.Checked)
                return;

            isUpdatingValue = true;
            decimal newWidth = heightNumericAdvanced.Value * originalRatio;

            if (newWidth >= widthNumericAdvanced.Minimum && newWidth <= widthNumericAdvanced.Maximum)
            {
                widthNumericAdvanced.Value = Math.Round(newWidth, 2);
            }

            isUpdatingValue = false;
        }

        private bool IsAspectRatioSelected()
        {
            return aspectRatioComboBoxAdvanced.SelectedIndex != -1;
        }

        private void enableProportionalScalingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (enableProportionalScalingCheckBox.Checked)
            {
                if (IsAspectRatioSelected())
                {
                    SetOriginalRatioFromComboBox();

                    widthNumericAdvanced.Enabled = true;
                    heightNumericAdvanced.Enabled = true;

                    isUpdatingValue = true;

                    decimal newHeight = widthNumericAdvanced.Value / originalRatio;
                    if (newHeight >= heightNumericAdvanced.Minimum && newHeight <= heightNumericAdvanced.Maximum)
                    {
                        heightNumericAdvanced.Value = Math.Round(newHeight, 2);
                    }

                    isUpdatingValue = false;
                }
                else
                {
                    widthNumericAdvanced.Enabled = false;
                    heightNumericAdvanced.Enabled = false;
                }
            }
            else
            {
                widthNumericAdvanced.Enabled = true;
                heightNumericAdvanced.Enabled = true;
            }
        }


        private void aspectRatioComboBoxAdvanced_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsAspectRatioSelected())
            {
                SetOriginalRatioFromComboBox();

                if (enableProportionalScalingCheckBox.Checked)
                {
                    widthNumericAdvanced.Enabled = true;
                    heightNumericAdvanced.Enabled = true;
                }
            }
        }

        private const string hdVideo = "16:9 HD video, modern screens";

        private void SetOriginalRatioFromComboBox()
        {
            switch (aspectRatioComboBoxAdvanced.SelectedItem.ToString())
            {
                case "1:1   Square":
                    originalRatio = 1m;
                    break;
                case "4:3   Classic monitors, photos":
                    originalRatio = 4m / 3m;
                    break;
                case "3:2   DSLR camera photos":
                    originalRatio = 3m / 2m;
                    break;
                case hdVideo:
                    originalRatio = 16m / 9m;
                    break;
                case "21:9 Ultrawide monitors":
                    originalRatio = 21m / 9m;
                    break;
                case "9:16 Vertical video(mobile)":
                    originalRatio = 9m / 16m;
                    break;
                case "2:3   Portrait photos":
                    originalRatio = 2m / 3m;
                    break;
                default:
                    originalRatio = 0;
                    break;
            }
        }

        private List<Control> fastSettingsControls;
        private List<Control> advancedSettingsControls;
        private List<Control> savePanelBlockControls;
        private readonly string templateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates");
        private readonly string templateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "templates", "video_templates.json");

        private void InitializeSettingsGroups()
        {
            fastSettingsControls = new List<Control>
            {
                nameTextBox,
                exportPathTextBox,
                resolutionComboBox,
                bitrateComboBox,
                codecComboBox,
                formatComboBox,
                framerateComboBox,
                exportImg
            };

            advancedSettingsControls = new List<Control>
            {
                nameTextBoxAdvanced,
                exportPathTextBoxAdvanced,
                exportImgAdvanced,
                aspectRatioComboBoxAdvanced,
                widthNumericAdvanced,
                heightNumericAdvanced,
                enableProportionalScalingCheckBox,
                bitrateNumericAdvanced,
                framerateNumericAdvanced,
                qualityCrfNumericAdvanced,
                formatComboBoxAdvanced,
                codecComboBoxAdvanced,
                encodingPresetComboBoxAdvanced,
                pixelFormatComboBoxAdvanced,
                cpuUseComboBoxAdvanced
            };

            savePanelBlockControls = new List<Control>
            {
                saveTemplateBtn,
                renderBtn,
                button1,
                button2
            };
        }

        public class VideoTemplate
        {
            public string ResolutionText { get; set; }
            public int Bitrate { get; set; }
            public string CodecFast { get; set; }
            public string FormatFast { get; set; }
            public string FrameRateFast { get; set; }

            public decimal Width { get; set; }
            public decimal Height { get; set; }
            public decimal AspectRatio { get; set; }
            public string AspectRatioName { get; set; }

            public string FormatAdvanced { get; set; }
            public string CodecAdvanced { get; set; }
            public string EncodingPreset { get; set; }
            public string PixelFormat { get; set; }

            public decimal FrameRateAdvanced { get; set; }
            public decimal QualityCrf { get; set; }
        }

        private Dictionary<string, VideoTemplate> templates;

        private void LoadTemplates()
        {
            var defaultTemplates = GetDefaultTemplates();
            var savedTemplates = LoadTemplatesFromFile();
            templates = savedTemplates ?? new Dictionary<string, VideoTemplate>();
            bool modified = false;
            foreach (var kvp in defaultTemplates)
            {
                if (!templates.ContainsKey(kvp.Key))
                {
                    templates[kvp.Key] = kvp.Value;
                    modified = true;
                }
            }
            if (modified || savedTemplates == null)
            {
                SaveTemplatesToFile();
            }
            templateComboBox.Items.Clear();
            templateComboBox.Items.AddRange(templates.Keys.ToArray());
        }

        private Dictionary<string, VideoTemplate> LoadTemplatesFromFile()
        {
            if (!File.Exists(templateFilePath))
                return new Dictionary<string, VideoTemplate>();

            try
            {
                string json = File.ReadAllText(templateFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, VideoTemplate>>(json);
            }
            catch
            {
                File.Delete(templateFilePath);
                return new Dictionary<string, VideoTemplate>();
            }
        }

        private void SaveTemplatesToFile()
        {
            Directory.CreateDirectory(templateDir);
            var json = JsonConvert.SerializeObject(templates, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(templateFilePath, json);
        }

        private void saveTemplateBtn_Click(object sender, EventArgs e)
        {
            templateNameTextBox.Text = "";
            saveTemplatePanel.Visible = true;
            foreach (var ctrl in savePanelBlockControls)
                ctrl.Enabled = false;
            foreach (var ctrl in fastSettingsControls)
                ctrl.Enabled = false;
            foreach (var ctrl in advancedSettingsControls)
                templateNameTextBox.Focus();
        }

        private void confirmSaveBtn_Click(object sender, EventArgs e)
        {
            string name = templateNameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Template name cannot be empty.");
                return;
            }
            if (templates.ContainsKey(name))
            {
                var overwrite = MessageBox.Show("Template already exists. Overwrite?", "Confirm", MessageBoxButtons.YesNo);
                if (overwrite != DialogResult.Yes) return;
            }
            templates[name] = new VideoTemplate
            {
                ResolutionText = resolutionComboBox.SelectedItem?.ToString(),
                Bitrate = (int)bitrateNumericAdvanced.Value,
                CodecFast = codecComboBox.SelectedItem?.ToString(),
                FormatFast = formatComboBox.SelectedItem?.ToString(),
                FrameRateFast = framerateComboBox.SelectedItem?.ToString(),
                Width = widthNumericAdvanced.Value,
                Height = heightNumericAdvanced.Value,
                AspectRatio = originalRatio,
                AspectRatioName = aspectRatioComboBoxAdvanced.SelectedItem?.ToString(),
                FormatAdvanced = formatComboBoxAdvanced.SelectedItem?.ToString(),
                CodecAdvanced = codecComboBoxAdvanced.SelectedItem?.ToString(),
                EncodingPreset = encodingPresetComboBoxAdvanced.SelectedItem?.ToString(),
                PixelFormat = pixelFormatComboBoxAdvanced.SelectedItem?.ToString(),
                FrameRateAdvanced = framerateNumericAdvanced.Value,
                QualityCrf = qualityCrfNumericAdvanced.Value
            };
            SaveTemplatesToFile();
            if (!templateComboBox.Items.Contains(name))
            {
                templateComboBox.Items.Add(name);
            }
            templateComboBox.SelectedItem = name;
            foreach (var ctrl in savePanelBlockControls)
                ctrl.Enabled = true;
            if (enableAdvancedCheckBox.Checked)
            {
                foreach (var ctrl in advancedSettingsControls)
                    ctrl.Enabled = true;
            }
            else
            {
                foreach (var ctrl in fastSettingsControls)
                    ctrl.Enabled = true;
            }
            saveTemplatePanel.Visible = false;
        }

        private void cancelSaveBtn_Click(object sender, EventArgs e)
        {
            foreach (var ctrl in savePanelBlockControls)
                ctrl.Enabled = true;
            if (enableAdvancedCheckBox.Checked)
            {
                foreach (var ctrl in advancedSettingsControls)
                    ctrl.Enabled = true;
            }
            else
            {
                foreach (var ctrl in fastSettingsControls)
                    ctrl.Enabled = true;
            }
            saveTemplatePanel.Visible = false;
        }

        private static string GetBitrateName(int bitrate)
        {
            if (bitrate <= 300) return "(144p) 300";
            if (bitrate <= 700) return "(240p) 700";
            if (bitrate <= 1000) return "(360p) 1000";
            if (bitrate <= 2500) return "(480p) 2500";
            if (bitrate <= 5000) return "(720p) 5000";
            if (bitrate <= 8000) return "(1080p) 8000";
            if (bitrate <= 12000) return "(1440p-2K) 12000";
            return "(2160p-4K) 35000";
        }

        private void ApplyTemplate(string templateName)
        {
            if (!templates.TryGetValue(templateName, out var template))
                return;

            resolutionComboBox.SelectedItem = template.ResolutionText;
            bitrateComboBox.SelectedItem = GetBitrateName(template.Bitrate);
            codecComboBox.SelectedItem = template.CodecFast;
            formatComboBox.SelectedItem = template.FormatFast;
            framerateComboBox.SelectedItem = template.FrameRateFast;

            isUpdatingValue = true;
            widthNumericAdvanced.Value = template.Width;
            heightNumericAdvanced.Value = template.Height;
            isUpdatingValue = false;

            aspectRatioComboBoxAdvanced.SelectedItem = template.AspectRatioName;
            formatComboBoxAdvanced.SelectedItem = template.FormatAdvanced;
            codecComboBoxAdvanced.SelectedItem = template.CodecAdvanced;
            encodingPresetComboBoxAdvanced.SelectedItem = template.EncodingPreset;
            pixelFormatComboBoxAdvanced.SelectedItem = template.PixelFormat;
            framerateNumericAdvanced.Value = template.FrameRateAdvanced;
            qualityCrfNumericAdvanced.Value = template.QualityCrf;
            bitrateNumericAdvanced.Value = template.Bitrate;

            cpuUseComboBoxAdvanced.SelectedItem = "80%";

            if (!enableAdvancedCheckBox.Checked)
            {
                widthNumericAdvanced.Enabled = false;
                heightNumericAdvanced.Enabled = false;
            }
        }

        private void enableAdvancedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool advancedEnabled = enableAdvancedCheckBox.Checked;

            foreach (var ctrl in fastSettingsControls)
            {
                ctrl.Enabled = !advancedEnabled;
            }

            foreach (var ctrl in advancedSettingsControls)
            {
                ctrl.Enabled = advancedEnabled;
            }
        }

        private void templateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (templates == null || templateComboBox.SelectedItem == null)
                return;
            if (templateComboBox.SelectedItem is string templateName && templates.ContainsKey(templateName))
            {
                ApplyTemplate(templateName);
            }
        }

        private const string hdSval = "HD (720p)";
        private const string codecVal = "Medium - Quality";
        private const string fpsVal = "Medium";
        private const string encodingPresets = "H.264 (libx264)";
        private const string pixelFormVal = "YUV 4:2:0 (yuv420p)";
        private const string fHdVal = "Full HD (1080p)";
        private const string hQualVal = "High - Quality";
        private const string hqVal = "High Quality";
        private const string aspVal = "9:16 Vertical video (mobile)";
        private const string lowVal = "Low - Quality";
        private const string sdVal = "SD+ (480p)";
        private const string libxVal = "libx264";

        private VideoTemplate CreateVerticalTemplate(int width, int height, int bitrate, int frameRateAdv, int qualityCrf, string quality = lowVal)
        {
            string resolutionText;
            if (width == 480)
                resolutionText = sdVal;
            else if (width == 720)
                resolutionText = hdSval;
            else
                resolutionText = fHdVal;

            return new VideoTemplate
            {
                ResolutionText = resolutionText,
                Bitrate = bitrate,
                CodecFast = quality,
                FormatFast = hqVal,
                FrameRateFast = fpsVal,
                Width = width,
                Height = height,
                AspectRatio = 9m / 16m,
                AspectRatioName = aspVal,
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = frameRateAdv,
                QualityCrf = qualityCrf
            };
        }

        private static void AddTemplate(Dictionary<string, VideoTemplate> dict, VideoTemplate template, params string[] keys)
        {
            foreach (var key in keys)
            {
                dict[key] = template;
            }
        }

        private Dictionary<string, VideoTemplate> GetDefaultTemplates()
        {
            var lowQualityTempVert = CreateVerticalTemplate(480, 852, 2500, 30, 24, lowVal);
            var mediumQualityTempVert = CreateVerticalTemplate(720, 1280, 5000, 30, 23);
            var highQualityTempVert = CreateVerticalTemplate(1080, 1920, 8000, 60, 22, hQualVal);

            var defaultTemplates = new Dictionary<string, VideoTemplate>();

            AddTemplate(defaultTemplates, lowQualityTempVert, "YTShorts - Low(480x852)", "TikTok - Low(480x852)");
            AddTemplate(defaultTemplates, mediumQualityTempVert, "YTShorts - Medium(720x1280)", "TikTok - Low(720x1280)");
            AddTemplate(defaultTemplates, highQualityTempVert, "YTShorts - High(1080x1920)", "TikTok - High(1080x1920)");

            defaultTemplates["YouTube - Low(480p)"] = new VideoTemplate
            {
                ResolutionText = sdVal,
                Bitrate = 2500,
                CodecFast = lowVal,
                FormatFast = "Web",
                FrameRateFast = fpsVal,
                Width = 854,
                Height = 480,
                AspectRatio = 16m / 9m,
                AspectRatioName = hdVideo,
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = 30,
                QualityCrf = 24
            };

            defaultTemplates["YouTube - Medium(720p)"] = new VideoTemplate
            {
                ResolutionText = hdSval,
                Bitrate = 5000,
                CodecFast = codecVal,
                FormatFast = "Web",
                FrameRateFast = fpsVal,
                Width = 1280,
                Height = 720,
                AspectRatio = 16m / 9m,
                AspectRatioName = hdVideo,
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = 30,
                QualityCrf = 23
            };

            defaultTemplates["YouTube - High(1080p)"] = new VideoTemplate
            {
                ResolutionText = fHdVal,
                Bitrate = 8000,
                CodecFast = hQualVal,
                FormatFast = "Web",
                FrameRateFast = "High",
                Width = 1920,
                Height = 1080,
                AspectRatio = 16m / 9m,
                AspectRatioName = hdVideo,
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = 60,
                QualityCrf = 22
            };

            defaultTemplates["Instagram - Low(720x720)"] = new VideoTemplate
            {
                ResolutionText = hdSval,
                Bitrate = 5000,
                CodecFast = codecVal,
                FormatFast = hqVal,
                FrameRateFast = fpsVal,
                Width = 720,
                Height = 720,
                AspectRatio = 1m,
                AspectRatioName = "1:1   Square",
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = 30,
                QualityCrf = 23
            };

            defaultTemplates["Instagram - Medium(1080x1080)"] = new VideoTemplate
            {
                ResolutionText = fHdVal,
                Bitrate = 8000,
                CodecFast = hQualVal,
                FormatFast = hqVal,
                FrameRateFast = fpsVal,
                Width = 1080,
                Height = 1080,
                AspectRatio = 1m,
                AspectRatioName = "1:1   Square",
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = 30,
                QualityCrf = 22
            };

            defaultTemplates["Instagram - High(1080x1350)"] = new VideoTemplate
            {
                ResolutionText = fHdVal,
                Bitrate = 8000,
                CodecFast = hQualVal,
                FormatFast = hqVal,
                FrameRateFast = "High",
                Width = 1080,
                Height = 1350,
                AspectRatio = 4m / 5m,
                AspectRatioName = "4:3   Classic monitors, photos",
                FormatAdvanced = ".mp4",
                CodecAdvanced = encodingPresets,
                EncodingPreset = encodingPresets,
                PixelFormat = pixelFormVal,
                FrameRateAdvanced = 60,
                QualityCrf = 21
            };
            return defaultTemplates;
        }

        private void removeTemplateBtn_Click(object sender, EventArgs e)
        {
            var selectedItem = templateComboBox.SelectedItem;
            if (!(selectedItem is string selectedTemplate))
            {
                MessageBox.Show("Please select a template to remove.");
                return;
            }

            var defaultTemplates = GetDefaultTemplates();
            if (defaultTemplates.ContainsKey(selectedTemplate))
            {
                MessageBox.Show("Default templates cannot be removed.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to delete the custom template \"{selectedTemplate}\"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            if (templates.Remove(selectedTemplate))
            {
                SaveTemplatesToFile();
                templateComboBox.Items.Remove(selectedTemplate);
                templateComboBox.SelectedIndex = -1;
                MessageBox.Show("Template removed successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Template could not be found or removed.", error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void widthComboBoxPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Focus();
        }

        private void heightComboBoxPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Focus();
        }

        private void fpsComboBoxPreview_SelectedIndexChanged(object sender, EventArgs e)
        {
            button7.Focus();
        }

        private string GetNameFromAdvancedSettings()
        {
            return nameTextBoxAdvanced.Text;
        }

        private string GetExportPathFromAdvancedSettings()
        {
            return exportPathTextBoxAdvanced.Text;
        }

        private string GetExportPathFromFastSettings()
        {
            return exportPathTextBox.Text;
        }

        private int GetWidthFromAdvancedSettings()
        {
            return (int)widthNumericAdvanced.Value;
        }

        private int GetHeightFromAdvancedSettings()
        {
            return (int)heightNumericAdvanced.Value;
        }

        private int GetBitrateFromAdvancedSettings()
        {
            return (int)bitrateNumericAdvanced.Value;
        }

        private int GetFrameRateFromAdvancedSettings()
        {
            return (int)framerateNumericAdvanced.Value;
        }

        private int GetCrfQualityFromAdvancedSettings()
        {
            return (int)qualityCrfNumericAdvanced.Value;
        }

        private string GetFormatFromAdvancedSettings()
        {
            var value = formatComboBoxAdvanced.SelectedItem?.ToString();
            if (value == ".mp4") return "mp4";
            if (value == ".mkv") return "mkv";
            if (value == ".webm") return "webm";
            if (value == ".avi") return "avi";
            if (value == ".mov") return "mov";
            return "mp4";
        }

        private string GetFormatCmdFromAdvancedSettings()
        {
            var value = formatComboBoxAdvanced.SelectedItem?.ToString();
            if (value == ".mp4") return "mp4";
            if (value == ".mkv") return "matroska";
            if (value == ".webm") return "webm";
            if (value == ".avi") return "avi";
            if (value == ".mov") return "mov";
            return "mp4";
        }

        private string GetEncodingFromAdvancedSettings()
        {
            var value = codecComboBoxAdvanced.SelectedItem?.ToString();
            if (value == "H.264 (libx264)") return libxVal;
            if (value == "H.264 NVENC (h264_nvenc)") return "h264_nvenc";
            if (value == "H.264 Intel QSV (h264_qsv)") return "h264_qsv";
            if (value == "H.264 AMD (h264_amf)") return "h264_amf";
            if (value == "H.265 (libx265)") return "libx265";
            if (value == "H.265 NVENC (hevc_nvenc)") return "hevc_nvenc";
            if (value == "H.265 Intel QSV (hevc_qsv)") return "hevc_qsv";
            if (value == "H.265 AMD (hevc_amf)") return "hevc_amf";
            if (value == "MPEG-4 (mpeg4)") return "mpeg4";
            if (value == "VP8 (libvpx)") return "libvpx";
            if (value == "VP9 (libvpx-vp9)") return "libvpx-vp9";
            if (value == "AV1 (libaom-av1)") return "libaom-av1";
            if (value == "AV1 Intel QSV (av1_qsv)") return "av1_qsv";
            if (value == "Apple ProRes (prores)") return "prores";
            if (value == "Apple ProRes (prores_ks)") return "prores_ks";
            return "error";
        }


        private string GetPixelFormatFromAdvancedSettings()
        {
            var value = pixelFormatComboBoxAdvanced.SelectedItem?.ToString();
            if (value == "YUV 4:2:0 (yuv420p)") return "yuv420p";
            if (value == "YUV 4:2:0 10-bit (yuv420p10le)") return "yuv420p10le";
            if (value == "YUV 4:2:2 (yuv422p)") return "yuv422p";
            if (value == "YUV 4:4:4 (yuv444p)") return "yuv444p";
            if (value == "RGB (rgb24)") return "rgb24";
            if (value == "RGBA (rgba)") return "rgba";
            if (value == "Grayscale (gray)") return "gray";
            if (value == "YUVA 4:2:0 (yuva420p)") return "yuva420p";
            if (value == "NV12 (nv12)") return "nv12";
            return "yuv420p";
        }

        private int GetWidthFromFastSettings()
        {
            var value = resolutionComboBox.SelectedItem?.ToString();
            if (value == "HD (720p)") return 1280;
            if (value == sdVal) return 854;
            if (value == "Full HD (1080p)") return 1920;
            return 1280;
        }

        private int GetHeightFromFastSettings()
        {
            var value = resolutionComboBox.SelectedItem?.ToString();
            if (value == "HD (720p)") return 720;
            if (value == sdVal) return 480;
            if (value == "Full HD (1080p)") return 1080;
            return 720;
        }

        private int GetBitrateFromFastSettings()
        {
            var value = bitrateComboBox.SelectedItem?.ToString();
            if (value != null && value.Contains("2500")) return 2500;
            if (value != null && value.Contains("5000")) return 5000;
            if (value != null && value.Contains("8000")) return 8000;
            return 5000;
        }

        private int GetFpsFromFastSettings()
        {
            var value = framerateComboBox.SelectedItem?.ToString();
            if (value == "Low") return 24;
            if (value == "Medium") return 30;
            if (value == "High") return 60;
            return 30;
        }

        private string GetFormatCmdFromFastSettings()
        {
            var value = formatComboBox.SelectedItem?.ToString();
            if (value == "Web") return "mp4";
            if (value == "High Quality") return "matroska";
            if (value == "Editing Workflows") return "mov";
            return "mp4";
        }

        private string GetFormatFromFastSettings()
        {
            var value = formatComboBox.SelectedItem?.ToString();
            if (value == "Web") return "mp4";
            if (value == "High Quality") return "mkv";
            if (value == "Editing Workflows") return "mov";
            return "mp4";
        }

        private string GetCodecFromFastSettings()
        {
            var value = codecComboBox.SelectedItem?.ToString();
            if (value == lowVal) return libxVal;
            if (value == "Medium - Quality") return libxVal;
            if (value == "High - Quality") return libxVal;
            return libxVal;
        }

        private string GetNameFromFastSettings()
        {
            return nameTextBox.Text.ToString();
        }

        private async Task RenderOutputFastSettingsAsync()
        {
            string outputDirectory = GetExportPathFromFastSettings();
            string tempDirectory = Path.Combine(Path.GetTempPath(), "tempBuildPreview");
            Directory.CreateDirectory(outputDirectory);
            Directory.CreateDirectory(tempDirectory);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string tempVideoPath = Path.Combine(tempDirectory, $"temp_video_{timestamp}.mp4");
            string tempAudioPath = Path.Combine(tempDirectory, $"temp_audio_{timestamp}.mp4");
            string finalOutputPath = Path.Combine(outputDirectory, $"{GetNameFromFastSettings()}.{GetFormatFromFastSettings()}");

            double videoDuration = 0;
            StringBuilder videoCmd = new StringBuilder("-y ");
            List<string> videoFilters = new List<string>();
            int videoIndex = 0;

            foreach (var segment in fullVideoRender)
            {
                double segLen = segment.EndTime - segment.StartTime;
                if (segLen < 0) segLen = 0;
                videoDuration += segLen;
                videoCmd.AppendFormat("-i \"{0}\" -an ", segment.FilePath);
                videoFilters.Add(
                    $"[{videoIndex}:v]trim=start={segment.StartTime}:end={segment.EndTime}," +
                    $"setpts=PTS-STARTPTS,scale={GetWidthFromFastSettings()}:{GetHeightFromFastSettings()}," +
                    $"fps={GetFpsFromFastSettings()},setsar=1,format=yuv420p[v{videoIndex}];"
                );
                videoIndex++;
            }

            string videoInputs = string.Join("", Enumerable.Range(0, videoIndex).Select(i => $"[v{i}]"));
            videoFilters.Add($"{videoInputs}concat=n={videoIndex}:v=1[outv]");
            videoCmd.Append(
                $"-filter_complex \"{string.Join(" ", videoFilters)}\" " +
                $"-map \"[outv]\" -an " +
                $"-c:v {GetCodecFromFastSettings()} " +
                $"-crf 18 -preset slow -b:v {GetBitrateFromFastSettings()}k " +
                $"-f {GetFormatCmdFromFastSettings()} " +
                "-progress pipe:2 " +
                $"\"{tempVideoPath}\""
            );

            long videoEstMs = (long)(videoDuration * 1000);
            await RunFFmpegCommandWithProgress(videoCmd.ToString(), "Video Generation", renderProgressBar, videoEstMs);

            double audioDuration = 0;
            StringBuilder audioCmd = new StringBuilder("-y ");
            List<string> audioFilters = new List<string>();
            int audioIndex = 0;

            foreach (var segment in fullAudioRender)
            {
                double segLen = segment.EndTime - segment.StartTime;
                if (segLen < 0) segLen = 0;
                audioDuration += segLen;
                audioCmd.AppendFormat("-i \"{0}\" ", segment.FilePath);
                audioFilters.Add(
                    $"[{audioIndex}:a]atrim=start={segment.StartTime}:end={segment.EndTime},asetpts=PTS-STARTPTS[a{audioIndex}];"
                );
                audioIndex++;
            }

            string audioInputs = string.Join("", Enumerable.Range(0, audioIndex).Select(i => $"[a{i}]"));
            audioFilters.Add($"{audioInputs}concat=n={audioIndex}:v=0:a=1[outa]");
            audioCmd.Append(
                $"-filter_complex \"{string.Join(" ", audioFilters)}\" " +
                $"-map \"[outa]\" -vn -c:a aac -b:a 192k " +
                "-progress pipe:2 " +
                $"\"{tempAudioPath}\""
            );

            long audioEstMs = (long)(audioDuration * 1000);
            await RunFFmpegCommandWithProgress(audioCmd.ToString(), "Audio Generation", renderProgressBar, audioEstMs);

            StringBuilder mergeCmd = new StringBuilder("-y ");
            mergeCmd.AppendFormat("-i \"{0}\" -i \"{1}\" -c:v copy -c:a copy \"{2}\"", tempVideoPath, tempAudioPath, finalOutputPath);
            await RunFFmpegCommand(mergeCmd.ToString(), "Final Merging");

            TempFolderRemovalHandling(tempDirectory, tempVideoPath, tempAudioPath);
            MessageBox.Show("Preview render completed.");
        }

        private async Task RenderOutputAdvancedSettingsAsync()
        {
            string outputDirectory = GetExportPathFromAdvancedSettings();
            string tempDirectory = Path.Combine(Path.GetTempPath(), "tempBuildPreview");

            Directory.CreateDirectory(outputDirectory);
            Directory.CreateDirectory(tempDirectory);

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string tempVideoPath = Path.Combine(tempDirectory, $"temp_video_{timestamp}.mp4");
            string tempAudioPath = Path.Combine(tempDirectory, $"temp_audio_{timestamp}.mp4");
            string finalOutputPath = Path.Combine(outputDirectory, $"{GetNameFromAdvancedSettings()}.{GetFormatFromAdvancedSettings()}");

            StringBuilder videoCmd = new StringBuilder("-y ");
            List<string> videoFilters = new List<string>();
            int videoIndex = 0;
            double estimatedDuration = 0;

            foreach (var segment in fullVideoRender)
            {
                double segmentDuration = segment.EndTime - segment.StartTime;
                estimatedDuration += segmentDuration;
                videoCmd.AppendFormat("-ss {0} -t {1} -i \"{2}\" -an ",
                    segment.StartTime,
                    segmentDuration,
                    segment.FilePath);
                videoFilters.Add($"[{videoIndex}:v]setpts=PTS-STARTPTS,scale={GetWidthFromAdvancedSettings()}:{GetHeightFromAdvancedSettings()},fps={GetFrameRateFromAdvancedSettings()},setsar=1,format={GetPixelFormatFromAdvancedSettings()}[v{videoIndex}];");
                videoIndex++;
            }

            string videoInputs = string.Join("", Enumerable.Range(0, videoIndex).Select(i => $"[v{i}]"));
            videoFilters.Add($"{videoInputs}concat=n={videoIndex}:v=1[outv]");
            videoCmd.Append(
                $"-filter_complex \"{string.Join(" ", videoFilters)}\" " +
                $"-map \"[outv]\" -an " +
                $"-c:v {GetEncodingFromAdvancedSettings()} " +
                $"-crf {GetCrfQualityFromAdvancedSettings()} -preset slow " +
                $"-b:v {GetBitrateFromAdvancedSettings()}k " +
                $"-pix_fmt {GetPixelFormatFromAdvancedSettings()} " +
                $"-f {GetFormatCmdFromAdvancedSettings()} " +
                "-progress pipe:2 " +
                $"\"{tempVideoPath}\""
            );

            long estimatedDurationMs = (long)(estimatedDuration * 1000);
            await RunFFmpegCommandWithProgress(videoCmd.ToString(), "Video Generation", renderProgressBar, estimatedDurationMs);

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
            audioCmd.Append(
                $"-filter_complex \"{string.Join(" ", audioFilters)}\" " +
                $"-map \"[outa]\" -vn -c:a aac -b:a 192k \"{tempAudioPath}\""
            );
            await RunFFmpegCommand(audioCmd.ToString(), "Audio Generation");

            StringBuilder mergeCmd = new StringBuilder("-y ");
            mergeCmd.AppendFormat("-i \"{0}\" -i \"{1}\" -c:v copy -c:a copy \"{2}\"", tempVideoPath, tempAudioPath, finalOutputPath);
            await RunFFmpegCommand(mergeCmd.ToString(), "Final Merging");

            TempFolderRemovalHandling(tempDirectory, tempVideoPath, tempAudioPath);
            MessageBox.Show("Advanced render completed.");
        }


        private async Task RunFFmpegCommandWithProgress(string arguments, string taskName, ProgressBar progressBar, long estimatedDurationMs)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                    return;
                if (e.Data.StartsWith("out_time="))
                {
                    string timeStr = e.Data.Substring(9).Trim();
                    if (TimeSpan.TryParse(timeStr, CultureInfo.InvariantCulture, out TimeSpan ts))
                    {
                        long currentMs = (long)ts.TotalMilliseconds;
                        int progress = estimatedDurationMs > 0 ? (int)(currentMs * 100 / estimatedDurationMs) : 0;
                        progress = progressReturn(progress);
                        progressBar.Invoke(new Action(() => { progressBar.Value = progress; }));
                    }
                }
            };

            process.Start();
            process.BeginErrorReadLine();
            await Task.Run(() => process.WaitForExit());
            progressBar.Invoke(new Action(() => { progressBar.Value = 100; }));
            if (process.ExitCode != 0)
                MessageBox.Show($"{taskName} failed", "FFmpeg Error");
        }

        public static int progressReturn(int progress)
        {
            if (progress < 0)
                return 0;
            if (progress > 100)
                return 100;
            return progress;
        }

        private async void renderBtn_Click(object sender, EventArgs e)
        {
            string fileName;
            string exportPath;
            string extension;

            if (!enableAdvancedCheckBox.Checked)
            {
                fileName = nameTextBox.Text.Trim();
                exportPath = exportPathTextBox.Text.Trim();
                extension = GetFormatFromFastSettings().TrimStart('.');
            }
            else
            {
                fileName = nameTextBoxAdvanced.Text.Trim();
                exportPath = exportPathTextBoxAdvanced.Text.Trim();
                extension = GetFormatFromAdvancedSettings().TrimStart('.');
            }

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(exportPath))
            {
                MessageBox.Show("Export name or path is empty.", error);
                return;
            }

            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show("The file name contains invalid characters.", error);
                return;
            }

            string fullOutputPath = Path.Combine(exportPath, $"{fileName}.{extension}");
            if (File.Exists(fullOutputPath))
            {
                DialogResult overwrite = MessageBox.Show(
                    $"The file '{fileName}.{extension}' already exists.\nDo you want to overwrite it?",
                    "File Already Exists",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (overwrite != DialogResult.Yes) return;
            }

            renderBtn.Enabled = false;

            if (!enableAdvancedCheckBox.Checked)
            {
                await RenderOutputFastSettingsAsync();
            }
            else
            {
                await RenderOutputAdvancedSettingsAsync();
            }

            renderProgressBar.Value = 0;
            renderBtn.Enabled = true;
        }

        private void exportImg_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select export directory";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string outputDirectory = dialog.SelectedPath;

                    string baseName = nameTextBox.Text.Trim();
                    string extension = GetFormatFromFastSettings().TrimStart('.');

                    if (string.IsNullOrWhiteSpace(baseName) || string.IsNullOrWhiteSpace(extension))
                    {
                        MessageBox.Show("Invalid name or format.");
                        return;
                    }

                    string tempName = baseName;
                    string fullPath = Path.Combine(outputDirectory, $"{tempName}.{extension}");
                    int counter = 1;

                    while (File.Exists(fullPath))
                    {
                        tempName = $"{baseName}_{counter}";
                        fullPath = Path.Combine(outputDirectory, $"{tempName}.{extension}");
                        counter++;
                    }

                    nameTextBox.Text = tempName;
                    nameTextBoxAdvanced.Text = tempName;
                    exportPathTextBox.Text = outputDirectory;
                    exportPathTextBoxAdvanced.Text = outputDirectory;
                }
            }
        }

        private void exportImgAdvanced_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select export directory";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string outputDirectory = dialog.SelectedPath;

                    string baseName = nameTextBoxAdvanced.Text.Trim();

                    string extension = GetFormatFromAdvancedSettings().TrimStart('.');

                    if (string.IsNullOrWhiteSpace(baseName) || string.IsNullOrWhiteSpace(extension))
                    {
                        MessageBox.Show("Invalid name or format.");
                        return;
                    }

                    string tempName = baseName;
                    string fullPath = Path.Combine(outputDirectory, $"{tempName}.{extension}");
                    int counter = 1;

                    while (File.Exists(fullPath))
                    {
                        tempName = $"{baseName}_{counter}";
                        fullPath = Path.Combine(outputDirectory, $"{tempName}.{extension}");
                        counter++;
                    }

                    nameTextBox.Text = tempName;
                    nameTextBoxAdvanced.Text = tempName;
                    exportPathTextBox.Text = outputDirectory;
                    exportPathTextBoxAdvanced.Text = outputDirectory;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Debug_Click(sender, e);
        }

        private async Task SafeGeneratePreview()
        {
            try
            {
                await GeneratePreview();
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    MessageBox.Show($"Preview generation failed:\n{ex.Message}", error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }
        private void DisableUI()
        {
            resyncBtn.Enabled = false;
            fastPreviewCheckBox.Enabled = false;
        }

        private void EnableUI()
        {
            resyncBtn.Enabled = true;
            fastPreviewCheckBox.Enabled = true;
        }

        private void resyncBtn_Click(object sender, EventArgs e)
        {
            _ = RunSafeGeneratePreviewAsync();
        }

        private void fastPreviewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _ = RunSafeGeneratePreviewAsync();
        }

        private async Task RunSafeGeneratePreviewAsync()
        {
            try
            {
                DisableUI();

                bool wasPlaying = false;

                try
                {
                    if (PreviewBox != null && PreviewBox.Length > 0 && PreviewBox.IsPlaying)
                    {
                        wasPlaying = true;
                        PreviewBox.Pause();
                        playbackTimer.Stop();
                        playbackStopwatch.Stop();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

                await SafeGeneratePreview();

                try
                {
                    if (wasPlaying && PreviewBox != null && PreviewBox.Length > 0)
                    {
                        PreviewBox.Play();
                        playbackStopwatch.Restart();
                        playbackTimer.Start();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                EnableUI();
            }
        }

        public class SerializableRectangleF
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public SerializableRectangleF() { }
            public SerializableRectangleF(RectangleF rect)
            {
                X = rect.X; Y = rect.Y; Width = rect.Width; Height = rect.Height;
            }
        }

        public class SerializableBar
        {
            public SerializableRectangleF Bar { get; set; }
            public int Id { get; set; }
        }

        public class SerializableThumbnail
        {
            public string Base64Image { get; set; }
            public float Position { get; set; }
            public float ThumbnailWidth { get; set; }
        }

        public class ProjectData
        {
            public List<VideoRenderSegment> VideoSegments { get; set; }
            public List<AudioRenderSegment> AudioSegments { get; set; }
            public List<SerializableRectangleF> VideoBounds { get; set; }
            public List<SerializableRectangleF> AudioBounds { get; set; }
            public List<SerializableBar> AudioAmplitudeBars { get; set; }
            public List<SerializableThumbnail> Thumbnails { get; set; }
            public int AmpID { get; set; }
            public List<string> ImportedMediaPaths { get; set; }
        }

        private string currentProjectFilePath = "";
        private int projectIncrement = 1;

        private string GetDefaultProjectFilePath()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName;
            do
            {
                fileName = $"project{projectIncrement}.rnc";
                projectIncrement++;
            }
            while (File.Exists(Path.Combine(folder, fileName)));
            return Path.Combine(folder, fileName);
        }

        private static string ImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private ProjectData GatherProjectData()
        {
            ProjectData data = new ProjectData();
            data.VideoSegments = fullVideoRender;
            data.AudioSegments = fullAudioRender;
            data.VideoBounds = allVideoBounds.Select(r => new SerializableRectangleF(r)).ToList();
            data.AudioBounds = allAudioSegments.Select(r => new SerializableRectangleF(r)).ToList();
            data.AudioAmplitudeBars = allAudioAmplitudeBars.Select(b => new SerializableBar { Bar = new SerializableRectangleF(b.bar), Id = b.id }).ToList();
            data.Thumbnails = allThumbnailsWithPositions.Select(t => new SerializableThumbnail
            {
                Base64Image = ImageToBase64(t.thumbnail),
                Position = t.position,
                ThumbnailWidth = t.thumbnailWidth
            }).ToList();
            data.AmpID = ampID;
            data.ImportedMediaPaths = importedMediaPanel.Controls.OfType<Panel>().Select(p => p.Tag as string).ToList();
            return data;
        }

        private void SaveProjectToFile(string filePath)
        {
            ProjectData data = GatherProjectData();
            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void SaveProjectAs()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Project Files (*.rnc)|*.rnc";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.FileName = Path.GetFileName(GetDefaultProjectFilePath());
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    currentProjectFilePath = sfd.FileName;
                    SaveProjectToFile(currentProjectFilePath);
                }
            }
        }

        public void OpenProject()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Project Files (*.rnc)|*.rnc";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentProjectFilePath = ofd.FileName;
                    string json = File.ReadAllText(currentProjectFilePath);
                    ProjectData data = JsonConvert.DeserializeObject<ProjectData>(json);
                    _ = LoadImportedMediaVisuals(data);
                    ApplyProjectData(data);
                }
            }
        }

        private void ApplyProjectData(ProjectData data)
        {
            fullVideoRender = data.VideoSegments;
            fullAudioRender = data.AudioSegments;
            allVideoBounds = data.VideoBounds.Select(r => new RectangleF(r.X, r.Y, r.Width, r.Height)).ToList();
            allAudioSegments = data.AudioBounds.Select(r => new RectangleF(r.X, r.Y, r.Width, r.Height)).ToList();
            allAudioAmplitudeBars = data.AudioAmplitudeBars.Select(b => (new RectangleF(b.Bar.X, b.Bar.Y, b.Bar.Width, b.Bar.Height), b.Id)).ToList();
            allThumbnailsWithPositions = data.Thumbnails.Select(t => (Base64ToImage(t.Base64Image), t.Position, t.ThumbnailWidth)).ToList();
            ampID = data.AmpID;
        }

        private static Image Base64ToImage(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return new Bitmap(Image.FromStream(ms));
            }
        }

        private Task ImportVisual()
        {
            button4.Enabled = false;
            float videoDuration = 0;

            foreach (VideoRenderSegment segment in fullVideoRender){
                videoDuration += segment.EndTime - segment.StartTime;
            }

            float newWidth = videoDuration * pixelsPerSecond;
            float widthEditing;

            if ((widthVideo + newWidth) / pixelsPerSecond > 600)
            {
                MessageBox.Show("Adding this video will exceed the 10-minute limit. Please adjust your timeline.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = true;
                return Task.CompletedTask;
            }

            Invoke(new Action(() =>
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

            Invoke(new Action(() =>
            {

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
            NormalizeSegmentPositions();
            NormalizeAudioSegmentPositions();
            SyncMediaTracks();
            SyncFullAudioRender();
            SyncFullVideoRender();
            _ = GeneratePreview();
            return Task.CompletedTask;
        }

        private void openToolTipMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();

            importedNewVideo = true;
            fullVideoRenderPreview.Clear();

            widthVideo = 0f;
            widthAudio = 0f;
            VideoTrack.Width = VideoTrackPlaceholder.Width;
            AudioTrack.Width = AudioTrackPlaceholder.Width;
            EditingRuller.Width = Math.Max(VideoTrack.Width, AudioTrack.Width);

            _ = ImportVisual();
        }

        public void SaveProject()
        {
            if (string.IsNullOrEmpty(currentProjectFilePath))
                SaveProjectAs();
            else
                SaveProjectToFile(currentProjectFilePath);
        }

        private async Task LoadImportedMediaVisuals(ProjectData data)
        {
            button1.Enabled = true;
            importedMediaPanel.Controls.Clear();
            if (data.ImportedMediaPaths == null) return;

            foreach (var filePath in data.ImportedMediaPaths.Where(File.Exists))
                await AddVideoToPanel(filePath);
        }

        private void SaveToolTipMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void SaveAsToolTipMenuItem_Click(object sender, EventArgs e)
        {
            SaveProjectAs();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Debug_Click(sender, e);
        }

        private void hideCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool hideVideo = hideCheckBox.Checked;

            VideoTrack.Enabled = !hideVideo;

            PreviewBox.Visible = !hideVideo;
        }

        private void muteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool muteAudio = muteCheckBox.Checked;

            AudioTrack.Enabled = !muteAudio;

            PreviewBox.Audio.IsMute = muteAudio;

            PreviewBox.Visible = true;
        }

        private Panel selectedMediaItem;
        private readonly Color mediaItemDefaultColor = Color.FromArgb(67, 38, 88);
        private readonly Color mediaItemHighlightColor = Color.FromArgb(100, 70, 130);

        private void ImportedMediaPanelItem_Click(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;

            var label = panel.Controls
                             .OfType<Label>()
                             .FirstOrDefault();
            if (label == null) return;

            if (selectedMediaItem != null && selectedMediaItem != panel)
            {
                var oldLabel = selectedMediaItem.Controls
                                                .OfType<Label>()
                                                .FirstOrDefault();
                if (oldLabel != null)
                    oldLabel.BackColor = mediaItemDefaultColor;
            }

            selectedMediaItem = panel;
            label.BackColor = mediaItemHighlightColor;
        }

        private bool isDraggingMediaItem = false;
        private Panel draggingPanel = null;
        private Control dragOriginalParent = null;
        private int dragOriginalRow, dragOriginalCol;
        private Point dragMouseOffset;

        private void ImportedMediaPanelItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Control ctrl = sender as Control;
            Panel panel = ctrl as Panel ?? ctrl.Parent as Panel;
            if (panel == null) return;

            ImportedMediaPanelItem_Click(panel, EventArgs.Empty);

            isDraggingMediaItem = true;
            draggingPanel = panel;
            dragMouseOffset = e.Location;

            dragOriginalParent = panel.Parent;
            if (dragOriginalParent is TableLayoutPanel tlp)
            {
                var pos = tlp.GetPositionFromControl(panel);
                dragOriginalRow = pos.Row;
                dragOriginalCol = pos.Column;
                tlp.Controls.Remove(panel);
            }

            this.Controls.Add(panel);
            panel.BringToFront();
            panel.Dock = DockStyle.None;
            panel.Margin = new Padding(0);

            var formMouse = this.PointToClient(Cursor.Position);
            panel.Location = new Point(
                formMouse.X - dragMouseOffset.X,
                formMouse.Y - dragMouseOffset.Y
            );
        }

        private void ImportedMediaPanelItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingMediaItem || draggingPanel == null) return;

            var formMouse = this.PointToClient(Cursor.Position);
            draggingPanel.Location = new Point(
                formMouse.X - dragMouseOffset.X,
                formMouse.Y - dragMouseOffset.Y
            );
        }

        private void ImportedMediaPanelItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDraggingMediaItem || draggingPanel == null) return;
            var panel = draggingPanel;
            isDraggingMediaItem = false;
            draggingPanel = null;

            var panelRect = new Rectangle(
                panel.PointToScreen(Point.Empty),
                panel.Size
            );
            bool overVideo = panelRect.IntersectsWith(
                VideoTrack.RectangleToScreen(VideoTrack.ClientRectangle)
            );
            bool overAudio = panelRect.IntersectsWith(
                AudioTrack.RectangleToScreen(AudioTrack.ClientRectangle)
            );

            if ((overVideo || overAudio) && panel.Tag is string filePath)
            {
                _ = AddVideoToTimeline(filePath);
            }

            this.Controls.Remove(panel);
            if (dragOriginalParent is TableLayoutPanel tlp)
            {
                tlp.Controls.Add(panel, dragOriginalCol, dragOriginalRow);
            }
            else
            {
                dragOriginalParent.Controls.Add(panel);
            }
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(4);
        }


        private void Media_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Media_DragDrop(object sender, DragEventArgs e)
        {
            var filePath = e.Data.GetData(DataFormats.StringFormat) as string;
            if (!string.IsNullOrEmpty(filePath))
                _ = AddVideoToTimeline(filePath);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            Debug_Click(sender, e);
        }

        ///
        /// Only import necessary metadata of the media file 
        ///
        private async Task AddVideoToPanel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not found.");
                return;
            }

            foreach (Control control in importedMediaPanel.Controls)
            {
                if (control is Panel panel && panel.Tag is string existingPath &&
                    string.Equals(existingPath, filePath, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            Image thumbnail = await Task.Run(() => ExtractSingleThumbnail(filePath));
            if (thumbnail == null)
            {
                MessageBox.Show("Failed to generate thumbnail.");
                return;
            }

            for (int row = 0; row < importedMediaPanel.RowCount; row++)
            {
                for (int col = 0; col < importedMediaPanel.ColumnCount; col++)
                {
                    if (importedMediaPanel.GetControlFromPosition(col, row) == null)
                    {
                        var panel = new Panel
                        {
                            Dock = DockStyle.Fill,
                            Margin = new Padding(4),
                            Padding = new Padding(3),
                            BackColor = Color.FromArgb(67, 38, 88),
                            Tag = filePath
                        };

                        var pictureBox = new PictureBox
                        {
                            Image = thumbnail,
                            Dock = DockStyle.Fill,
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };

                        var label = new Label
                        {
                            Text = Path.GetFileName(filePath),
                            Dock = DockStyle.Bottom,
                            Height = 20,
                            TextAlign = ContentAlignment.MiddleCenter,
                            ForeColor = Color.White,
                            Font = new Font("Segoe UI", 7, FontStyle.Regular),
                            AutoEllipsis = true,
                            BackColor = Color.FromArgb(67, 38, 88)
                        };

                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(label);

                        importedMediaPanel.Controls.Add(panel, col, row);
                        importedMediaPanel.PerformLayout();
                        importedMediaPanel.Refresh();

                        panel.Click += ImportedMediaPanelItem_Click;
                        pictureBox.Click += (s, e) => ImportedMediaPanelItem_Click(panel, EventArgs.Empty);
                        label.Click += (s, e) => ImportedMediaPanelItem_Click(panel, EventArgs.Empty);

                        panel.MouseDown += ImportedMediaPanelItem_MouseDown;
                        panel.MouseMove += ImportedMediaPanelItem_MouseMove;
                        panel.MouseUp += ImportedMediaPanelItem_MouseUp;

                        pictureBox.MouseDown += ImportedMediaPanelItem_MouseDown;
                        pictureBox.MouseMove += ImportedMediaPanelItem_MouseMove;
                        pictureBox.MouseUp += ImportedMediaPanelItem_MouseUp;

                        label.MouseDown += ImportedMediaPanelItem_MouseDown;
                        label.MouseMove += ImportedMediaPanelItem_MouseMove;
                        label.MouseUp += ImportedMediaPanelItem_MouseUp;

                        return;
                    }
                }
            }

            MessageBox.Show("No available space in the 3x4 panel.");
        }
    }
}