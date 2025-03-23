using System.Collections;
using System.Collections.Generic;
using ProjectF.Quests;
using ProjectF.DataTables;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using ProjectF.Datas;
using System.Buffers;

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
                quest = Activator.CreateInstance(
                    type, 
                    questTableRow.questType, 
                    questTableRow.questName, 
                    questTableRow.rewordType1,
                    questTableRow.rewordAmount1,
                    questTableRow.rewordType2,
                    questTableRow.rewordAmount2,
                    questTableRow.rewordType3,
                    questTableRow.rewordAmount3,
                    ParseTypedValues(questTableRow.parameters)) as Quest;
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
    
        public static void MakeReword(string rewordType, int rewordAmount)
        {
            SplitByUnderscore(rewordType, out string[] rewordTypeData);
            if(Enum.TryParse<EQuestRewordType>(rewordTypeData[0], out var result))
            {
                switch(result)
                {
                    case EQuestRewordType.Gold:
                    Debug.Log($"Make reword {rewordType} : {rewordAmount}");
                    //GameInstance.MainUser.monetaData.gold += rewordAmount;
                    break;

                    case EQuestRewordType.FreeGem:
                    Debug.Log($"Make reword {rewordType} : {rewordAmount}");
                    //GameInstance.MainUser.monetaData.freeGem += rewordAmount;
                    break;

                    case EQuestRewordType.CashGem:
                    Debug.Log($"Make reword {rewordType} : {rewordAmount}");
                    //GameInstance.MainUser.monetaData.cashGem += rewordAmount;
                    break;

                    case EQuestRewordType.Crop:
                    Debug.Log($"Make reword {rewordType} : {rewordAmount}");
                    //rewordTypeData[1] : 작물 종류, rewordTypeData[2] : 등급
                    break;

                    case EQuestRewordType.Egg:
                    Debug.Log($"Make reword {rewordType} : {rewordAmount}");
                    //rewordTypeData[1] : 등급
                    break;

                    default :
                    break;
                }
            }
        }

        /// <summary>
    /// '_' 구분자로 문자열을 나누고, 결과를 재사용 가능한 string[] 배열에 담아 리턴.
    /// GC 최소화를 위해 ArrayPool 사용.
    /// </summary>
    public static int SplitByUnderscore(string input, out string[] results)
    {
        ReadOnlySpan<char> span = input.AsSpan();
        int count = 1;

        // 미리 구분자 개수 세기
        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '_') count++;
        }

        // 풀에서 배열 임대
        results = ArrayPool<string>.Shared.Rent(count);

        int start = 0;
        int resultIndex = 0;

        for (int i = 0; i < span.Length; i++)
        {
            if (span[i] == '_')
            {
                if (i > start)
                {
                    results[resultIndex++] = span.Slice(start, i - start).ToString();
                }
                else
                {
                    results[resultIndex++] = string.Empty;
                }

                start = i + 1;
            }
        }

        // 마지막 조각
        if (start < span.Length)
        {
            results[resultIndex++] = span.Slice(start).ToString();
        }
        else if (span.Length > 0 && span[^1] == '_')
        {
            results[resultIndex++] = string.Empty;
        }

        return resultIndex; // 유효한 항목 수
    }

    /// <summary>
    /// 호출 후 반드시 이걸로 배열 반환
    /// </summary>
    public static void ReturnArray(string[] array)
    {
        ArrayPool<string>.Shared.Return(array, clearArray: true);
    }

    }
}
