using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flow;
public class CarEntryState : FlowBaseState
{
    public CanvasFader canvasFader;

    private void Start()
    {
        canvasFader.Hide(0f);
        canvasFader.FullyShown += NextState;

    }


    public override void EndState(bool force = false)
    {
        base.StartState(force);
        canvasFader.Show(1f);
    }

}
