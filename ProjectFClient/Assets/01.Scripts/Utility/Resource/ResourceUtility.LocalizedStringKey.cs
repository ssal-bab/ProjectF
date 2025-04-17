using System;
using System.Collections.Generic;
using ProjectF.Datas;
using ProjectF.Dialogues;

namespace ProjectF
{
    public static partial class ResourceUtility
    {
        private static Dictionary<string, Dictionary<int, string>> LocalizedStringKeyCache = null;
        private static Dictionary<ESpeakerType, Dictionary<string, string>> LocalizedDialogueStringKeyCache = null;

        private static void InitializeLocalizedStringKeyUtility()
        {
            LocalizedStringKeyCache = new Dictionary<string, Dictionary<int, string>>();
        }

        public static string GetCropNameLocalKey(int id) => GetLocalKey("CropName", id);
        public static string GetMaterialNameLocalKey(int id) => GetLocalKey("MaterialName", id);
        public static string GetCropGradeNameLocalKey(ECropGrade cropGrade) => GetLocalKey("CropGradeName", (int)cropGrade);
        public static string GetRarityNameLocalKey(ERarity rarity) => GetLocalKey("RarityName", (int)rarity);
        public static string GetQusetDescriptionLocalKey(EActionType type) => GetLocalKey("QuestType", (int)type);
        public static string GetStatDescriptionLocakKey(EFarmerStatType type) => GetLocalKey("StatType", (int)type);
        public static string GetAdventureAreaNameLocalKey(int areaID) => GetLocalKey("AdventureAreaName", areaID);
        // public static string GetDialogueSpeakerNameLocakKey(ESpeakerType speakerType) => GetLocalKey("SpeakerName", (int)speakerType);
        // public static string GetDialogueLocakKey(string situation, ESpeakerType speakerType) => GetLocalKey(situation, speakerType);
        // public static string GetAdventureProgressStateKey(bool value) => GetLocalKey("AdventureProgress", Convert.ToInt16(value));

        private static string GetLocalKey(string prefix, int id)
        {
            if (LocalizedStringKeyCache.TryGetValue(prefix, out Dictionary<int, string> cache) == false)
            {
                cache = new Dictionary<int, string>();
                LocalizedStringKeyCache.Add(prefix, cache);
            }

            if(cache.TryGetValue(id, out string resourceName) == false)
            {
                resourceName = $"{prefix}_{id}";
                cache.Add(id, resourceName);
            }

            return resourceName;
        }

        private static string GetLocalKey(string situation, ESpeakerType speakerType)
        {
            if (LocalizedDialogueStringKeyCache.TryGetValue(speakerType, out Dictionary<string, string> cache) == false)
            {
                cache = new Dictionary<string, string>();
                LocalizedDialogueStringKeyCache.Add(speakerType, cache);
            }

            if(cache.TryGetValue(situation, out string dialogue) == false)
            {
                dialogue = $"{speakerType}_{situation}";
                cache.Add(situation, dialogue);
            }

            return dialogue;
        }
    }
}