using System;
using System.IO;

namespace RenCloud
{
    partial class UserInterfaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterfaceForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.TimeStamp = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.controlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.RemoveSegment = new System.Windows.Forms.Button();
            this.Split = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.closePanel = new System.Windows.Forms.Button();
            this.videoTrackMove = new System.Windows.Forms.Button();
            this.leftMove = new System.Windows.Forms.Button();
            this.audioTrackMove = new System.Windows.Forms.Button();
            this.rightMove = new System.Windows.Forms.Button();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.PauseButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.PreviewPanel = new System.Windows.Forms.Panel();
            this.fpsComboBoxPreview = new System.Windows.Forms.ComboBox();
            this.heightComboBoxPreview = new System.Windows.Forms.ComboBox();
            this.widthComboBoxPreview = new System.Windows.Forms.ComboBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.resyncBtn = new System.Windows.Forms.Button();
            this.fastPreviewCheckBox = new System.Windows.Forms.CheckBox();
            this.help_label = new System.Windows.Forms.Label();
            this.controlPanel_label = new System.Windows.Forms.Label();
            this.file_label = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.AudioTrack = new System.Windows.Forms.Panel();
            this.AudioTrackPlaceholder = new System.Windows.Forms.Panel();
            this.EditingRuller = new System.Windows.Forms.Panel();
            this.VideoTrack = new System.Windows.Forms.Panel();
            this.VideoTrackPlaceholder = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.muteCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.hideCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.importedMediaPanel = new System.Windows.Forms.TableLayoutPanel();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.saveTemplatePanel = new System.Windows.Forms.TableLayoutPanel();
            this.label31 = new System.Windows.Forms.Label();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.cancelSaveBtn = new System.Windows.Forms.Button();
            this.confirmSaveBtn = new System.Windows.Forms.Button();
            this.templateNameTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.removeTemplateBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.exportPathTextBox = new System.Windows.Forms.TextBox();
            this.exportImg = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.bitrateComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.codecComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.formatComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.framerateComboBox = new System.Windows.Forms.ComboBox();
            this.templateComboBox = new System.Windows.Forms.ComboBox();
            this.resolutionComboBox = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.saveTemplateBtn = new System.Windows.Forms.Button();
            this.renderBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.enableAdvancedCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.aspectRatioComboBoxAdvanced = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.pixelFormatComboBoxAdvanced = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.encodingPresetComboBoxAdvanced = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.framerateNumericAdvanced = new System.Windows.Forms.NumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.cpuUseComboBoxAdvanced = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.qualityCrfNumericAdvanced = new System.Windows.Forms.NumericUpDown();
            this.label25 = new System.Windows.Forms.Label();
            this.formatComboBoxAdvanced = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.bitrateNumericAdvanced = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.label19 = new System.Windows.Forms.Label();
            this.enableProportionalScalingCheckBox = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.widthNumericAdvanced = new System.Windows.Forms.NumericUpDown();
            this.heightNumericAdvanced = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.exportPathTextBoxAdvanced = new System.Windows.Forms.TextBox();
            this.exportImgAdvanced = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.nameTextBoxAdvanced = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.codecComboBoxAdvanced = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.renderProgressBar = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel13.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.PreviewPanel.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.AudioTrack.SuspendLayout();
            this.VideoTrack.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.tableLayoutPanel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.panel11.SuspendLayout();
            this.RenderPanel.SuspendLayout();
            this.saveTemplatePanel.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportImg)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framerateNumericAdvanced)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityCrfNumericAdvanced)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bitrateNumericAdvanced)).BeginInit();
            this.tableLayoutPanel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericAdvanced)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericAdvanced)).BeginInit();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportImgAdvanced)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Navy;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 734);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(207, 58);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(897, 676);
            this.panel4.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MediumBlue;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Image = global::RenCloud.Properties.Resources.DashboardV2;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(1, 104);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(205, 55);
            this.button2.TabIndex = 3;
            this.button2.Text = "Editor";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Enabled = false;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::RenCloud.Properties.Resources.Box2;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(1, 58);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(205, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Render";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Location = new System.Drawing.Point(1, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(206, 58);
            this.panel3.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RenCloud.Properties.Resources.NoBG__2_;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Purple;
            this.panel2.Controls.Add(this.button3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(127)))));
            this.panel2.Location = new System.Drawing.Point(207, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(897, 58);
            this.panel2.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::RenCloud.Properties.Resources.Close;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button3.Dock = System.Windows.Forms.DockStyle.Right;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(848, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(49, 58);
            this.button3.TabIndex = 13;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label22);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Controls.Add(this.button6);
            this.panel5.Controls.Add(this.button7);
            this.panel5.Controls.Add(this.TimeStamp);
            this.panel5.Controls.Add(this.panel13);
            this.panel5.Controls.Add(this.PreviewPanel);
            this.panel5.Controls.Add(this.panel10);
            this.panel5.Location = new System.Drawing.Point(209, 58);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(895, 676);
            this.panel5.TabIndex = 2;
            this.panel5.Visible = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(658, 449);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(191, 23);
            this.label22.TabIndex = 7;
            this.label22.Text = "Add media to timeline";
            // 
            // button4
            // 
            this.button4.FlatAppearance.BorderSize = 3;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Image = global::RenCloud.Properties.Resources.Inputs;
            this.button4.Location = new System.Drawing.Point(792, 438);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 38);
            this.button4.TabIndex = 0;
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Image = global::RenCloud.Properties.Resources.Paus22e;
            this.button6.Location = new System.Drawing.Point(557, 438);
            this.button6.Margin = new System.Windows.Forms.Padding(0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(74, 38);
            this.button6.TabIndex = 6;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Image = global::RenCloud.Properties.Resources.Play;
            this.button7.Location = new System.Drawing.Point(482, 438);
            this.button7.Margin = new System.Windows.Forms.Padding(0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(74, 38);
            this.button7.TabIndex = 5;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // TimeStamp
            // 
            this.TimeStamp.ForeColor = System.Drawing.Color.White;
            this.TimeStamp.Location = new System.Drawing.Point(419, 447);
            this.TimeStamp.Margin = new System.Windows.Forms.Padding(0);
            this.TimeStamp.Name = "TimeStamp";
            this.TimeStamp.Size = new System.Drawing.Size(78, 22);
            this.TimeStamp.TabIndex = 1;
            this.TimeStamp.Text = "00:00:00";
            // 
            // panel13
            // 
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.controlPanel);
            this.panel13.Location = new System.Drawing.Point(0, 305);
            this.panel13.Margin = new System.Windows.Forms.Padding(0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(419, 172);
            this.panel13.TabIndex = 2;
            // 
            // controlPanel
            // 
            this.controlPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.controlPanel.ColumnCount = 2;
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.1295F));
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.8705F));
            this.controlPanel.Controls.Add(this.tableLayoutPanel12, 0, 0);
            this.controlPanel.Controls.Add(this.tableLayoutPanel11, 1, 0);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.RowCount = 1;
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.controlPanel.Size = new System.Drawing.Size(417, 170);
            this.controlPanel.TabIndex = 3;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.RemoveSegment, 1, 1);
            this.tableLayoutPanel12.Controls.Add(this.Split, 0, 1);
            this.tableLayoutPanel12.Controls.Add(this.label21, 1, 0);
            this.tableLayoutPanel12.Controls.Add(this.label20, 0, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.70588F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65.29412F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(157, 168);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // RemoveSegment
            // 
            this.RemoveSegment.BackgroundImage = global::RenCloud.Properties.Resources.Cancel1;
            this.RemoveSegment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RemoveSegment.Dock = System.Windows.Forms.DockStyle.Top;
            this.RemoveSegment.FlatAppearance.BorderSize = 0;
            this.RemoveSegment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RemoveSegment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.RemoveSegment.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RemoveSegment.Location = new System.Drawing.Point(81, 61);
            this.RemoveSegment.Name = "RemoveSegment";
            this.RemoveSegment.Size = new System.Drawing.Size(73, 52);
            this.RemoveSegment.TabIndex = 18;
            this.RemoveSegment.UseVisualStyleBackColor = true;
            this.RemoveSegment.Click += new System.EventHandler(this.RemoveSegment_Click);
            // 
            // Split
            // 
            this.Split.BackgroundImage = global::RenCloud.Properties.Resources.Invert_Selection;
            this.Split.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Split.Dock = System.Windows.Forms.DockStyle.Top;
            this.Split.FlatAppearance.BorderSize = 0;
            this.Split.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Split.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Split.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Split.Location = new System.Drawing.Point(3, 61);
            this.Split.Name = "Split";
            this.Split.Size = new System.Drawing.Size(72, 52);
            this.Split.TabIndex = 17;
            this.Split.UseVisualStyleBackColor = true;
            this.Split.Click += new System.EventHandler(this.Split_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label21.Font = new System.Drawing.Font("Microsoft JhengHei Light", 11.25F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(81, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(73, 58);
            this.label21.TabIndex = 3;
            this.label21.Text = "Delete";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label20.Font = new System.Drawing.Font("Microsoft JhengHei Light", 11.25F);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(3, 29);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 29);
            this.label20.TabIndex = 2;
            this.label20.Text = "Split";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel11.ColumnCount = 3;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel11.Controls.Add(this.button9, 2, 2);
            this.tableLayoutPanel11.Controls.Add(this.button8, 0, 2);
            this.tableLayoutPanel11.Controls.Add(this.closePanel, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.videoTrackMove, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.leftMove, 0, 1);
            this.tableLayoutPanel11.Controls.Add(this.audioTrackMove, 1, 2);
            this.tableLayoutPanel11.Controls.Add(this.rightMove, 2, 1);
            this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel13, 1, 1);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(159, 1);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 3;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel11.Size = new System.Drawing.Size(257, 168);
            this.tableLayoutPanel11.TabIndex = 1;
            // 
            // button9
            // 
            this.button9.BackgroundImage = global::RenCloud.Properties.Resources.Double_Right;
            this.button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button9.Location = new System.Drawing.Point(161, 113);
            this.button9.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(95, 54);
            this.button9.TabIndex = 4;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.BackgroundImage = global::RenCloud.Properties.Resources.Double_Left;
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button8.Location = new System.Drawing.Point(2, 113);
            this.button8.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(84, 54);
            this.button8.TabIndex = 5;
            this.button8.UseVisualStyleBackColor = true;
            // 
            // closePanel
            // 
            this.closePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closePanel.BackgroundImage = global::RenCloud.Properties.Resources.Close;
            this.closePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.closePanel.FlatAppearance.BorderSize = 0;
            this.closePanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closePanel.Location = new System.Drawing.Point(220, 4);
            this.closePanel.Name = "closePanel";
            this.closePanel.Size = new System.Drawing.Size(33, 29);
            this.closePanel.TabIndex = 14;
            this.closePanel.UseVisualStyleBackColor = true;
            this.closePanel.Click += new System.EventHandler(this.closePanel_Click);
            // 
            // videoTrackMove
            // 
            this.videoTrackMove.BackgroundImage = global::RenCloud.Properties.Resources.Thick_Arrow_Pointing_Up;
            this.videoTrackMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.videoTrackMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoTrackMove.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia;
            this.videoTrackMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.videoTrackMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.videoTrackMove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.videoTrackMove.Location = new System.Drawing.Point(87, 2);
            this.videoTrackMove.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.videoTrackMove.Name = "videoTrackMove";
            this.videoTrackMove.Size = new System.Drawing.Size(72, 57);
            this.videoTrackMove.TabIndex = 0;
            this.videoTrackMove.UseVisualStyleBackColor = true;
            this.videoTrackMove.Click += new System.EventHandler(this.videoTrackMove_Click);
            // 
            // leftMove
            // 
            this.leftMove.BackgroundImage = global::RenCloud.Properties.Resources.Arrow_Pointing_Left;
            this.leftMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.leftMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.leftMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.leftMove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.leftMove.Location = new System.Drawing.Point(2, 61);
            this.leftMove.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.leftMove.Name = "leftMove";
            this.leftMove.Size = new System.Drawing.Size(84, 51);
            this.leftMove.TabIndex = 3;
            this.leftMove.UseVisualStyleBackColor = true;
            this.leftMove.Click += new System.EventHandler(this.leftMove_Click);
            // 
            // audioTrackMove
            // 
            this.audioTrackMove.BackgroundImage = global::RenCloud.Properties.Resources.Thick_Arrow_Pointing_Down;
            this.audioTrackMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.audioTrackMove.Dock = System.Windows.Forms.DockStyle.Top;
            this.audioTrackMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.audioTrackMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.audioTrackMove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.audioTrackMove.Location = new System.Drawing.Point(87, 114);
            this.audioTrackMove.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.audioTrackMove.Name = "audioTrackMove";
            this.audioTrackMove.Size = new System.Drawing.Size(72, 52);
            this.audioTrackMove.TabIndex = 1;
            this.audioTrackMove.UseVisualStyleBackColor = true;
            this.audioTrackMove.Click += new System.EventHandler(this.audioTrackMove_Click);
            // 
            // rightMove
            // 
            this.rightMove.BackgroundImage = global::RenCloud.Properties.Resources.Arrow;
            this.rightMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rightMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rightMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.rightMove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rightMove.Location = new System.Drawing.Point(160, 61);
            this.rightMove.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.rightMove.Name = "rightMove";
            this.rightMove.Size = new System.Drawing.Size(95, 51);
            this.rightMove.TabIndex = 2;
            this.rightMove.UseVisualStyleBackColor = true;
            this.rightMove.Click += new System.EventHandler(this.rightMove_Click);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 2;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.PauseButton, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.PlayButton, 1, 0);
            this.tableLayoutPanel13.Location = new System.Drawing.Point(87, 61);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(72, 51);
            this.tableLayoutPanel13.TabIndex = 3;
            // 
            // PauseButton
            // 
            this.PauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PauseButton.Image = global::RenCloud.Properties.Resources.Paus22e;
            this.PauseButton.Location = new System.Drawing.Point(0, 0);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(36, 51);
            this.PauseButton.TabIndex = 4;
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PlayButton.Image = global::RenCloud.Properties.Resources.Play;
            this.PlayButton.Location = new System.Drawing.Point(36, 0);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(36, 51);
            this.PlayButton.TabIndex = 3;
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // PreviewPanel
            // 
            this.PreviewPanel.BackColor = System.Drawing.Color.Black;
            this.PreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PreviewPanel.Controls.Add(this.fpsComboBoxPreview);
            this.PreviewPanel.Controls.Add(this.heightComboBoxPreview);
            this.PreviewPanel.Controls.Add(this.widthComboBoxPreview);
            this.PreviewPanel.Location = new System.Drawing.Point(419, 22);
            this.PreviewPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PreviewPanel.Name = "PreviewPanel";
            this.PreviewPanel.Size = new System.Drawing.Size(473, 416);
            this.PreviewPanel.TabIndex = 2;
            // 
            // fpsComboBoxPreview
            // 
            this.fpsComboBoxPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(38)))), ((int)(((byte)(88)))));
            this.fpsComboBoxPreview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpsComboBoxPreview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fpsComboBoxPreview.ForeColor = System.Drawing.SystemColors.Menu;
            this.fpsComboBoxPreview.FormattingEnabled = true;
            this.fpsComboBoxPreview.Items.AddRange(new object[] {
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60",
            "120"});
            this.fpsComboBoxPreview.Location = new System.Drawing.Point(135, 384);
            this.fpsComboBoxPreview.Name = "fpsComboBoxPreview";
            this.fpsComboBoxPreview.Size = new System.Drawing.Size(65, 31);
            this.fpsComboBoxPreview.TabIndex = 2;
            this.fpsComboBoxPreview.SelectedIndexChanged += new System.EventHandler(this.fpsComboBoxPreview_SelectedIndexChanged);
            // 
            // heightComboBoxPreview
            // 
            this.heightComboBoxPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(38)))), ((int)(((byte)(88)))));
            this.heightComboBoxPreview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.heightComboBoxPreview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.heightComboBoxPreview.ForeColor = System.Drawing.SystemColors.Menu;
            this.heightComboBoxPreview.FormattingEnabled = true;
            this.heightComboBoxPreview.Items.AddRange(new object[] {
            "414",
            "622",
            "828",
            "1036",
            "1242",
            "1450",
            "1656",
            "1696"});
            this.heightComboBoxPreview.Location = new System.Drawing.Point(69, 384);
            this.heightComboBoxPreview.Name = "heightComboBoxPreview";
            this.heightComboBoxPreview.Size = new System.Drawing.Size(62, 31);
            this.heightComboBoxPreview.TabIndex = 1;
            this.heightComboBoxPreview.SelectedIndexChanged += new System.EventHandler(this.heightComboBoxPreview_SelectedIndexChanged);
            // 
            // widthComboBoxPreview
            // 
            this.widthComboBoxPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(38)))), ((int)(((byte)(88)))));
            this.widthComboBoxPreview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.widthComboBoxPreview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.widthComboBoxPreview.ForeColor = System.Drawing.SystemColors.Menu;
            this.widthComboBoxPreview.FormattingEnabled = true;
            this.widthComboBoxPreview.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.widthComboBoxPreview.Items.AddRange(new object[] {
            "472",
            "708",
            "944",
            "1180",
            "1416",
            "1652",
            "1888",
            "1934"});
            this.widthComboBoxPreview.Location = new System.Drawing.Point(3, 384);
            this.widthComboBoxPreview.Name = "widthComboBoxPreview";
            this.widthComboBoxPreview.Size = new System.Drawing.Size(63, 31);
            this.widthComboBoxPreview.TabIndex = 0;
            this.widthComboBoxPreview.SelectedIndexChanged += new System.EventHandler(this.widthComboBoxPreview_SelectedIndexChanged);
            // 
            // panel10
            // 
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.resyncBtn);
            this.panel10.Controls.Add(this.fastPreviewCheckBox);
            this.panel10.Controls.Add(this.help_label);
            this.panel10.Controls.Add(this.controlPanel_label);
            this.panel10.Controls.Add(this.file_label);
            this.panel10.Location = new System.Drawing.Point(0, 1);
            this.panel10.Margin = new System.Windows.Forms.Padding(0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(892, 21);
            this.panel10.TabIndex = 0;
            // 
            // resyncBtn
            // 
            this.resyncBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.resyncBtn.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.resyncBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.resyncBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.resyncBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resyncBtn.Font = new System.Drawing.Font("Bahnschrift Condensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resyncBtn.ForeColor = System.Drawing.Color.Lime;
            this.resyncBtn.Location = new System.Drawing.Point(808, 0);
            this.resyncBtn.Margin = new System.Windows.Forms.Padding(0);
            this.resyncBtn.Name = "resyncBtn";
            this.resyncBtn.Size = new System.Drawing.Size(54, 19);
            this.resyncBtn.TabIndex = 4;
            this.resyncBtn.Text = "Resync";
            this.resyncBtn.UseVisualStyleBackColor = true;
            this.resyncBtn.Click += new System.EventHandler(this.resyncBtn_Click);
            // 
            // fastPreviewCheckBox
            // 
            this.fastPreviewCheckBox.AutoSize = true;
            this.fastPreviewCheckBox.ForeColor = System.Drawing.Color.White;
            this.fastPreviewCheckBox.Location = new System.Drawing.Point(541, 1);
            this.fastPreviewCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.fastPreviewCheckBox.Name = "fastPreviewCheckBox";
            this.fastPreviewCheckBox.Size = new System.Drawing.Size(389, 27);
            this.fastPreviewCheckBox.TabIndex = 5;
            this.fastPreviewCheckBox.Text = "Fast Preview (Requires resync time-to-time)";
            this.fastPreviewCheckBox.UseVisualStyleBackColor = true;
            this.fastPreviewCheckBox.CheckedChanged += new System.EventHandler(this.fastPreviewCheckBox_CheckedChanged);
            // 
            // help_label
            // 
            this.help_label.AutoSize = true;
            this.help_label.Dock = System.Windows.Forms.DockStyle.Left;
            this.help_label.Font = new System.Drawing.Font("Microsoft JhengHei Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.help_label.ForeColor = System.Drawing.Color.White;
            this.help_label.Location = new System.Drawing.Point(200, 0);
            this.help_label.Margin = new System.Windows.Forms.Padding(0);
            this.help_label.Name = "help_label";
            this.help_label.Size = new System.Drawing.Size(72, 29);
            this.help_label.TabIndex = 6;
            this.help_label.Text = "Open";
            // 
            // controlPanel_label
            // 
            this.controlPanel_label.AutoSize = true;
            this.controlPanel_label.Dock = System.Windows.Forms.DockStyle.Left;
            this.controlPanel_label.Font = new System.Drawing.Font("Microsoft JhengHei Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlPanel_label.ForeColor = System.Drawing.Color.White;
            this.controlPanel_label.Location = new System.Drawing.Point(48, 0);
            this.controlPanel_label.Margin = new System.Windows.Forms.Padding(0);
            this.controlPanel_label.Name = "controlPanel_label";
            this.controlPanel_label.Size = new System.Drawing.Size(152, 29);
            this.controlPanel_label.TabIndex = 5;
            this.controlPanel_label.Text = "Control Panel";
            // 
            // file_label
            // 
            this.file_label.AutoSize = true;
            this.file_label.Dock = System.Windows.Forms.DockStyle.Left;
            this.file_label.Font = new System.Drawing.Font("Microsoft JhengHei Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_label.ForeColor = System.Drawing.Color.White;
            this.file_label.Location = new System.Drawing.Point(0, 0);
            this.file_label.Margin = new System.Windows.Forms.Padding(0);
            this.file_label.Name = "file_label";
            this.file_label.Size = new System.Drawing.Size(48, 29);
            this.file_label.TabIndex = 3;
            this.file_label.Text = "File";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Location = new System.Drawing.Point(209, 535);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(895, 199);
            this.panel6.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.AutoScroll = true;
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.AudioTrack);
            this.panel8.Controls.Add(this.EditingRuller);
            this.panel8.Controls.Add(this.VideoTrack);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(159, 0);
            this.panel8.Margin = new System.Windows.Forms.Padding(0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(736, 199);
            this.panel8.TabIndex = 0;
            // 
            // AudioTrack
            // 
            this.AudioTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AudioTrack.Controls.Add(this.AudioTrackPlaceholder);
            this.AudioTrack.Location = new System.Drawing.Point(0, 98);
            this.AudioTrack.Margin = new System.Windows.Forms.Padding(0);
            this.AudioTrack.Name = "AudioTrack";
            this.AudioTrack.Size = new System.Drawing.Size(734, 75);
            this.AudioTrack.TabIndex = 5;
            // 
            // AudioTrackPlaceholder
            // 
            this.AudioTrackPlaceholder.Location = new System.Drawing.Point(1, 21);
            this.AudioTrackPlaceholder.Margin = new System.Windows.Forms.Padding(0);
            this.AudioTrackPlaceholder.Name = "AudioTrackPlaceholder";
            this.AudioTrackPlaceholder.Size = new System.Drawing.Size(733, 31);
            this.AudioTrackPlaceholder.TabIndex = 3;
            this.AudioTrackPlaceholder.Paint += new System.Windows.Forms.PaintEventHandler(this.AudioTrackPlaceholder_Paint);
            // 
            // EditingRuller
            // 
            this.EditingRuller.AutoScroll = true;
            this.EditingRuller.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EditingRuller.Location = new System.Drawing.Point(0, 0);
            this.EditingRuller.Margin = new System.Windows.Forms.Padding(0);
            this.EditingRuller.Name = "EditingRuller";
            this.EditingRuller.Size = new System.Drawing.Size(733, 23);
            this.EditingRuller.TabIndex = 3;
            // 
            // VideoTrack
            // 
            this.VideoTrack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VideoTrack.Controls.Add(this.VideoTrackPlaceholder);
            this.VideoTrack.Location = new System.Drawing.Point(0, 23);
            this.VideoTrack.Margin = new System.Windows.Forms.Padding(0);
            this.VideoTrack.Name = "VideoTrack";
            this.VideoTrack.Size = new System.Drawing.Size(733, 75);
            this.VideoTrack.TabIndex = 4;
            // 
            // VideoTrackPlaceholder
            // 
            this.VideoTrackPlaceholder.BackColor = System.Drawing.Color.Transparent;
            this.VideoTrackPlaceholder.Location = new System.Drawing.Point(0, 20);
            this.VideoTrackPlaceholder.Margin = new System.Windows.Forms.Padding(0);
            this.VideoTrackPlaceholder.Name = "VideoTrackPlaceholder";
            this.VideoTrackPlaceholder.Size = new System.Drawing.Size(733, 31);
            this.VideoTrackPlaceholder.TabIndex = 2;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.tableLayoutPanel14);
            this.panel7.Location = new System.Drawing.Point(1, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(158, 199);
            this.panel7.TabIndex = 0;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel16, 0, 2);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel15, 0, 1);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 4;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(156, 197);
            this.tableLayoutPanel14.TabIndex = 0;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.28205F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.71795F));
            this.tableLayoutPanel16.Controls.Add(this.pictureBox5, 1, 1);
            this.tableLayoutPanel16.Controls.Add(this.muteCheckBox, 0, 1);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(0, 96);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 3;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(156, 75);
            this.tableLayoutPanel16.TabIndex = 1;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox5.Image = global::RenCloud.Properties.Resources.Mute;
            this.pictureBox5.Location = new System.Drawing.Point(119, 21);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(37, 34);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            // 
            // muteCheckBox
            // 
            this.muteCheckBox.AutoSize = true;
            this.muteCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.muteCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.muteCheckBox.Location = new System.Drawing.Point(3, 24);
            this.muteCheckBox.Name = "muteCheckBox";
            this.muteCheckBox.Size = new System.Drawing.Size(113, 28);
            this.muteCheckBox.TabIndex = 1;
            this.muteCheckBox.Text = "checkBox2";
            this.muteCheckBox.UseVisualStyleBackColor = true;
            this.muteCheckBox.CheckedChanged += new System.EventHandler(this.muteCheckBox_CheckedChanged);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.28205F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.71795F));
            this.tableLayoutPanel15.Controls.Add(this.hideCheckBox, 0, 1);
            this.tableLayoutPanel15.Controls.Add(this.pictureBox6, 1, 1);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(0, 21);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 3;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(156, 75);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // hideCheckBox
            // 
            this.hideCheckBox.AutoSize = true;
            this.hideCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.hideCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.hideCheckBox.Location = new System.Drawing.Point(3, 25);
            this.hideCheckBox.Name = "hideCheckBox";
            this.hideCheckBox.Size = new System.Drawing.Size(113, 24);
            this.hideCheckBox.TabIndex = 2;
            this.hideCheckBox.Text = "checkBox3";
            this.hideCheckBox.UseVisualStyleBackColor = true;
            this.hideCheckBox.CheckedChanged += new System.EventHandler(this.hideCheckBox_CheckedChanged);
            // 
            // pictureBox6
            // 
            this.pictureBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox6.Image = global::RenCloud.Properties.Resources.Hide;
            this.pictureBox6.Location = new System.Drawing.Point(119, 22);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(37, 30);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 1;
            this.pictureBox6.TabStop = false;
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.importedMediaPanel);
            this.panel11.Location = new System.Drawing.Point(209, 80);
            this.panel11.Margin = new System.Windows.Forms.Padding(0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(419, 283);
            this.panel11.TabIndex = 1;
            // 
            // importedMediaPanel
            // 
            this.importedMediaPanel.ColumnCount = 3;
            this.importedMediaPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.importedMediaPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.importedMediaPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.importedMediaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.importedMediaPanel.Location = new System.Drawing.Point(0, 0);
            this.importedMediaPanel.Name = "importedMediaPanel";
            this.importedMediaPanel.RowCount = 3;
            this.importedMediaPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.importedMediaPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.importedMediaPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.importedMediaPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.importedMediaPanel.Size = new System.Drawing.Size(417, 281);
            this.importedMediaPanel.TabIndex = 0;
            // 
            // RenderPanel
            // 
            this.RenderPanel.AccessibleName = "";
            this.RenderPanel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.RenderPanel.Controls.Add(this.saveTemplatePanel);
            this.RenderPanel.Controls.Add(this.tableLayoutPanel1);
            this.RenderPanel.Controls.Add(this.button5);
            this.RenderPanel.Controls.Add(this.renderProgressBar);
            this.RenderPanel.Location = new System.Drawing.Point(209, 59);
            this.RenderPanel.Margin = new System.Windows.Forms.Padding(0);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(895, 675);
            this.RenderPanel.TabIndex = 0;
            // 
            // saveTemplatePanel
            // 
            this.saveTemplatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveTemplatePanel.ColumnCount = 1;
            this.saveTemplatePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.saveTemplatePanel.Controls.Add(this.label31, 0, 0);
            this.saveTemplatePanel.Controls.Add(this.tableLayoutPanel18, 0, 2);
            this.saveTemplatePanel.Controls.Add(this.templateNameTextBox, 0, 1);
            this.saveTemplatePanel.Location = new System.Drawing.Point(143, 419);
            this.saveTemplatePanel.Margin = new System.Windows.Forms.Padding(0);
            this.saveTemplatePanel.Name = "saveTemplatePanel";
            this.saveTemplatePanel.RowCount = 3;
            this.saveTemplatePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.94118F));
            this.saveTemplatePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.05882F));
            this.saveTemplatePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.saveTemplatePanel.Size = new System.Drawing.Size(266, 109);
            this.saveTemplatePanel.TabIndex = 6;
            this.saveTemplatePanel.Visible = false;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label31.ForeColor = System.Drawing.Color.MintCream;
            this.label31.Location = new System.Drawing.Point(0, 0);
            this.label31.Margin = new System.Windows.Forms.Padding(0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(266, 36);
            this.label31.TabIndex = 5;
            this.label31.Text = "Template Name";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 2;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Controls.Add(this.cancelSaveBtn, 1, 0);
            this.tableLayoutPanel18.Controls.Add(this.confirmSaveBtn, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(0, 68);
            this.tableLayoutPanel18.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 1;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(266, 41);
            this.tableLayoutPanel18.TabIndex = 0;
            // 
            // cancelSaveBtn
            // 
            this.cancelSaveBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancelSaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelSaveBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cancelSaveBtn.Location = new System.Drawing.Point(136, 5);
            this.cancelSaveBtn.Name = "cancelSaveBtn";
            this.cancelSaveBtn.Size = new System.Drawing.Size(127, 30);
            this.cancelSaveBtn.TabIndex = 2;
            this.cancelSaveBtn.Text = "Cancel";
            this.cancelSaveBtn.UseVisualStyleBackColor = true;
            this.cancelSaveBtn.Click += new System.EventHandler(this.cancelSaveBtn_Click);
            // 
            // confirmSaveBtn
            // 
            this.confirmSaveBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.confirmSaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.confirmSaveBtn.ForeColor = System.Drawing.Color.Lime;
            this.confirmSaveBtn.Location = new System.Drawing.Point(3, 5);
            this.confirmSaveBtn.Name = "confirmSaveBtn";
            this.confirmSaveBtn.Size = new System.Drawing.Size(127, 30);
            this.confirmSaveBtn.TabIndex = 1;
            this.confirmSaveBtn.Text = "Save";
            this.confirmSaveBtn.UseVisualStyleBackColor = true;
            this.confirmSaveBtn.Click += new System.EventHandler(this.confirmSaveBtn_Click);
            // 
            // templateNameTextBox
            // 
            this.templateNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.templateNameTextBox.Location = new System.Drawing.Point(50, 39);
            this.templateNameTextBox.Name = "templateNameTextBox";
            this.templateNameTextBox.Size = new System.Drawing.Size(165, 31);
            this.templateNameTextBox.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.30151F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.69849F));
            this.tableLayoutPanel1.Controls.Add(this.removeTemplateBtn, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(54, 103);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 394F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(798, 467);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // removeTemplateBtn
            // 
            this.removeTemplateBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.removeTemplateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeTemplateBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.removeTemplateBtn.Location = new System.Drawing.Point(286, 431);
            this.removeTemplateBtn.Name = "removeTemplateBtn";
            this.removeTemplateBtn.Size = new System.Drawing.Size(132, 30);
            this.removeTemplateBtn.TabIndex = 6;
            this.removeTemplateBtn.Text = "Remove Template";
            this.removeTemplateBtn.UseVisualStyleBackColor = true;
            this.removeTemplateBtn.Click += new System.EventHandler(this.removeTemplateBtn_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.MintCream;
            this.label3.Location = new System.Drawing.Point(283, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(513, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Advanced Settings";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.58768F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.41232F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(222, 27);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::RenCloud.Properties.Resources.Key;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 21);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.MintCream;
            this.label2.Location = new System.Drawing.Point(36, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fast Settings/Templates";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 299F));
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.bitrateComboBox, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.codecComboBox, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.formatComboBox, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label11, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.framerateComboBox, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.templateComboBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.resolutionComboBox, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.nameTextBox, 1, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 31);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 8;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(279, 394);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label6.ForeColor = System.Drawing.Color.MintCream;
            this.label6.Location = new System.Drawing.Point(1, 106);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 34);
            this.label6.TabIndex = 6;
            this.label6.Text = "Export to";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label5.ForeColor = System.Drawing.Color.MintCream;
            this.label5.Location = new System.Drawing.Point(1, 57);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 34);
            this.label5.TabIndex = 4;
            this.label5.Text = "Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label4.ForeColor = System.Drawing.Color.MintCream;
            this.label4.Location = new System.Drawing.Point(1, 8);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 34);
            this.label4.TabIndex = 3;
            this.label4.Text = "Template";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
            this.tableLayoutPanel4.Controls.Add(this.exportPathTextBox, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.exportImg, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(87, 102);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(188, 41);
            this.tableLayoutPanel4.TabIndex = 8;
            // 
            // exportPathTextBox
            // 
            this.exportPathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.exportPathTextBox.Enabled = false;
            this.exportPathTextBox.Location = new System.Drawing.Point(0, 5);
            this.exportPathTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.exportPathTextBox.Name = "exportPathTextBox";
            this.exportPathTextBox.Size = new System.Drawing.Size(156, 31);
            this.exportPathTextBox.TabIndex = 7;
            // 
            // exportImg
            // 
            this.exportImg.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.exportImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exportImg.Image = global::RenCloud.Properties.Resources.DashboardV2;
            this.exportImg.Location = new System.Drawing.Point(161, 8);
            this.exportImg.Margin = new System.Windows.Forms.Padding(0);
            this.exportImg.Name = "exportImg";
            this.exportImg.Size = new System.Drawing.Size(27, 24);
            this.exportImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.exportImg.TabIndex = 4;
            this.exportImg.TabStop = false;
            this.exportImg.Click += new System.EventHandler(this.exportImg_Click);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label7.ForeColor = System.Drawing.Color.MintCream;
            this.label7.Location = new System.Drawing.Point(1, 155);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 34);
            this.label7.TabIndex = 9;
            this.label7.Text = "Resolution";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label8.ForeColor = System.Drawing.Color.MintCream;
            this.label8.Location = new System.Drawing.Point(1, 204);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 34);
            this.label8.TabIndex = 11;
            this.label8.Text = "BitRate";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bitrateComboBox
            // 
            this.bitrateComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.bitrateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bitrateComboBox.FormattingEnabled = true;
            this.bitrateComboBox.Items.AddRange(new object[] {
            "(144p) 300",
            "(240p) 700",
            "(360p) 1000",
            "(480p) 2500",
            "(720p) 5000",
            "(1080p) 8000",
            "(1440p-2K) 12000",
            "(2160p-4K) 35000"});
            this.bitrateComboBox.Location = new System.Drawing.Point(87, 207);
            this.bitrateComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.bitrateComboBox.Name = "bitrateComboBox";
            this.bitrateComboBox.Size = new System.Drawing.Size(188, 31);
            this.bitrateComboBox.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label9.ForeColor = System.Drawing.Color.MintCream;
            this.label9.Location = new System.Drawing.Point(1, 253);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 34);
            this.label9.TabIndex = 13;
            this.label9.Text = "Codec";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // codecComboBox
            // 
            this.codecComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.codecComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codecComboBox.FormattingEnabled = true;
            this.codecComboBox.Items.AddRange(new object[] {
            "Low - Quality",
            "Medium - Quality",
            "High - Quality"});
            this.codecComboBox.Location = new System.Drawing.Point(87, 256);
            this.codecComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.codecComboBox.Name = "codecComboBox";
            this.codecComboBox.Size = new System.Drawing.Size(188, 31);
            this.codecComboBox.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label10.ForeColor = System.Drawing.Color.MintCream;
            this.label10.Location = new System.Drawing.Point(1, 302);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 34);
            this.label10.TabIndex = 15;
            this.label10.Text = "Format";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formatComboBox
            // 
            this.formatComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.formatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBox.FormattingEnabled = true;
            this.formatComboBox.Items.AddRange(new object[] {
            "Web",
            "High Quality",
            "Editing Workflows"});
            this.formatComboBox.Location = new System.Drawing.Point(87, 305);
            this.formatComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.formatComboBox.Name = "formatComboBox";
            this.formatComboBox.Size = new System.Drawing.Size(188, 31);
            this.formatComboBox.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label11.ForeColor = System.Drawing.Color.MintCream;
            this.label11.Location = new System.Drawing.Point(1, 351);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 34);
            this.label11.TabIndex = 17;
            this.label11.Text = "FrameRate";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // framerateComboBox
            // 
            this.framerateComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.framerateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.framerateComboBox.FormattingEnabled = true;
            this.framerateComboBox.Items.AddRange(new object[] {
            "Low",
            "Medium",
            "High"});
            this.framerateComboBox.Location = new System.Drawing.Point(87, 354);
            this.framerateComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.framerateComboBox.Name = "framerateComboBox";
            this.framerateComboBox.Size = new System.Drawing.Size(188, 31);
            this.framerateComboBox.TabIndex = 18;
            // 
            // templateComboBox
            // 
            this.templateComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.templateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateComboBox.FormattingEnabled = true;
            this.templateComboBox.Items.AddRange(new object[] {
            "Instagram - High(1080x1350)",
            "Instagram - Low(720x720)",
            "Instagram - Medium(1080x1080)",
            "TikTok - High(1080x1920)",
            "TikTok - Low(480x852)",
            "TikTok - Medium(720x1280)",
            "Video Default",
            "YouTube - High(1080p)",
            "YouTube - Low(480p)",
            "YouTube - Medium(720p)",
            "YTShorts - High(1080x1920)",
            "YTShorts - Low(480x852)",
            "YTShorts - Medium(720x1280)"});
            this.templateComboBox.Location = new System.Drawing.Point(87, 11);
            this.templateComboBox.Name = "templateComboBox";
            this.templateComboBox.Size = new System.Drawing.Size(188, 31);
            this.templateComboBox.Sorted = true;
            this.templateComboBox.TabIndex = 0;
            this.templateComboBox.SelectedIndexChanged += new System.EventHandler(this.templateComboBox_SelectedIndexChanged);
            // 
            // resolutionComboBox
            // 
            this.resolutionComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionComboBox.FormattingEnabled = true;
            this.resolutionComboBox.Items.AddRange(new object[] {
            "Low (144p)",
            "Low-Mid (240p)",
            "SD (360p)",
            "SD+ (480p)",
            "HD (720p)",
            "Full HD (1080p)",
            "2K Quad HD (1440p)",
            "4K Ultra HD (2160p)"});
            this.resolutionComboBox.Location = new System.Drawing.Point(87, 158);
            this.resolutionComboBox.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.resolutionComboBox.Name = "resolutionComboBox";
            this.resolutionComboBox.Size = new System.Drawing.Size(188, 31);
            this.resolutionComboBox.TabIndex = 10;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nameTextBox.Location = new System.Drawing.Point(87, 58);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 14, 3);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(188, 31);
            this.nameTextBox.TabIndex = 5;
            this.nameTextBox.Text = "output";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.saveTemplateBtn, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.renderBtn, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2, 427);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(279, 38);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // saveTemplateBtn
            // 
            this.saveTemplateBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.saveTemplateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveTemplateBtn.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.saveTemplateBtn.Location = new System.Drawing.Point(143, 4);
            this.saveTemplateBtn.Name = "saveTemplateBtn";
            this.saveTemplateBtn.Size = new System.Drawing.Size(132, 30);
            this.saveTemplateBtn.TabIndex = 1;
            this.saveTemplateBtn.Text = "Save as template";
            this.saveTemplateBtn.UseVisualStyleBackColor = true;
            this.saveTemplateBtn.Click += new System.EventHandler(this.saveTemplateBtn_Click);
            // 
            // renderBtn
            // 
            this.renderBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.renderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.renderBtn.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.renderBtn.Location = new System.Drawing.Point(4, 4);
            this.renderBtn.Name = "renderBtn";
            this.renderBtn.Size = new System.Drawing.Size(132, 30);
            this.renderBtn.TabIndex = 0;
            this.renderBtn.Text = "Render";
            this.renderBtn.UseVisualStyleBackColor = true;
            this.renderBtn.Click += new System.EventHandler(this.renderBtn_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel8, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(283, 31);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.329949F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 372F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(513, 394);
            this.tableLayoutPanel6.TabIndex = 5;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 3;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.26415F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.73585F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 402F));
            this.tableLayoutPanel7.Controls.Add(this.label13, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.label12, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.enableAdvancedCheckBox, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(513, 22);
            this.tableLayoutPanel7.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Comic Sans MS", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.MintCream;
            this.label13.Location = new System.Drawing.Point(110, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(403, 22);
            this.label13.TabIndex = 5;
            this.label13.Text = "(WARNING: might cause hardware damage. Professionals only)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label12.ForeColor = System.Drawing.Color.MintCream;
            this.label12.Location = new System.Drawing.Point(13, 0);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 22);
            this.label12.TabIndex = 4;
            this.label12.Text = "Enable Advanced Settings";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // enableAdvancedCheckBox
            // 
            this.enableAdvancedCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enableAdvancedCheckBox.Location = new System.Drawing.Point(0, 0);
            this.enableAdvancedCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.enableAdvancedCheckBox.Name = "enableAdvancedCheckBox";
            this.enableAdvancedCheckBox.Size = new System.Drawing.Size(13, 22);
            this.enableAdvancedCheckBox.TabIndex = 0;
            this.enableAdvancedCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enableAdvancedCheckBox.UseVisualStyleBackColor = true;
            this.enableAdvancedCheckBox.CheckedChanged += new System.EventHandler(this.enableAdvancedCheckBox_CheckedChanged);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 452F));
            this.tableLayoutPanel8.Controls.Add(this.aspectRatioComboBoxAdvanced, 1, 2);
            this.tableLayoutPanel8.Controls.Add(this.label30, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.pixelFormatComboBoxAdvanced, 1, 10);
            this.tableLayoutPanel8.Controls.Add(this.label29, 0, 10);
            this.tableLayoutPanel8.Controls.Add(this.encodingPresetComboBoxAdvanced, 1, 9);
            this.tableLayoutPanel8.Controls.Add(this.label28, 0, 9);
            this.tableLayoutPanel8.Controls.Add(this.framerateNumericAdvanced, 1, 5);
            this.tableLayoutPanel8.Controls.Add(this.label27, 0, 5);
            this.tableLayoutPanel8.Controls.Add(this.cpuUseComboBoxAdvanced, 1, 11);
            this.tableLayoutPanel8.Controls.Add(this.label26, 0, 11);
            this.tableLayoutPanel8.Controls.Add(this.qualityCrfNumericAdvanced, 1, 6);
            this.tableLayoutPanel8.Controls.Add(this.label25, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.formatComboBoxAdvanced, 1, 7);
            this.tableLayoutPanel8.Controls.Add(this.label24, 0, 7);
            this.tableLayoutPanel8.Controls.Add(this.label23, 0, 8);
            this.tableLayoutPanel8.Controls.Add(this.bitrateNumericAdvanced, 1, 4);
            this.tableLayoutPanel8.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 1, 3);
            this.tableLayoutPanel8.Controls.Add(this.label16, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.label15, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.nameTextBoxAdvanced, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.codecComboBoxAdvanced, 1, 8);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(0, 22);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 12;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(513, 372);
            this.tableLayoutPanel8.TabIndex = 6;
            // 
            // aspectRatioComboBoxAdvanced
            // 
            this.aspectRatioComboBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.aspectRatioComboBoxAdvanced.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aspectRatioComboBoxAdvanced.Enabled = false;
            this.aspectRatioComboBoxAdvanced.FormattingEnabled = true;
            this.aspectRatioComboBoxAdvanced.Items.AddRange(new object[] {
            "1:1   Square",
            "4:3   Classic monitors, photos",
            "3:2   DSLR camera photos",
            "16:9 HD video, modern screens",
            "21:9 Ultrawide monitors",
            "9:16 Vertical video (mobile)",
            "2:3   Portrait photos"});
            this.aspectRatioComboBoxAdvanced.Location = new System.Drawing.Point(60, 64);
            this.aspectRatioComboBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.aspectRatioComboBoxAdvanced.Name = "aspectRatioComboBoxAdvanced";
            this.aspectRatioComboBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.aspectRatioComboBoxAdvanced.TabIndex = 29;
            this.aspectRatioComboBoxAdvanced.SelectedIndexChanged += new System.EventHandler(this.aspectRatioComboBoxAdvanced_SelectedIndexChanged);
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Dock = System.Windows.Forms.DockStyle.Right;
            this.label30.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label30.ForeColor = System.Drawing.Color.MintCream;
            this.label30.Location = new System.Drawing.Point(1, 63);
            this.label30.Margin = new System.Windows.Forms.Padding(0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(58, 30);
            this.label30.TabIndex = 28;
            this.label30.Text = "Aspect Ratio";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pixelFormatComboBoxAdvanced
            // 
            this.pixelFormatComboBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pixelFormatComboBoxAdvanced.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pixelFormatComboBoxAdvanced.Enabled = false;
            this.pixelFormatComboBoxAdvanced.FormattingEnabled = true;
            this.pixelFormatComboBoxAdvanced.Items.AddRange(new object[] {
            "YUV 4:2:0 (yuv420p)",
            "YUV 4:2:0 10-bit (yuv420p10le)",
            "YUV 4:2:2 (yuv422p)",
            "YUV 4:4:4 (yuv444p)",
            "RGB (rgb24)",
            "RGBA (rgba)",
            "Grayscale (gray)",
            "YUVA 4:2:0 (yuva420p)",
            "NV12 (nv12)"});
            this.pixelFormatComboBoxAdvanced.Location = new System.Drawing.Point(60, 312);
            this.pixelFormatComboBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.pixelFormatComboBoxAdvanced.Name = "pixelFormatComboBoxAdvanced";
            this.pixelFormatComboBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.pixelFormatComboBoxAdvanced.TabIndex = 27;
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.Transparent;
            this.label29.Dock = System.Windows.Forms.DockStyle.Right;
            this.label29.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label29.ForeColor = System.Drawing.Color.MintCream;
            this.label29.Location = new System.Drawing.Point(1, 311);
            this.label29.Margin = new System.Windows.Forms.Padding(0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(58, 30);
            this.label29.TabIndex = 26;
            this.label29.Text = "Pixel Format";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // encodingPresetComboBoxAdvanced
            // 
            this.encodingPresetComboBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.encodingPresetComboBoxAdvanced.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encodingPresetComboBoxAdvanced.Enabled = false;
            this.encodingPresetComboBoxAdvanced.FormattingEnabled = true;
            this.encodingPresetComboBoxAdvanced.Items.AddRange(new object[] {
            "H.264 (libx264)",
            "H.264 NVENC (h264_nvenc)",
            "H.264 Intel QSV (h264_qsv)",
            "H.264 AMD (h264_amf)",
            "H.265 (libx265)",
            "H.265 NVENC (hevc_nvenc)",
            "H.265 Intel QSV (hevc_qsv)",
            "H.265 AMD (hevc_amf)",
            "MPEG-4 (mpeg4)",
            "VP8 (libvpx)",
            "VP9 (libvpx-vp9)",
            "AV1 (libaom-av1)",
            "AV1 Intel QSV (av1_qsv)",
            "Apple ProRes (prores)",
            "Apple ProRes (prores_ks)"});
            this.encodingPresetComboBoxAdvanced.Location = new System.Drawing.Point(60, 281);
            this.encodingPresetComboBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.encodingPresetComboBoxAdvanced.Name = "encodingPresetComboBoxAdvanced";
            this.encodingPresetComboBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.encodingPresetComboBoxAdvanced.TabIndex = 25;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Dock = System.Windows.Forms.DockStyle.Right;
            this.label28.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label28.ForeColor = System.Drawing.Color.MintCream;
            this.label28.Location = new System.Drawing.Point(1, 280);
            this.label28.Margin = new System.Windows.Forms.Padding(0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(58, 30);
            this.label28.TabIndex = 24;
            this.label28.Text = "Encoding Preset";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // framerateNumericAdvanced
            // 
            this.framerateNumericAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.framerateNumericAdvanced.Enabled = false;
            this.framerateNumericAdvanced.Location = new System.Drawing.Point(60, 156);
            this.framerateNumericAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.framerateNumericAdvanced.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.framerateNumericAdvanced.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.framerateNumericAdvanced.Name = "framerateNumericAdvanced";
            this.framerateNumericAdvanced.Size = new System.Drawing.Size(184, 31);
            this.framerateNumericAdvanced.TabIndex = 23;
            this.framerateNumericAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.framerateNumericAdvanced.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Dock = System.Windows.Forms.DockStyle.Right;
            this.label27.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label27.ForeColor = System.Drawing.Color.MintCream;
            this.label27.Location = new System.Drawing.Point(1, 156);
            this.label27.Margin = new System.Windows.Forms.Padding(0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(58, 30);
            this.label27.TabIndex = 22;
            this.label27.Text = "FrameRate";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cpuUseComboBoxAdvanced
            // 
            this.cpuUseComboBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cpuUseComboBoxAdvanced.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cpuUseComboBoxAdvanced.Enabled = false;
            this.cpuUseComboBoxAdvanced.FormattingEnabled = true;
            this.cpuUseComboBoxAdvanced.Items.AddRange(new object[] {
            "IN DEVELOPMENT"});
            this.cpuUseComboBoxAdvanced.Location = new System.Drawing.Point(60, 342);
            this.cpuUseComboBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.cpuUseComboBoxAdvanced.Name = "cpuUseComboBoxAdvanced";
            this.cpuUseComboBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.cpuUseComboBoxAdvanced.TabIndex = 21;
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Dock = System.Windows.Forms.DockStyle.Right;
            this.label26.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label26.ForeColor = System.Drawing.Color.MintCream;
            this.label26.Location = new System.Drawing.Point(1, 342);
            this.label26.Margin = new System.Windows.Forms.Padding(0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(58, 29);
            this.label26.TabIndex = 20;
            this.label26.Text = "CPU_Use(%)";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // qualityCrfNumericAdvanced
            // 
            this.qualityCrfNumericAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.qualityCrfNumericAdvanced.Enabled = false;
            this.qualityCrfNumericAdvanced.Location = new System.Drawing.Point(60, 187);
            this.qualityCrfNumericAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.qualityCrfNumericAdvanced.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.qualityCrfNumericAdvanced.Name = "qualityCrfNumericAdvanced";
            this.qualityCrfNumericAdvanced.Size = new System.Drawing.Size(184, 31);
            this.qualityCrfNumericAdvanced.TabIndex = 19;
            this.qualityCrfNumericAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.qualityCrfNumericAdvanced.Value = new decimal(new int[] {
            23,
            0,
            0,
            0});
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Dock = System.Windows.Forms.DockStyle.Right;
            this.label25.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label25.ForeColor = System.Drawing.Color.MintCream;
            this.label25.Location = new System.Drawing.Point(1, 187);
            this.label25.Margin = new System.Windows.Forms.Padding(0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(58, 30);
            this.label25.TabIndex = 18;
            this.label25.Text = "Quality (CRF)";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formatComboBoxAdvanced
            // 
            this.formatComboBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.formatComboBoxAdvanced.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatComboBoxAdvanced.Enabled = false;
            this.formatComboBoxAdvanced.FormattingEnabled = true;
            this.formatComboBoxAdvanced.Items.AddRange(new object[] {
            ".mp4",
            ".mkv",
            ".webm",
            ".avi",
            ".mov"});
            this.formatComboBoxAdvanced.Location = new System.Drawing.Point(60, 219);
            this.formatComboBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.formatComboBoxAdvanced.Name = "formatComboBoxAdvanced";
            this.formatComboBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.formatComboBoxAdvanced.TabIndex = 17;
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Dock = System.Windows.Forms.DockStyle.Right;
            this.label24.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label24.ForeColor = System.Drawing.Color.MintCream;
            this.label24.Location = new System.Drawing.Point(1, 218);
            this.label24.Margin = new System.Windows.Forms.Padding(0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(58, 30);
            this.label24.TabIndex = 16;
            this.label24.Text = "Format";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Dock = System.Windows.Forms.DockStyle.Right;
            this.label23.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label23.ForeColor = System.Drawing.Color.MintCream;
            this.label23.Location = new System.Drawing.Point(1, 249);
            this.label23.Margin = new System.Windows.Forms.Padding(0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(58, 30);
            this.label23.TabIndex = 14;
            this.label23.Text = "Codec";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bitrateNumericAdvanced
            // 
            this.bitrateNumericAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.bitrateNumericAdvanced.Enabled = false;
            this.bitrateNumericAdvanced.Location = new System.Drawing.Point(60, 125);
            this.bitrateNumericAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.bitrateNumericAdvanced.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.bitrateNumericAdvanced.Minimum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.bitrateNumericAdvanced.Name = "bitrateNumericAdvanced";
            this.bitrateNumericAdvanced.Size = new System.Drawing.Size(184, 31);
            this.bitrateNumericAdvanced.TabIndex = 13;
            this.bitrateNumericAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bitrateNumericAdvanced.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label1.ForeColor = System.Drawing.Color.MintCream;
            this.label1.Location = new System.Drawing.Point(1, 125);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 30);
            this.label1.TabIndex = 12;
            this.label1.Text = "BitRate";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel10.ColumnCount = 6;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.48515F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.51485F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel10.Controls.Add(this.label19, 5, 0);
            this.tableLayoutPanel10.Controls.Add(this.enableProportionalScalingCheckBox, 4, 0);
            this.tableLayoutPanel10.Controls.Add(this.label18, 2, 0);
            this.tableLayoutPanel10.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.widthNumericAdvanced, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.heightNumericAdvanced, 3, 0);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(60, 94);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(400, 30);
            this.tableLayoutPanel10.TabIndex = 11;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label19.ForeColor = System.Drawing.Color.MintCream;
            this.label19.Location = new System.Drawing.Point(240, 0);
            this.label19.Margin = new System.Windows.Forms.Padding(0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(160, 36);
            this.label19.TabIndex = 16;
            this.label19.Text = "Proportional scaling";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // enableProportionalScalingCheckBox
            // 
            this.enableProportionalScalingCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enableProportionalScalingCheckBox.Checked = true;
            this.enableProportionalScalingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableProportionalScalingCheckBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.enableProportionalScalingCheckBox.Enabled = false;
            this.enableProportionalScalingCheckBox.Location = new System.Drawing.Point(224, 0);
            this.enableProportionalScalingCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.enableProportionalScalingCheckBox.Name = "enableProportionalScalingCheckBox";
            this.enableProportionalScalingCheckBox.Size = new System.Drawing.Size(16, 36);
            this.enableProportionalScalingCheckBox.TabIndex = 15;
            this.enableProportionalScalingCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.enableProportionalScalingCheckBox.UseVisualStyleBackColor = true;
            this.enableProportionalScalingCheckBox.CheckedChanged += new System.EventHandler(this.enableProportionalScalingCheckBox_CheckedChanged);
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Dock = System.Windows.Forms.DockStyle.Right;
            this.label18.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label18.ForeColor = System.Drawing.Color.MintCream;
            this.label18.Location = new System.Drawing.Point(101, 0);
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 36);
            this.label18.TabIndex = 14;
            this.label18.Text = "Height";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Dock = System.Windows.Forms.DockStyle.Right;
            this.label17.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label17.ForeColor = System.Drawing.Color.MintCream;
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 36);
            this.label17.TabIndex = 11;
            this.label17.Text = "Width";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // widthNumericAdvanced
            // 
            this.widthNumericAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.widthNumericAdvanced.Enabled = false;
            this.widthNumericAdvanced.Location = new System.Drawing.Point(52, 2);
            this.widthNumericAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.widthNumericAdvanced.Maximum = new decimal(new int[] {
            3840,
            0,
            0,
            0});
            this.widthNumericAdvanced.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.widthNumericAdvanced.Name = "widthNumericAdvanced";
            this.widthNumericAdvanced.Size = new System.Drawing.Size(49, 31);
            this.widthNumericAdvanced.TabIndex = 12;
            this.widthNumericAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.widthNumericAdvanced.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.widthNumericAdvanced.ValueChanged += new System.EventHandler(this.widthNumericAdvanced_ValueChanged);
            // 
            // heightNumericAdvanced
            // 
            this.heightNumericAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.heightNumericAdvanced.Enabled = false;
            this.heightNumericAdvanced.Location = new System.Drawing.Point(154, 2);
            this.heightNumericAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.heightNumericAdvanced.Maximum = new decimal(new int[] {
            2160,
            0,
            0,
            0});
            this.heightNumericAdvanced.Minimum = new decimal(new int[] {
            144,
            0,
            0,
            0});
            this.heightNumericAdvanced.Name = "heightNumericAdvanced";
            this.heightNumericAdvanced.Size = new System.Drawing.Size(68, 31);
            this.heightNumericAdvanced.TabIndex = 13;
            this.heightNumericAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.heightNumericAdvanced.Value = new decimal(new int[] {
            144,
            0,
            0,
            0});
            this.heightNumericAdvanced.ValueChanged += new System.EventHandler(this.heightNumericAdvanced_ValueChanged);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Dock = System.Windows.Forms.DockStyle.Right;
            this.label16.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label16.ForeColor = System.Drawing.Color.MintCream;
            this.label16.Location = new System.Drawing.Point(1, 94);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 30);
            this.label16.TabIndex = 10;
            this.label16.Text = "Resolution";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.97826F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.02174F));
            this.tableLayoutPanel9.Controls.Add(this.exportPathTextBoxAdvanced, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.exportImgAdvanced, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(60, 32);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(195, 30);
            this.tableLayoutPanel9.TabIndex = 9;
            // 
            // exportPathTextBoxAdvanced
            // 
            this.exportPathTextBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.exportPathTextBoxAdvanced.Enabled = false;
            this.exportPathTextBoxAdvanced.Location = new System.Drawing.Point(0, 0);
            this.exportPathTextBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.exportPathTextBoxAdvanced.Name = "exportPathTextBoxAdvanced";
            this.exportPathTextBoxAdvanced.Size = new System.Drawing.Size(148, 31);
            this.exportPathTextBoxAdvanced.TabIndex = 7;
            // 
            // exportImgAdvanced
            // 
            this.exportImgAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.exportImgAdvanced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exportImgAdvanced.Image = global::RenCloud.Properties.Resources.DashboardV2;
            this.exportImgAdvanced.Location = new System.Drawing.Point(157, 3);
            this.exportImgAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.exportImgAdvanced.Name = "exportImgAdvanced";
            this.exportImgAdvanced.Size = new System.Drawing.Size(27, 24);
            this.exportImgAdvanced.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.exportImgAdvanced.TabIndex = 4;
            this.exportImgAdvanced.TabStop = false;
            this.exportImgAdvanced.Click += new System.EventHandler(this.exportImgAdvanced_Click);
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Dock = System.Windows.Forms.DockStyle.Right;
            this.label15.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label15.ForeColor = System.Drawing.Color.MintCream;
            this.label15.Location = new System.Drawing.Point(1, 32);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 30);
            this.label15.TabIndex = 7;
            this.label15.Text = "Export to";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nameTextBoxAdvanced
            // 
            this.nameTextBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nameTextBoxAdvanced.Enabled = false;
            this.nameTextBoxAdvanced.Location = new System.Drawing.Point(60, 1);
            this.nameTextBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.nameTextBoxAdvanced.Name = "nameTextBoxAdvanced";
            this.nameTextBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.nameTextBoxAdvanced.TabIndex = 6;
            this.nameTextBoxAdvanced.Text = "output";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Dock = System.Windows.Forms.DockStyle.Right;
            this.label14.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.label14.ForeColor = System.Drawing.Color.MintCream;
            this.label14.Location = new System.Drawing.Point(1, 1);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 30);
            this.label14.TabIndex = 5;
            this.label14.Text = "Name";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // codecComboBoxAdvanced
            // 
            this.codecComboBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.codecComboBoxAdvanced.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codecComboBoxAdvanced.Enabled = false;
            this.codecComboBoxAdvanced.FormattingEnabled = true;
            this.codecComboBoxAdvanced.Items.AddRange(new object[] {
            "H.264 (libx264)",
            "H.265 (libx265)",
            "MPEG-4 (mpeg4)",
            "VP8 (libvpx)",
            "VP9 (libvpx-vp9)",
            "AV1 (libaom-av1)",
            "Apple ProRes (prores)",
            "Apple ProRes (prores_ks)"});
            this.codecComboBoxAdvanced.Location = new System.Drawing.Point(60, 250);
            this.codecComboBoxAdvanced.Margin = new System.Windows.Forms.Padding(0);
            this.codecComboBoxAdvanced.Name = "codecComboBoxAdvanced";
            this.codecComboBoxAdvanced.Size = new System.Drawing.Size(184, 31);
            this.codecComboBoxAdvanced.TabIndex = 15;
            // 
            // button5
            // 
            this.button5.AutoSize = true;
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(38)))), ((int)(((byte)(88)))));
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Image = global::RenCloud.Properties.Resources.DashboardV2;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(54, 45);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(355, 56);
            this.button5.TabIndex = 4;
            this.button5.Text = "Output Render Settings";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // renderProgressBar
            // 
            this.renderProgressBar.Location = new System.Drawing.Point(54, 573);
            this.renderProgressBar.Margin = new System.Windows.Forms.Padding(0);
            this.renderProgressBar.Name = "renderProgressBar";
            this.renderProgressBar.Size = new System.Drawing.Size(798, 23);
            this.renderProgressBar.Step = 1;
            this.renderProgressBar.TabIndex = 0;
            // 
            // UserInterfaceForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(38)))), ((int)(((byte)(88)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1104, 734);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RenderPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft JhengHei Light", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(38)))), ((int)(((byte)(88)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserInterfaceForm";
            this.Text = "UserInterfaceForm";
            this.Load += new System.EventHandler(this.UserInterfaceForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.PreviewPanel.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.AudioTrack.ResumeLayout(false);
            this.VideoTrack.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.panel11.ResumeLayout(false);
            this.RenderPanel.ResumeLayout(false);
            this.RenderPanel.PerformLayout();
            this.saveTemplatePanel.ResumeLayout(false);
            this.saveTemplatePanel.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportImg)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.framerateNumericAdvanced)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualityCrfNumericAdvanced)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bitrateNumericAdvanced)).EndInit();
            this.tableLayoutPanel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericAdvanced)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericAdvanced)).EndInit();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.exportImgAdvanced)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel PreviewPanel;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel EditingRuller;
        private System.Windows.Forms.Panel VideoTrack;
        private System.Windows.Forms.Panel VideoTrackPlaceholder;
        private System.Windows.Forms.Panel AudioTrack;
        private System.Windows.Forms.Panel AudioTrackPlaceholder;
        private System.Windows.Forms.Label TimeStamp;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Label file_label;
        private System.Windows.Forms.Label help_label;
        private System.Windows.Forms.Label controlPanel_label;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ProgressBar renderProgressBar;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox templateComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.PictureBox exportImg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox exportPathTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox resolutionComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox bitrateComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox codecComboBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox formatComboBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox framerateComboBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button saveTemplateBtn;
        private System.Windows.Forms.Button renderBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.CheckBox enableAdvancedCheckBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TextBox exportPathTextBoxAdvanced;
        private System.Windows.Forms.PictureBox exportImgAdvanced;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox nameTextBoxAdvanced;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox enableProportionalScalingCheckBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown widthNumericAdvanced;
        private System.Windows.Forms.NumericUpDown heightNumericAdvanced;
        private System.Windows.Forms.TableLayoutPanel importedMediaPanel;
        private System.Windows.Forms.TableLayoutPanel controlPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.Button closePanel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button videoTrackMove;
        private System.Windows.Forms.Button leftMove;
        private System.Windows.Forms.Button audioTrackMove;
        private System.Windows.Forms.Button rightMove;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button Split;
        private System.Windows.Forms.Button RemoveSegment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.CheckBox muteCheckBox;
        private System.Windows.Forms.CheckBox hideCheckBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown bitrateNumericAdvanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox codecComboBoxAdvanced;
        private System.Windows.Forms.NumericUpDown qualityCrfNumericAdvanced;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox formatComboBoxAdvanced;
        private System.Windows.Forms.ComboBox cpuUseComboBoxAdvanced;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox pixelFormatComboBoxAdvanced;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox encodingPresetComboBoxAdvanced;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown framerateNumericAdvanced;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.ComboBox aspectRatioComboBoxAdvanced;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TableLayoutPanel saveTemplatePanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.TextBox templateNameTextBox;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button cancelSaveBtn;
        private System.Windows.Forms.Button confirmSaveBtn;
        private System.Windows.Forms.Button removeTemplateBtn;
        private System.Windows.Forms.ComboBox fpsComboBoxPreview;
        private System.Windows.Forms.ComboBox heightComboBoxPreview;
        private System.Windows.Forms.ComboBox widthComboBoxPreview;
        private System.Windows.Forms.Button resyncBtn;
        private System.Windows.Forms.CheckBox fastPreviewCheckBox;
    }
}