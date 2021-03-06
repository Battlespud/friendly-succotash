﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POP : NonShipEntity {


	public static float ReproductionRate = .0033f;   //once per five min

    //1 unit of population.  Updating is handled by planet class via list, in ships they wont update (cryosleep or whatever)

    //99% of this is (not) fluff vital to fielding an army of muscular african mandingos

    public static int updateCycleTime = 60; //How often to update pops


    public enum Race { White=0, Black, Asian, Cuck};
    public static string[] RaceNames = new string[4] { "White", "Mandingo", "ChingChing", "Cuck" };
    public enum Gender {Male, Female, Apache};

	public Jobs Job;
    //Where this pop contributes their labor to
    public Facilities workAssignment;
    //prevents null ref when no workassignment set
    public bool unemployed = true;

    //chance to join military
    public float chanceToEnlist = 0.0f;

    //How much money they have, affected by taxation and industry and job
    float wealth = 0;

    //Name,  Race, gender
    public string firstName;
    public string lastName;
	public string Name{ get { return firstName + " " + lastName; } }
    public Gender gender;
    public Race race;
	public string raceName{ get { return RaceNames [(int)race]; } }

    //How much work this pop does
    public float laborOutput; //optimal at 1
    //How happy the pop is with their life
    public float satisfaction =  .85f;
    //pop is rebelling
    public bool riot = false;

    //holocaust
    public bool toExecute = false;
    //send to concentration camp
    public bool toCamp = false;

	public POP(){}

}
