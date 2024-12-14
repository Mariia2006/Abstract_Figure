using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WinFormsApp5
{
    abstract class Figure
    {
        public int CenterX { get; protected set; }
        public int CenterY { get; protected set; }

        protected Figure(int centerX, int centerY)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public abstract void DrawBlack(Graphics g);
        public abstract void HideDrawingBackGround(Graphics g);

        public void MoveRight(Graphics g, int steps, int stepSize)
        {
            for (int i = 0; i < steps; i++)
            {
                DrawBlack(g); // малюємо фігуру
                Thread.Sleep(100); // очікуємо
                HideDrawingBackGround(g); // приховуємо фігуру
                CenterX += stepSize; // зміщуємо координату X
            }
        }
    }
    class Circle : Figure
    {
        public int Radius { get; private set; }

        public Circle(int centerX, int centerY, int radius) : base(centerX, centerY)
        {
            Radius = radius;
        }

        public override void DrawBlack(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black))
            {
                g.DrawEllipse(pen, CenterX - Radius, CenterY - Radius, Radius * 2, Radius * 2);
            }
        }

        public override void HideDrawingBackGround(Graphics g)
        {
            using (Pen pen = new Pen(Color.White))
            {
                g.DrawEllipse(pen, CenterX - Radius, CenterY - Radius, Radius * 2, Radius * 2);
            }
        }
    }
    class Square : Figure
    {
        public int SideLength { get; private set; }

        public Square(int centerX, int centerY, int sideLength) : base(centerX, centerY)
        {
            SideLength = sideLength;
        }

        public override void DrawBlack(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black))
            {
                g.DrawRectangle(pen, CenterX - SideLength / 2, CenterY - SideLength / 2, SideLength, SideLength);
            }
        }

        public override void HideDrawingBackGround(Graphics g)
        {
            using (Pen pen = new Pen(Color.White))
            {
                g.DrawRectangle(pen, CenterX - SideLength / 2, CenterY - SideLength / 2, SideLength, SideLength);
            }
        }
    }
    class Rhomb : Figure
    {
        public int HorDiagLen { get; private set; }
        public int VertDiagLen { get; private set; }

        public Rhomb(int centerX, int centerY, int horDiagLen, int vertDiagLen) : base(centerX, centerY)
        {
            HorDiagLen = horDiagLen;
            VertDiagLen = vertDiagLen;
        }

        public override void DrawBlack(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black))
            {
                Point[] points = new Point[]
                {
                    new Point(CenterX, CenterY - VertDiagLen / 2),
                    new Point(CenterX + HorDiagLen / 2, CenterY),
                    new Point(CenterX, CenterY + VertDiagLen / 2),
                    new Point(CenterX - HorDiagLen / 2, CenterY)
                };
                g.DrawPolygon(pen, points);
            }
        }

        public override void HideDrawingBackGround(Graphics g)
        {
            using (Pen pen = new Pen(Color.White))
            {
                Point[] points = new Point[]
                {
                new Point(CenterX, CenterY - VertDiagLen / 2),
                new Point(CenterX + HorDiagLen / 2, CenterY),
                new Point(CenterX, CenterY + VertDiagLen / 2),
                new Point(CenterX - HorDiagLen / 2, CenterY)
                };
                g.DrawPolygon(pen, points);
            }
        }
    }
    class TestForm : Form
    {
        private Circle circle;
        private Square square;
        private Rhomb rhomb;

        public TestForm()
        {
            Text = "Figure Test";
            Size = new Size(800, 600);
            BackColor = Color.White;

            circle = new Circle(100, 100, 50);
            square = new Square(300, 100, 80);
            rhomb = new Rhomb(500, 100, 100, 60);

            Paint += TestForm_Paint;
        }

        private void TestForm_Paint(object sender, PaintEventArgs e)
        {
            // тест малювання
            circle.DrawBlack(e.Graphics);
            square.DrawBlack(e.Graphics);
            rhomb.DrawBlack(e.Graphics);
            // тест руху вправо
            new Thread(() =>
            {
                circle.MoveRight(CreateGraphics(), 10, 10);
                square.MoveRight(CreateGraphics(), 10, 10);
                rhomb.MoveRight(CreateGraphics(), 10, 10);
                Invalidate(); // оновлення форми
            }).Start();
        }
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new TestForm());
        }
    }
}