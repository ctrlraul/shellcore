using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CodeTraverser;

public class CodeCanvasTask : MonoBehaviour
{
    public static Task ParseTask(int lineIndex, int charIndex,
         string[] lines, Dictionary<FileCoord, FileCoord> stringScopes,
        Dictionary<string, string> localMap, out FileCoord coord)
    {
        return ParseTaskHelper(0, CodeTraverser.GetScope(lineIndex, lines, stringScopes, out coord), localMap);
    }

    // TODO: Add property inheritance to child nodes like speaker ID, typing speed, color etc
    private static Task ParseTaskHelper(int index, string line, Dictionary<string, string> localMap)
    {
        List<string> stx = new List<string>()
        {
            "taskID=",
            "objectives=",
            "creditReward=",
            "reputationReward=",
            "shardReward=",
            "partID=",
            "abilityID=",
            "tier=",
        };
        bool skipToComma = false;
        int brax = 0;

        var task = new Task();

        index = CodeTraverser.GetNextOccurenceInScope(index, line, stx, ref brax, ref skipToComma, '(', ')');
        for (int i = index; i < line.Length; i = CodeTraverser.GetNextOccurenceInScope(i, line, stx, ref brax, ref skipToComma, '(', ')'))
        {
            skipToComma = true;
            var lineSubstr = line.Substring(i);
            var name = "";
            var val = "";
            CodeCanvasSequence.GetNameAndValue(lineSubstr, out name, out val);

            switch (name)
            {
                case "taskID":
                    task.taskID = val;
                    break;
                case "objectives":
                    task.objectived = localMap[val];
                    break;
                case "creditReward":
                    task.creditReward = int.Parse(val);
                    break;
                case "reputationReward":
                    task.reputationReward = int.Parse(val);
                    break;
                case "shardReward":
                    task.shardReward = int.Parse(val);
                    break;
                case "partID":
                    task.partReward.partID = val;
                    break;
                case "abilityID":
                    task.partReward.abilityID = int.Parse(val);
                    break;
                case "tier":
                    task.partReward.tier = int.Parse(val);
                    break;
            }
        }
        return task;
    }
}
