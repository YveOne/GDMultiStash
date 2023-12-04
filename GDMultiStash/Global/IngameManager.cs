using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GDMultiStash.Global
{
    using GrimDawnLib;
    using Ingame;
    namespace Ingame
    {

    }

    internal partial class IngameManager : Base.Manager
    {

        private bool _stashReopening = false;
        protected bool _characterMovementDisabled = false;

        public bool StashIsReopening => _stashReopening;

        private void ReopenStashAction(Action action)
        {
            Console.WriteLine("ReopenStashAction() START!");

            ushort keyEscape = (ushort)Native.Keyboard.KeyToScanCode(Keys.Escape);
            ushort keyInteract = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.Interact).Primary;

            bool _transferStashSaved = false;
            void TransferStashSavedHandler(object sender, EventArgs e) => _transferStashSaved = true;
            G.Runtime.TransferFileSaved += TransferStashSavedHandler;

            Console.WriteLine("ReopenStashAction() - sending escape key");
            Native.Keyboard.SendKey(keyEscape);

            Console.WriteLine("ReopenStashAction() - waiting for stash beeing saved to file");
            bool closedSuccessfully = Utils.Funcs.WaitFor(() => !G.Runtime.StashIsOpened && _transferStashSaved, 5000, 33);
            G.Runtime.TransferFileSaved -= TransferStashSavedHandler;
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

            if (!Utils.Funcs.WaitFor(() => G.Runtime.StashIsOpened, 2000, 33))
            {   // reopening failed
                Console.WriteLine("ReopenStashAction() - Warning: stash not reopened");
                G.Runtime.InvokeStashClosed();
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
            if (stashID == G.Stashes.ActiveStashID) return;
            ReopenStash(() => G.Stashes.SwitchToStash(stashID));
        }

        public void SwitchToMainStash()
        {
            int mainStashID = G.Configuration.GetMainStashID(G.Runtime.ActiveExpansion, G.Runtime.ActiveMode);
            if (mainStashID != G.Stashes.ActiveStashID)
            {
                ReopenStash(() => {
                    G.Stashes.SwitchToStash(mainStashID);
                    G.StashGroups.ActiveGroupID = 0;
                });
            }
            else
            {
                G.StashGroups.ActiveGroupID = 0;
            }
        }

        public void SaveCurrentStash()
        {
            ReopenStash(() => {
                G.Stashes.ImportStash(G.Stashes.ActiveStashID, G.Configuration.Settings.SaveOverwritesLocked);
                G.Stashes.ExportStash(G.Stashes.ActiveStashID);
            });
        }

        public void LoadCurrentStash()
        {
            ReopenStash(() => G.Stashes.ExportStash(G.Stashes.ActiveStashID));
        }

        public void ReloadCurrentStash()
        {
            string file = GrimDawn.GetTransferFilePath(G.Runtime.ActiveExpansion, G.Runtime.ActiveMode);
            string temp = Utils.Funcs.GetTempFileName();
            File.Copy(file, temp);
            ReopenStash(() => {
                if (File.Exists(file))
                    File.Delete(file);
                G.FileSystem.Watcher.SkipNextFile(Path.GetFileName(file));
                File.Move(temp, file);
            });
        }

        public void EnableMovement()
        {
            if (!_characterMovementDisabled) return;
            _characterMovementDisabled = false;
            ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
            Native.Keyboard.SendKeyUp(k);
            InvokeCharacterMovementEnabled();
        }

        public void DisableMovement()
        {
            if (_characterMovementDisabled) return;
            _characterMovementDisabled = true;
            ushort k = GrimDawn.Keybindings.GetKeyBinding(GrimDawnKey.StationaryAttack).Primary;
            Native.Keyboard.SendKeyDown(k);
            InvokeCharacterMovementDisabled();
        }

    }
}
