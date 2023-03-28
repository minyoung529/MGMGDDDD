using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class LinePuzzle : MonoBehaviour
{
    [Header("PUZZLE")]
    [SerializeField]
    private List<string> boardInformation;
    [SerializeField]
    private int connectCount = 0;
    [SerializeField]
    private Color[] colors;

    [Header("BOARD")]
    [SerializeField]
    private ProBuilderMesh board;

    [SerializeField]
    private BoxCollider boardCollider;

    [Header("PIECE")]
    [SerializeField]
    //private NavMeshSurface platformPiece;
    private PlatformPiece platformPiece;

    [Header("OIL_PORTAL")]
    [SerializeField]
    private ConnectionPortal oilPortal;
    private List<ConnectionPortal> oilPortals = new();
    [SerializeField]
    private BoxCollider oilPortalTransform;

    [Header("FIRE_PORTAL")]
    [SerializeField]
    private ConnectionPortal firePortal;
    private List<ConnectionPortal> firePortals = new();
    [SerializeField]
    private BoxCollider firePortalTransform;

    #region Property
    public IReadOnlyList<ConnectionPortal> OilPortals => oilPortals;
    #endregion

    private List<PlatformPiece> pieces = new();
    private int destroyPuzzleCnt = 0;

    public Action OnClear { get; set; }

    private void Awake()
    {
        Initilize();
    }

    public void StartGame()
    {
        CreatePortal(oilPortal, oilPortals, oilPortalTransform);
        CreatePortal(firePortal, firePortals, firePortalTransform);

        InitilizeFirePortal();
    }

    private void Initilize()
    {
        float weight = boardCollider.size.x;
        float height = boardCollider.size.z;

        int boardCnt = boardInformation.Count;

        Vector3 offset = board.transform.position + Vector3.up * 0.7f /*+ new Vector3(weight / boardCnt * 0.5f, 0, -height / boardCnt * 0.5f)*/;

        for (int i = 0; i < boardCnt; i++)
        {
            int length = boardInformation[i].Length;
            for (int j = 0; j < length; j++)
            {
                PlatformPiece newObj = Instantiate(platformPiece);
                newObj.name = $"({i}, {j}) : {boardInformation[i][j]}";
                newObj.Initialize(boardInformation[i][j] - '1', ref colors);

                newObj.OnDestroyPlatform += CheckSolve;

                newObj.transform.position =
                    new Vector3
                    (
                        weight / length * j,
                        0f,
                        -height / boardCnt * i
                    ) + offset;

                newObj.transform.SetParent(transform);

                Vector3 scale = newObj.transform.localScale;
                scale.x = 1 / (float)boardInformation.Count;
                scale.z = 1 / (float)boardInformation[0].Length;

                newObj.transform.localScale = scale;

                pieces.Add(newObj);
            }
        }
    }

    private void CreatePortal(ConnectionPortal portalPrefab, List<ConnectionPortal> portals, BoxCollider box)
    {
        for (int i = 0; i < connectCount; i++)
        {
            ConnectionPortal newPortal = Instantiate(portalPrefab);
            newPortal.Initialize(i, ref colors);
            portals.Add(newPortal);

            Vector3 pos = box.transform.position;
            pos.z -= i * box.size.z / connectCount + box.size.z / connectCount * 0.5f;
            pos.y += 1f;

            pos += box.transform.right * 2f * box.transform.localScale.x;

            newPortal.transform.position = pos;
        }
    }

    private void InitilizeFirePortal()
    {
        for (int i = 0; i < connectCount; i++)
        {
            FirePortal fPortal = firePortals[i] as FirePortal;
            fPortal.Listen(pieces.Find(x => x.Index == i).Burn);
            fPortal.Listen(pieces.FindLast(x => x.Index == i).Burn);
        }
    }

    public void EndPuzzle()
    {
        for (int i = 0; i < connectCount; i++)
        {
            Destroy(oilPortals[i].gameObject);
            Destroy(firePortals[i].gameObject);
        }
    }

    private void CheckSolve()
    {
        // »ö±ò ¹Ù²î´Â ¹ö±×
        // Trigger ¶§¹®¿¡~~

        // Oil Paint ¾ÈµÊ
        // ¤À°¡¤³

        if (++destroyPuzzleCnt == boardInformation.Count * boardInformation[0].Length)
        {
            EndPuzzle();
            OnClear?.Invoke();
        }
    }

    public void ResetOil()
    {
        pieces.ForEach(x => x.ResetOilSpread());
    }

    public void ResetPuzzle()
    {
        ResetOil();
        pieces.ForEach(x => x.ResetPuzzle());
        destroyPuzzleCnt = 0;
    }
}
