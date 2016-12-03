using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SampleResourceTemplate : MonoBehaviour
{

    float[] total = new float[2] { 1000f, 1000f };
    float[] avail = new float[2] { .2f, .55f };

    public NaturalResources res;

    void Start()
    {
        res = new NaturalResources(total, avail);
        this.gameObject.GetComponent<Planets>().setupNaturalResources(res);
    }


}
