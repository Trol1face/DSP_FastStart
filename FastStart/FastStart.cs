﻿using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System;
using xiaoye97;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace FastStart
{
        
    [BepInPlugin(__GUID__, __NAME__, "1.1.0")]

    public class FastStart : BaseUnityPlugin
    {
        
        public const string __NAME__ = "FastStart";
        public const string __GUID__ = "com.Trol1face.dsp." + __NAME__;
        static int matrixLabId = 2901;
        static int minerId = 2301;
        static int smelterId = 2302;
        static int windTurbineId = 2203;
        static int teslaTowerId = 2201;
        static int sorterId = 2011;
        static int beltId = 2001;
        static int splitterId = 2020;
        static int storageId = 2101;
        static int assemblerId = 2303;
        static int thermalStationId = 2204;
        static int WirelessPowerTowerId = 2202;
        static int circuitId = 1301;
        static int gearId = 1201;
        static int motorId = 1203;
        static int blueMatrixId = 6001;
        static int coilId = 1202;
        static int beltMk1Id = 2001;
        static int sorterk1Id = 2011;
        static int storageMk1Id = 2101;
        static int wirelessTowerId = 2202;
        static int ILSId = 2104;
        static int vesselId = 5002;

        public static ConfigEntry<bool> researchMode;
        public static ConfigEntry<String> suppliesAmount;
        public static ConfigEntry<bool> giveBlueMatrix;
        public static ConfigEntry<bool> giveFreeILS;
        public static ConfigEntry<bool> speedUpItemTechs;

        private void Awake()
        {
            // Plugin startup logic
            suppliesAmount = Config.Bind("General", "Amount of supplies", "enough",
                "AVAILABLE OPTIONS (type in): few, enough, huge.\nDEFAULT: enough.");
            researchMode = Config.Bind("General", "Research mode enabler", false,
                "IMPORTANT: playing with this enabled will surely fail abnormality check (fixes with mod) and disable upload to milky way.\n"+
                "Check thunderstore modpage for more info. Some options for research mode below"
                );
            giveBlueMatrix = Config.Bind("General", "Give 300 blue matrixes (only enough/huge)", false,
                "If enabled you will receive 300 blue cubes, mainly for drones speed+count upgrade.\nDEFAULT: false");
            giveFreeILS = Config.Bind("Research mode options", "Free ILS (only enough/huge)", false,  
                "If enabled completing Interplanetary Logistics research will award you with 2 ILS and 6 vessels.\nDEFAULT: false");
            speedUpItemTechs = Config.Bind("Research mode options", "Speedup some techs research", false,
                "Lowers amount of hashes needed to research techs that use items as input, "+
                "number of items needed remains the same.\nDEFAULT: false");
            
            new Harmony(__GUID__).PatchAll(typeof(Patch));

            if (researchMode.Value && (suppliesAmount.Value == "few" || suppliesAmount.Value == "enough" || suppliesAmount.Value == "huge")) 
            {
                LDBToolEditData();
            }
        }

        static class Patch
        {
            [HarmonyTranspiler, HarmonyPatch(typeof(Player), "SetForNewGame")]
            public static IEnumerable<CodeInstruction> Player_SetForNewGame_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                if (!researchMode.Value && (suppliesAmount.Value == "few" || suppliesAmount.Value == "enough" || suppliesAmount.Value == "huge")) 
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
                    return matcher.InstructionEnumeration();
                }
                return instructions;
            }
                
        }

        public static IDCNTINC[] SuppliesDecider() 
        {
            String amount = suppliesAmount.Value;
            List <IDCNTINC> supplies = new(){
                    new(coilId, 30, 0),
                    new(gearId, 20, 0),
                    new(circuitId, 40, 0)
                };  
            if (amount == "few")
            {
                supplies.Add(new(teslaTowerId, 10, 0));
                supplies.Add(new(windTurbineId, 8, 0));
                supplies.Add(new(minerId, 2, 0));
                supplies.Add(new(smelterId, 3, 0));
                supplies.Add(new(beltId, 100, 0));
                supplies.Add(new(storageId, 1, 0));
                supplies.Add(new(sorterId, 15, 0));
                supplies.Add(new(assemblerId, 3, 0));
            }
            else if (amount == "enough")
            {
                supplies.Add(new(teslaTowerId, 60, 0));
                supplies.Add(new(windTurbineId, 30, 0));
                supplies.Add(new(thermalStationId, 2, 0));
                supplies.Add(new(minerId, 14, 0));
                supplies.Add(new(smelterId, 24, 0));
                supplies.Add(new(beltId, 600, 0));
                supplies.Add(new(storageId, 6, 0));
                supplies.Add(new(sorterId, 120, 0));
                supplies.Add(new(splitterId, 10, 0));
                supplies.Add(new(assemblerId, 24, 0));
                supplies.Add(new(matrixLabId, 12, 0));
                supplies.Add(new(motorId, 60, 0));
            }
            else if (amount == "huge")
            {
                supplies.Add(new(teslaTowerId, 120, 0));
                supplies.Add(new(windTurbineId, 40, 0));
                supplies.Add(new(WirelessPowerTowerId, 20, 0));
                supplies.Add(new(thermalStationId, 8, 0));
                supplies.Add(new(minerId, 24, 0));
                supplies.Add(new(smelterId, 48, 0));
                supplies.Add(new(beltId, 1200, 0));
                supplies.Add(new(sorterId, 300, 0));
                supplies.Add(new(splitterId, 40, 0));
                supplies.Add(new(storageId, 20, 0));
                supplies.Add(new(assemblerId, 48, 0));
                supplies.Add(new(matrixLabId, 24, 0));
                supplies.Add(new(motorId, 60, 0));
            }
            if (giveBlueMatrix.Value && (amount == "enough" || amount == "huge")) 
            {
                supplies.Add(new(blueMatrixId, 300, 0));
            }
            return supplies.ToArray();
        }

        public static void ModifySpaceCapsuleLoot(Proto proto)
        {
            if (proto is VegeProto vegeProto && proto.ID == 9999)
            {
                vegeProto.MiningTime = 150;
                vegeProto.MiningItem = new int[] {1801, coilId, circuitId, gearId};
                vegeProto.MiningCount = new int[] {3, 30, 40, 20};
            }
        }
        public static void ModifyElectromagnetismTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1001) 
            {
                if (amount == "few") techProto.AddItemCounts = new int[] {10, 10, 3};
                if (amount == "enough") techProto.AddItemCounts = new int[] {30, 60, 15};
                if (amount == "huge") techProto.AddItemCounts = new int[] {40, 100, 30};
                if (speedUpItemTechs.Value) 
                {
                    techProto.HashNeeded = 600;
                    techProto.ItemPoints = new int[] {60};
                }
            }
        }
        public static void ModifySmeltingTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1401) 
            {
                if (amount == "few") techProto.AddItemCounts = new int[] {4};
                if (amount == "enough") techProto.AddItemCounts = new int[] {24};
                if (amount == "huge") techProto.AddItemCounts = new int[] {48};
                if (speedUpItemTechs.Value) 
                {
                    techProto.HashNeeded = 600;
                    techProto.ItemPoints = new int[] {60, 60};
                }
            }
        }
        public static void ModifyAssemblyTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1201) 
            {
                if (amount == "few") techProto.AddItemCounts = new int[] {4};
                if (amount == "enough") techProto.AddItemCounts = new int[] {24};
                if (amount == "huge") techProto.AddItemCounts = new int[] {48};
                if (speedUpItemTechs.Value) 
                {
                    techProto.HashNeeded = 600;
                    techProto.ItemPoints = new int[] {60, 60};
                }
            }
        }
        public static void ModifyMatrixTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1002) 
            {
                if (giveBlueMatrix.Value && amount != "few") 
                {
                    techProto.AddItems = new int[] {matrixLabId, blueMatrixId}; 
                    if (amount == "enough") techProto.AddItemCounts = new int[] {12, 300};
                    if (amount == "huge") techProto.AddItemCounts = new int[] {24, 300};
                } 
                else 
                {
                    if (amount == "few") techProto.AddItemCounts = new int[] {2};
                    if (amount == "enough") techProto.AddItemCounts = new int[] {12};
                    if (amount == "huge") techProto.AddItemCounts = new int[] {24};
                }
                if (speedUpItemTechs.Value) 
                {
                    techProto.HashNeeded = 600;
                    techProto.ItemPoints = new int[] {60, 60};
                }
            }
        }
        public static void ModifyLogisticsMk1Tech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1601) 
            {
                techProto.AddItems = new int[] {beltMk1Id, sorterk1Id, storageMk1Id};
                if (amount == "few") techProto.AddItemCounts = new int[] {120, 20, 2};
                if (amount == "enough") techProto.AddItemCounts = new int[] {600, 120, 10};
                if (amount == "huge") techProto.AddItemCounts = new int[] {1200, 300, 30};
                if (speedUpItemTechs.Value) 
                {
                    techProto.HashNeeded = 600;
                    techProto.ItemPoints = new int[] {60, 60};
                }
            }
        }
        public static void ModifyLogisticsMk2Tech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1602) 
            {
                if (amount == "enough") techProto.AddItemCounts = new int[] {10, 20, 5};
                if (amount == "huge") techProto.AddItemCounts = new int[] {20, 40, 5};
            }
        }
        public static void ModifyThermalPowerTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1412) 
            {
                if (amount == "enough") techProto.AddItemCounts = new int[] {4};
                if (amount == "huge") techProto.AddItemCounts = new int[] {9};
            }
        }
        public static void ModifyPlasmaControlTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1101) 
            {
                techProto.AddItems = new int[] {wirelessTowerId}; 
                if (amount == "enough") techProto.AddItemCounts = new int[] {5};
                if (amount == "huge") techProto.AddItemCounts = new int[] {20};
            }
        }
        public static void ModifyInterplanetaryLogisticsTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 1605 && giveFreeILS.Value) 
            {
                techProto.AddItems = new int[] {ILSId, vesselId};
                techProto.AddItemCounts = new int[] {2, 6};
            }
        }
        public static void ModifyElectromagneticDriveTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == 1701) 
            {
                techProto.AddItems = new int[] {motorId};
                techProto.AddItemCounts = new int[] {60};
            }
        }
        public static void ModifyMechaCoreTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 2101) 
            {
                techProto.HashNeeded = 600;
                techProto.ItemPoints = new int[] {120, 120};
            }
        }
        public static void ModifyMechanicalFrameTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 2201) 
            {
                techProto.HashNeeded = 1200;
                techProto.ItemPoints = new int[] {180};
            }
        }
        public static void ModifyInventoryCapacityTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 2301) 
            {
                techProto.HashNeeded = 1200;
                techProto.ItemPoints = new int[] {360, 360};
            }
        }
        public static void ModifyMassConstructionTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 2701) 
            {
                techProto.HashNeeded = 1200;
                techProto.ItemPoints = new int[] {300};
            }
        }
        public static void ModifyEnergyCircuitTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 2501) 
            {
                techProto.HashNeeded = 1200;
                techProto.ItemPoints = new int[] {180, 180, 180};
            }
        }
        public static void ModifyMechaFlightTech(Proto proto) 
        {
            if (proto is TechProto techProto && proto.ID == 2901) 
            {
                techProto.HashNeeded = 2400;
                techProto.ItemPoints = new int[] {225, 75};
            }
        }

        public static void LDBToolEditData() {
            LDBTool.EditDataAction += ModifySpaceCapsuleLoot;
            LDBTool.EditDataAction += ModifyElectromagnetismTech;
            LDBTool.EditDataAction += ModifyAssemblyTech;
            LDBTool.EditDataAction += ModifySmeltingTech;
            LDBTool.EditDataAction += ModifyMatrixTech;
            LDBTool.EditDataAction += ModifyLogisticsMk1Tech;
            if (speedUpItemTechs.Value)
            {
                LDBTool.EditDataAction += ModifyMechaCoreTech;
                LDBTool.EditDataAction += ModifyMechanicalFrameTech;
                LDBTool.EditDataAction += ModifyMechaFlightTech;
                LDBTool.EditDataAction += ModifyEnergyCircuitTech;
                LDBTool.EditDataAction += ModifyInventoryCapacityTech;
                LDBTool.EditDataAction += ModifyMassConstructionTech;
            }
            if (suppliesAmount.Value != "few") 
            {
                LDBTool.EditDataAction += ModifyLogisticsMk2Tech;
                LDBTool.EditDataAction += ModifyThermalPowerTech;
                LDBTool.EditDataAction += ModifyPlasmaControlTech;
                LDBTool.EditDataAction += ModifyElectromagneticDriveTech;
                if (giveFreeILS.Value) 
                {
                    LDBTool.EditDataAction += ModifyInterplanetaryLogisticsTech;
                }
            }
        }
    }

}
