using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewBotLuv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            /*
            for (;;)
            {
                // display a command prompt and wait for user input
                Console.Write("> ");
                string expr = Console.ReadLine();

                // catch ?xxx and restate as help(xxx)
                if (expr.Substring(0, 1) == "?")
                    expr = "help(" + expr.Substring(1, 0) + ");";

                // This evaluates the inputted expression and sends the text output
                // to the text callback (cbText).  It also returns a handle to the
                // result.  Use a colon to terminate the statement if you don't
                // want any output (a result is still returned).
                IntPtr val = MapleEngine.EvalMapleStatement(kv, expr + ";");

                // check if user typed quit/done/stop
                if (MapleEngine.IsMapleStop(kv, val))
                    break;
            }

            MapleEngine.StopMaple(kv);*/
        }

        // When evaluating something Maple will send all displayed
        // output through this function.
        public static void cbText(IntPtr data, int tag, String output)
        {
            //Console.WriteLine("tag is " + tag );
            Console.WriteLine(output);
        }

        // When evaluating something Maple will send errors through this function.
        // If not defined, errors will go through the text callback.
        public static void cbError(IntPtr data, IntPtr offset, String msg)
        {
            //Console.WriteLine("offset is " + offset.ToInt32() );
            Console.WriteLine(msg);
        }

        // When evaluating something Maple will send a message about resources
        // used once per garbage collection.  If you don't want to see these
        // messages, just comment out the WriteLine command inside.
        public static void cbStatus(IntPtr data, IntPtr used, IntPtr alloc, double time)
        {
            Console.WriteLine("cputime=" + time
              + "; memory used=" + used.ToInt64() + "kB"
              + " alloc=" + alloc.ToInt64() + "kB"
            );
        }

        private string TinhHamso()
        {
            //BeginLoadMaple
            MapleEngine.MapleCallbacks cb;
            byte[] err = new byte[2048];
            IntPtr kv;

            // pass -A2 which sets kernelopts(assertlevel=2) just to show
            // how in this example.  The corresponding argc parameter 
            // (the first argument to StartMaple) should then be 2
            // argv[0] should always be filled in with something.
            String[] argv = new String[2];
            argv[0] = "maple";
            argv[1] = "-A2";

            // only a textcallback is really needed here, but error and status
            // callbacks are also provided to show how to handle the IntPtr
            // arguments
            cb.textCallBack = cbText;
            cb.errorCallBack = cbError;
            cb.statusCallBack = cbStatus;
            cb.readlineCallBack = null;
            cb.redirectCallBack = null;
            cb.streamCallBack = null;
            cb.queryInterrupt = null;
            cb.callbackCallBack = null;
            //EndIntilia//

            try
            {
                kv = MapleEngine.StartMaple(2, argv, ref cb, IntPtr.Zero, IntPtr.Zero, err);

            }
            catch (DllNotFoundException e)
            {
                return e.ToString();
            }
            catch (EntryPointNotFoundException e)
            {
                return e.ToString();
            }

            if (kv.ToInt64() == 0)
            {
                // Maple didn't start properly.  The "err" parameter will be filled
                // in with the reason why (usually a license error)
                // Note that since we passed in a byte[] array we need to trim
                // the characters past \0 during conversion to string
                return ("Fatal Error, could not start Maple: "
                        + System.Text.Encoding.ASCII.GetString(err, 0, Array.IndexOf(err, (byte)0))
                    );
            }
            return "";
        }

        string temp;
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                temp = textBox1.Text;
                textBox1.Clear();
                textBox2.Text += Environment.NewLine + "Me: " + temp
                    + Environment.NewLine + "Bot: " + CalltheAnswer(temp);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && textBox1.Text != null)
            {
                e.Handled = true;
                temp = textBox1.Text;
                textBox1.Clear();
                textBox2.Text += Environment.NewLine + "Me: " + temp
                    + Environment.NewLine + "Bot: " + CalltheAnswer(temp);
            }
        }

        string CalltheAnswer(string temp)
        {
            string meow;
            List<string> greetings = new List<string>(new string[] { "hello", "hi", "xin chào", "chào"});
            List<string> goodbyeph = new List<string>(new string[] { "bye", "tạm biệt", "hẹn gặp lại", "good bye", "see ya"});

            if (greetings.Contains(temp.ToLower()))
            {
                Random randNum = new Random();
                return greetings[randNum.Next(0, greetings.Count)];
            }
            else if (temp.Contains("x^3"))
            {
                return "Đang tính hàm số cấp 3:..."; //chỗ này return string output ở cbText nè :)))
            }
            else if(goodbyeph.Contains(temp.ToLower()))
            {
                Random randNum = new Random();
                return goodbyeph[randNum.Next(0, goodbyeph.Count)];
            }            
            meow = "";

            return meow;
        }


    }
}
