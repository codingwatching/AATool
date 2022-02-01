﻿using System.Collections.Generic;
using AATool.Data.Objectives;

namespace AATool.Data.Categories 
{
    public class AdventuringTime : SingleAdvancement
    {
        private const string Id = "minecraft:adventure/adventuring_time";

        public static readonly List<string> SupportedVersions = new () {
            "1.18"
        };

        public AdventuringTime() : base()
        {
            this.Name      = "Adventuring Time";
            this.Acronym   = "AT";
            this.Objective = "Biomes";
            this.Action    = "Visited";
        }

        public override int GetTargetCount()
        {
            return Tracker.TryGetAdvancement(Id, out this.Advancement)
                ? this.Advancement.Criteria.Count
                : 0;
        }

        public override int GetCompletedCount()
        {
            return Tracker.TryGetAdvancement(Id, out this.Advancement)
                ? this.Advancement.Criteria.MostCompleted
                : 0;
        }

        public override IEnumerable<string> GetSupportedVersions() => SupportedVersions;
    }
}