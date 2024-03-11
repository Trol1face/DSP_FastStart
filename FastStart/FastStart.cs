using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using System;
using xiaoye97;

namespace FastStart
{
        
    [BepInPlugin(__GUID__, __NAME__, "1.1.0")]

    public class FastStart : BaseUnityPlugin
    {
        
        public const string __NAME__ = "FastStart";
        public const string __GUID__ = "com.Trol1face.dsp." + __NAME__;

        static int electromagnetismTechId = 1001;
        static int smeltingTechId = 1401;
        static int assemblyTechId = 1201;
        static int matrixTechId = 1002;
        static int logisticsMk1TechId = 1601;
        static int logisticsMk2TechId = 1602;
        static int thermalPowerTechId = 1412;
        static int plasmaControlTechId = 1101;
        static int electromagneticDriveTechId = 1701;
        static int interplanetaryLogisticsTechId = 1605;
        static int matrixLabId = 2901;
        static int coilId = 1202;
        static int beltMk1Id = 2001;
        static int sorterk1Id = 2011;
        static int storageMk1Id = 2101;
        static int circuitId = 1301;
        static int gearId = 1201;
        static int motorId = 1203;
        static int blueMatrixId = 6001;
        static int wirelessTowerId = 2202;
        static int ILSId = 2104;
        static int vesselId = 5002;

        public static ConfigEntry<String> suppliesAmount;
        public static ConfigEntry<bool> giveBlueMatrix;
        public static ConfigEntry<bool> giveFreeILS;

        private void Awake()
        {
            // Plugin startup logic
            suppliesAmount = Config.Bind("General", "Amount of supplies", "enough",
                "'few' gives a couple of machines to make work a bit less manual, \n"+
                "'enough' gives a few dozens (enough to start a small mall), \n"+
                "huge is huge. Check thunderstore modpage for more info\n"+
                "AVAILABLE OPTIONS (type in): few, enough, huge. \nDEFAULT: enough.");
            giveBlueMatrix = Config.Bind("General", "Drones too slow (only enough/huge)", false,
                "If enabled completing matrixlab research will also give 300 blue matrixes to fast upgrade drones speed+count. Or something else\nDEFAULT: false");
            giveFreeILS = Config.Bind("General", "Free ILS (only enough/huge)", false,
                "If enabled completing Interplanetary Logistics research will award you with 2 ILS and 6 vessels.\nDEFAULT: false");
            new Harmony(__GUID__).PatchAll(typeof(Patch));
            
            LDBTool.EditDataAction += editElectromagnetismTech;
            LDBTool.EditDataAction += editAssemblyTech;
            LDBTool.EditDataAction += editSmeltingTech;
            LDBTool.EditDataAction += editMatrixTech;
            LDBTool.EditDataAction += editLogisticsMk1Tech;
            LDBTool.EditDataAction += editLogisticsMk2Tech;
            LDBTool.EditDataAction += editThermalPowerTech;
            LDBTool.EditDataAction += editPlasmaControlTech;
            LDBTool.EditDataAction += editElectromagneticDriveTech;
            LDBTool.EditDataAction += editInterplanetaryLogisticsTech;
            LDBTool.EditDataAction += editSpaceCapsuleLoot;
        }

        public static void editElectromagnetismTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == electromagnetismTechId) 
            {
                if (amount == "few") techProto.AddItemCounts = new int[] {10, 10, 3};
                if (amount == "enough") techProto.AddItemCounts = new int[] {30, 60, 15};
                if (amount == "huge") techProto.AddItemCounts = new int[] {40, 100, 30};
            }
        }
        public static void editSmeltingTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == smeltingTechId) 
            {
                if (amount == "few") techProto.AddItemCounts = new int[] {4};
                if (amount == "enough") techProto.AddItemCounts = new int[] {24};
                if (amount == "huge") techProto.AddItemCounts = new int[] {48};
            }
        }
        public static void editAssemblyTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == assemblyTechId) 
            {
                if (amount == "few") techProto.AddItemCounts = new int[] {4};
                if (amount == "enough") techProto.AddItemCounts = new int[] {24};
                if (amount == "huge") techProto.AddItemCounts = new int[] {48};
            }
        }
        public static void editMatrixTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == matrixTechId) 
            {
                if (giveBlueMatrix.Value && amount != "few") 
                {
                    techProto.AddItems = new int[] {matrixLabId, blueMatrixId}; 
                    if (amount == "enough") techProto.AddItemCounts = new int[] {12, 300};
                    if (amount == "huge") techProto.AddItemCounts = new int[] {24, 300};
                } else {
                    if (amount == "few") techProto.AddItemCounts = new int[] {2};
                    if (amount == "enough") techProto.AddItemCounts = new int[] {12};
                    if (amount == "huge") techProto.AddItemCounts = new int[] {24};
                }
            }
        }
        public static void editLogisticsMk1Tech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == logisticsMk1TechId) 
            {
                techProto.AddItems = new int[] {beltMk1Id, sorterk1Id, storageMk1Id};
                if (amount == "few") techProto.AddItemCounts = new int[] {120, 20, 2};
                if (amount == "enough") techProto.AddItemCounts = new int[] {600, 120, 10};
                if (amount == "huge") techProto.AddItemCounts = new int[] {1200, 300, 30};
            }
        }
        public static void editLogisticsMk2Tech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == logisticsMk2TechId && amount != "few") 
            {
                if (amount == "enough") techProto.AddItemCounts = new int[] {10, 20};
                if (amount == "huge") techProto.AddItemCounts = new int[] {20, 40};
            }
        }
        public static void editThermalPowerTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == thermalPowerTechId && amount != "few") 
            {
                if (amount == "enough") techProto.AddItemCounts = new int[] {4};
                if (amount == "huge") techProto.AddItemCounts = new int[] {9};
            }
        }
        public static void editPlasmaControlTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == plasmaControlTechId && amount != "few") 
            {
                techProto.AddItems = new int[] {wirelessTowerId}; 
                if (amount == "enough") techProto.AddItemCounts = new int[] {5};
                if (amount == "huge") techProto.AddItemCounts = new int[] {20};
            }
        }
        public static void editElectromagneticDriveTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == electromagneticDriveTechId && amount != "few") 
            {
                techProto.AddItems = new int[] {motorId};
                techProto.AddItemCounts = new int[] {60};
            }
        }
        public static void editInterplanetaryLogisticsTech(Proto proto) 
        {
            String amount = suppliesAmount.Value;
            if (proto is TechProto techProto && proto.ID == interplanetaryLogisticsTechId && amount != "few" && giveFreeILS.Value) 
            {
                techProto.AddItems = new int[] {ILSId, vesselId};
                techProto.AddItemCounts = new int[] {2, 6};
            }
        }
        public static void editSpaceCapsuleLoot(Proto proto)
        {
            if (proto is VegeProto vegeProto && proto.ID == 9999)
            {
                vegeProto.MiningTime = 120;
                vegeProto.MiningItem = new int[] {1801, coilId, circuitId, gearId};
                vegeProto.MiningCount = new int[] {3, 30, 40, 20};
            }
        }
    }

}
