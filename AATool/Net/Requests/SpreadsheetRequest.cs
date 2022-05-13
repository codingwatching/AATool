﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AATool.Data.Players;

namespace AATool.Net.Requests
{
    public sealed class SpreadsheetRequest : NetRequest
    { 
        public static HashSet<string> DownloadedPages = new ();
        private readonly string sheet;
        private readonly string page;

        public SpreadsheetRequest(string sheet, string page = "0") : base (Paths.Web.GetSpreadsheetUrl(sheet, page))
        {
            this.sheet = sheet;
            this.page = page;
        }

        public override async Task<bool> DownloadAsync()
        {
            using var client = new HttpClient() { 
                Timeout = TimeSpan.FromMilliseconds(Protocol.Requests.TimeoutMs) 
            };
            try
            {
                //download leaderboard spreadsheet as csv string
                string response = await client.GetStringAsync(this.Url);
                if (this.HandleResponse(response))
                {
                    DownloadedPages.Add(this.page);
                    return true;
                }
                return false;
            }
            catch (OperationCanceledException)
            {
                //request canceled, nothing left to do here
            }
            catch (HttpRequestException)
            {
                //error getting response, safely move on
            }
            return false;
        }

        private bool HandleResponse(string csv)
        {
            if (this.sheet is Paths.Web.LeaderboardSpreadsheet)
                return Leaderboard.SyncRecords(this.page, csv);
            else if (this.sheet is Paths.Web.NicknameSpreadsheet)
                return Leaderboard.SyncNicknames(csv);
            return false;
        }
    }
}