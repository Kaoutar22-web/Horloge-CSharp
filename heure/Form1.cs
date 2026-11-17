
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace heure
{
    public partial class Form1 : Form
    {
        Graphics graphics;
        Bitmap bitmap;

        Point O_P;
        Point CLK_P;

        int CLK_Radius;
        bool SHOW_HIDE_F = true;

        public Form1()
        {
            InitializeComponent();

            CLK_Radius = 300;

            O_P = new Point(100, 100);

            CLK_P = new Point(
                O_P.X + CLK_Radius,
                O_P.Y + CLK_Radius
            );

            bitmap = new Bitmap(800, 800);

            graphics = Graphics.FromImage(bitmap);

            timer1.Interval = 1000;
            timer1.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        // تحديث الساعة كل ثانية
        private void timer1_Tick(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // رسم الساعة
            Draw_Clock();

            // رسم علامات الدقائق والساعات
            DRAW_M_Lines();

            // رسم الأرقام
            Draw_hour_Numbers();

            // رسم العقارب
            Draw_Clock_Hands();

            // مركز الساعة
            using (SolidBrush brush = new SolidBrush(Color.Red))
            {
                graphics.FillEllipse(
                    brush,
                    CLK_P.X - 8,
                    CLK_P.Y - 8,
                    16,
                    16
                );
            }

            pictureBox1.Image = bitmap;
        }

        // رسم الدائرة الخارجية
        private void Draw_Clock()
        {
            using (Pen pen = new Pen(Color.Gray, 15))
            {
                graphics.DrawEllipse(
                    pen,
                    O_P.X,
                    O_P.Y,
                    CLK_Radius * 2,
                    CLK_Radius * 2
                );
            }
        }

        // رسم دائرة
        private void Draw_Cicle(int r, Pen pen)
        {
            Rectangle rect = new Rectangle(
                CLK_P.X - r,
                CLK_P.Y - r,
                r * 2,
                r * 2
            );

            graphics.DrawEllipse(pen, rect);
        }

        // حساب موقع النقطة حسب الزاوية ونصف القطر
        private Point GET_X_P(double A, int R)
        {
            Point P_XY;

            P_XY = new Point(
                CLK_P.X + (int)(R * Math.Cos(Math.PI * A / 180)),
                CLK_P.Y + (int)(R * Math.Sin(Math.PI * A / 180))
            );

            return P_XY;
        }

        // رسم علامات الدقائق والساعات
        private void DRAW_M_Lines()
        {
            for (int i = 0; i < 60; i++)
            {
                double angle = i * 6 - 90;

                int start;
                int end;

                Pen pen;

                if (i % 5 == 0)
                {
                    // علامات الساعات
                    start = 250;
                    end = 280;

                    pen = new Pen(Color.Black, 4);
                }
                else
                {
                    // علامات الدقائق
                    start = 265;
                    end = 280;

                    pen = new Pen(Color.Gray, 2);
                }

                Point P1 = GET_X_P(angle, start);
                Point P2 = GET_X_P(angle, end);

                graphics.DrawLine(pen, P1, P2);

                pen.Dispose();
            }
        }

        // رسم الأرقام من 1 إلى 12

        private void Draw_hour_Numbers()
        {
            int R = 215;

            using (Font font = new Font("Arial", 30, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                StringFormat format = new StringFormat();

                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                for (int i = 1; i <= 12; i++)
                {
                    double angle = i * 30 - 90;

                    Point P = GET_X_P(angle, R);

                    // توسيع المساحة حتى تظهر الأرقام المكونة من رقمين
                    RectangleF rect = new RectangleF(
                        P.X - 50,
                        P.Y - 30,
                        100,
                        60
                    );

                    graphics.DrawString(
                        i.ToString(),
                        font,
                        brush,
                        rect,
                        format
                    );
                }
            }
        }

        

        // رسم العقارب
        private void Draw_Clock_Hands()
        {
            DateTime now = DateTime.Now;

            // الثواني
            double secondAngle =
                now.Second * 6 - 90;

            // الدقائق
            double minuteAngle =
                now.Minute * 6 + now.Second * 0.1 - 90;

            // الساعات
            double hourAngle =
                (now.Hour % 12) * 30 +
                now.Minute * 0.5 - 90;

            // عقرب الساعات
            Draw_Hand(
                hourAngle,
                140,
                Color.Black,
                9
            );

            // عقرب الدقائق
            Draw_Hand(
                minuteAngle,
                190,
                Color.Blue,
                6
            );

            // عقرب الثواني
            Draw_Hand(
                secondAngle,
                225,
                Color.Red,
                2
            );
        }

        // رسم عقرب حسب الزاوية والطول
        private void Draw_Hand(
            double angle,
            int length,
            Color color,
            int width)
        {
            Point end = GET_X_P(angle, length);

            using (Pen pen = new Pen(color, width))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;

                graphics.DrawLine(
                    pen,
                    CLK_P,
                    end
                );
            }
        }

    }
}
 