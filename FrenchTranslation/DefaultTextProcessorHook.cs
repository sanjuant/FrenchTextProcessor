// Copyright © 2021, https://sanjuant.fr - All rights reserved. 
// https://github.com/sanjuant/FrenchTextProcessor

using System.Text;
using HarmonyLib;
using TaleWorlds.Localization.TextProcessor;

namespace FrenchTranslation
{
    [HarmonyPatch(typeof(DefaultTextProcessor))]
    [HarmonyPatch("ProcessToken")]
    class DefaultTextProcessorHook
    {
        static readonly FrenchTextProcessor frenchTextProcessor = new FrenchTextProcessor();
        static bool Prefix(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
        {
            frenchTextProcessor.ProcessToken(sourceText, ref cursorPos, token, outputString);
            return false;
        }
    }
}
