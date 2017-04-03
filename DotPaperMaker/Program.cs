using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotPaperMaker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            run();
             
            //Keep the log open
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        [STAThread]
        static void run()
        {
            //╔╗╚╝╟╢║═─╠╣○●◌ ┬┴╧╙╜
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║     Welcome to Dot Paper Maker     ║");
            Console.WriteLine("║                           v0.1     ║");
            Console.WriteLine("╟────────────────────────────────────╢");
            Console.WriteLine("║            by Ariel Lothlorien     ║");
            Console.WriteLine("╠════════════════════════════════════╣");

            Bitmap map = new Bitmap(33 * 9 + 1 + 2, 33 * 95 + 2);
            Bitmap zeroImg = null;
            if (File.Exists("0.bmp")) zeroImg = new Bitmap("0.bmp");
            Bitmap oneImg = null;
            if (File.Exists("1.bmp")) oneImg = new Bitmap("1.bmp");
            Bitmap readImg = null;
            if (File.Exists("r.bmp")) readImg = new Bitmap("r.bmp");

            if (zeroImg == null || oneImg == null || readImg == null)
            {
                if (zeroImg == null)
                {
                    Console.WriteLine("║ - Error loading 0.bmp              ║");
                }
                if (oneImg == null)
                {
                    Console.WriteLine("║ - Error loading 1.bmp              ║");
                }
                if (readImg == null)
                {
                    Console.WriteLine("║ - Error loading r.bmp              ║");
                }
                Console.WriteLine("╟────────────────────────────────────╢");
                Console.WriteLine("║ FATAL ERROR TERMINATING TRANSPILE  ║");
                Console.WriteLine("╚════════════════════════════════════╝");
                return;
            }

            //Settings
            int TileSizeX = 33, TileSizeY = 33;

            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.InitialDirectory = Directory.GetCurrentDirectory();
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Console.WriteLine("║ + Loading File                     ║");
                System.IO.BinaryReader r = null;
                try
                {
                    r = new System.IO.BinaryReader(File.Open(open.FileName, FileMode.Open));
                } catch(IOException e)
                {
                    if (e.Message.Contains("because it is being used by another process.")) //		Message	"The process cannot access the file 'C:\\Learning\\CPU\\Projects\\DotPaperMaker\\DotPaperMaker\\bin\\Debug\\0.bmp' because it is being used by another process."	string
                    {
                        Console.WriteLine("║ - File Load Failed (File in use)   ║");
                        Console.WriteLine("╚════════════════════════════════════╝");
                        return;
                    }

                    Console.WriteLine("║ - File Load Failed                 ║");
                    Console.WriteLine("╚════════════════════════════════════╝");
                    return;
                }
                Console.WriteLine("║ + File Loaded                      ║");
                Console.WriteLine("║ + Writeing File to Dot Paper Img   ║");
                Console.WriteLine("╟────┬───────────────────┬───────────╜");

                //Console.WriteLine("║ tt │ ○ ● ○ ○ · ● ● ● ○ │");
                string line = "";
                byte t;
                byte mask;
                char one = '*', zero = 'O';
                Graphics g = Graphics.FromImage(map);

                g.DrawLine(Pens.Black, 0, 0, 1 + 9 * TileSizeX - 1, 0);

                int h = 0;
                while (h < 95)
                {
                    line = "║ " + String.Format("{0,2}", h) + " │ ";
                    t = r.ReadByte();
                    //for (byte mask = 1 << 7; mask != 0; mask = (byte)((int)(mask) >> 1))
                    //{
                    //    if (mask == 8) line += "· ";
                    //    if ((t & mask) != 0) line += "● ";
                    //    else line += "○ ";
                    //}

                    g.DrawLine(Pens.Black, 0, 1 + h * TileSizeY, 0, 1 + (h + 1) * TileSizeY - 1);
                    //g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 0 * TileSizeX, 1 + h * TileSizeY);

                    mask = 128; // 1 0 0 0  0 0 0 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 0 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 64; //  0 1 0 0  0 0 0 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 1 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 32; //  0 0 1 0  0 0 0 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 2 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 16; //  0 0 0 1  0 0 0 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 3 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    line += "· ";
                    g.DrawImage(readImg, 1 + 4 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 8; //   0 0 0 0  1 0 0 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 5 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 4; //   0 0 0 0  0 1 0 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 6 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 2; //   0 0 0 0  0 0 1 0
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 7 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    mask = 1; //   0 0 0 0  0 0 0 1
                    line += (t & mask) != 0 ? one + " " : zero + " ";
                    g.DrawImage((t & mask) != 0 ? oneImg : zeroImg, 1 + 8 * TileSizeX, 1 + h * TileSizeY, TileSizeX, TileSizeY);

                    line += "│";
                    g.DrawLine(Pens.Black, 1 + 9 * TileSizeX - 1, 1 + h * TileSizeY, 1 + 9 * TileSizeX - 1, 1 + (h + 1) * TileSizeY - 1);

                    Console.WriteLine(line);

                    //Confirm there is more in the buffer
                    h++;
                }
                g.DrawLine(Pens.Black, 0, 1 + (h + 1) * TileSizeY - 1, 1 + 9 * TileSizeX - 1, 1 + (h + 1) * TileSizeY - 1); //ToDo this line is not showing up in the right place
                //Console.WriteLine("║ 00 │ ○ ● ○ ○ · ● ● ● ○ │");
                Console.WriteLine("╟────┴───────────────────┴───────────╖");
                Console.WriteLine("║ + Ready to save target             ║");
                Console.WriteLine("╟────────────────────────────────────╢");
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = Directory.GetCurrentDirectory();
                if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Console.WriteLine("║ + Writing file to disk             ║");
                    try
                    {
                        map.Save(save.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch
                    {
                        Console.WriteLine("║ - File Save Error (May be in use?) ║");
                        Console.WriteLine("╟────────────────────────────────────╢");
                        Console.WriteLine("║   Aborting Output Save Operation   ║");
                        Console.WriteLine("╚════════════════════════════════════╝");
                    }
                    Console.WriteLine("║ + File Saved (probably)            ║");
                    Console.WriteLine("╚════════════════════════════════════╝");
                } else
                {
                    Console.WriteLine("║ - No output location selected      ║");
                    Console.WriteLine("╟────────────────────────────────────╢");
                    Console.WriteLine("║   Aborting Output Save Operation   ║");
                    Console.WriteLine("╚════════════════════════════════════╝");
                }
            }
            else
            {
                Console.WriteLine("║ - File Load Failed                 ║");
                Console.WriteLine("╚════════════════════════════════════╝");
            }
        }
    }
}
