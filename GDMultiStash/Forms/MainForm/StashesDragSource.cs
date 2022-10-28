using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly List<GlobalHandlers.StashObject> _draggingStashes = new List<GlobalHandlers.StashObject>();
        public List<GlobalHandlers.StashObject> DraggingStashes => _draggingStashes;

        private bool _isDraggingMainStash = false;
        public bool IsDraggingMainStash => _isDraggingMainStash;

        private readonly List<GlobalHandlers.StashObject> _originalOrderedModels = new List<GlobalHandlers.StashObject>();
        public List<GlobalHandlers.StashObject> OriginalOrderedModels => _originalOrderedModels;

        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {
            _draggingStashes.Clear();
            foreach (OLVListItem i in olv.SelectedItems)
            {
                GlobalHandlers.StashObject stash = (GlobalHandlers.StashObject)i.RowObject;
                _draggingStashes.Add(stash);
            }
            _originalOrderedModels.Clear();
            foreach (OLVListItem i in olv.Items)
            {
                GlobalHandlers.StashObject stash = (GlobalHandlers.StashObject)i.RowObject;
                _originalOrderedModels.Add(stash);
            }

            // dont let original transfer files be dragged to desktop/explorer
            // create temp zip file

            Common.ExportZipFile zipFile = new Common.ExportZipFile();
            foreach (GlobalHandlers.StashObject s in _draggingStashes) zipFile.AddStash(s);
            zipFile.SaveTo(Path.Combine(Path.GetTempPath(), "transfer." + Path.ChangeExtension(Guid.NewGuid().ToString(), ".zip")));


            object obj = base.StartDrag(olv, button, item);
            DragStart?.Invoke();

            DataObject dataObj = (DataObject)obj;
            dataObj.SetData(DataFormats.FileDrop, new string[] { zipFile.DstFile });

            return obj;
        }

        public override void EndDrag(object dragObject, DragDropEffects effect)
        {
            base.EndDrag(dragObject, effect);
            DragEnd?.Invoke();
            _draggingStashes.Clear();
            _originalOrderedModels.Clear();
        }

    }

}
