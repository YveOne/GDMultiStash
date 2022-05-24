using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using GrimDawnLib;

using BrightIdeasSoftware;

namespace GDMultiStash.Forms
{
    class StashesDragSource : SimpleDragSource
    {
        private readonly StashesDragHandler _handler;

        public delegate void DragStartEventHandler();
        public event DragStartEventHandler DragStart;

        public delegate void DragEndEventHandler();
        public event DragEndEventHandler DragEnd;

        public StashesDragSource(StashesDragHandler handler)
        {
            _handler = handler;
        }

        private readonly List<Common.Stash> _draggingStashes = new List<Common.Stash>();
        public List<Common.Stash> DraggingStashes => _draggingStashes;

        private bool _isDraggingMainStash = false;
        public bool IsDraggingMainStash => _isDraggingMainStash;

        private int _originalIndex = -1;
        public int OriginalIndex => _originalIndex;

        private string _tempDir = Path.Combine(Application.StartupPath, "_dragging");

        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {

            Common.Stash stash = (Common.Stash)item.RowObject;
            _isDraggingMainStash = Core.Stashes.IsMainStash(stash);
            _originalIndex = item.Index;

            object obj = base.StartDrag(olv, button, item);
            DragStart?.Invoke();

            _draggingStashes.Clear();
            if (olv.SelectedItems.Count == 0)
            {
                _draggingStashes.Add(stash);
            }
            else
            {
                foreach (OLVListItem i in olv.SelectedItems)
                    _draggingStashes.Add((Common.Stash)i.RowObject);
            }



            // so... dont let original transfer files be dragged to desktop/explorer
            // create a temp dire and copy/rename dragging transfer files into

            List<string> tempFiles = new List<string>();
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
            Directory.CreateDirectory(_tempDir);
            foreach (Common.Stash s in _draggingStashes)
            {
                string transferName = string.Format("{0} - {1}", s.ID, s.Name);
                transferName = string.Join("_", transferName.Split(Path.GetInvalidFileNameChars()));
                transferName = Path.Combine(_tempDir, transferName);
                string transferExt;
                string transferFile;
                Console.WriteLine(transferName);
                if (!s.SC && !s.HC)
                {
                    transferExt = GrimDawn.GetTransferExtension(s.Expansion, GrimDawnGameMode.None);
                    transferFile = string.Format("{0} [no mode]{1}", transferName, transferExt);
                    File.Copy(s.FilePath, transferFile);
                    tempFiles.Add(transferFile);
                }
                else
                {
                    if (s.SC)
                    {
                        transferExt = GrimDawn.GetTransferExtension(s.Expansion, GrimDawnGameMode.SC);
                        transferFile = string.Format("{0} [softcore mode]{1}", transferName, transferExt);
                        File.Copy(s.FilePath, transferFile);
                        tempFiles.Add(transferFile);
                    }
                    if (s.HC)
                    {
                        transferExt = GrimDawn.GetTransferExtension(s.Expansion, GrimDawnGameMode.HC);
                        transferFile = string.Format("{0} [hardcore mode]{1}", transferName, transferExt);
                        File.Copy(s.FilePath, transferFile);
                        tempFiles.Add(transferFile);
                    }
                }
            }
            // feature still disabled
            //DataObject dataObj = (DataObject)obj;
            //dataObj.SetData(DataFormats.FileDrop, tempFiles.ToArray());
            

            return obj;
        }

        public override void EndDrag(object dragObject, DragDropEffects effect)
        {
            base.EndDrag(dragObject, effect);
            _draggingStashes.Clear();
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
            DragEnd?.Invoke();
        }

    }

}
