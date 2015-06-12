using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCalc
{
    public partial class Form1 : Form
    {
        double Ans = 0;
        double Memory = 0;
        List<int> AboveDecimal = new List<int>();
        List<int> BelowDecimal = new List<int>();
        LastOperator _LO = LastOperator.None;
        bool isNeg = false;
        public LastOperator LO
        {
            get { return _LO; }
            set
            {
                _LO = value;
                SetSecondaryText();
            }
        }
        bool isDecimal = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            primaryDigitLabel.Text = "0";
            SecondaryDigitLabel.Text = "";
            this.Width = 222;
        }

        private void digitClick(object sender, EventArgs e)
        {
            int btnValue;
            if (!int.TryParse(((Button)sender).Text, out btnValue))
                return;
            AddDigit(btnValue);
        }

        void AddDigit(int val)
        {
            if (isDecimal)
                BelowDecimal.Add(val);
            else
                AboveDecimal.Add(val);
            primaryDigitLabel.Text = getCurrentNumber().ToString();
        }
        public double getCurrentNumber()
        {
            double l = 0;
            for (int i = 0; i < AboveDecimal.Count; i++)
                l += AboveDecimal[i] * Math.Pow(10, AboveDecimal.Count - i - 1);
            for (int i = 0; i < BelowDecimal.Count; i++)
                l += BelowDecimal[i] / Math.Pow(10, BelowDecimal.Count - i);
            if (isNeg)
                return l * -1;
            else
                return l;
        }
        void AddHistory(params string[] st)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string s in st)
            {
                sb.Append(s);
            }
            listBox1.Items.Add(sb.ToString());
        }
        void MoveToSecondary()
        {
            if (LO == LastOperator.None || Ans == 0)
            {
                Ans = getCurrentNumber();
            }
            else
            {
                switch (LO)
                {
                    case LastOperator.Addition:
                        AddHistory(Ans.ToString(), " + ", getCurrentNumber().ToString());
                        Ans += getCurrentNumber();
                        break;
                    case LastOperator.Division:
                        AddHistory(Ans.ToString(), " / ", getCurrentNumber().ToString());
                        Ans /= getCurrentNumber();
                        break;
                    case LastOperator.Multiplication:
                        AddHistory(Ans.ToString(), " * ", getCurrentNumber().ToString());
                        Ans *= getCurrentNumber();
                        break;
                    case LastOperator.Subtraction:
                        AddHistory(Ans.ToString(), " - ", getCurrentNumber().ToString());
                        Ans -= getCurrentNumber();
                        break;
                }
            }
            ClearWorkingSpace();
            primaryDigitLabel.Text = getCurrentNumber().ToString();
        }

        void ClearWorkingSpace()
        {
            AboveDecimal.Clear();
            BelowDecimal.Clear();
            isDecimal = false;
            isNeg = false;
            primaryDigitLabel.Text = getCurrentNumber().ToString();
        }

        void SetSecondaryText()
        {
            if (Ans == 0)
            {
                SecondaryDigitLabel.Text = "";
            }
            else
            {
                switch (LO)
                {
                    case LastOperator.Addition:
                        SecondaryDigitLabel.Text = Ans.ToString() + "  +";
                        break;
                    case LastOperator.Division:
                        SecondaryDigitLabel.Text = Ans.ToString() + "  /";
                        break;
                    case LastOperator.Multiplication:
                        SecondaryDigitLabel.Text = Ans.ToString() + "  *";
                        break;
                    case LastOperator.Subtraction:
                        SecondaryDigitLabel.Text = Ans.ToString() + "  -";
                        break;
                    default:
                        SecondaryDigitLabel.Text = Ans.ToString();
                        break;
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (BelowDecimal.Count == 0)
                isDecimal = false;
            if (isDecimal)
            {
                BelowDecimal.RemoveAt(BelowDecimal.Count - 1);
            }
            else
            {
                if(AboveDecimal.Count != 0)
                    AboveDecimal.RemoveAt(AboveDecimal.Count - 1);
            }
            if (BelowDecimal.Count == 0)
                isDecimal = false;
            primaryDigitLabel.Text = getCurrentNumber().ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            isDecimal = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MoveToSecondary();
            LO = LastOperator.Addition;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveToSecondary();
            LO = LastOperator.Subtraction;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MoveToSecondary();
            LO = LastOperator.Multiplication;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MoveToSecondary();
            LO = LastOperator.Division;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MoveToSecondary();
            LO = LastOperator.None;
            double outcome = Ans;
            Ans = 0;
            ClearWorkingSpace();
            SetSecondaryText();
            Ans = outcome;
            SetNumber(outcome);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ClearWorkingSpace();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Ans = 0;
            ClearWorkingSpace();
            primaryDigitLabel.Text = getCurrentNumber().ToString();
            SetSecondaryText();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Memory = 0;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            SetNumber(Memory);
        }

        void SetNumber(double num)
        {
            string strNum = num.ToString();
            ClearWorkingSpace();
            int start = 0;
            if(strNum[0] == '-')
            {
                isNeg = true;
                start = 1;
            }
            for (int i = start; i < strNum.Length; i++)
            {
                if (strNum[i] == '.')
                    isDecimal = true;
                else
                    AddDigit(int.Parse(strNum[i].ToString()));
            }
            primaryDigitLabel.Text = getCurrentNumber().ToString();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Memory = getCurrentNumber();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Memory += getCurrentNumber();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            Memory -= getCurrentNumber();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            isNeg = !isNeg;
            primaryDigitLabel.Text = getCurrentNumber().ToString();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            SetNumber(Math.Sqrt(getCurrentNumber()));
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 222;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void showHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 343;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(getCurrentNumber().ToString());
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dat = Clipboard.GetText();
            double num;
            if (!double.TryParse(dat, out num))
                return;
            SetNumber(num);
        }
    }

    public enum LastOperator
    {
        Addition,
        Subtraction,
        Division,
        Multiplication,
        None
    }
}
