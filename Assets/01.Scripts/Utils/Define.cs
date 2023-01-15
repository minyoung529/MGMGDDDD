using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 상수 매니저
/// </summary>
public class Define
{
    #region LAYER_MASK
    public const int BOTTOM_LAYER               = 1 << 7;
    public const int PLAYER_LAYER               = 1 << 3;
    public const int PET_LAYER                  = 1 << 8;
    public const int CONNECTED_OBJECT_LAYER     = 1 << 9;
    #endregion
}