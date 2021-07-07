using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace GorselProgramlamaFinal2
{
    public class MyCircularProgressBar : Control
    {
  
        //PRİVATE DEĞİŞKENLER

        /*İlerleme çubuğu ilk oluşturulduğunda varsayılan olarak gelecek özelliklerin değişkenleri.*/
        private long _Value;
        private long _Maximum = 100;
        private int _LineWitdh;
        private float _BarWidth = 13f;
        private Color _ProgressColor = Color.FromArgb(52, 229, 230); 
        private Color _LineColor;


        //KURUCU METOD

        public MyCircularProgressBar()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = SystemColors.Control;
            this.ForeColor = Color.DimGray;
            this.Size = new Size(130, 130);
            this.Font = new Font("Segoe UI", 15);
            this.MinimumSize = new Size(100, 100);
            this.LineWidth = 4;
            this.LineColor = Color.DimGray;
            Value = 71;
        }


        //DEĞİŞKENLER

        /// <summary>
        /// Progressbar ilerleme değerini tutar.
        /// </summary>
        public long Value
        {
            get 
            {
                return _Value; 
            }
            set
            {
                if (value > _Maximum)
                    value = _Maximum;
                _Value = value;
                Invalidate();
            }
        }

        /// <summary>
        /// İlerleme çubuğunun Maksimum Değerini Alır veya Ayarlar.
        /// </summary>
        public long Maximum
        {
            get 
            {
                return _Maximum;
            }
            set
            {
                if (value < 1)
                    value = 1;
                _Maximum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// İlerleme Çubuğunun Rengini Ayarlar.
        /// </summary>
        public Color BarColor
        {
            get 
            { 
                return _ProgressColor; 
            }
            set
            {
                _ProgressColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// İlerleme Çubuğu Genişliğini Ayarlar.
        /// </summary>
        public float BarWidth
        {
            get 
            { 
                return _BarWidth; 
            }
            set
            {
                _BarWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// İlerleme Çubuğunda %100'e tamamlanmaya Kalan Kısmın Rengi.
        /// </summary>
        public Color LineColor
        {
            get 
            { 
                return _LineColor; 
            }
            set
            {
                _LineColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// İlerleme Çubuğunda %100'e tamamlanmaya Kalan Kısmın Genişliği.
        /// </summary>
        public int LineWidth
        {
            get 
            { 
                return _LineWitdh; 
            }
            set
            {
                _LineWitdh = value;
                Invalidate();
            }
        }

        //EVENTARGS

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetStandardSize();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetStandardSize();
        }

        protected override void OnPaintBackground(PaintEventArgs p)
        {
            base.OnPaintBackground(p);
        }

        //METHODS

        /// <summary>
        /// İlerleme çubuğu kullanılırken veya deneme esnasında değerler artarken veya azalırken araya bekleme süresi ekler.
        /// </summary>
        /// <param name="time">Beklenecek süre</param>
        public void Wait(int time)
        {
            Thread thread = new Thread(delegate ()
            {
                Thread.Sleep(time);
            });
            thread.Start();
            while (thread.IsAlive)
                Application.DoEvents();
        }

        private void SetStandardSize()
        {
            int _Size = Math.Max(Width, Height);
            Size = new Size(_Size, _Size);
        }

        /// <summary>
        /// İlerleme çubuğunda değere sayı eklenmesini sağlar.
        /// </summary>
        /// <param name="Val">İlerleme çubuğuna eklenecek değer.</param>
        public void Increment(int Val)
        {
            this._Value += Val;
            Invalidate();
        }

        /// <summary>
        /// İlerleme çubuğunda değerden sayı çıkarılmasını sağlar.
        /// </summary>
        /// <param name="Val">İlerleme çubuğundan çıkarılacak değer.</param>
        public void Decrement(int Val)
        {
            this._Value -= Val;
            Invalidate();
        }

        
        //EVENTS

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Bitmap bitmap = new Bitmap(this.Width, this.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    PaintTransparentBackground(this, e);

                    //İç beyaz daireyi doldurur:
                    using (Brush mBackColor = new SolidBrush(this.BackColor))
                    {
                        graphics.FillEllipse(mBackColor,
                                18, 18,
                                (this.Width - 36),
                                (this.Height - 36));
                    }
                    // İç beyaz daireyi çizer:
                    using (Pen pen2 = new Pen(LineColor, this.LineWidth))
                    {
                        graphics.DrawEllipse(pen2, 18, 18, (this.Width - 36) , (this.Height - 36));
                    }
  
                    using (Pen pen = new Pen(BarColor , this.BarWidth))
                    {   
                    
                                pen.StartCap = LineCap.Flat;
                                pen.EndCap = LineCap.Flat;
                               
                        //Asıl ilerleme çubuğu burada çizilir
                        graphics.DrawArc(pen,
                            18, 18,
                            (this.Width - 37),
                            (this.Height - 37),
                            -90,
                            (int)Math.Round((double)((360.0 / ((double)this._Maximum)) * this._Value)));
                    }
                    
                    this.Text = Convert.ToString(Convert.ToInt32((100 / _Maximum) * _Value));


                    if (this.Text != string.Empty)
                    {
                        using (Brush FontColor = new SolidBrush(this.ForeColor))
                        {
                            SizeF MS = graphics.MeasureString(this.Text, this.Font);
                            SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, this.ForeColor));
                            graphics.DrawString(this.Text, this.Font, FontColor,
                                Convert.ToInt32(Width / 2 - MS.Width / 2),
                                Convert.ToInt32(Height / 2 - MS.Height / 2));
                        }
                    }

                    //Burada tüm Kontrol çizilir:
                    e.Graphics.DrawImage(bitmap, 0, 0);
                    graphics.Dispose();
                    bitmap.Dispose();
                }
            }
        }

        private static void PaintTransparentBackground(Control c, PaintEventArgs e)
        {
            if (c.Parent == null || !Application.RenderWithVisualStyles)
                return;

            ButtonRenderer.DrawParentBackground(e.Graphics, c.ClientRectangle, c);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
