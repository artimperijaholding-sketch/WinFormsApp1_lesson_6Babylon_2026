using Printer;
using SharedInterfaces;

namespace WinFormsApp1_lesson_6Babylon_2026
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logger.Instance.LogData(LogLevel.Debug, "Message");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Logger.Instance.LogData(LogLevel.Warning, "Message");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(SetMessage);
            Thread thread2 = new Thread(SetMessage);

            thread1.Start(1);
            thread2.Start(2);
        }

        private void SetMessage(object n)
        {
            int number = (int)n;
            Logger.Instance.LogData(LogLevel.Error, $"Message from different thread. Number = {n}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string data = textBox1.Text;
           var printerFactoryes=new PrinterFactoryes();
            IPrint printer = printerFactoryes.GetPrinter();
            printer.Print(data);

        }
    }

}
