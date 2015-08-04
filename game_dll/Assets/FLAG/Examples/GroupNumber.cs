using UnityEngine;
using System.Collections;

/// <summary>
/// This example shows how to control the Leader's generation stats,
/// by having a total number of Agents you desire to create, then defining 
/// how many positions need generating.
/// </summary>
public class GroupNumber : MonoBehaviour 
{
    public LdrMain goLeaderToEdit;
    public int iNumberOfFollowers = 10;
    public LdrCreate.SpawnMethod eMethod;
    public bool bRecreate = false;

    void Update()
    {
        if(bRecreate)
        {
            bRecreate = false;

            //take number, divide down by 2, set leader to size
            int _divide = (iNumberOfFollowers / 2);

            goLeaderToEdit.GenXP = _divide;
            goLeaderToEdit.GenXN = _divide;
            goLeaderToEdit.GenYP = _divide;
            goLeaderToEdit.GenYN = _divide;
            goLeaderToEdit.eLdrGenType = eMethod;

            goLeaderToEdit.vRegenerate();
        }
    }
}
