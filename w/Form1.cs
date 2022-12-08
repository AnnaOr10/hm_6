using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Windows.Forms;

namespace w
{
    public partial class Form1 : Form
    {
        Dictionary<int, double> dataset;
        Dictionary<int, List<double>> samples;
        public static String path = "C:\\Users\\carme\\OneDrive\\Desktop\\Statistics\\Statistics_students_dataset_22_23.csv";
        Random random = new Random();
        Boolean boolWeight = true;
        
        
        Bitmap b;
        Graphics g;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextFieldParser parser = new TextFieldParser(path);
            dataset = new Dictionary<int, double>();
            samples = new Dictionary<int, List<double>>();
            this.b = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            this.g = Graphics.FromImage(b);
            this.g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.g.Clear(Color.White);


            int nSamples = 100;
            int samplesSize = 10;

            double minX = 0;
            double maxX = samplesSize;
            double maxValue = 0;
            double minValue = 0;
            double minY = minValue;
            double maxY = maxValue;

            Rectangle VirtualWindow = new Rectangle(0, 0, this.b.Width - 1, this.b.Height - 1);

            g.DrawRectangle(Pens.Black, VirtualWindow);

            List<double> populationValues = new List<double>();

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.ReadLine();
            int userId = 0;
            while (!parser.EndOfData)
            {
                string row = parser.ReadLine();
                int f = row.LastIndexOf(",") + 1;
                string attribute;
                if (boolWeight == false)
                {
                    string tempS = row.Substring(0, f - 1);
                    int startFrom = tempS.LastIndexOf(",") + 1;
                    attribute = row.Substring(startFrom, f - startFrom - 1);
                }
                else
                {
                    attribute = row.Substring(f);
                }

                double newData = convertData(boolWeight, double.Parse(attribute, CultureInfo.InvariantCulture));
                populationValues.Add(newData);
                dataset[userId] = newData;
                userId++;
            }
          

            //samples's distribution:

            List<double> lastAvgNormal = new List<double>();

            for (int i = 0; i < nSamples; i++)
            {
                List<int> skip = new List<int>();
                List<double> attributes = new List<double>();
                List<double> avgList = new List<double>();

                List<Point> Punti = new List<Point>();

                for (int x = 0; x <= samplesSize; x++)
                {
                    int randomNumber = random.Next(0, userId);
                    while (skip.Contains(randomNumber))
                    {
                        randomNumber = random.Next(0, userId);
                    }
                    skip.Add(randomNumber);
                    attributes.Add(dataset[randomNumber]);
                    double avg = attributes.Average();
                    avgList.Add(avg);

                    if (minValue == 0 || avg < minValue)
                    {
                        minValue = avg;
                    }

                    if (minValue == 0 || avg > maxValue)
                    {
                        maxValue = avg;
                    }

                }

                minY = minValue;

                maxY = maxValue;


                int coordX = 0;
                foreach (double avg in avgList)
                {
                    int xDevice = FromXRealToXVirtual(coordX, minX, maxX, VirtualWindow.Left, VirtualWindow.Width);
                    int yDevice = FromYRealToYVirtual(avg, minY, maxY, VirtualWindow.Top, VirtualWindow.Height);
                    Punti.Add(new Point(xDevice, yDevice));
                    coordX++;
                }
                samples[i] = attributes;
                lastAvgNormal.Add(avgList.Last());
                this.richTextBox1.Text = "Population's Mean: " + populationValues.Average() + "\n" + "Samples's Mean: " + lastAvgNormal.Average();


            }


            drawIsto(lastAvgNormal);

            this.pictureBox1.Image = b;

        }

        private int FromXRealToXVirtual(double X, double minX, double maxX, int Left, int W)
        {
            if (maxX - minX == 0)
            {
                return 0;
            }
            else
            {
                return (int)(Left + W * (X - minX) / (maxX - minX));
            }
        }

        private int FromYRealToYVirtual(double Y, double minY, double maxY, int Top, int H)
        {
            if (maxY - minY == 0)
            {
                return 0;
            }
            else
            {

                return (int)(Top + H - H * (Y - minY) / (maxY - minY));
            }
        }

        private double convertData(Boolean boolWeight, double number)
        {
            if (boolWeight) return number * 0.453592;
            else return number * 2.54;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (boolWeight) boolWeight = false;
            else boolWeight = true;
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            TextFieldParser parser = new TextFieldParser(path);
            dataset = new Dictionary<int, double>();
            samples = new Dictionary<int, List<double>>();
            b = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(b);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);


            int nSamples = 100;
            int samplesSize = 10;

            double minX = 0;
            double maxX = samplesSize;
            double maxValue = 0;
            double minValue = 0;
            double minY = minValue;
            double maxY = maxValue;

            Rectangle VirtualWindow = new Rectangle(0, 0, this.b.Width - 1, this.b.Height - 1);

            g.DrawRectangle(Pens.Black, VirtualWindow);

            List<double> populationValues = new List<double>();

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.ReadLine();
            int userId = 0;
            while (!parser.EndOfData)
            {
                string row = parser.ReadLine();
                int f = row.LastIndexOf(",") + 1;
                string attribute;
                if (boolWeight == false)
                {
                    string tempS = row.Substring(0, f - 1);
                    int startFrom = tempS.LastIndexOf(",") + 1;
                    attribute = row.Substring(startFrom, f - startFrom - 1);
                }
                else
                {
                    attribute = row.Substring(f);
                }

                double newData = convertData(boolWeight, double.Parse(attribute, CultureInfo.InvariantCulture));
                populationValues.Add(newData);
                dataset[userId] = newData;
                userId++;
            }
          
            //samples's distribution:

            List<double> lastAvgNormal = new List<double>();

            for (int i = 0; i < nSamples; i++)
            {
                List<int> skip = new List<int>();
                List<double> attributes = new List<double>();
                List<double> avgList = new List<double>();

                List<Point> Punti = new List<Point>();

                for (int x = 0; x <= samplesSize; x++)
                {
                    int randomNumber = random.Next(0, userId);
                    while (skip.Contains(randomNumber))
                    {
                        randomNumber = random.Next(0, userId);
                    }
                    skip.Add(randomNumber);
                    attributes.Add(dataset[randomNumber]);

                    double avg1 = attributes.Average();
                    double variance1 = 0.0;
                    variance1 += Math.Pow(dataset[randomNumber] - avg1, 2.0);
                    double var1 = variance1 / attributes.Count;

                    avgList.Add(var1);

                    if (minValue == 0 || var1 < minValue)
                    {
                        minValue = var1;
                    }

                    if (minValue == 0 || var1 > maxValue)
                    {
                        maxValue = var1;
                    }

                }

                minY = minValue;

                maxY = maxValue;


                int coordX = 0;
                foreach (double avg in avgList)
                {
                    int xDevice = FromXRealToXVirtual(coordX, minX, maxX, VirtualWindow.Left, VirtualWindow.Width);
                    int yDevice = FromYRealToYVirtual(avg, minY, maxY, VirtualWindow.Top, VirtualWindow.Height);
                    Punti.Add(new Point(xDevice, yDevice));
                    coordX++;
                }
                samples[i] = attributes;
                lastAvgNormal.Add(avgList.Last());

            }

            double avgPop = populationValues.Average();
            List<double> varList = new List<double>();

            foreach (int value in populationValues)
            {
                double variancePop = 0.0;
                variancePop += Math.Pow(value - avgPop, 2.0);
                double var = variancePop / populationValues.Count;
                varList.Add(var);
            }

            this.richTextBox1.Text = "Population's Variance: " + varList.Average() + "\n" + "Samples's Variance: " + lastAvgNormal.Average();

         

            drawIsto(lastAvgNormal);

            this.pictureBox1.Image = b;

        }


        private void drawIsto(List <double> lastAvgNormal) {


            double minAvg = lastAvgNormal.Min();
            double maxAvg = lastAvgNormal.Max();
            double delta = maxAvg - minAvg;
            double nintervals = 10;
            double intervalsSize = delta / nintervals;
            Dictionary<double, int> istogramDict = new Dictionary<double, int>();

            double tempValue = minAvg;
            for (int i = 0; i < nintervals; i++)
            {
                istogramDict[tempValue] = 0;
                tempValue = tempValue + intervalsSize;
            }

            int total = 0;

            foreach (double value in lastAvgNormal)
            {
                foreach (double key in istogramDict.Keys)
                {
                    if (value < key + intervalsSize)
                    {
                        total++;
                        istogramDict[key] += 1;
                        break;
                    }
                }
            }

            List<Control> labelList = new List<Control>();

            foreach (Control ctrl in this.Controls.OfType<Label>().Where(x => x.Name.Contains("tempLabel")))
            {
                labelList.Add(ctrl);
            }

            foreach (Control ctrl in labelList)
            {
                this.Controls.Remove(ctrl);
            }


            g.TranslateTransform(0, this.b.Height);
            g.ScaleTransform(1, -1);

            int idIstogram = 0;
            int widthIstogram = (int)(this.b.Width / nintervals);
            foreach (double key in istogramDict.Keys)
            {
                int newHeight = istogramDict[key] * this.b.Height / total;
                int newX = (widthIstogram * idIstogram) + 1;
                Rectangle isto = new Rectangle(newX, 0, widthIstogram, newHeight);
                idIstogram++;

                int nextWidthIstogram = (int)(widthIstogram * idIstogram * 1);


                Label label = new Label();
                label.Name = "tempLabel";
                label.Location = new Point(newX + this.pictureBox1.Location.X, this.pictureBox1.Height + this.pictureBox1.Location.Y);

                label.Text = (key).ToString("N2") + " - " + (key + intervalsSize).ToString("N2");
                label.Visible = true;
                label.AutoSize = true;
                label.Font = new Font("Calibri", 7);

                if (label.Text.Count() > 14)
                {
                    label.Font = new Font("Calibri", 6);
                    label.Location = new Point(newX + this.pictureBox1.Location.X + 5, this.pictureBox1.Height + this.pictureBox1.Location.Y);

                }

                label.ForeColor = Color.Black;

                this.Controls.Add(label);



                g.DrawRectangle(Pens.Black, isto);
                g.FillRectangle(Brushes.Blue, isto);    
            }
        }
}
}
