using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
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
    [SerializeField]
    private Color[] matColors;

    [Header("BOARD")]
    [SerializeField]
    private Transform board;

    [SerializeField]
    private BoxCollider boardCollider;

    [Header("PIECE")]
    [SerializeField]
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
    public PlatformPiece this[int index]
    {
        get => pieces[index];
    }

    public int BoardCount => boardInformation.Count;

    private int destroyPuzzleCnt = 0;

    public Action OnClear { get; set; }


    private void Start()
    {
        Initialize();
        gameObject.SetActive(false);
    }

    public void StartGame(Func<int, bool> func)
    {
        CreatePortal(oilPortal, oilPortals, oilPortalTransform);
        CreatePortal(firePortal, firePortals, firePortalTransform);

        InitializeFirePortal(func);
    }

    private void Initialize()
    {
        float scaleWeight = boardCollider.transform.localScale.x;
        float width = boardCollider.size.x;
        float height = boardCollider.size.z;

        int boardCnt = boardInformation.Count;

        Vector3 offset = board.position;

        for (int i = 0; i < boardCnt; i++)
        {
            int length = boardInformation[i].Length;
            for (int j = 0; j < length; j++)
            {
                PlatformPiece newObj = Instantiate(platformPiece);
                newObj.name = $"({i}, {j}) : {boardInformation[i][j]}";
                newObj.Initialize(boardInformation[i][j] - '1', ref colors, ref matColors);
                newObj.OnDestroyPlatform += CheckSolve;
                newObj.transform.SetParent(transform);

                Vector3 platformPos = offset;
                platformPos += board.forward * -width / (float)length * i * scaleWeight;
                platformPos += board.right * height / (float)boardCnt * j * scaleWeight;

                newObj.transform.position = platformPos;
                newObj.transform.localRotation = Quaternion.identity;

                Vector3 scale = newObj.transform.localScale;
                scale.x = 1 / (float)boardInformation.Count * scaleWeight;
                scale.z = 1 / (float)boardInformation[0].Length * scaleWeight;

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
            pos += -box.transform.forward * i * box.size.z / (float)connectCount;
            pos += box.transform.right * box.transform.localScale.x * 1.5f;
            pos.x -= box.size.z / (float)connectCount * 0.5f;
            pos.y += 1f;

            newPortal.transform.position = pos;
        }
    }

    private void InitializeFirePortal(Func<int, bool> func)
    {
        for (int i = 0; i < connectCount; i++)
        {
            FirePortal fPortal = firePortals[i] as FirePortal;
            fPortal.StartListeningBurn(pieces.Find(x => x.Index == i).Burn);
            fPortal.StartListeningBurn(pieces.FindLast(x => x.Index == i).Burn);
            fPortal.StartListeningCanNotBurn(func);
            fPortal.StartListeningCanNotBurn(IsDestroyed);
            fPortal.SetIndex(i);
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

    private bool IsDestroyed(int idx)
    {
        return pieces.Find(x => x.Index == idx).IsDestroyed;
    }
}
