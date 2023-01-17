using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.GlobalHandlers
{
    internal partial class IngameHandler : Base.BaseHandler
    {

        private bool _stashReopening = false;

        public bool StashIsReopening => _stashReopening;

        private void ReopenStashAction(Action action)
        {
            Console.WriteLine("ReopenStashAction() START!");

            ushort keyEscape = (ushort)Native.Keyboard.KeyToScanCode(Keys.Escape);
            ushort keyInteract = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.Interact).Primary;

            bool _transferStashSaved = false;
            void TransferStashSavedHandler(object sender, EventArgs e) => _transferStashSaved = true;
            Global.Runtime.TransferFileSaved += TransferStashSavedHandler;

            Console.WriteLine("ReopenStashAction() - sending escape key");
            Native.Keyboard.SendKey(keyEscape);

            Console.WriteLine("ReopenStashAction() - waiting for stash beeing saved to file");
            bool closedSuccessfully = Utils.Funcs.WaitFor(() => !Global.Runtime.StashIsOpened && _transferStashSaved, 5000, 33);
            Global.Runtime.TransferFileSaved -= TransferStashSavedHandler;
            if (closedSuccessfully)
            {
                Console.WriteLine("ReopenStashAction() - stash saved to file");
            }
            else
            {
                Console.WriteLine("ReopenStashAction() - failed closing stash - Aborting!");
                return;
            }

            Console.WriteLine("ReopenStashAction() - action() START!");
            action();
            Console.WriteLine("ReopenStashAction() - action() DONE!");
            System.Threading.Thread.Sleep(100);

            Console.WriteLine("ReopenStashAction() - sending interact key");
            Native.Keyboard.SendKey(keyInteract);

            if (!Utils.Funcs.WaitFor(() => Global.Runtime.StashIsOpened, 2000, 33))
            {   // reopening failed
                Console.WriteLine("ReopenStashAction() - Warning: stash not reopened");
                Global.Runtime.InvokeStashClosed();
            }

            Console.WriteLine("ReopenStashAction() DONE!");
        }

        public void ReopenStash(Action action)
        {
            if (_stashReopening) return;
            _stashReopening = true;
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                InvokeStashReopenStart();
                ReopenStashAction(action);
                InvokeStashReopenEnd();
                _stashReopening = false;
            })).Start();
        }

        public void SwitchToStash(int stashID)
        {
            if (stashID == Global.Runtime.ActiveStashID) return;
            ReopenStash(() => Global.Stashes.SwitchToStash(stashID));
        }

        public void SwitchToMainStash()
        {
            int mainStashID = Global.Configuration.GetMainStashID(Global.Runtime.ActiveExpansion, Global.Runtime.ActiveMode);
            if (mainStashID != Global.Runtime.ActiveStashID)
            {
                ReopenStash(() => {
                    Global.Stashes.SwitchToStash(mainStashID);
                    Global.Runtime.ActiveGroupID = 0;
                });
            }
            else
            {
                Global.Runtime.ActiveGroupID = 0;
            }
        }

        public void SaveCurrentStash()
        {
            ReopenStash(() => {
                Global.Stashes.ImportStash(Global.Runtime.ActiveStashID, Global.Configuration.Settings.SaveOverwritesLocked);
                Global.Stashes.ExportStash(Global.Runtime.ActiveStashID);
            });
        }

        public void LoadCurrentStash()
        {
            ReopenStash(() => Global.Stashes.ExportStash(Global.Runtime.ActiveStashID));
        }

        public void ReloadCurrentStash()
        {
            string file = GrimDawn.GetTransferFilePath(Global.Runtime.ActiveExpansion, Global.Runtime.ActiveMode);
            string temp = Utils.Funcs.GetTempFileName();
            File.Copy(file, temp);
            ReopenStash(() => {
                if (File.Exists(file))
                    File.Delete(file);
                Global.FileSystem.Watcher.SkipNextFile(Path.GetFileName(file));
                File.Move(temp, file);
            });
        }

    }
}
