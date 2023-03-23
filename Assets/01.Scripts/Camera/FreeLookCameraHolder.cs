using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class FreeLookCameraHolder : MonoBehaviour
{
    new private CinemachineFreeLook camera;
    private Orbit[] originalOrbits;

    private void Start()
    {
        camera = GetComponent<CinemachineFreeLook>();
        originalOrbits = camera.m_Orbits;
    }

    public void ChangeCameraRig(Orbit[] orbit)
    {
        for(int i = 0; i < orbit.Length; i++)
        {
            SetZeroValueOriginal(ref orbit[i].m_Height, originalOrbits[i].m_Height);
            SetZeroValueOriginal(ref orbit[i].m_Radius, originalOrbits[i].m_Radius);
        }

        camera.m_Orbits = orbit;
    }

    public void SetCameraRigOriginal()
    {
        camera.m_Orbits = originalOrbits;
    }

    private void SetZeroValueOriginal(ref float val, float original)
    {
        if (val <= 0f)
            val = original;
    }
}
