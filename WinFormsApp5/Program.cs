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
                DrawBlack(g); // ìàëþºìî ô³ãóðó
                Thread.Sleep(100); // î÷³êóºìî
                HideDrawingBackGround(g); // ïðèõîâóºìî ô³ãóðó
                CenterX += stepSize; // çì³ùóºìî êîîðäèíàòó X
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
        private Button circleButton;
        private Button rhombButton;
        private Button squareButton;
        private Button exitButton;
        private Figure currentFigure;
        private Graphics graphics;

        public TestForm()
        {
            Text = "Figure Animation";
            Width = 800;
            Height = 600;
            BackColor = Color.White;

            circleButton = new Button { Text = "Circle", Location = new Point(10, 10), Size = new Size(100, 40) };
            rhombButton = new Button { Text = "Rhomb", Location = new Point(120, 10), Size = new Size(100, 40) };
            squareButton = new Button { Text = "Square", Location = new Point(230, 10), Size = new Size(100, 40) };
            exitButton = new Button { Text = "Exit", Location = new Point(340, 10), Size = new Size(100, 40) };

            circleButton.Click += (s, e) => StartFigure(new Circle(100, Height / 2, 50));
            rhombButton.Click += (s, e) => StartFigure(new Rhomb(100, Height / 2, 100, 60));
            squareButton.Click += (s, e) => StartFigure(new Square(100, Height / 2, 80));
            exitButton.Click += (s, e) => Close();

            Controls.Add(circleButton);
            Controls.Add(rhombButton);
            Controls.Add(squareButton);
            Controls.Add(exitButton);

            graphics = CreateGraphics();
        }

        private void StartFigure(Figure newFigure)
        {
            if (currentFigure != null)
            {
                currentFigure.HideDrawingBackGround(graphics);
            }

            currentFigure = newFigure;

            Thread thread = new Thread(() => currentFigure.MoveRight(graphics, 50, 10));
            thread.IsBackground = true;
            thread.Start();
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
