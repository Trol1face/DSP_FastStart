using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace FastStart
{
    [BepInPlugin(__GUID__, __NAME__, "1.0.0")]
    public class FastStart : BaseUnityPlugin
    {
        //public static float suppliesAmount;

        public const string __NAME__ = "FastStart";
        public const string __GUID__ = "com.Trol1face.dsp." + __NAME__;
        public static ConfigEntry<String> suppliesAmount;

        private void Awake()
        {
            // Plugin startup logic
            suppliesAmount = Config.Bind("General", "Amount of supplies", "enough",
            "'few' gives a couple of machines to make work a bit less manual, \n'enough' gives a few dozens (enough to start a small mall), \nhuge is huge. Check thunderstore modpage for more info\nAVAILABLE OPTIONS (type in): few, enough, huge. \nDEFAULT: enough.");

            new Harmony(__GUID__).PatchAll(typeof(Patch));
        }

        static class Patch
        {
            [HarmonyTranspiler, HarmonyPatch(typeof(Player), "SetForNewGame")]
            public static IEnumerable<CodeInstruction> Player_SetForNewGame_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                String value = suppliesAmount.Value;
                if (value == "few" || value == "enough" || value == "huge") 
                {
                    CodeMatcher matcher = new(instructions);
                    MethodInfo rep = typeof(FastStart).GetMethod("SuppliesDecider");
                    MethodInfo anchorM = typeof(Configs).GetMethod("get_freeMode");
                    FieldInfo anchorF = typeof(ModeConfig).GetField("items");
                    matcher.MatchForward(false, 
                        new CodeMatch(i => i.opcode == OpCodes.Call && i.operand is MethodInfo m && m == anchorM),
                        new CodeMatch(i => i.opcode == OpCodes.Ldfld && i.operand is FieldInfo f && f == anchorF)
                    );
                    matcher.RemoveInstructions(2);
                    matcher.Insert(
                        new CodeInstruction(OpCodes.Call, rep)
                    );
                    //foreach (CodeInstruction ins in matcher.Instructions()) Debug.Log(" .. " + ins.ToString());

                    return matcher.InstructionEnumeration();
                
                }
                return instructions;
            }
        }

        public static IDCNTINC[] SuppliesDecider() 
        {
            String amount = suppliesAmount.Value;
            if (amount == "few" || amount == "enough" || amount == "huge") {
                int minerId = 2301;
                int smelterId = 2302;
                int windTurbineId = 2203;
                int teslaTowerId = 2201;
                int sorterId = 2011;
                int beltId = 2001;
                int splitterId = 2020;
                int storageId = 2101;
                int assemblerId = 2303;
                int matrixLabId = 2901;
                int thermalStationId = 2204;
                int WirelessPowerTowerId = 2202;
                int coilId = 1202;
                int circuitId = 1301;
                int gearId = 1201;

                IDCNTINC[] suppliesFew = {
                    new(teslaTowerId, 10, 0),
                    new(windTurbineId, 6, 0),
                    new(minerId, 2, 0),
                    new(smelterId, 3, 0),
                    new(beltId, 100, 0),
                    new(storageId, 1, 0),
                    new(sorterId, 15, 0),
                    //new(splitterId, 1, 0),
                    new(assemblerId, 3, 0),
                    new(coilId, 30, 0),
                    new(gearId, 10, 0),
                    new(circuitId, 30, 0)
                };
                IDCNTINC[] suppliesEnough = {
                    new(teslaTowerId, 50, 0),
                    new(windTurbineId, 30, 0),
                    new(minerId, 14, 0),
                    new(smelterId, 24, 0),
                    new(beltId, 600, 0),
                    new(storageId, 6, 0),
                    new(sorterId, 100, 0),
                    new(splitterId, 10, 0),
                    new(assemblerId, 20, 0),
                    new(matrixLabId, 8, 0),
                    new(coilId, 30, 0),
                    new(gearId, 10, 0),
                    new(circuitId, 30, 0)
                };
                IDCNTINC[] suppliesHuge = {
                    new(teslaTowerId, 200, 0),
                    new(WirelessPowerTowerId, 20, 0),
                    new(windTurbineId, 30, 0),
                    new(thermalStationId, 8, 0),
                    new(minerId, 30, 0),
                    new(smelterId, 50, 0),
                    new(beltId, 900, 0),
                    new(sorterId, 400, 0),
                    new(splitterId, 50, 0),
                    new(storageId, 30, 0),
                    new(assemblerId, 50, 0),
                    new(matrixLabId, 20, 0),
                    new(coilId, 30, 0),
                    new(gearId, 10, 0),
                    new(circuitId, 30, 0),
                };
                if (amount == "few")
                {
                    return suppliesFew;
                }
                else if (amount == "enough")
                {
                    return suppliesEnough;
                }
                else if (amount == "huge")
                {
                    return suppliesHuge;
                }
            }
            return Configs.freeMode.items;
            
        }
    }
}
