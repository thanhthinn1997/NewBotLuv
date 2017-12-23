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
            textBox2.Text += "***** Tớ là bot chuyên thực hiện khảo sát hàm số!" + Environment.NewLine +
                "***** Hãy nhập hàm số tớ cần khảo sát nhé!" + Environment.NewLine +
                "***** Lưu ý: " + Environment.NewLine +
                "***** - Tớ chỉ đủ thông minh giải tới hàm số cấp 3. Xin lỗi người ấy nhe!" + Environment.NewLine +
                "***** - Tớ không đủ thông minh để chatchit với cậu :) Nếu tớ im lặng, nghĩa là tớ không biết thôi." + Environment.NewLine +
                "***** - Có lẽ tớ sẽ có bugs, nếu có thì hãy kiên nhẫn với tớ." + Environment.NewLine +
                "Hết rồi! Chúc người ấy may mắn nhé :)" + Environment.NewLine +
                "--------------------------------------------------------------" + Environment.NewLine;
        }

        public static String finalOutput;

        // When evaluating something Maple will send all displayed
        // output through this function.
        public static void cbText(IntPtr data, int tag, String output)
        {
            //Console.WriteLine("tag is " + tag );
            finalOutput = output;
        }

        // When evaluating something Maple will send errors through this function.
        // If not defined, errors will go through the text callback.
        public static void cbError(IntPtr data, IntPtr offset, String msg)
        {
            //Console.WriteLine("offset is " + offset.ToInt32() );
            finalOutput = msg + "abc";
        }

        // When evaluating something Maple will send a message about resources
        // used once per garbage collection.  If you don't want to see these
        // messages, just comment out the WriteLine command inside.
        public static void cbStatus(IntPtr data, IntPtr used, IntPtr alloc, double time)
        {
            finalOutput = "cputime=" + time
              + "; memory used=" + used.ToInt64() + "kB"
              + " alloc=" + alloc.ToInt64() + "kB";
        }

        IntPtr kv;

        private string TinhHamso(string equa)
        {
            //BeginLoadMaple
            MapleEngine.MapleCallbacks cb;
            byte[] err = new byte[2048];

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


            // This evaluates the inputted expression and sends the text output
            // to the text callback (cbText).  It also returns a handle to the
            // result.  Use a colon to terminate the statement if you don't
            // want any output (a result is still returned).
            IntPtr val = MapleEngine.EvalMapleStatement(kv, "solve(" + equa + ");");

            return finalOutput;
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
                if(temp.Contains("x^3"))
                {
                    textBox2.Text += Environment.NewLine +
                        "***** Đã nhận được hàm số, hãy chọn bài toán bạn cần mình giải nhé: " +
                        Environment.NewLine +
                        "***** Nhấp 1: Khảo sát hàm số + vẽ đồ thị" + Environment.NewLine +
                        "***** Nhấp 2: Lập phương trình tiếp tuyến" + Environment.NewLine +
                        "***** Nhấp 3: Xét tính đơn điệu hàm số" + Environment.NewLine +
                        "***** Nhấp 4: Tìm điểm cực đại + cực tiểu";
                }
            }
        }

        string CalltheAnswer(string temp)
        {
            string meow;
            List<string> greetings = new List<string>(new string[] { "hello", "hi", "xin chào", "chào"});
            List<string> goodbyeph = new List<string>(new string[] { "bye", "tạm biệt", "hẹn gặp lại", "good bye", "see ya"});

            if (greetings.Contains(temp.ToLower()))
            {
                return "Tớ không tính chào cậu lại đâu hehe :)";
            }
            else if (temp.Contains("x^3"))
            {
                List<char> formula = new List<char>(new char[] { ' ', 'x', 'y', '^', '+', '-', '*', '/', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '='});
                string tmp = "";
                int beg = -1;
                int length = 0;
                for(int i = 0; i < temp.Count(); i++)
                {
                    if (formula.Contains(temp[i]) && beg == -1 && temp[i]!=' ')
                    {
                        beg = i;
                        length = 1;
                    }
                    else
                    {
                        if (formula.Contains(temp[i])) length++;
                    }                    
                }
                tmp = temp.Substring(beg, length);

                return tmp; //chỗ này return string output ở cbText nè :)))
            }
            else if(goodbyeph.Contains(temp.ToLower()))
            {
                Random randNum = new Random();
                MapleEngine.StopMaple(kv);
                return goodbyeph[randNum.Next(0, goodbyeph.Count)];
            }
            else if(temp.Contains("1"))
            {
                return "Thuc hien ngu nguoi 1 :)";
            }
            else if(temp.Contains("2"))
            {
                return "Thuc hien ngu nguoi 2 :)";
            }            
            else if(temp.Contains("3"))
            {
                return "Thuc hien ngu nguoi 3 :)";
            }
            meow = "Tớ không hiểu gì cả hiu hiu :)";

            return meow;
        }
    }
}
