using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingApp
{
    public partial class Form1 : Form
    {
        private bool rectangleFirstPointClick = true;
        private bool circleFirstPointClick = true;
        private bool lineFirstPointClick = true;
        private Point firstPoint, secondPoint;
        private string drawingMode="lines";
        private Bitmap picture;
        private List<Point> lines = new List<Point>();
        private List<Rectangle> rectangles = new List<Rectangle>();
        private List<Rectangle> circles = new List<Rectangle>();
        private Pen pen = new Pen(Color.Black);

        public Form1()
        {
            InitializeComponent();
            picture = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = picture;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            switch (drawingMode)
            {
                case "rectangles":
                    circleFirstPointClick = true;
                    lineFirstPointClick = true;
                    switch (rectangleFirstPointClick)
                    {
                        case true:
                            firstPoint = e.Location;
                            rectangleFirstPointClick = !rectangleFirstPointClick;
                            break;
                        case false:
                            secondPoint = e.Location;
                            int height = Math.Abs(firstPoint.Y - secondPoint.Y);
                            int width = Math.Abs(firstPoint.X - secondPoint.X);
                            adjustFirstPoint();
                            rectangles.Add(new Rectangle(firstPoint.X,firstPoint.Y,
                                width,height));
                            rectangleFirstPointClick = !rectangleFirstPointClick;
                            DrawRectangle();
                            break;
                    }
                break;
                case "circles":
                    rectangleFirstPointClick = true;
                    lineFirstPointClick = true;
                    switch (circleFirstPointClick)
                    {
                        case true:
                            firstPoint = e.Location;
                            circleFirstPointClick = !circleFirstPointClick;
                            break;
                        case false:
                            secondPoint = e.Location;
                            adjustFirstPoint();
                            int height = Math.Abs(firstPoint.Y - secondPoint.Y);
                            int width = Math.Abs(firstPoint.X - secondPoint.X);
                            circles.Add(new Rectangle(firstPoint.X, firstPoint.Y,
                                Math.Max(width, height), Math.Max(width, height)));
                            circleFirstPointClick = !circleFirstPointClick;
                            DrawCircle();
                            break;
                    }
                    break;
                case "lines":
                    rectangleFirstPointClick = true;
                    circleFirstPointClick = true;
                    if (lineFirstPointClick)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            firstPoint = e.Location;
                            lines.Add(firstPoint);
                            lineFirstPointClick = false;
                            Console.WriteLine("Switch body: "+lines.Last());
                        }
                        else if (e.Button == MouseButtons.Right)
                        {
                            lineFirstPointClick = true;
                        }
                    }
                    else
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            secondPoint = e.Location;
                            lines.Add(secondPoint);
                            DrawLine();
                            firstPoint = secondPoint;
                        }else if (e.Button == MouseButtons.Right)
                        {
                            lineFirstPointClick = true;
                        }
                    }
                    break;
            }
        }

        private void DrawLine()
        {
            Graphics graphic = Graphics.FromImage(picture);
            Point[] points = { lines.ElementAt(lines.Count - 2), lines.Last() };
            Console.WriteLine("Draw method: "+ lines.ElementAt(lines.Count - 1)+" "+
                lines.Last());
            graphic.DrawLines(pen, points);
            pictureBox1.Image = picture;
        }

        private void DrawCircle()
        {
            Graphics graphic = Graphics.FromImage(picture);
            graphic.DrawEllipse(pen, circles.Last());
            pictureBox1.Image = picture;
        }

        private void adjustFirstPoint()
        {
            switch (drawingMode)
            {
                case "rectangles":
                    if (firstPoint.X > secondPoint.X)
                        firstPoint.X -= firstPoint.X - secondPoint.X;
                    if (firstPoint.Y > secondPoint.Y)
                        firstPoint.Y -= firstPoint.Y - secondPoint.Y;
                    break;
                case "circles":
                    Point circleCenter = new Point(firstPoint.X+( secondPoint.X - firstPoint.X) / 2,
                        firstPoint.Y+(secondPoint.Y - firstPoint.Y) / 2);
                    int radius = Convert.ToInt32(Math.Sqrt(Math.Pow(Convert.ToDouble(circleCenter.X - firstPoint.X), 2)+
                        Math.Pow(Convert.ToDouble(circleCenter.Y - firstPoint.Y), 2)));
                    firstPoint.X = circleCenter.X - radius;
                    firstPoint.Y = circleCenter.Y - radius;
                    secondPoint.X = circleCenter.X + radius;
                    secondPoint.Y = circleCenter.Y + radius;
                    break;
            }
        }

        private void DrawRectangle()
        {
            Graphics graphic = Graphics.FromImage(picture);
            graphic.DrawRectangle(pen, rectangles.Last());
            pictureBox1.Image = picture;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Graphics graphic = Graphics.FromImage(picture);
                pen = new Pen(colorDialog1.Color);
                toolStripButton1.BackColor = pen.Color;
                //toolStripButton1.ForeColor = pen.Color;
                this.toolStripButton1.DisplayStyle = 
                    System.Windows.Forms.ToolStripItemDisplayStyle.None;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Location.ToString();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picture.Save(saveFileDialog1.FileName);
            }
        }
        private void rbShapeChoice_click(object sender, EventArgs e)
        {
            if (sender.Equals(rbLines))
            {
                drawingMode = "lines";
            }else if (sender.Equals(rbCircles))
            {
                drawingMode = "circles";
            }else
            {
                drawingMode = "rectangles";
            }
        }
    }
}
