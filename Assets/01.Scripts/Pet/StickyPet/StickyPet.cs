using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StickyPet : Pet
{
    private Sticky stickyObject;
    public Sticky StickyObject { get { return stickyObject; }set { stickyObject = value; } }

    protected override void ResetPet()
    {
        base.ResetPet();
    }
  
}