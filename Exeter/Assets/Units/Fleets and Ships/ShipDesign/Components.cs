using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Components {

//Parent class for all the various component types

    public enum ComponentType { BRIDGE, LIFESUPPORT, CREW, ENGINE, RCS, SENSOR};
    public ComponentType componentType;

    //size and weight
    public int mass;
    public int volume;

    //cost to build in resources
    public float[] cost;

    //how much to research when first designed
    public float researchCost;
}
