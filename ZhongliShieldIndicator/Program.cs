using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZhongliShieldIndicator
{
    internal static class Program
    {
        public const string appid = "com.zyfdroid.zsi_v100114514";
        public static void makeFileExists(byte[] data, String filename)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            if (!File.Exists(filename))
            {
                File.WriteAllBytes(filename + ".tmp", data);
                File.Move(filename + ".tmp", filename);
            }
        }

        public static int extractAndMakeChannel(byte[] data,String filename)
        {
            makeFileExists(data,filename);
            int chan = Un4seen.Bass.Bass.BASS_StreamCreateFile(filename, 0, new FileInfo(filename).Length, Un4seen.Bass.BASSFlag.BASS_DEFAULT);
            Console.WriteLine(Un4seen.Bass.Bass.BASS_ErrorGetCode());
            return chan;
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>

        public static int channelSfxWarning = 0;
        public static int channelSfxBroken = 0;
        public static int channelSfxAvailable = 0;

        [STAThread]
        static void Main()
        {
            String rootpath = Path.Combine(Path.GetTempPath(), appid, "libs");
            String soundpath = Path.Combine(rootpath, "sfx");
            makeFileExists(Properties.Resources.bass, Path.Combine(rootpath, "bass.dll"));
            if (Un4seen.Bass.Bass.LoadMe(Path.Combine(rootpath)))
            {
                Un4seen.Bass.Bass.BASS_Init(-1, 44100, Un4seen.Bass.BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
                channelSfxAvailable = extractAndMakeChannel(Properties.Resources.available, Path.Combine(soundpath, "available.ogg"));
                channelSfxBroken = extractAndMakeChannel(Properties.Resources._break, Path.Combine(soundpath, "broken.ogg"));
                channelSfxWarning = extractAndMakeChannel(Properties.Resources.warn, Path.Combine(soundpath, "warn.ogg"));
            }
            else
            {
                Console.WriteLine(Un4seen.Bass.Bass.BASS_ErrorGetCode());
            }


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
