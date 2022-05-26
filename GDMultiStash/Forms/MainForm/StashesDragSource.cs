using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;

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

        private readonly List<Common.Stash> _originalOrderedModels = new List<Common.Stash>();
        public List<Common.Stash> OriginalOrderedModels => _originalOrderedModels;



        private string _tempDragFile = null;



        public override object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item)
        {
            _draggingStashes.Clear();
            foreach (OLVListItem i in olv.SelectedItems)
            {
                Common.Stash stash = (Common.Stash)i.RowObject;
                _draggingStashes.Add(stash);
            }
            _originalOrderedModels.Clear();
            foreach (OLVListItem i in olv.Items)
            {
                Common.Stash stash = (Common.Stash)i.RowObject;
                _originalOrderedModels.Add(stash);
            }




            // dont let original transfer files be dragged to desktop/explorer
            // create temp zip file

            Dictionary<string, string> files2zip = new Dictionary<string, string>();
            foreach (Common.Stash s in _draggingStashes)
            {
                string transferName = string.Format("{0} - {1}", s.ID, s.Name);
                transferName = string.Join("_", transferName.Split(Path.GetInvalidFileNameChars()));

                string transferExt;
                string transferFile;
                if (!s.SC && !s.HC)
                {
                    transferExt = GrimDawn.GetTransferExtension(s.Expansion, GrimDawnGameMode.None);
                    transferFile = string.Format("{0} [no mode]{1}", transferName, transferExt);
                    files2zip.Add(s.FilePath, transferFile);
                }
                else
                {
                    if (s.SC)
                    {
                        transferExt = GrimDawn.GetTransferExtension(s.Expansion, GrimDawnGameMode.SC);
                        transferFile = string.Format("{0} [softcore mode]{1}", transferName, transferExt);
                        files2zip.Add(s.FilePath, transferFile);
                    }
                    if (s.HC)
                    {
                        transferExt = GrimDawn.GetTransferExtension(s.Expansion, GrimDawnGameMode.HC);
                        transferFile = string.Format("{0} [hardcore mode]{1}", transferName, transferExt);
                        files2zip.Add(s.FilePath, transferFile);
                    }
                }
            }

            string tempZipFile = Path.Combine(Path.GetTempPath(), "transfer." + Path.ChangeExtension(Guid.NewGuid().ToString(), ".zip"));
            using (FileStream zipToOpen = new FileStream(tempZipFile, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach(KeyValuePair<string,string> kvp in files2zip)
                    {
                        string tFile = kvp.Key;
                        string tName = kvp.Value;

                        ZipArchiveEntry readmeEntry = archive.CreateEntry(tName);
                        using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                        {
                            byte[] buffer = File.ReadAllBytes(tFile);
                            writer.BaseStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }








            object obj = base.StartDrag(olv, button, item);
            DragStart?.Invoke();

            DataObject dataObj = (DataObject)obj;
            dataObj.SetData(DataFormats.FileDrop, new string[] { tempZipFile });

            return obj;
        }

        public override void EndDrag(object dragObject, DragDropEffects effect)
        {
            base.EndDrag(dragObject, effect);
            _draggingStashes.Clear();
            _originalOrderedModels.Clear();
            DragEnd?.Invoke();
        }

    }

}
