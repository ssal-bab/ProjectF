using System;
using System.Collections.Generic;
using ProjectF.DataTables;
using System.Text.RegularExpressions;

namespace ProjectF.Datas
{
    public struct MakeQuestParams
    {
        public object[] parameters;

        public MakeQuestParams(QuestTableRow tableRow)
        {
            parameters = ParseTypedValues(tableRow.parameters);
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