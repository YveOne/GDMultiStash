using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using GDMultiStash.Common;
using GDMultiStash.Common.Objects;

using GrimDawnLib;

using GDMultiStash.Forms.ContextMenues.SortStashes.Criteria;
using GDMultiStash.Forms.ContextMenues.SortStashes.SortingStructures;
namespace GDMultiStash.Forms.ContextMenues.SortStashes
{
    internal class SortHandler
    {
        private static readonly SortedList<string, SortCriteria> criteriaList = new SortedList<string, SortCriteria>()
        {
            {"none", new NoneSortCriteria()},
            {"class", new ClassSortCriteria()},
            {"level", new LevelSortCriteria()},
            {"type", new TypeSortCriteria()},
            {"quality", new QualitySortCriteria()},
            {"rarity", new RaritySortCriteria()},
            {"set", new SetSortCriteria()},
        };

        private readonly List<StashObject> stashesIn;

        public SortHandler(IEnumerable<StashObject> stashes)
        {
            stashesIn = stashes.Where(stash => !stash.Locked && !stash.IsMainStash).ToList();
            foreach (var stash in stashesIn)
            {
                Global.FileSystem.BackupStashTransferFile(stash.ID);
            }
        }

        public void Sort(string sortPattern)
        {
            if (stashesIn.Count() == 0)
                return;

            sortPattern = sortPattern.Replace(@"\/", "{SLASH}");
            if (sortPattern.Split('/').Length != 2)
            {
                Console.Warning("Invalid sort pattern '" + sortPattern + "'\nValid pattern: '<group pattern>/<stash pattern>'");
                return;
            }
            sortPattern = sortPattern.Replace("{SLASH}", @"\/");

            var criteriaKeys = new Dictionary<string, string>();
            foreach (Match match in Regex.Matches(sortPattern, @"{(\w+)}", RegexOptions.IgnoreCase))
            {
                var kFound = match.Groups[1].Value;
                var kLower = kFound.ToLower();
                if (criteriaList.ContainsKey(kLower))
                {
                    criteriaKeys[match.Value] = kLower;
                }
                else
                {
                    Console.Warning("Unknown sort criteria '"+ kFound + "'");
                    return;
                }
            }

            var gdExpansion = stashesIn[0].Expansion;
            var stashWidth = stashesIn[0].Width; // all selected stashes should have same width and hight
            var stashHeight = stashesIn[0].Height;
            var foundAnyItem = false;

            SortingStructureBase sortStruct;
            if (criteriaKeys.Values.ToList().Contains("set"))
                sortStruct = new RecordSortingStructure();
            else
                sortStruct = new SizeSortingStructure();

            var sortStringsToDisplayStrings = new Dictionary<string , string>();

            foreach (var stash in stashesIn)
            {
                foreach (var tab in stash.Tabs)
                {
                    var removeTabItems = new List<GDIALib.Parser.Stash.Item>();
                    foreach (var item in tab.Items)
                    {
                        if (!Global.Database.GetItemInfo(item, out GlobalHandlers.DatabaseHandler.ItemInfo itemInfo))
                        {
                            continue;
                        }

                        // 1) getting sort uint keys
                        var criteriaValues = new Dictionary<string, uint>();
                        foreach (var criteriaKey in criteriaKeys.Values)
                            criteriaValues[criteriaKey] = criteriaList[criteriaKey].GetKey(itemInfo);
                        if (criteriaValues.Values.ToList().Contains(0))
                        {
                            continue; // contains one or more ignored keys
                        }

                        // 2) convert sortpattern to sortstring
                        string sortString = string.Join("_", criteriaValues.Select(kvp => criteriaList[kvp.Key].FormatKey(kvp.Value)));

                        // 3) convert sortpattern to displaystring
                        if (!sortStringsToDisplayStrings.ContainsKey(sortString))
                            sortStringsToDisplayStrings[sortString] = Regex.Replace(sortPattern, @"{\w+}",
                                m => criteriaList[criteriaKeys[m.Value]]
                                .GetText(criteriaValues[criteriaKeys[m.Value]]));

                        sortStruct.FillSortedList(sortString, item, itemInfo);
                        foundAnyItem = true;
                        removeTabItems.Add(item);
                    }
                    foreach(var item in removeTabItems)
                        tab.Items.Remove(item);
                }
            }

            if (!foundAnyItem)
                return;

            // loop sortStringsToDisplayStrings to get groups to create
            //                                   sortStr, grpname 
            var groupNames = new SortedDictionary<string, string>();
            //                                             grpName,           dict<sortStr, stashname> 
            var groupsSortedStashes = new SortedDictionary<string, SortedDictionary<string, string>>();
            foreach(var kvp in sortStringsToDisplayStrings)
            {
                var sortString = kvp.Key;
                var displayText = kvp.Value;
                var displayTextSplits = displayText.Replace(@"\/", "{SLASH}").Split('/');
                var groupName = displayTextSplits[0].Trim().Replace("{SLASH}", @"/");
                var stashName = displayTextSplits[1].Trim().Replace("{SLASH}", @"/");

                if (!groupsSortedStashes.ContainsKey(groupName))
                {
                    groupNames[sortString] = groupName;
                    groupsSortedStashes[groupName] = new SortedDictionary<string, string>();
                }
                groupsSortedStashes[groupName][sortString] = stashName;
            }

            var addedGroups = new List<StashGroupObject>();
            var addedStashes = new List<StashObject>();

            foreach(var kvp1 in groupNames)
            {
                var groupSortedKey = kvp1.Key;
                var groupName = kvp1.Value;

                var group = Global.Groups.CreateGroup(groupName);
                addedGroups.Add(group);
                foreach (var kvp2 in groupsSortedStashes[groupName])
                {
                    var sortString = kvp2.Key;
                    var stashName = kvp2.Value;

                    // first we need to create a list of tabs
                    var tabsQueue = sortStruct.GetTabsQueue(sortString, stashWidth, stashHeight);

                    // now we got all items inside the tabs queue
                    while (tabsQueue.Count != 0)
                    {
                        var stash = Global.Stashes.CreateStash(stashName, gdExpansion, GrimDawnGameMode.Both, 0, false);
                        stash.GroupID = group.ID;
                        int maxTabs = (int)TransferFile.GetStashInfoForExpansion(gdExpansion).MaxTabs;
                        for (int tabI = 0; tabI < maxTabs; tabI += 1)
                        {
                            if (tabsQueue.Count == 0) continue; // just skip...
                            stash.AddTab(tabsQueue[0]);
                            tabsQueue.RemoveAt(0);
                        }
                        stash.SaveTransferFile();
                        stash.LoadTransferFile();
                        addedStashes.Add(stash);
                    }
                }
            }

            // first save and load (also to refresh usage of transfer file)
            foreach (var stash in stashesIn)
            {
                stash.SaveTransferFile();
                stash.LoadTransferFile();
            }

            // delete empty stashes (but not if they are locked!)
            var deletedStashes = Global.Stashes.DeleteStashes(stashesIn, true); // true = only empty
            Global.Runtime.InvokeStashesRemoved(deletedStashes);

            // delete empty stash groups
            var emptyGroups = deletedStashes
                .Select(stash => stash.GroupID)
                .Distinct()
                .Where(grpId => Global.Stashes.GetStashesForGroup(grpId).Length == 0)
                .Select(grpId => Global.Groups.GetGroup(grpId))
                .Where(grp => grp != null)
                .ToArray();
            if (emptyGroups.Length != 0)
            {
                var deletedGroups = Global.Groups.DeleteGroups(emptyGroups);
                Global.Runtime.InvokeStashGroupsRemoved(deletedGroups);
            }
            

            // remove deleted stashes from stashesIn
            var remainingStashes = stashesIn
                .Where((s) => Global.Stashes.GetStash(s.ID) != null).ToList();
            stashesIn.Clear();
            stashesIn.AddRange(remainingStashes);

            // refresh rest
            Global.Configuration.Save();
            foreach (var stash in stashesIn)
                Global.Runtime.InvokeStashesContentChanged(stash, true);
            Global.Runtime.InvokeStashGroupsAdded(addedGroups);
            Global.Runtime.InvokeStashesAdded(addedStashes);
        }

    }
}
