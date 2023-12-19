using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace NoDeadline
{
    [BepInPlugin("NoDeadline", "NoDeadline", "1.0.0")]
    public class NoDeadlineMod : BaseUnityPlugin
    {
        private void Awake()
        {
            if(NoDeadlineMod.instance == null)
            {
                NoDeadlineMod.instance = this;
            }
            this.harmony.PatchAll();
            base.Logger.LogInfo("Plugin No Deadline loaded!");
        }

        private const string modGUID = "NoDeadline";

        private const string modName = "NoDeadline";

        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony("NoDeadline");

        private static NoDeadlineMod instance;
    }

    [HarmonyPatch(typeof(TimeOfDay), "UpdateProfitQuotaCurrentTime")]
    public class InfiniteDeadline
    {
        [HarmonyPostfix]
        public static void SetInfiniteDeadline()
        {
            TimeOfDay.Instance.timeUntilDeadline = (float)((int)(TimeOfDay.Instance.totalTime * (float)TimeOfDay.Instance.quotaVariables.deadlineDaysAmount));
            StartOfRound.Instance.deadlineMonitorText.text = "DEADLINE:\n NONE";
        }
    }

    [HarmonyPatch(typeof(TimeOfDay), "SetBuyingRateForDay")]
    public class BuyingRate
    {
        [HarmonyPostfix]
        public static void SetBuyingRate()
        {
            StartOfRound.Instance.companyBuyingRate = 1f;
        }
    }

    [HarmonyPatch(typeof(HUDManager), "DisplayDaysLeft")]
    public class DisplayInfiniteLeft
    {
        [HarmonyPostfix]
        public static void SetDisplayInfiniteLeft()
        {
            HUDManager.Instance.profitQuotaDaysLeftText.text = "Infinite Days Left";
            HUDManager.Instance.profitQuotaDaysLeftText2.text = "Infinite Days Left";
        }
    }
}
