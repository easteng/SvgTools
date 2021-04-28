namespace SVGEditor2
{
    partial class WorkSpaceControlBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkSpaceControlBox));
            this.trackBarZoom = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_zoomout = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.button_noZoom = new System.Windows.Forms.Button();
            this.button_zoomin = new System.Windows.Forms.Button();
            this.label_Zoom = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_minorGrids = new System.Windows.Forms.NumericUpDown();
            this.checkBox_Grid = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_description = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minorGrids)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarZoom
            // 
            this.trackBarZoom.Location = new System.Drawing.Point(6, 18);
            this.trackBarZoom.Minimum = 1;
            this.trackBarZoom.Name = "trackBarZoom";
            this.trackBarZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarZoom.Size = new System.Drawing.Size(45, 154);
            this.trackBarZoom.TabIndex = 0;
            this.trackBarZoom.Value = 1;
            this.trackBarZoom.ValueChanged += new System.EventHandler(this.TrackBarZoomValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_zoomout);
            this.groupBox1.Controls.Add(this.button_noZoom);
            this.groupBox1.Controls.Add(this.button_zoomin);
            this.groupBox1.Controls.Add(this.label_Zoom);
            this.groupBox1.Controls.Add(this.trackBarZoom);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(106, 178);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "缩放";
            // 
            // button_zoomout
            // 
            this.button_zoomout.ImageKey = "zoom_out32x32.ico";
            this.button_zoomout.ImageList = this.imageList;
            this.button_zoomout.Location = new System.Drawing.Point(57, 59);
            this.button_zoomout.Name = "button_zoomout";
            this.button_zoomout.Size = new System.Drawing.Size(40, 34);
            this.button_zoomout.TabIndex = 2;
            this.button_zoomout.UseVisualStyleBackColor = true;
            this.button_zoomout.Click += new System.EventHandler(this.ButtonZoomoutClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "zoom_in32x32.ico");
            this.imageList.Images.SetKeyName(1, "zoom_out32x32.ico");
            this.imageList.Images.SetKeyName(2, "minimize.png");
            // 
            // button_noZoom
            // 
            this.button_noZoom.ImageKey = "minimize.png";
            this.button_noZoom.ImageList = this.imageList;
            this.button_noZoom.Location = new System.Drawing.Point(57, 99);
            this.button_noZoom.Name = "button_noZoom";
            this.button_noZoom.Size = new System.Drawing.Size(40, 34);
            this.button_noZoom.TabIndex = 2;
            this.button_noZoom.UseVisualStyleBackColor = true;
            this.button_noZoom.Click += new System.EventHandler(this.ButtonNoZoomClick);
            // 
            // button_zoomin
            // 
            this.button_zoomin.ImageKey = "zoom_in32x32.ico";
            this.button_zoomin.ImageList = this.imageList;
            this.button_zoomin.Location = new System.Drawing.Point(57, 20);
            this.button_zoomin.Name = "button_zoomin";
            this.button_zoomin.Size = new System.Drawing.Size(40, 34);
            this.button_zoomin.TabIndex = 2;
            this.button_zoomin.UseVisualStyleBackColor = true;
            this.button_zoomin.Click += new System.EventHandler(this.ButtonZoominClick);
            // 
            // label_Zoom
            // 
            this.label_Zoom.AutoSize = true;
            this.label_Zoom.Location = new System.Drawing.Point(62, 145);
            this.label_Zoom.Name = "label_Zoom";
            this.label_Zoom.Size = new System.Drawing.Size(17, 12);
            this.label_Zoom.TabIndex = 1;
            this.label_Zoom.Text = "1X";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDown_minorGrids);
            this.groupBox2.Controls.Add(this.checkBox_Grid);
            this.groupBox2.Location = new System.Drawing.Point(15, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(103, 67);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "方格";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Divisions";
            // 
            // numericUpDown_minorGrids
            // 
            this.numericUpDown_minorGrids.Location = new System.Drawing.Point(4, 39);
            this.numericUpDown_minorGrids.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDown_minorGrids.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_minorGrids.Name = "numericUpDown_minorGrids";
            this.numericUpDown_minorGrids.Size = new System.Drawing.Size(33, 21);
            this.numericUpDown_minorGrids.TabIndex = 1;
            this.numericUpDown_minorGrids.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_minorGrids.ValueChanged += new System.EventHandler(this.NumericUpDownMinorGridsValueChanged);
            // 
            // checkBox_Grid
            // 
            this.checkBox_Grid.AutoSize = true;
            this.checkBox_Grid.Location = new System.Drawing.Point(4, 18);
            this.checkBox_Grid.Name = "checkBox_Grid";
            this.checkBox_Grid.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Grid.TabIndex = 0;
            this.checkBox_Grid.Text = "显示格子";
            this.checkBox_Grid.UseVisualStyleBackColor = true;
            this.checkBox_Grid.CheckedChanged += new System.EventHandler(this.CheckBoxGridCheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox_description);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.numHeight);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.numWidth);
            this.groupBox3.Location = new System.Drawing.Point(17, 286);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(100, 134);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工作空间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "描述";
            // 
            // textBox_description
            // 
            this.textBox_description.Location = new System.Drawing.Point(3, 90);
            this.textBox_description.Name = "textBox_description";
            this.textBox_description.Size = new System.Drawing.Size(89, 21);
            this.textBox_description.TabIndex = 9;
            this.textBox_description.TextChanged += new System.EventHandler(this.textBox_description_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Height";
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(44, 50);
            this.numHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(50, 21);
            this.numHeight.TabIndex = 7;
            this.numHeight.ValueChanged += new System.EventHandler(this.NumHeightValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(44, 25);
            this.numWidth.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(50, 21);
            this.numWidth.TabIndex = 5;
            this.numWidth.ValueChanged += new System.EventHandler(this.NumWidthValueChanged);
            // 
            // WorkSpaceControlBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(145, 479);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorkSpaceControlBox";
            this.Text = "控制";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minorGrids)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarZoom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_Zoom;
        private System.Windows.Forms.Button button_zoomout;
        private System.Windows.Forms.Button button_zoomin;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_Grid;
        private System.Windows.Forms.Button button_noZoom;
        private System.Windows.Forms.NumericUpDown numericUpDown_minorGrids;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_description;
    }
}