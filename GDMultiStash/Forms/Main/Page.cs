﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMultiStash.Forms.Main
{
    internal partial class Page : Form
    {
        protected readonly MainForm Main;

        public Page(MainForm mainForm)
        {
            InitializeComponent();
            Main = mainForm;
        }

        public Page()
        { // used for Designer
            InitializeComponent();
        }

        protected virtual void Localize(GlobalHandlers.LocalizationHandler.StringsHolder L)
        {
        }

        public void Localize()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Localize(); });
                return;
            }
            Localize(Global.L);
        }

    }
}
