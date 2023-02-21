using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ??? ?????
/// </summary>
public class Define
{
    #region LAYER_MASK
    public const int ROPE_LAYER                 = 6;
    public const int BOTTOM_LAYER               = 7;
    public const int PLAYER_LAYER               = 3;
    public const int PET_LAYER                  = 8;
    public const int CONNECTED_OBJECT_LAYER     = 9;
    public const int CONNECTED_ROPE_LAYER       = 10;
    #endregion

    #region TAG
    public const string PLAYER_TAG = "Player";
    public const string OIL_BULLET_TAG = "OilBullet";
    public const string OIL_PET_TAG = "OilPet";
    public const string FIRE_PET_TAG = "FirePet";
    #endregion

    #region ROPE_VALUE
    public const float MAX_ROPE_DISTANCE = 15f;
    #endregion

    #region PET_VALUE

    public const float ICE_MELTING_TIME = 3f;
    public const float BURN_TIME = 10f;
    public const float FIRE_RADIUS = 1.5f;

    #endregion
}