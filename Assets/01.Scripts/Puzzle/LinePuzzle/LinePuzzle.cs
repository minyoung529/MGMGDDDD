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

    public void StartGame()
    {
        Initilize();
        CreatePortal(oilPortal, oilPortals, oilPortalTransform);
        CreatePortal(firePortal, firePortals, firePortalTransform);

        InitilizeFirePortal();
    }

    private void Initilize()
    {
        float weight = boardCollider.size.x;
        float height = boardCollider.size.z;

        int boardCnt = boardInformation.Count;

        Vector3 offset = board.transform.position /*+ new Vector3(weight / boardCnt * 0.5f, 0, -height / boardCnt * 0.5f)*/;

        for (int i = 0; i < boardCnt; i++)
        {
            int length = boardInformation[i].Length;
            for (int j = 0; j < length; j++)
            {
                PlatformPiece newObj = Instantiate(platformPiece);
                newObj.name = $"({i}, {j}) : {boardInformation[i][j]}";
                newObj.Initialize(boardInformation[i][j] - '1', ref colors);

                newObj.OnDestroyPlatform += DestroyPuzzle;

                newObj.transform.position =
                    new Vector3
                    (
                        weight / length * j,
                        board.transform.position.y,
                        -height / boardCnt * i
                    );

                newObj.transform.position += offset;
                newObj.transform.SetParent(transform);

                pieces.Add(newObj);
            }
        }

        platformPiece.gameObject.SetActive(false);
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

    private void DestroyPuzzle()
    {
        if (++destroyPuzzleCnt == boardInformation.Count * boardInformation[0].Length)
        {
            Debug.Log("CLEAR");
        }
    }

    public void ResetOil()
    {
        pieces.ForEach(x => x.ResetOilSpread());
    }

    public void ResetPuzzle()
    {
        ResetOil();
        destroyPuzzleCnt = 0;
    }
}
