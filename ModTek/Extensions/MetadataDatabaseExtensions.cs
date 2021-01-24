using BattleTech.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModTek.Util.Logger;

namespace ModTek.Extensions
{
    public static class MetadataDatabaseExtensions
    {
        public static void RemoveAll(this MetadataDatabase mdd, string tags)
        {

        }

        public static bool AddOrUpdate(this MetadataDatabase mdd, Tag_MDD tag)
        {

            bool success = false;

            Tag_MDD tag_MDD = mdd.Query<Tag_MDD>(
                "SELECT * FROM Tag WHERE Name = @TagName COLLATE NOCASE",
                new { TagName = tag.Name })
                .FirstOrDefault();

            if (tag_MDD == null)
            {
                try
                {
                    mdd.Execute("INSERT INTO Tag (Name, Important, PlayerVisible, FriendlyName, Description)" +
                        " VALUES (@TagName, @TagImportant, @TagPlayerVisible, @TagFriendlyName, @TagDescription)",
                        new
                        {
                            TagName = tag.Name,
                            TagImportant = tag.Important,
                            TagPlayerVisible = tag.PlayerVisible,
                            TagFriendlyName = tag.FriendlyName,
                            TagDescription = tag.Description
                        });
                    Log($"Inserted tag: {tag.Name}");
                    success = true;
                }
                catch (Exception e)
                {
                    Log($"Failed to insert tag: {tag.Name} into MDD due to: {e}");
                    return false;
                }
            }
            else
            {
                try
                {
                    mdd.Execute("UPDATE Tag (Important, PlayerVisible, FriendlyName, Description)" +
                            " VALUES (@TagImportant, @TagPlayerVisible, @TagFriendlyName, @TagDescription)" +
                            " WHERE Name = @TagName",
                            new
                            {
                                TagName = tag.Name,
                                TagImportant = tag.Important,
                                TagPlayerVisible = tag.PlayerVisible,
                                TagFriendlyName = tag.FriendlyName,
                                TagDescription = tag.Description
                            });
                    Log($"Updated tag: {tag.Name}");
                    success = true;
                }
                catch (Exception e)
                {
                    Log($"Failed to update tag: {tag.Name} in MDD due to: {e}");
                    return false;
                }
            }

            return success;
        }
    }
}