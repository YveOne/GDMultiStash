using System;
using System.Windows.Forms;

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

        private bool _isDraggingMainStash = false;
        public bool IsDraggingMainStash => _isDraggingMainStash;

        //private string _tempDir = Path.Combine(Application.StartupPath, "_dragging");

        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {
            Common.Stash stash = (Common.Stash)item.RowObject;
            _isDraggingMainStash = Core.Stashes.IsMainStash(stash);

            object obj = base.StartDrag(olv, button, item);
            DragStart?.Invoke();

            // NOT IMPLEMENTED YET
            /*
            // so... dont let original transfer files be dragged to desktop/explorer
            // create a temp dire and copy/rename dragging transfer files into

            List<string> tempFiles = new List<string>();
            Directory.CreateDirectory(_tempDir);
            foreach (Common.Stash s in _handler.DraggingStashes)
            {

                string tmpFile = string.Format("{0} {1}", s.ID);
                tmpFile = string.Join("_", tmpFile.Split(Path.GetInvalidFileNameChars()));
                tmpFile = Path.Combine(_tempDir, tmpFile);
                File.Copy(s.FilePath, tmpFile);
                tempFiles.Add(tmpFile);
            }
            DataObject dataObj = (DataObject)obj;
            dataObj.SetData(DataFormats.FileDrop, tempFiles.ToArray());
            */

            return obj;
        }

        public override void EndDrag(object dragObject, DragDropEffects effect)
        {
            base.EndDrag(dragObject, effect);
            //Directory.Delete(_tempDir, true);
            DragEnd?.Invoke();
        }

    }

}
