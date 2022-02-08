using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZhongliShieldIndicator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);

        private Properties.Settings Set
        {
            get
            {
                return Properties.Settings.Default;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            TopMost = true;
            ShowInTaskbar = false;
            gdi = new MyGDIFramework.GdiSystem(this);
            gdi.Graphics.Clear(Color.Transparent);
            gdi.UpdateWindow();
            imgBarFilled = new Bitmap(Properties.Resources.bar_bg_glow, this.Size);
            imgBarNormal = new Bitmap(Properties.Resources.bar_bg, this.Size);
            imgFgShield = new Bitmap(Properties.Resources.bar_shield, rgnProgress.Size);
            imgFgSkill = new Bitmap(Properties.Resources.bar_skill, rgnProgress.Size);
            notifyIcon1.Icon = this.Icon;
            gdi.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gdi.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            renderTimer.Start();
            this.Top = Set.windowX;
            this.Left = Set.windowY;
            chkPlaySound.Checked = Set.playSound;
            whoisZhongli = Set.zhongliPos;
        }
        MyGDIFramework.GdiSystem gdi = null;
        Bitmap imgBarNormal = null;
        Bitmap imgBarFilled = null;


        Brush shieldBrush = new SolidBrush(Color.FromArgb(169,214,127,0));
        Brush skillBrush = new SolidBrush(Color.FromArgb(169, 214,204,0));

        Bitmap imgFgSkill, imgFgShield;

        // state
        int currentCharactor = -1;
        long shieldEndTime = 0;
        long skillEndTime = 0;
        long pressBeginTime = 0;
        bool isEPressed = false;
        long skillTime = 4000;

        // config
        int whoisZhongli = 1;
        long skillShortCdTime = 4000;
        long skillLongCdTime = 12000;
        long shieldTime = 20000;

        long longSkillTriggerTime = 800;
        long skillShortPrefixTime = 400;

        private int _topMostCd = 200;
        private void renderTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
            GetAsyncKeyState(Keys.D1);
            GetAsyncKeyState(Keys.D2);
            GetAsyncKeyState(Keys.D3);
            GetAsyncKeyState(Keys.D4);
            GetAsyncKeyState(Keys.E);
            PlaySounds();
            Graphics g = gdi.Graphics;
            g.Clear(Color.Transparent);
            RenderUI(g);
            gdi.UpdateWindow();
            _topMostCd--;
            if(_topMostCd < 0)
            {
                _topMostCd = 200;
                TopMost = true;
            }
        }
        void UpdateStatus()
        {
            if (GetAsyncKeyState(Keys.D1))
            {
                currentCharactor = 1;
            }
            if (GetAsyncKeyState(Keys.D2))
            {
                currentCharactor = 2;
            }
            if (GetAsyncKeyState(Keys.D3))
            {
                currentCharactor = 3;
            }
            if (GetAsyncKeyState(Keys.D4))
            {
                currentCharactor = 4;
            }
            if(currentCharactor != whoisZhongli)
            {
                return;
            }
            bool canPressE = skillEndTime < SysClock;
            if (!canPressE)
            {
                return;
            }
            bool keyEState = GetAsyncKeyState(Keys.E);
            if (keyEState && !isEPressed)
            {
                pressBeginTime = SysClock;
                isEPressed = true;
            }
            if(isEPressed && SysClock - pressBeginTime > longSkillTriggerTime)
            {
                skillEndTime = SysClock + skillLongCdTime;
                shieldEndTime = SysClock + shieldTime;
                skillTime = skillLongCdTime;
                isEPressed=false;
                return;
            }
            if (isEPressed && !keyEState)
            {
                isEPressed = false;
                skillTime = skillShortCdTime;
                skillEndTime = pressBeginTime+ skillShortPrefixTime + skillShortCdTime;
            }
            
        }
        void RenderUI(Graphics g)
        {

            bool canPressE = skillEndTime < SysClock;
            if (canPressE)
            {
                g.DrawImage(imgBarFilled, 0, 0);
            }
            else
            {
                g.DrawImage(imgBarNormal, 0, 0);
            }
            long skillDuration = skillEndTime - SysClock;
            long shieldDuration = shieldEndTime - SysClock;
            Rectangle srcRect = Rectangle.Empty;
            Rectangle dstRect = Rectangle.Empty;
            if (shieldDuration > 0)
            {
                int w = (int)(rgnProgress.Width * (shieldDuration) / (shieldTime));
                if (w > rgnProgress.Width)
                {
                    w = rgnProgress.Width;
                }
                srcRect.Width = w;
                srcRect.Height = rgnProgress.Height;
                dstRect.X = rgnProgress.Left;
                dstRect.Y = rgnProgress.Top;
                dstRect.Width = w;
                dstRect.Height = rgnProgress.Height;
                g.DrawImage(imgFgShield, dstRect, srcRect, GraphicsUnit.Pixel);
            }
            if (skillDuration > 0)
            {
                int w = (int)(rgnProgress.Width * (skillDuration) / (skillTime));
                if(w > rgnProgress.Width)
                {
                    w = rgnProgress.Width;
                }
                srcRect.Width = w;
                srcRect.Height = rgnProgress.Height;
                dstRect.X = rgnProgress.Left;
                dstRect.Y = rgnProgress.Top;
                dstRect.Width = w;
                dstRect.Height = rgnProgress.Height;
                g.DrawImage(imgFgSkill, dstRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        int lastSec = 0;
        bool lastShieldStatus = true;
        bool lastSkillStatus = true;
        void PlaySounds()
        {
            bool nowSkillStatus = skillEndTime < SysClock;
            bool nowShieldStatus = shieldEndTime < SysClock;
            if(nowSkillStatus && !lastSkillStatus)
            {
                PlaySfx(Program.channelSfxAvailable);
            }
            lastSkillStatus = nowSkillStatus;

            if(!lastShieldStatus && nowShieldStatus)
            {
                PlaySfx(Program.channelSfxBroken);
            }
            lastShieldStatus = nowShieldStatus;

            int nowSec = (int)((shieldEndTime - SysClock) / 1000);
            if(nowSec <=4 && nowSec >=0 && lastSec - 1 == nowSec)
            {
                PlaySfx(Program.channelSfxWarning);
            }
            lastSec = nowSec;
        }

        private void PlaySfx(int handle)
        {
            if (!chkPlaySound.Checked) { return; }
            if(!Un4seen.Bass.Bass.BASS_ChannelPlay(handle, true))
            {
                Console.WriteLine(Un4seen.Bass.Bass.BASS_ErrorGetCode());
            }
        }

        Point moveOrigin = Point.Empty;
        bool isMoving = false;
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                moveOrigin = e.Location;
                isMoving = true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                this.Top += e.Y - moveOrigin.Y;
                this.Left += e.X - moveOrigin.X;
            }
        }

        DateTime _dateOrigin = DateTime.Now; 
        long SysClock
        {
            get
            {
                return (long)(DateTime.Now - _dateOrigin).TotalMilliseconds;
            }
        }

        private void 位置1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = ((ToolStripMenuItem)sender).Text.Last() - '0';
            whoisZhongli = i;
            位置1ToolStripMenuItem.Checked = false;
            位置2ToolStripMenuItem.Checked = false;
            位置3ToolStripMenuItem.Checked = false;
            位置4ToolStripMenuItem.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Set.windowX = this.Top;
            Set.windowY = this.Left;
            Set.playSound = chkPlaySound.Checked;
            Set.zhongliPos = whoisZhongli;
            Set.Save();
        }

        private void 重置小部件位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Top = Screen.GetWorkingArea(this).Height * 8 / 10;
            this.Left = Screen.GetWorkingArea(this).Width / 2 - this.Width / 2; 
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                isMoving = false;
            }
        }
    }
}
