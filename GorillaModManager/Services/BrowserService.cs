using GameBananaAPI;
using GameBananaAPI.Data;
using GorillaModManager.Models;
using GorillaModManager.Models.Mods;
using GorillaModManager.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GorillaModManager.Services
{
    public class BrowserService
    {
        public BlockList blockList = new();
        public const string BlockListUrl = "https://raw.githubusercontent.com/pl2w/GorillaModManager/master/blocklist-ids.json";

        public BrowserService()
        {
            WebClient wb = new();
            blockList = JsonConvert.DeserializeObject<BlockList>(wb.DownloadString(BlockListUrl));
        }

        public async Task<IEnumerable<BrowserMod>> GetMods(int page)
        {
            if (API.gameId == -1)
                API.SetCurrentGame(9496);

            SubfeedData data = await API.GetSubfeedData(page, string.Empty, "Mod", string.Empty);

            List<RecordData> subData = data._aRecords;
            List<BrowserMod> modsToReturn = [];

            foreach (RecordData item in subData)
            {
                ProfilePageData profile = await API.GetModProfilePage(item._idRow);

                if (profile._sDescription == null || profile._sDescription == string.Empty || profile._sDescription.Length == 0)
                    continue;

                if (profile._bIsWithheld || profile._bIsTrashed || profile._bIsPrivate)
                    continue;

                if (blockList.blockedAuthors.Contains(item._aSubmitter._idRow))
                    continue;

                if (blockList.blockedPages.Contains(item._idRow))
                    continue;

                DateTime dt = DateUtils.UnixTimeStampToDateTime(profile._tsDateAdded);
                if (DateTime.Now < dt.AddDays(3))
                    continue;

                modsToReturn.Add(
                    new BrowserMod(
                        item._sName,
                        profile._sDescription,
                        item._aSubmitter._sName,
                        API.GetDownloadURL(profile._aFiles[0]._idRow),
                        API.GetCompleteImageURL(profile._aPreviewMedia._aImages[0]._sFile),
                        profile._nDownloadCount,
                        profile._nLikeCount,
                        profile._aRequirements,
                        profile._aFiles[0]._sMd5Checksum
                    )
                );
            }

            return modsToReturn;
        }
    }
}
