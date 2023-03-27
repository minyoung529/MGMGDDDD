using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;

public class FreeLookCameraHolder : MonoBehaviour
{
    new private CinemachineFreeLook camera;
    private Orbit[] originalOrbits;

    private bool isChanging = false;

    private void Awake()
    {
        camera = GetComponent<CinemachineFreeLook>();
        originalOrbits = camera.m_Orbits.ToArray();
    }

    public void ChangeCameraRig(Orbit[] orbit, float duration)
    {
        if (isChanging) return;
        isChanging = true;

        for (int i = 0; i < orbit.Length; i++)
        {
            ChangeOrbit(orbit[i], originalOrbits[i], duration, i);
        }
    }

    public void SetCameraRigOriginal(float duration)
    {
        if (isChanging) return;
        isChanging = true;

        for (int i = 0; i < originalOrbits.Length; i++)
        {
            ChangeOrbit(originalOrbits[i], originalOrbits[i], duration, i);
        }
    }

    private void ChangeOrbit(Orbit orbit, Orbit original, float duration, int index)
    {
        if (orbit.m_Height <= 0f)
            orbit.m_Height = original.m_Height;

        if (orbit.m_Radius <= 0f)
            orbit.m_Radius = original.m_Radius;

        DOTween.To(() => camera.m_Orbits[index].m_Height, (x) => camera.m_Orbits[index].m_Height = x, orbit.m_Height, duration);
        DOTween.To(() => camera.m_Orbits[index].m_Radius, (x) => camera.m_Orbits[index].m_Radius = x, orbit.m_Radius, duration).OnComplete(() => isChanging = false);
    }
}
