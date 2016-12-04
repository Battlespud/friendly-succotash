using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FacilityCosts {


    //Put the build costs of all facilties here to make balancing and revision easier

    public const int numberOfResources = 2;

    public static float[] MineCost = new float[numberOfResources] { 0, 200 }; //100 steel
    public static int mineBuildTime = 30;
    public const int mineMass = 2000;
    public const int mineVolume = 2000;
    public const bool mineMovable = true;

    public static float[] ArmyAcademyCost = new float[numberOfResources] { 0, 2400 }; //100 steel
    public static int ArmyAcademyBuildTime = 30;
    public static float[] FleetAcademyCost = new float[numberOfResources] { 0, 3600 }; //100 steel
    public static int FleetAcademyBuildTime = 30;
    //For both space and army
    public const int AcademyVolume = 200000;
    public const int AcademyMass = 200000;
    public const bool AcademyMovable = false;




    public static float[] InfraCost = new float[numberOfResources] { 0, 50 }; //100 steel
    public static int InfraBuildTime = 30;
    public const bool InfraMovable = true;
    public const int InfraVolume = 250;
    public const int InfraMass = 500;


}
