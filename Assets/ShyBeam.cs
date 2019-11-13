using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyBeam : MonoBehaviour
{
    public GameObject BeamBodyElement;

    private bool isInit;

    private bool isDrawn;

    public bool IsVisibleInFrame;

    private List<GameObject> BeamBody { get; set; } = new List<GameObject>();

    void Start()
    {
        InitBeam();
    }

    private void Update()
    {
        if (!isInit)
            return;

        if (!IsVisibleInFrame && !isDrawn)
            TurnBeamOn();
        else if (IsVisibleInFrame && isDrawn)
            TurnBeamOff();
    }

    public void InitBeam()
    {
        var rotatedToVector = transform.rotation * Vector2.down;
        Physics.Raycast(transform.position, rotatedToVector, out RaycastHit hit);

        if (hit.collider == null)
            return;

        var distanceInSteps = 0;
        var distanceOfEachStep = 0f;

        // Hard snapping to 90 angles only (but works with any)
        // If beaming horizontally
        if (transform.rotation.eulerAngles.z == 90 || transform.rotation.eulerAngles.z == 270)
        {
            distanceOfEachStep = Settings.PixelsInGridX;
            distanceInSteps = (int) (hit.distance / Settings.PixelsInGridX);
        }
        // If beaming vertically
        else if (transform.rotation.eulerAngles.z == 0 || transform.rotation.eulerAngles.z == 180)
        {
            distanceOfEachStep = Settings.PixelsInGridY;
            distanceInSteps = (int) (hit.distance / Settings.PixelsInGridY);
        }

        Debug.DrawRay(transform.position, distanceInSteps * rotatedToVector);

        if (!isInit)
            InstantiateBeamBody(distanceInSteps, distanceOfEachStep, rotatedToVector);
    }

    private void TurnBeamOn()
    {
        BeamBody.ForEach(x => x.SetActive(true));
        isDrawn = true;
    }

    private void TurnBeamOff()
    {
        BeamBody.ForEach(x => x.SetActive(false));
        isDrawn = false;
    }

    private void InstantiateBeamBody(int distanceInGridSteps, float distanceOfEachStep, Vector3 direction)
    {
        var originRotation = transform.rotation.eulerAngles;
        var parentRotation = Quaternion.Euler(originRotation.x, originRotation.y, originRotation.z + 90);

        //Leaving 0 - for beam source
        for (int step = 1; step < distanceInGridSteps; step++)
            BeamBody.Add(Instantiate(BeamBodyElement, transform.position + step * distanceOfEachStep * direction,
                parentRotation));

        isInit = true;
    }
}