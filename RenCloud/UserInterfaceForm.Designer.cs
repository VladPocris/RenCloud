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
            this.PauseButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.TimeStamp = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.Debug = new System.Windows.Forms.Button();
            this.TestGen = new System.Windows.Forms.Button();
            this.RemoveSegment = new System.Windows.Forms.Button();
            this.Split = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.PreviewPanel = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
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
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel8.SuspendLayout();
            this.AudioTrack.SuspendLayout();
            this.VideoTrack.SuspendLayout();
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
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
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
            this.panel5.Controls.Add(this.PauseButton);
            this.panel5.Controls.Add(this.PlayButton);
            this.panel5.Controls.Add(this.TimeStamp);
            this.panel5.Controls.Add(this.panel13);
            this.panel5.Controls.Add(this.button4);
            this.panel5.Controls.Add(this.PreviewPanel);
            this.panel5.Controls.Add(this.panel10);
            this.panel5.Location = new System.Drawing.Point(209, 58);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(895, 676);
            this.panel5.TabIndex = 2;
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(497, 438);
            this.PauseButton.Margin = new System.Windows.Forms.Padding(0);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(109, 38);
            this.PauseButton.TabIndex = 4;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(613, 438);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(0);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(109, 38);
            this.PlayButton.TabIndex = 3;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
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
            this.panel13.Controls.Add(this.Debug);
            this.panel13.Controls.Add(this.TestGen);
            this.panel13.Controls.Add(this.RemoveSegment);
            this.panel13.Controls.Add(this.Split);
            this.panel13.Location = new System.Drawing.Point(0, 305);
            this.panel13.Margin = new System.Windows.Forms.Padding(0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(419, 172);
            this.panel13.TabIndex = 2;
            // 
            // Debug
            // 
            this.Debug.Location = new System.Drawing.Point(22, 132);
            this.Debug.Name = "Debug";
            this.Debug.Size = new System.Drawing.Size(120, 31);
            this.Debug.TabIndex = 0;
            this.Debug.Text = "DataDebug";
            this.Debug.UseVisualStyleBackColor = true;
            this.Debug.Click += new System.EventHandler(this.Debug_Click);
            // 
            // TestGen
            // 
            this.TestGen.Location = new System.Drawing.Point(332, 132);
            this.TestGen.Margin = new System.Windows.Forms.Padding(0);
            this.TestGen.Name = "TestGen";
            this.TestGen.Size = new System.Drawing.Size(75, 32);
            this.TestGen.TabIndex = 2;
            this.TestGen.Text = "Generate";
            this.TestGen.UseVisualStyleBackColor = true;
            this.TestGen.Click += new System.EventHandler(this.TestGen_Click);
            // 
            // RemoveSegment
            // 
            this.RemoveSegment.Location = new System.Drawing.Point(243, 135);
            this.RemoveSegment.Margin = new System.Windows.Forms.Padding(0);
            this.RemoveSegment.Name = "RemoveSegment";
            this.RemoveSegment.Size = new System.Drawing.Size(75, 32);
            this.RemoveSegment.TabIndex = 1;
            this.RemoveSegment.Text = "Delete";
            this.RemoveSegment.UseVisualStyleBackColor = true;
            this.RemoveSegment.Click += new System.EventHandler(this.RemoveSegment_Click);
            // 
            // Split
            // 
            this.Split.Location = new System.Drawing.Point(158, 135);
            this.Split.Margin = new System.Windows.Forms.Padding(0);
            this.Split.Name = "Split";
            this.Split.Size = new System.Drawing.Size(75, 32);
            this.Split.TabIndex = 0;
            this.Split.Text = "Split";
            this.Split.UseVisualStyleBackColor = true;
            this.Split.Click += new System.EventHandler(this.Split_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(728, 438);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(164, 39);
            this.button4.TabIndex = 0;
            this.button4.Text = "TestAdd";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // PreviewPanel
            // 
            this.PreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PreviewPanel.Location = new System.Drawing.Point(419, 22);
            this.PreviewPanel.Margin = new System.Windows.Forms.Padding(0);
            this.PreviewPanel.Name = "PreviewPanel";
            this.PreviewPanel.Size = new System.Drawing.Size(476, 416);
            this.PreviewPanel.TabIndex = 2;
            // 
            // panel10
            // 
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.help_label);
            this.panel10.Controls.Add(this.controlPanel_label);
            this.panel10.Controls.Add(this.file_label);
            this.panel10.Location = new System.Drawing.Point(0, 1);
            this.panel10.Margin = new System.Windows.Forms.Padding(0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(895, 21);
            this.panel10.TabIndex = 0;
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
            this.help_label.Size = new System.Drawing.Size(62, 29);
            this.help_label.TabIndex = 6;
            this.help_label.Text = "Help";
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
            this.panel7.Location = new System.Drawing.Point(1, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(158, 199);
            this.panel7.TabIndex = 0;
            // 
            // panel11
            // 
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Location = new System.Drawing.Point(209, 80);
            this.panel11.Margin = new System.Windows.Forms.Padding(0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(419, 283);
            this.panel11.TabIndex = 1;
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
            this.panel13.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.AudioTrack.ResumeLayout(false);
            this.VideoTrack.ResumeLayout(false);
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
        private System.Windows.Forms.Button Split;
        private System.Windows.Forms.Button RemoveSegment;
        private System.Windows.Forms.Button TestGen;
        private System.Windows.Forms.Button Debug;
        private System.Windows.Forms.Label file_label;
        private System.Windows.Forms.Label help_label;
        private System.Windows.Forms.Label controlPanel_label;
    }
}