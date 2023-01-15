using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using GrimDawnLib;
using GDMultiStash.Common.Objects;

namespace GDMultiStash.Common
{

    internal class StashesZipFile : Utils.ZipFileWriter
    {
        public StashesZipFile()
        {
        }
        public void AddStash(StashObject stash)
        {
            string stashName = string.Join("_", stash.Name.Split(Path.GetInvalidFileNameChars()));
            string transferName = $"#{stash.ID} {stashName}";
            string expKey = stash.Expansion.ToString();
            string transferExt;
            transferExt = GrimDawn.GetTransferFileExtension(stash.Expansion, GrimDawnGameMode.SC);
            AddFile($"{expKey}/SoftCore/{transferName}{transferExt}", stash.FilePath);
            transferExt = GrimDawn.GetTransferFileExtension(stash.Expansion, GrimDawnGameMode.HC);
            AddFile($"{expKey}/HardCore/{transferName}{transferExt}", stash.FilePath);
        }
    }
}
