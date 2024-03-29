﻿#region BSD License
/*
 * Use of this source code is governed by a BSD-style
 * license that can be found in the LICENSE.md file or at
 * https://github.com/Wagnerp/Krypton-Toolkit-Suite-Extended-NET-5.461/blob/master/LICENSE
 *
 */
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ExtendedControls.ExtendedToolkit.Controls.Charting
{
    /// <summary>
    /// Aqua Gauge Control - A Windows User Control.
    /// Author  : Ambalavanar Thirugnanam, Angelo Cresta
    /// Date    : 24th August 2007, 30 june 2008
    /// email   : ambalavanar.thiru@gmail.com, angelo@angelonline.net
    /// This is control is for free. You can use for any commercial or non-commercial purposes.
    /// [Please do no remove this header when using this control in your application.]
    /// </summary>
    [ToolboxBitmap(typeof(Timer)), ToolboxItem(false)]
    public partial class AquaGauge : UserControl
    {
        #region Designer Code
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AquaGauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AquaGauge";
            this.ResumeLayout(false);

        }

        #endregion
        #endregion

        #region ... Private Attributes ...
        //private int oldWidth, oldHeight;
        private int x, y, width, height;
        private float fromAngle = 135F;
        private float toAngle = 405F;
        private bool requiresRedraw;
        private Image backgroundImg;
        private Rectangle rectImg;
        #endregion

        #region ... Constructor ...
        public AquaGauge()
        {
            InitializeComponent();
            x = 10;
            y = 10;
            width = this.Width - 15;
            height = this.Height - 15;
            this._noOfDivisions = 10;
            this._noOfSubDivisions = 3;
            _dialTextFont = this.Font;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.BackColor = Color.Transparent;
            //this.Resize += new EventHandler(AquaGauge_Resize);
            this.requiresRedraw = true;
        }
        #endregion

        #region ... Enum ...

        public enum HandTypes
        {
            Normal = 1,
            Medium = 2,
            Small = 3
        }
        public enum CenterPointSizes
        {
            Normal = 1,
            Medium = 2,
            Small = 3,
            Smaller = 4
        }
        public enum CenterPointTypes
        {
            Round = 1,
            Square = 2
        }


        #endregion

        #region ... Public Properties ...
        /// <summary>
        /// Minimun Size
        /// </summary>
        private int _leadingZeros = 0;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Leading Zeros")]
        public int LeadingZeros
        {
            get { return _leadingZeros; }
            set
            {
                _leadingZeros = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Minimun Size
        /// </summary>
        private int _miminumSize = 140;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Minimum Size")]
        public int MiminumSize
        {
            get { return _miminumSize; }
            set
            {
                _miminumSize = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Center Point Rotate Enable
        /// </summary>
        private bool _centerPointRotateEnable = true;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Center Point Type")]
        public bool CenterPointRotateEnable
        {
            get { return _centerPointRotateEnable; }
            set
            {
                _centerPointRotateEnable = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Center Point Size
        /// </summary>
        private CenterPointTypes _centerPointType = CenterPointTypes.Round;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Center Point Type")]
        public CenterPointTypes CenterPointType
        {
            get { return _centerPointType; }
            set
            {
                _centerPointType = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Center Point Size
        /// </summary>
        private CenterPointSizes _centerPointSize = CenterPointSizes.Normal;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Center Point Type")]
        public CenterPointSizes CenterPointSize
        {
            get { return _centerPointSize; }
            set
            {
                _centerPointSize = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Hand Type
        /// </summary>
        private HandTypes _handType = HandTypes.Normal;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Hand Type")]
        public HandTypes HandType
        {
            get { return _handType; }
            set
            {
                _handType = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Threshold Color
        /// </summary>
        private Color _thresholdColor = Color.LawnGreen;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color ThresholdColor
        {
            get { return _thresholdColor; }
            set
            {
                _thresholdColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Internal Rim Color
        /// </summary>
        private Color _internalRimColor = Color.Gainsboro;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color InternalRimColor
        {
            get { return _internalRimColor; }
            set
            {
                _internalRimColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// External Rim Color
        /// </summary>
        private Color _externalRimColor = Color.SlateGray;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color ExternalRimColor
        {
            get { return _externalRimColor; }
            set
            {
                _externalRimColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Dial Text Font
        /// </summary>
        private Font _dialTextFont;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Font DialTextFont
        {
            get { return _dialTextFont; }
            set
            {
                _dialTextFont = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Digit Color
        /// </summary>
        private Color _digitColor = Color.Black;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color DigitColor
        {
            get { return _digitColor; }
            set
            {
                _digitColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// CenterPoint Color
        /// </summary>
        private Color _centerPointColor = Color.Black;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color CenterPointColor
        {
            get { return _centerPointColor; }
            set
            {
                _centerPointColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// CenterPoint Color
        /// </summary>
        private Color _thinCalibrationColor = Color.Black;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color ThinCalibrationColor
        {
            get { return _thinCalibrationColor; }
            set
            {
                _thinCalibrationColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// CenterPoint Color
        /// </summary>
        private Color _thickCalibrationColor = Color.Black;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color ThickCalibrationColor
        {
            get { return _thickCalibrationColor; }
            set
            {
                _thickCalibrationColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Draw Hand over CenterPoint
        /// </summary>
        private bool _handOverCenterPoint = false;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Draw Hand over CenterPoint")]
        public bool HandOverCenterPoint
        {
            get { return _handOverCenterPoint; }
            set
            {
                _handOverCenterPoint = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Color of the Hand
        /// </summary>
        private Color _handColor = Color.Black;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color of the Hand")]
        public Color HandColor
        {
            get { return _handColor; }
            set
            {
                _handColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Digit Back Panel Color
        /// </summary>
        private Color _digitBackPanelColor = Color.Gray;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color Digit Back Panel")]
        public Color DigitBackPanelColor
        {
            get { return _digitBackPanelColor; }
            set
            {
                _digitBackPanelColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Digit Back Panel Border Color
        /// </summary>
        private Color _digitBackPanelBorderColor = Color.Gray;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Color Digit Back Panel")]
        public Color DigitBackPanelBorderColor
        {
            get { return _digitBackPanelBorderColor; }
            set
            {
                _digitBackPanelBorderColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Show Digit Back Panel
        /// </summary>
        private bool _digitBackPanelEnabled;
        [DefaultValue(true)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Show Digit BackPanel")]
        public bool DigitBackPanelEnabled
        {
            get { return _digitBackPanelEnabled; }
            set
            {
                _digitBackPanelEnabled = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Show Digit Back Panel Border
        /// </summary>
        private bool _digitBackPanelBorderEnabled;
        [DefaultValue(true)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Show Digit BackPanel Border")]
        public bool DigitBackPanelBorderEnabled
        {
            get { return _digitBackPanelBorderEnabled; }
            set
            {
                _digitBackPanelBorderEnabled = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Mininum value on the scale
        /// </summary>
        private float _minValue;
        [DefaultValue(0)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Mininum value on the scale")]
        public float MinValue
        {
            get { return _minValue; }
            set
            {
                if (value < _maxValue)
                {
                    _minValue = value;
                    if (_currentValue < _minValue)
                        _currentValue = _minValue;
                    if (_recommendedValue < _minValue)
                        _recommendedValue = _minValue;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Maximum value on the scale
        /// </summary>
        private float _maxValue = 100.0F;
        [DefaultValue(100)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Maximum value on the scale")]
        public float MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (value > _minValue)
                {
                    _maxValue = value;
                    if (_currentValue > _maxValue)
                        _currentValue = _maxValue;
                    if (_recommendedValue > _maxValue)
                        _recommendedValue = _maxValue;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }


        /// <summary>
        /// Gets or Sets the Threshold area from the Recommended Value. (1-99%)
        /// </summary>
        private float _threshold;
        [DefaultValue(25)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Gets or Sets the Threshold area from the Recommended Value. (1-99%)")]
        public float ThresholdPercent
        {
            get { return _threshold; }
            set
            {
                if (value > 0 && value < 100)
                {
                    _threshold = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Threshold value from which green area will be marked.
        /// </summary>
        private float _recommendedValue;
        [DefaultValue(25)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Threshold value from which green area will be marked.")]
        public float RecommendedValue
        {
            get { return _recommendedValue; }
            set
            {
                if (value > _minValue && value < _maxValue)
                {
                    _recommendedValue = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Value where the pointer will point to.
        /// </summary>
        private float _currentValue;
        [DefaultValue(0)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Value where the pointer will point to.")]
        public float Value
        {
            get { return _currentValue; }
            set
            {
                if (value >= _minValue && value <= _maxValue)
                {
                    _currentValue = value;
                    requiresRedraw = true;
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// Background color of the dial
        /// </summary>
        private Color _dialColor = Color.Lavender;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Background color of the dial")]
        public Color DialColor
        {
            get { return _dialColor; }
            set
            {
                _dialColor = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Glossiness strength. Range: 0-100
        /// </summary>
        private float _glossinessAlpha = 75;
        [DefaultValue(75)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Glossiness strength. Range: 0-100")]
        public float Glossiness
        {
            get
            {
                return (_glossinessAlpha * 100) / 220;
            }
            set
            {
                float val = value;
                if (val > 100)
                    value = 100;
                if (val < 0)
                    value = 0;
                _glossinessAlpha = (value * 220) / 100;
                this.Refresh();
            }
        }

        /// <summary>
        /// Get or Sets the number of Divisions in the dial scale.
        /// </summary>
        private int _noOfDivisions;
        [DefaultValue(10)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Get or Sets the number of Divisions in the dial scale.")]
        public int NoOfDivisions
        {
            get { return this._noOfDivisions; }
            set
            {
                if (value > 1 && value < 25)
                {
                    this._noOfDivisions = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the number of Sub Divisions in the scale per Division.
        /// </summary>
        private int _noOfSubDivisions;
        [DefaultValue(3)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Gets or Sets the number of Sub Divisions in the scale per Division.")]
        public int NoOfSubDivisions
        {
            get { return this._noOfSubDivisions; }
            set
            {
                if (value > 0 && value <= 10)
                {
                    this._noOfSubDivisions = value;
                    requiresRedraw = true;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Text to be displayed in the dial
        /// </summary>
        private string _dialText;
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Gets or Sets the Text to be displayed in the dial")]
        public string DialText
        {
            get { return this._dialText; }
            set
            {
                this._dialText = value;
                requiresRedraw = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Enables or Disables Transparent Background color.
        /// Note: Enabling this will reduce the performance and may make the control flicker.
        /// </summary>
        private bool _enableTransparentBackground;
        [DefaultValue(false)]
        [Browsable(true), Category("Appearance-Extended")]
        [Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
        public bool EnableTransparentBackground
        {
            get { return this._enableTransparentBackground; }
            set
            {
                this._enableTransparentBackground = value;
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, !_enableTransparentBackground);
                requiresRedraw = true;
                this.Refresh();
            }
        }
        #endregion

        #region ... Overriden Control methods ...
        /// <summary>
        /// Draws the pointer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            width = this.Width - x * 2;
            height = this.Height - y * 2;
            DrawPointer(e.Graphics, ((width) / 2) + x, ((height) / 2) + y);
        }

        /// <summary>
        /// Draws the dial background.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (!_enableTransparentBackground)
            {
                base.OnPaintBackground(e);
            }

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, Width, Height));
            if (backgroundImg == null || requiresRedraw)
            {
                backgroundImg = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(backgroundImg);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                width = this.Width - x * 2;
                height = this.Height - y * 2;
                rectImg = new Rectangle(x, y, width, height);

                //Draw background color
                Brush backGroundBrush = new SolidBrush(Color.FromArgb(120, _dialColor));
                if (_enableTransparentBackground && this.Parent != null)
                {
                    float gg = width / 60;
                    //g.FillEllipse(new SolidBrush(this.Parent.BackColor), -gg, -gg, this.Width+gg*2, this.Height+gg*2);
                }
                g.FillEllipse(backGroundBrush, x, y, width, height);

                //Draw Rim
                SolidBrush outlineBrush = new SolidBrush(Color.FromArgb(100, _externalRimColor));
                Pen outline = new Pen(outlineBrush, (float)(width * .03));
                g.DrawEllipse(outline, rectImg);
                Pen darkRim = new Pen(_externalRimColor);
                g.DrawEllipse(darkRim, x, y, width, height);

                //Draw Callibration
                DrawCalibration(g, rectImg, ((width) / 2) + x, ((height) / 2) + y);

                //Draw Colored Rim
                Pen colorPen = new Pen(Color.FromArgb(190, _internalRimColor), this.Width / 40);
                Pen blackPen = new Pen(Color.FromArgb(250, Color.Black), this.Width / 200);
                int gap = (int)(this.Width * 0.03F);
                Rectangle rectg = new Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2);
                g.DrawArc(colorPen, rectg, 135, 270);

                //Draw Threshold
                colorPen = new Pen(Color.FromArgb(200, _thresholdColor), this.Width / 50);
                rectg = new Rectangle(rectImg.X + gap, rectImg.Y + gap, rectImg.Width - gap * 2, rectImg.Height - gap * 2);
                float val = MaxValue - MinValue;
                val = (100 * (this._recommendedValue - MinValue)) / val;
                val = ((toAngle - fromAngle) * val) / 100;
                val += fromAngle;
                float stAngle = val - ((270 * _threshold) / 200);
                if (stAngle <= 135) stAngle = 135;
                float sweepAngle = ((270 * _threshold) / 100);
                if (stAngle + sweepAngle > 405) sweepAngle = 405 - stAngle;
                g.DrawArc(colorPen, rectg, stAngle, sweepAngle);

                //Draw Digital Value
                RectangleF digiRect = new RectangleF((float)this.Width / 2F - (float)this.width / 5F, (float)this.height / 1.2F, (float)this.width / 2.5F, (float)this.Height / 9F);
                RectangleF digiFRect = new RectangleF(this.Width / 2 - this.width / 7, (int)(this.height / 1.18), this.width / 4, this.Height / 12);

                //DigitBackPanel 
                if (_digitBackPanelEnabled)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(90, _digitBackPanelColor)), digiRect);
                    if (_digitBackPanelBorderEnabled)
                    {
                        g.DrawRectangle(new Pen(Color.FromArgb(120, _digitBackPanelBorderColor)), digiRect.X, digiRect.Y, digiRect.Width, digiRect.Height);
                    }
                }
                DisplayNumber(g, this._currentValue, digiFRect);

                //draw Text
                g.SmoothingMode = SmoothingMode.AntiAlias;
                SizeF textSize = g.MeasureString(this._dialText, _dialTextFont);
                RectangleF digiFRectText = new RectangleF(this.Width / 2 - textSize.Width / 2, (int)(this.height / 1.5), textSize.Width, textSize.Height);
                g.DrawString(_dialText, _dialTextFont, new SolidBrush(this.ForeColor), digiFRectText);
                requiresRedraw = false;
            }
            e.Graphics.DrawImage(backgroundImg, rectImg);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AquaGauge_Resize();
            requiresRedraw = true;
            this.Refresh();
        }
        #endregion

        #region ... Private methods ...
        /// <summary>
        /// Draws the Pointer.
        /// </summary>
        /// <param name="gr"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        private void DrawPointer(Graphics gr, int cx, int cy)
        {
            float radius = this.Width / 2 - (this.Width * .12F);
            float val = MaxValue - MinValue;

            Image img = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(img);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            val = (100 * (this._currentValue - MinValue)) / val;
            val = ((toAngle - fromAngle) * val) / 100;
            val += fromAngle;


            //draw hand over CenterPoint
            if (_handOverCenterPoint)
            {
                //Draw Center Point
                Rectangle rect = new Rectangle(x, y, width, height);
                DrawCenterPoint(g, rect, ((width) / 2) + x, ((height) / 2) + y, val);
            }



            float angle = GetRadian(val);
            float gradientAngle = angle;

            PointF[] pts = new PointF[5];

            double AngleOffset = 0.02;
            float PositionOffset = 0.09F;

            switch (_handType)
            {
                case HandTypes.Normal:
                    AngleOffset = 0.02;
                    PositionOffset = 0.09F;
                    break;
                case HandTypes.Medium:
                    AngleOffset = 0.01;
                    PositionOffset = 0.045F;
                    break;
                case HandTypes.Small:
                    AngleOffset = 0.001;
                    PositionOffset = 0.01F;
                    break;
            }


            pts[0].X = (float)(cx + radius * Math.Cos(angle));
            pts[0].Y = (float)(cy + radius * Math.Sin(angle));

            pts[4].X = (float)(cx + radius * Math.Cos(angle - AngleOffset));
            pts[4].Y = (float)(cy + radius * Math.Sin(angle - AngleOffset));

            angle = GetRadian((val + 20));
            pts[1].X = (float)(cx + (this.Width * PositionOffset) * Math.Cos(angle));
            pts[1].Y = (float)(cy + (this.Width * PositionOffset) * Math.Sin(angle));

            pts[2].X = cx;
            pts[2].Y = cy;

            angle = GetRadian((val - 20));
            pts[3].X = (float)(cx + (this.Width * PositionOffset) * Math.Cos(angle));
            pts[3].Y = (float)(cy + (this.Width * PositionOffset) * Math.Sin(angle));

            //hand part (bottom)
            Brush pointer = new SolidBrush(_handColor);
            g.FillPolygon(pointer, pts);

            PointF[] shinePts = new PointF[3];
            angle = GetRadian(val);
            shinePts[0].X = (float)(cx + radius * Math.Cos(angle));
            shinePts[0].Y = (float)(cy + radius * Math.Sin(angle));

            angle = GetRadian(val + 20);
            shinePts[1].X = (float)(cx + (this.Width * PositionOffset) * Math.Cos(angle));
            shinePts[1].Y = (float)(cy + (this.Width * PositionOffset) * Math.Sin(angle));

            shinePts[2].X = cx;
            shinePts[2].Y = cy;

            //hand part top
            LinearGradientBrush gpointer = new LinearGradientBrush(shinePts[0], shinePts[2], Color.SlateGray, _handColor);
            g.FillPolygon(gpointer, shinePts);

            //draw CenterPoint over hand
            if (!_handOverCenterPoint)
            {
                //Draw Center Point
                Rectangle rect = new Rectangle(x, y, width, height);
                DrawCenterPoint(g, rect, ((width) / 2) + x, ((height) / 2) + y, val);
            }

            //Draw Gloss
            DrawGloss(g);

            //  Draw image
            gr.DrawImage(img, 0, 0);
        }

        /// <summary>
        /// Draws the glossiness.
        /// </summary>
        /// <param name="g"></param>
        private void DrawGloss(Graphics g)
        {
            RectangleF glossRect = new RectangleF(
               x + (float)(width * 0.10),
               y + (float)(height * 0.07),
               (float)(width * 0.80),
               (float)(height * 0.7));
            LinearGradientBrush gradientBrush =
                new LinearGradientBrush(glossRect,
                Color.FromArgb((int)_glossinessAlpha, Color.White),
                Color.Transparent,
                LinearGradientMode.Vertical);
            g.FillEllipse(gradientBrush, glossRect);

            //TODO: Gradient from bottom
            glossRect = new RectangleF(
               x + (float)(width * 0.25),
               y + (float)(height * 0.77),
               (float)(width * 0.50),
               (float)(height * 0.2));
            int gloss = (int)(_glossinessAlpha / 3);
            gradientBrush =
                new LinearGradientBrush(glossRect,
                Color.Transparent, Color.FromArgb(gloss, this.BackColor),
                LinearGradientMode.Vertical);
            g.FillEllipse(gradientBrush, glossRect);

            gradientBrush.Dispose();
        }

        /// <summary>
        /// Draws the center point.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cX"></param>
        /// <param name="cY"></param>
        private void DrawCenterPoint(Graphics g, Rectangle rect, int cX, int cY, float angle)
        {

            // try rotate the Center
            // Create new matrix
            Matrix mx = new Matrix();
            // Set the point
            Point pnt = new Point(cX, cY);

            GraphicsState State;
            //save state
            State = g.Save();

            if (_centerPointRotateEnable)
            {
                // Rotate angle degrees around a point
                mx.RotateAt(angle, pnt);
                // Apply the matrix to the graphic object
                g.Transform = mx;
            }

            float shiftBig = Width / 7;
            float shift = Width / 9;

            switch (_centerPointSize)
            {
                case CenterPointSizes.Normal:
                    shiftBig = Width / 7;
                    shift = Width / 9;
                    break;
                case CenterPointSizes.Medium:
                    shiftBig = Width / 9;
                    shift = Width / 12;
                    break;
                case CenterPointSizes.Small:
                    shiftBig = Width / 11;
                    shift = Width / 14;
                    break;
                case CenterPointSizes.Smaller:
                    shiftBig = Width / 20;
                    shift = Width / 30;
                    break;

            }

            //Outher
            RectangleF rectangleBig = new RectangleF(cX - (shiftBig / 2), cY - (shiftBig / 2), shiftBig, shiftBig);
            LinearGradientBrush brushBig = new LinearGradientBrush(rect, _centerPointColor, Color.FromArgb(100, this._dialColor), LinearGradientMode.Vertical);

            //inner
            RectangleF rectangle = new RectangleF(cX - (shift / 2), cY - (shift / 2), shift, shift);
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.SlateGray, _centerPointColor, LinearGradientMode.ForwardDiagonal);

            //CenterPoint Type
            switch (_centerPointType)
            {
                case CenterPointTypes.Round:
                    g.FillEllipse(brushBig, rectangleBig);
                    g.FillEllipse(brush, rectangle);
                    break;
                case CenterPointTypes.Square:
                    g.FillRectangle(brushBig, rectangleBig);
                    g.FillRectangle(brush, rectangle);
                    break;
            }


            // try rotate the Center back
            if (_centerPointRotateEnable)
            {
                // Rotate -angle degrees around a point
                mx.Invert();
                // Apply the matrix to the graphic object
                mx.RotateAt(-angle, pnt);
                //restore 
                g.Restore(State);
            }

            //dispose objects
            brush.Dispose();
            brushBig.Dispose();
        }

        /// <summary>
        /// Draws the Ruler
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cX"></param>
        /// <param name="cY"></param>
        private void DrawCalibration(Graphics g, Rectangle rect, int cX, int cY)
        {
            int noOfParts = this._noOfDivisions + 1;
            int noOfIntermediates = this._noOfSubDivisions;
            float currentAngle = GetRadian(fromAngle);
            int gap = (int)(this.Width * 0.01F);
            float shift = this.Width / 25;
            Rectangle rectangle = new Rectangle(rect.Left + gap, rect.Top + gap, rect.Width - gap, rect.Height - gap);

            float x, y, x1, y1, tx, ty, radius;
            radius = rectangle.Width / 2 - gap * 5;
            float totalAngle = toAngle - fromAngle;
            float incr = GetRadian(((totalAngle) / ((noOfParts - 1) * (noOfIntermediates + 1))));

            Pen thickPen = new Pen(_thickCalibrationColor, Width / 50);
            Pen thinPen = new Pen(_thinCalibrationColor, Width / 100);
            float rulerValue = MinValue;
            for (int i = 0; i <= noOfParts; i++)
            {
                //Draw Thick Line
                x = (float)(cX + radius * Math.Cos(currentAngle));
                y = (float)(cY + radius * Math.Sin(currentAngle));
                x1 = (float)(cX + (radius - Width / 20) * Math.Cos(currentAngle));
                y1 = (float)(cY + (radius - Width / 20) * Math.Sin(currentAngle));
                g.DrawLine(thickPen, x, y, x1, y1);

                //Draw Strings
                StringFormat format = new StringFormat();
                tx = (float)(cX + (radius - Width / 10) * Math.Cos(currentAngle));
                ty = (float)(cY - shift + (radius - Width / 10) * Math.Sin(currentAngle));
                Brush stringPen = new SolidBrush(this.ForeColor);
                StringFormat strFormat = new StringFormat(StringFormatFlags.NoClip);
                strFormat.Alignment = StringAlignment.Center;
                Font f = new Font(this.Font.FontFamily, (float)(this.Width / 23), this.Font.Style);
                g.DrawString(rulerValue.ToString() + "", f, stringPen, new PointF(tx, ty), strFormat);
                rulerValue += (float)((MaxValue - MinValue) / (noOfParts - 1));
                rulerValue = (float)Math.Round(rulerValue, 2);

                //currentAngle += incr;
                if (i == noOfParts - 1)
                    break;
                for (int j = 0; j <= noOfIntermediates; j++)
                {
                    //Draw thin lines 
                    currentAngle += incr;
                    x = (float)(cX + radius * Math.Cos(currentAngle));
                    y = (float)(cY + radius * Math.Sin(currentAngle));
                    x1 = (float)(cX + (radius - Width / 50) * Math.Cos(currentAngle));
                    y1 = (float)(cY + (radius - Width / 50) * Math.Sin(currentAngle));
                    g.DrawLine(thinPen, x, y, x1, y1);
                }
            }
        }

        /// <summary>
        /// Converts the given degree to radian.
        /// </summary>
        /// <param name="theta"></param>
        /// <returns></returns>
        public float GetRadian(float theta)
        {
            return theta * (float)Math.PI / 180F;
        }

        /// <summary>
        /// Displays the given number in the 7-Segement format.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="number"></param>
        /// <param name="drect"></param>
        private void DisplayNumber(Graphics g, float number, RectangleF drect)
        {
            try
            {

                /*
                float shift = 0;
                string num = number.ToString("000.00");
                num.PadLeft(3, '0');
                */
                //
                // The minimum string length is 4 characters:
                // "0.00". If the user wants leading zeros,
                // prepend to acheive the desired width.
                // The decimal point doesn't contribute to the width.
                string num = number.ToString("0.00");
                float shift = 0;

                while ((num.Length - 1) < _leadingZeros)
                {
                    num = "0" + num;
                }

                /* test on measurement
                //SizeF textSize = g.MeasureString(num, this.Font);
                float numLenght = num.Length * 10F * drect.Height / 13;
                float Xoffset = (width - numLenght) / 4;
                drect.Offset(-Xoffset, 0);
                */

                // If the width is less than the original value,
                // adjust the horizontal position to center.
                if ((num.Length - 1) < 5)
                {
                    shift = (5 - num.Length + 1) / 2 * (width / 17);
                }
                else
                {
                    shift -= width / 17;
                }
                //

                bool drawDPS = false;
                char[] chars = num.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    char c = chars[i];
                    if (i < chars.Length - 1 && chars[i + 1] == '.')
                        drawDPS = true;
                    else
                        drawDPS = false;
                    if (c != '.')
                    {
                        if (c == '-')
                        {
                            DrawDigit(g, -1, new PointF(drect.X + shift, drect.Y), drawDPS, drect.Height);
                        }
                        else
                        {
                            DrawDigit(g, Int16.Parse(c.ToString()), new PointF(drect.X + shift, drect.Y), drawDPS, drect.Height);
                        }
                        shift += 15 * this.width / 250;
                    }
                    else
                    {
                        shift += 2 * this.width / 250;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Draws a digit in 7-Segement format.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="number"></param>
        /// <param name="position"></param>
        /// <param name="dp"></param>
        /// <param name="height"></param>
        private void DrawDigit(Graphics g, int number, PointF position, bool dp, float height)
        {
            float width;
            width = 10F * height / 13;

            Pen outline = new Pen(Color.FromArgb(40, this._dialColor));
            Pen fillPen = new Pen(_digitColor);

            #region Form Polygon Points
            //Segment A
            PointF[] segmentA = new PointF[5];
            segmentA[0] = segmentA[4] = new PointF(position.X + GetX(2.8F, width), position.Y + GetY(1F, height));
            segmentA[1] = new PointF(position.X + GetX(10, width), position.Y + GetY(1F, height));
            segmentA[2] = new PointF(position.X + GetX(8.8F, width), position.Y + GetY(2F, height));
            segmentA[3] = new PointF(position.X + GetX(3.8F, width), position.Y + GetY(2F, height));

            //Segment B
            PointF[] segmentB = new PointF[5];
            segmentB[0] = segmentB[4] = new PointF(position.X + GetX(10, width), position.Y + GetY(1.4F, height));
            segmentB[1] = new PointF(position.X + GetX(9.3F, width), position.Y + GetY(6.8F, height));
            segmentB[2] = new PointF(position.X + GetX(8.4F, width), position.Y + GetY(6.4F, height));
            segmentB[3] = new PointF(position.X + GetX(9F, width), position.Y + GetY(2.2F, height));

            //Segment C
            PointF[] segmentC = new PointF[5];
            segmentC[0] = segmentC[4] = new PointF(position.X + GetX(9.2F, width), position.Y + GetY(7.2F, height));
            segmentC[1] = new PointF(position.X + GetX(8.7F, width), position.Y + GetY(12.7F, height));
            segmentC[2] = new PointF(position.X + GetX(7.6F, width), position.Y + GetY(11.9F, height));
            segmentC[3] = new PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.7F, height));

            //Segment D
            PointF[] segmentD = new PointF[5];
            segmentD[0] = segmentD[4] = new PointF(position.X + GetX(7.4F, width), position.Y + GetY(12.1F, height));
            segmentD[1] = new PointF(position.X + GetX(8.4F, width), position.Y + GetY(13F, height));
            segmentD[2] = new PointF(position.X + GetX(1.3F, width), position.Y + GetY(13F, height));
            segmentD[3] = new PointF(position.X + GetX(2.2F, width), position.Y + GetY(12.1F, height));

            //Segment E
            PointF[] segmentE = new PointF[5];
            segmentE[0] = segmentE[4] = new PointF(position.X + GetX(2.2F, width), position.Y + GetY(11.8F, height));
            segmentE[1] = new PointF(position.X + GetX(1F, width), position.Y + GetY(12.7F, height));
            segmentE[2] = new PointF(position.X + GetX(1.7F, width), position.Y + GetY(7.2F, height));
            segmentE[3] = new PointF(position.X + GetX(2.8F, width), position.Y + GetY(7.7F, height));

            //Segment F
            PointF[] segmentF = new PointF[5];
            segmentF[0] = segmentF[4] = new PointF(position.X + GetX(3F, width), position.Y + GetY(6.4F, height));
            segmentF[1] = new PointF(position.X + GetX(1.8F, width), position.Y + GetY(6.8F, height));
            segmentF[2] = new PointF(position.X + GetX(2.6F, width), position.Y + GetY(1.3F, height));
            segmentF[3] = new PointF(position.X + GetX(3.6F, width), position.Y + GetY(2.2F, height));

            //Segment G
            PointF[] segmentG = new PointF[7];
            segmentG[0] = segmentG[6] = new PointF(position.X + GetX(2F, width), position.Y + GetY(7F, height));
            segmentG[1] = new PointF(position.X + GetX(3.1F, width), position.Y + GetY(6.5F, height));
            segmentG[2] = new PointF(position.X + GetX(8.3F, width), position.Y + GetY(6.5F, height));
            segmentG[3] = new PointF(position.X + GetX(9F, width), position.Y + GetY(7F, height));
            segmentG[4] = new PointF(position.X + GetX(8.2F, width), position.Y + GetY(7.5F, height));
            segmentG[5] = new PointF(position.X + GetX(2.9F, width), position.Y + GetY(7.5F, height));

            //Segment DP
            #endregion

            #region Draw Segments Outline
            g.FillPolygon(outline.Brush, segmentA);
            g.FillPolygon(outline.Brush, segmentB);
            g.FillPolygon(outline.Brush, segmentC);
            g.FillPolygon(outline.Brush, segmentD);
            g.FillPolygon(outline.Brush, segmentE);
            g.FillPolygon(outline.Brush, segmentF);
            g.FillPolygon(outline.Brush, segmentG);
            #endregion

            #region Fill Segments
            //Fill SegmentA
            if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentA);
            }

            //Fill SegmentB
            if (IsNumberAvailable(number, 0, 1, 2, 3, 4, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentB);
            }

            //Fill SegmentC
            if (IsNumberAvailable(number, 0, 1, 3, 4, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentC);
            }

            //Fill SegmentD
            if (IsNumberAvailable(number, 0, 2, 3, 5, 6, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentD);
            }

            //Fill SegmentE
            if (IsNumberAvailable(number, 0, 2, 6, 8))
            {
                g.FillPolygon(fillPen.Brush, segmentE);
            }

            //Fill SegmentF
            if (IsNumberAvailable(number, 0, 4, 5, 6, 7, 8, 9))
            {
                g.FillPolygon(fillPen.Brush, segmentF);
            }

            //Fill SegmentG
            if (IsNumberAvailable(number, 2, 3, 4, 5, 6, 8, 9, -1))
            {
                g.FillPolygon(fillPen.Brush, segmentG);
            }
            #endregion

            //Draw decimal point
            if (dp)
            {
                g.FillEllipse(fillPen.Brush, new RectangleF(
                    position.X + GetX(10F, width),
                    position.Y + GetY(12F, height),
                    width / 7,
                    width / 7));
            }
        }

        /// <summary>
        /// Gets Relative X for the given width to draw digit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private float GetX(float x, float width)
        {
            return x * width / 12;
        }

        /// <summary>
        /// Gets relative Y for the given height to draw digit
        /// </summary>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private float GetY(float y, float height)
        {
            return y * height / 15;
        }

        /// <summary>
        /// Returns true if a given number is available in the given list.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="listOfNumbers"></param>
        /// <returns></returns>
        private bool IsNumberAvailable(int number, params int[] listOfNumbers)
        {
            if (listOfNumbers.Length > 0)
            {
                foreach (int i in listOfNumbers)
                {
                    if (i == number)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Restricts the size to make sure the height and width are always same.
        /// </summary>
        private void AquaGauge_Resize()
        {
            if ((this.Width < _miminumSize) || (this.Width < _miminumSize))
            {
                this.Width = _miminumSize;
                this.Height = _miminumSize;
            }

            if (this.Width > this.Height)
            {
                this.Width = this.Height;
                this.width = this.height;
            }
            if (this.Height > this.Width)
            {
                this.Height = this.Width;
                this.height = this.width;
            }

            /*
            if (oldWidth != this.Width)
            {
                this.Height = this.Width;
                oldHeight = this.Width;
            }
            if (oldHeight != this.Height)
            {
                this.Width = this.Height;
                oldWidth = this.Width;
            }
            */
        }
        #endregion
    }
}