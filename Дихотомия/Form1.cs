using DichtomyApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Дихотомия
{
    public partial class Form1 : Form
    {
        private double a = 1, b = 10, epsilon = 0.1;
        private int AccuracyForView;
        private string StringFunction;
        private Fuction fuction;

        public Form1()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private double Round(double x)
        {
            double Accuracy = Math.Log10(epsilon) * -1.0;
            int AccuracyForView = Convert.ToInt32(Accuracy);
            if (AccuracyForView < 0)
            {
                AccuracyForView = 0;
            }
            double RoundX = Math.Round(x, AccuracyForView);
            return RoundX;
        }
        private void setupFunction()
        {
            if (textBoxFunc.Text == "" || textBoxFunc.Text == " ")
            {
                StringFunction = "(27 - 18 * x + 2 * x^2) * exp(-x/3)";
            }
            else
            {
                StringFunction = textBoxFunc.Text;
            }
            fuction = new Fuction(StringFunction);
        }
        private void ClearTextBox()
        {
            textBoxFunctionMeaning.Text = "";
            textBoxIntersectionPoint.Text = "";
            textBoxMin.Text = "";
            textBoxMax.Text = "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Visualisation()
        {
            double x;
            double y;
            if (a < b)
            {
                x = a;

                chart1.Series[0].Points.Clear();
                while (x <= b)
                {
                    y = fuction.StandartFunction(x);
                    chart1.Series[0].Points.AddXY(x, y);
                    x += 0.01;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParsFormView();
            ClearTextBox();

            if (epsilon <= 0)
            {
                MessageBox.Show("Введите точность через запятую", "error");
            }
            else
            {
                if (a >= b)
                {
                    MessageBox.Show("А должно быть меньше B", "Ошибка");
                }
                else
                {
                    setupFunction();
                    Visualisation();
                    DichtomyMethod();
                }
            }
        }
        private void GoldenMethodPoint()
        {
            double start = a;
            double end = b;

            double phi = (1 + Math.Sqrt(5)) / 2;
            double h = end - start;
            double c = end - h / phi;
            double d = start + h / phi;
            double fc = fuction.AbsFunction(c);
            double fd = fuction.AbsFunction(d);

            while (Math.Abs(fc - fd) > epsilon)
            {
                if (fc < fd)
                {
                    end = d;
                    d = c;
                    c = end - (end - start) / phi;
                    fd = fc;
                    fc = fuction.AbsFunction(c);
                }
                else
                {
                    start = c;
                    c = d;
                    d = start + (end - start) / phi;
                    fc = fd;
                    fd = fuction.AbsFunction(d);
                }
            }

            double result = (start + end) / 2;
            textBoxIntersectionPoint.Text = Convert.ToString(Math.Round(result, AccuracyForView));
            textBoxFunctionMeaning.Text = Convert.ToString(fuction.StandartFunction(Math.Round(result)));
        }
        private void GoldenMethodMinimum()
        {
            double start = a;
            double end = b;

            double phi = (1 + Math.Sqrt(5)) / 2;
            double h = end - start;
            double c = end - h / phi;
            double d = start + h / phi;
            double fc = fuction.StandartFunction(c);
            double fd = fuction.StandartFunction(d);

            while (Math.Abs(fc - fd) > epsilon)
            {
                if (fc < fd)
                {
                    end = d;
                    d = c;
                    c = end - (end - start) / phi;
                    fd = fc;
                    fc = fuction.StandartFunction(c);
                }
                else
                {
                    start = c;
                    c = d;
                    d = start + (end - start) / phi;
                    fc = fd;
                    fd = fuction.StandartFunction(d);
                }
            }

            double result = (start + end) / 2;
            textBoxMin.Text = Convert.ToString(Math.Round(result, AccuracyForView));
        }

        private void GoldenMethodMaximum()
        {
            double start = a;
            double end = b;

            double phi = (1 + Math.Sqrt(5)) / 2;
            double h = end - start;
            double c = end - h / phi;
            double d = start + h / phi;
            double fc = fuction.MinusFunction(c);
            double fd = fuction.MinusFunction(d);

            while (Math.Abs(fc - fd) > epsilon)
            {
                if (fc < fd)
                {
                    end = d;
                    d = c;
                    c = end - (end - start) / phi;
                    fd = fc;
                    fc = fuction.MinusFunction(c);
                }
                else
                {
                    start = c;
                    c = d;
                    d = start + (end - start) / phi;
                    fc = fd;
                    fd = fuction.MinusFunction(d);
                }
            }

            double result = (start + end) / 2;
            textBoxMax.Text = Convert.ToString(Math.Round(result, AccuracyForView));
        }
        private void NewtonsPoint()
        {
            double x0, x1, x2;
            x0 = b;
            x1 = x0 - (fuction.StandartFunction(x0) / fuction.DerivFunction(x0));
            x2 = x1 - (fuction.StandartFunction(x1) / fuction.DerivFunction(x1));

            while (x1 - x2 >= epsilon)
            {
                x1 = x2;
                x2 = x1 - (fuction.StandartFunction(x1) / fuction.DerivFunction(x1));
            }

            double x = x2;
            double fx = fuction.StandartFunction(x2);

            if (x < a || x > b)
            {
                textBoxFunctionMeaning.Text = "нет";
                textBoxIntersectionPoint.Text = "Нет значений";
                MessageBox.Show("x = " + x + "     fx = " + fx);
                return;
            }

            if (fx > 1 || fx < -1)
            {
                textBoxFunctionMeaning.Text = "нет";
                textBoxIntersectionPoint.Text = "Нет значений";
                MessageBox.Show("x = " + x + "     fx = " + fx);
            }
            else
            {
                textBoxFunctionMeaning.Text = Convert.ToString(Math.Round(fx, AccuracyForView));
                textBoxIntersectionPoint.Text = Convert.ToString(Math.Round(x, AccuracyForView));
            }
        }
        private void textBoxIntersectionPoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                ParsFormView();
                ClearTextBox();
                if (epsilon <= 0)
                {
                    MessageBox.Show("Введите точность через запятую", "error");
                }
                else
                {
                    if (a >= b)
                    {
                        MessageBox.Show("А должно быть меньше B", "Ошибка");
                    }
                    else
                    {
                        setupFunction();
                        Visualisation();
                        GoldenMethodPoint();
                        GoldenMethodMinimum();
                        GoldenMethodMaximum();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ParsFormView();
            ClearTextBox();

            if (epsilon <= 0)
            {
                MessageBox.Show("Введите точность через запятую", "error");
            }
            else
            {
                if (a >= b)
                {
                    MessageBox.Show("А должно быть меньше B", "Ошибка");
                }
                else
                {
                    setupFunction();
                    Visualisation();
                    NewtonsPoint();
                }
            }
        }

        private void textBoxFunctionMeaning_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxA.Clear();
            textBoxB.Clear();
            textBoxEps.Clear();
            chart1.Series[0].Points.Clear();
            ClearTextBox();
        }

        private void ParsFormView()
        {
            try
            {
                a = Convert.ToDouble(textBoxA.Text);
                b = Convert.ToDouble(textBoxB.Text);
                epsilon = Convert.ToDouble(textBoxEps.Text);

                double Accuracy = Math.Log10(epsilon) * -1.0;
                AccuracyForView = Convert.ToInt32(Accuracy);
                if (AccuracyForView < 0)
                {
                    AccuracyForView = 0;
                }
            }
            catch
            {
            }
        }
        private void DichtomyMethod()
        {
            double x1 = a;
            double x2 = b;

            do
            {
                double x3 = (x1 + x2) / 2;
                double f1 = fuction.StandartFunction(x1);
                double f2 = fuction.StandartFunction(x3);

                if (f1 * f2 < 0)
                {
                    x2 = x3;
                }
                else
                {
                    x1 = x3;
                }
            } while (Math.Abs(x2 - x1) > epsilon);
            double result = Round((x1 + x2) / 2);

            double RoundResult = fuction.StandartFunction((x1 + x2) / 2);
            if (RoundResult > 1 || RoundResult < -1)
            {
                textBoxFunctionMeaning.Text = "нет";
                textBoxIntersectionPoint.Text = "Нет значений";
            }
            else
            {
                textBoxFunctionMeaning.Text = Convert.ToString(Math.Round(RoundResult, AccuracyForView));
                textBoxIntersectionPoint.Text = Convert.ToString(result);
            }
        }
    }
}
