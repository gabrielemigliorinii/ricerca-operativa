﻿using System;
using System.Windows.Forms;
using System.Threading;

namespace RicercaOperativa
{
    public partial class Loading : Form
    {
        private delegate void CloseDelegate();
        readonly System.Windows.Forms.Timer timer;

        public Loading()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = CONST.MinMsTimeout;
            progressBar.Width = 0;
        }

        public void InvokeControls(Action action)
        {
            if (InvokeRequired)
                Invoke(new CloseDelegate(action));
            else action();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            Run(delay: CONST.MinMsTimeout);
        }

        private void Run(int delay)
        {
            timer.Tick += delegate
            {
                InvokeControls(() => progressBar.Width += 10);
                if (progressBar.Width == 1200) 
                { 
                    Thread.Sleep(delay);
                    timer.Stop();
                    InvokeControls(Hide);
                    try { new Main().ShowDialog(); } catch (ThreadStateException e) { Console.WriteLine(e); }
                    InvokeControls(Close);
                }
            };
            timer.Start();
        }
    }
}
