using System.Collections;
using System.Collections.Generic;
using ProjectF.Quests;
using ProjectF.DataTables;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace ProjectF
{
    public static class QuestUtility
    {
        public static Quest CreateQuest(QuestTableRow questTableRow)
        {
            Quest quest = null;

            if(questTableRow != null)
            {
                Type type = Type.GetType($"ProjectF.Quests.{questTableRow.questType}Quest");
                quest = Activator.CreateInstance(type, questTableRow.questType, questTableRow.questName, ParseTypedValues(questTableRow.parameters)) as Quest;
            }
            
            return quest;
        }

        static object[] ParseTypedValues(string input)
        {
            var results = new List<object>();

            // 정규표현식: {타입}값  또는  타입{값}
            var regex = new Regex(@"\{(?<type>\w+)\}(?<value>[^,]+)|(?<type2>\w+)\{(?<value2>[^\}]+)\}");

            foreach (Match match in regex.Matches(input))
            {
                string type = match.Groups["type"].Success ? match.Groups["type"].Value : match.Groups["type2"].Value;
                string value = match.Groups["value"].Success ? match.Groups["value"].Value : match.Groups["value2"].Value;

                object parsed = ParseValue(type, value.Trim());
                results.Add(parsed);
            }

            return results.ToArray();
        }

        static object ParseValue(string type, string value)
        {
            return type switch
            {
                "int" => int.Parse(value),
                "float" => float.Parse(value),
                "double" => double.Parse(value),
                "string" => value,
                "bool" => bool.Parse(value),
            _   => throw new Exception($"알 수 없는 타입: {type}")
            };
        }
    }
}
