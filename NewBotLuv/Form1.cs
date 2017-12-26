using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NewBotLuv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text += "***** To la bot chuyen khao sat ham so!" + Environment.NewLine +
                "***** Hay nhap ham so to can khao sat nha!" + Environment.NewLine +
                "***** Luu y: " + Environment.NewLine +
                "***** - To chi du thong minh giai ham so cap ba" + Environment.NewLine +
                "***** - abc" + Environment.NewLine +
                "***** - abcdhhfjdjgr" + Environment.NewLine +
                "hahdjeyfyhgbbsgfs" + Environment.NewLine +
                "--------------------------------------------------------------" + Environment.NewLine;
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
                MessageBox.Show(e.ToString());
            }
            catch (EntryPointNotFoundException e)
            {
                MessageBox.Show(e.ToString());
            }

            if (kv.ToInt64() == 0)
            {
                // Maple didn't start properly.  The "err" parameter will be filled
                // in with the reason why (usually a license error)
                // Note that since we passed in a byte[] array we need to trim
                // the characters past \0 during conversion to string
                MessageBox.Show("Fatal Error, could not start Maple: "
                        + System.Text.Encoding.ASCII.GetString(err, 0, Array.IndexOf(err, (byte)0))
                    );
            }
        }

        private IntPtr kv;
        private IntPtr val;
        private string temp;
        private int stepMath = -1;
        private string tmp = "";
        private string a = "0", b = "0", c = "0", d = "0";

        public static String finalOutput;
        public static String stepOutput;

        List<Image> storageImage = new List<Image>();
        private int currInd = -1;

        // When evaluating something Maple will send all displayed
        // output through this function.
        public static void cbText(IntPtr data, int tag, String output)
        {
            //Console.WriteLine("tag is " + tag );
            stepOutput = output;
        }

        // When evaluating something Maple will send errors through this function.
        // If not defined, errors will go through the text callback.
        public static void cbError(IntPtr data, IntPtr offset, String msg)
        {
            //Console.WriteLine("offset is " + offset.ToInt32() );
            stepOutput += msg;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                temp = textBox1.Text;
                textBox1.Clear();
                richTextBox1.Text += Environment.NewLine + "Me: " + temp
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
                
                richTextBox1.Text += Environment.NewLine + "Me: " + temp
                    + Environment.NewLine + "Bot: " + CalltheAnswer(temp);
                if(temp.Contains("x^3"))
                {
                    richTextBox1.Text += Environment.NewLine +
                        "***** Nhan ham so nha nha nha: " +
                        Environment.NewLine +
                        "***** Nhap 1: Khao sat ham so + ve do thi" + Environment.NewLine +
                        "***** Nhap 2: Viet phuong trinh tiep tuyen";
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(richTextBox1.Visible)
            {
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.ScrollToCaret();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currInd > 0)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = storageImage[currInd - 1];
                currInd--;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currInd != -1 && currInd < storageImage.Count - 1)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = storageImage[currInd + 1];
                currInd++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //quit chuong trinh;
        }

        private string Khoitao()
        {

            // This evaluates the inputted expression and sends the text output
            // to the text callback (cbText).  It also returns a handle to the
            // result.  Use a colon to terminate the statement if you don't
            // want any output (a result is still returned).
            finalOutput = "";

            switch (stepMath)
            {
                case 0:
                    val = MapleEngine.EvalMapleStatement(kv, "a:=" + a + ";" + "b:=" + b + ";" + "c:=" + c + ";" + "d:=" + d + ";");
                    val = MapleEngine.EvalMapleStatement(kv, "y :=a*x^3+b*x^2+c*x+d;"
                        + "dhl:=diff(y, x);"
                        + "k:=4*b^2-12*a*c;");
                    //finalOutput += "a:=" + a + ";" + "b:=" + b + ";" + "c:=" + c + ";" + "d:=" + d + ";" + Environment.NewLine;
                    val = MapleEngine.EvalMapleStatement(kv, "latex(y);");
                    finalOutput += "\\text{Ham so ban vua nhap la : y = }" + stepOutput +
                        "\\\\ \\text{TXD: D=R} \\\\";
                    val = MapleEngine.EvalMapleStatement(kv, "latex(Limit(y, x = -infinity) = limit(y, x = -infinity));");
                    finalOutput += "\\text{Cac gioi han vo cuc la: }" +
                        stepOutput + ", ";
                    val = MapleEngine.EvalMapleStatement(kv, "latex(Limit(y, x = infinity) = limit(y, x = infinity));");
                    finalOutput += stepOutput + "\\\\";
                    val = MapleEngine.EvalMapleStatement(kv, "latex(dhl);");
                    finalOutput += "\\text{Dao ham: y' = }" + stepOutput + "\\\\";
                    stepMath++;
                    break;
                case 1:
                    val = MapleEngine.EvalMapleStatement(kv, "k;");
                    if(Convert.ToInt32(stepOutput) > 0)
                    {
                        val = MapleEngine.EvalMapleStatement(kv, "x1:=min(solve(dhl=0,x)); x2:=max(solve(dhl=0,x)); y1:=a*x1^3+b*x1^2+c*x1+d; y2:=a*x2^3+b*x2^2+c*x2+d; u:=solve(dif(dhl,x)=0); yu:=a*u^3+b*u^2+c*u+d;");
                        finalOutput += "\\text{Phuong trinh y'=0 co cac nghiem lan luot la: }";
                        val = MapleEngine.EvalMapleStatement(kv, "x1;");
                        finalOutput += stepOutput + ", ";
                        val = MapleEngine.EvalMapleStatement(kv, "x2;");
                        finalOutput += stepOutput + "\\\\ \\text{Hs DB tren tung khoang (}";
                        val = MapleEngine.EvalMapleStatement(kv, "latex(-infinity);");
                        finalOutput += stepOutput + ", ";
                        val = MapleEngine.EvalMapleStatement(kv, "x1;");
                        finalOutput += stepOutput + "), (";
                        val = MapleEngine.EvalMapleStatement(kv, "x2);");
                        finalOutput += stepOutput + ", ";
                        val = MapleEngine.EvalMapleStatement(kv, "latex(infinity)");
                        finalOutput += stepOutput + "\\text{) Va Hs NB tren (}";
                        val = MapleEngine.EvalMapleStatement(kv, "x1;");
                        finalOutput += stepOutput + ", ";
                        val = MapleEngine.EvalMapleStatement(kv, "x2;");
                        finalOutput += stepOutput + ") \\\\";
                    }
                    else
                    {
                        val = MapleEngine.EvalMapleStatement(kv, "u:=solve(diff(dhl,x)=0); yu:=a*u^3+b*u^2+c*u+d;");
                        if(Convert.ToInt32(a) < 0)
                        {
                            finalOutput += "\\text{Hs luon NB tren R} \\\\";
                        }
                        else
                        {
                            finalOutput += "\\text{Hs luon DB tren R} \\\\";
                        }
                    }
                    stepMath++;
                    break;
                case 2:
                    val = MapleEngine.EvalMapleStatement(kv, "k;");
                    if(Convert.ToInt32(stepOutput) > 0)
                    {
                        finalOutput += "\\text{Diem cuc tieu la: (}";
                        val = MapleEngine.EvalMapleStatement(kv, "x1;");
                        finalOutput += stepOutput + ", ";
                        val = MapleEngine.EvalMapleStatement(kv, "y1;");
                        finalOutput += stepOutput + ") \\\\ \\text{Va diem cuc dai la: (}";
                        val = MapleEngine.EvalMapleStatement(kv, "x2;");
                        finalOutput += stepOutput + ", ";
                        val = MapleEngine.EvalMapleStatement(kv, "y2;");
                        finalOutput += stepOutput + ")";
                    }
                    else
                    {
                        finalOutput += "\\text{Hs khong dat cuc tri tren R} \\\\";

                    }
                    stepMath++;
                    break;
                case 3:
                    finalOutput += "\\text{Diem uon: (}";
                    val = MapleEngine.EvalMapleStatement(kv, "u;");
                    finalOutput += stepOutput + ", ";
                    val = MapleEngine.EvalMapleStatement(kv, "yu;");
                    finalOutput += stepOutput + ") \\\\";
                    finalOutput += "\\text{Do thi co tam doi xung la diem uon} \\\\";
                    stepMath++;
                    break;
                case 4:
                    finalOutput += "\\text{Do thi giao voi Ox tai diem: (}";
                    val = MapleEngine.EvalMapleStatement(kv, "latex(fsolve(y=0));");
                    finalOutput += stepOutput + ",0) \\\\ \\text{Do thi giao voi Oy tai diem: (0,}" + d + ") \\\\";
                    stepMath++;
                    break;
                case 5:
                    //val = MapleEngine.EvalMapleStatement(kv, "plot(y, color=red);");
                    //finalOutput += stepOutput;
                    
                    stepMath++;
                    break;
            }

            return finalOutput;
        }

        private string GetGifFilePath()
        {
            return Path.Combine(Path.GetTempPath(), "Eq2ImgWinForms_" + (stepMath - 1).ToString() + ".gif");
        }

        private string CalltheAnswer(string temp)
        {
            string meow;
            List<string> greetings = new List<string>(new string[] { "hello", "hi", "xin chào", "chào"});
            List<string> goodbyeph = new List<string>(new string[] { "bye", "tạm biệt", "hẹn gặp lại", "good bye", "see ya"});

            if (greetings.Contains(temp.ToLower()))
            {
                return "nahdgdfjjs";
            }
            else if (temp.Contains("x^3"))
            {
                List<char> formula = new List<char>(new char[] { ' ', 'x', 'y', '^', '+', '-', '*', '/', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '='});
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

                return tmp;
            }
            else if(goodbyeph.Contains(temp.ToLower()))
            {
                Random randNum = new Random();
                MapleEngine.StopMaple(kv);
                return goodbyeph[randNum.Next(0, goodbyeph.Count)];
            }
            else if(temp.Contains("1") && tmp != "")
            {
                stepMath = 0;
                string luuSo = "";
                List<char> numFind = new List<char>(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' , '/'});
                
                //tim a
                int i = tmp.IndexOf("x^3");
                int j = i - 1;
                if(j >= 0)
                {
                    if (tmp[j] == '*')
                    {
                        j--;
                        i--;
                    }
                }
                while(j >= 0 && numFind.Contains(tmp[j]) )
                {
                    j--;
                }
                j++;
                if (j == i) luuSo = "1";
                else luuSo = tmp.Substring(j, i - j);
                if (j > 0)
                {
                    if (tmp[j - 1] == '-') luuSo = "-" + luuSo;
                    else luuSo = "+" + luuSo;
                }
                else luuSo = "+" + luuSo;
                a = luuSo;
                //end a

                //tim b
                if (tmp.Contains("x^2"))
                {
                    i = tmp.IndexOf("x^2");
                    j = i - 1;
                    if (j >= 0)
                    {
                        if (tmp[j] == '*')
                        {
                            j--;
                            i--;
                        }
                    }
                    while (j >= 0 && numFind.Contains(tmp[j]))
                    {
                        j--;
                    }
                    j++;
                    if (j == i) luuSo = "1";
                    else luuSo = tmp.Substring(j, i - j);
                    if (j > 0)
                    {
                        if (tmp[j - 1] == '-') luuSo = "-" + luuSo;
                        else luuSo = "+" + luuSo;
                    }
                    else luuSo = "+" + luuSo;
                    b = luuSo;
                }
                //end b

                //tim c
                i = 0;
                while(i < tmp.Count())
                {
                    if ((tmp[i] == 'x') && (i + 1 == tmp.Count())) break;
                    else if (tmp[i] == 'x' && tmp[i + 1] != '^') break;
                    i++;
                }
                if(i < tmp.Count())
                {
                    j = i - 1;
                    if (j >= 0)
                    {
                        if (tmp[j] == '*')
                        {
                            j--;
                            i--;
                        }
                    }
                    while (j >= 0 && numFind.Contains(tmp[j]))
                    {
                        j--;
                    }
                    j++;
                    if (j == i) luuSo = "1";
                    else luuSo = tmp.Substring(j, i - j);
                    if (j > 0)
                    {
                        if (tmp[j - 1] == '-') luuSo = "-" + luuSo;
                        else luuSo = "+" + luuSo;
                    }
                    else luuSo = "+" + luuSo;
                    c = luuSo;
                }
                //end c

                //tim d
                for(i = 0; i < tmp.Count(); i++)
                {
                    j = i - 1;
                    if(numFind.Contains(tmp[i]))
                    {
                        j = i;
                        while(j < tmp.Count())
                        {
                            if (numFind.Contains(tmp[j])) j++;
                            else break;
                        }
                        if(j < tmp.Count())
                        {
                            if (tmp[j] == 'x' || tmp[j] == '*') j = i - 1;
                        }
                        if(j > 1)
                        {
                            if (tmp[j - 2] == '^') j = i - 1;
                        }
                    }
                    if(j >= i)
                    {
                        luuSo = tmp.Substring(i, j - i);
                        if (i > 0)
                        {
                            if (tmp[i - 1] == '-') luuSo = "-" + luuSo;
                            else luuSo = "+" + luuSo;
                        }
                        else luuSo = "+" + luuSo;
                        d = luuSo;
                        break;
                    }
                }
                //end d
                string dir = GetGifFilePath();
                string outputFinal = Khoitao();
                if (pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();

                if (outputFinal.Length > 0)
                {
                    richTextBox1.Text += Environment.NewLine;
                    try
                    {
                        IntPtr val = NativeMethods.CreateGifFromEq(outputFinal, dir);
                        pictureBox1.Image = Image.FromFile(dir);
                        storageImage.Add(Image.FromFile(dir));
                        currInd++;
                    }
                    catch (Exception e)
                    {
                        return e.ToString();
                    }
                }

                return "Den day ban biet lam chua? (Co/Khong)";
            }
            else if(temp.Contains("2") && tmp != "")
            {
                return "Thuc hien ngu nguoi 2 :)";
            }
            else if(tmp != "" && (temp.Contains("khong") || temp.Contains("Khong")))
            {
                if(stepMath == 5)
                {
                    tmp = "";
                    return "Tks kiu nha <3 Muon giup gi cu noi minh";
                }
                string dir = GetGifFilePath();
                string outputFinal = Khoitao();
                if (pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();

                if (outputFinal.Length > 0)
                {
                    richTextBox1.Text += Environment.NewLine;
                    try
                    {
                        IntPtr val = NativeMethods.CreateGifFromEq(outputFinal, dir);
                        pictureBox1.Image = Image.FromFile(dir);
                        storageImage.Add(Image.FromFile(dir));
                        currInd++;
                    }
                    catch (Exception e)
                    {
                        return e.ToString();
                    }
                }
                List<string> arrQues = new List<string>(new string[] { "Giup cau roi do, biet lam chua? ", "Sao cau ngu the, gio biet chua? ", "Toi day roi thi sao? " });
                Random randNum = new Random();
                return arrQues[randNum.Next(0, arrQues.Count)] + "(Co/Khong)";
            }
            else if(tmp != "" && ((temp.Contains("co") || temp.Contains("Co") || stepMath == 4)))
            {
                if (stepMath < 5)
                {
                    stepMath++;
                    return "Ban co muon giai bai toan tiep tuyen voi ham so nay khong? (Co/Khong)";
                }
                else if(stepMath == 5)
                {
                    //thuc hien bai toan no.2;
                    stepMath++;
                    return "Nhap hai toa do cua diem di qua tiep tuyen.";
                }
                else
                {
                    tmp = "";
                    return "Tks kiu ban <3 Con muon giai gi thi noi nhe!";
                }
            }            

            meow = "hiu hiu";

            return meow;
        }
    }
}
