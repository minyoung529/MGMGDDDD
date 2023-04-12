using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MathPuzzleController : MonoBehaviour
{
    [Header("GROUP")]
    private List<PlateGroup> plateGroups;
    // [0] = NUMBER, [1] = OPERATOR, [2] = NUMBER

    [SerializeField]
    private List<MathPuzzleSetting> settings;

    private int targetValue;
    private int curIdx = 0;

    [Header("CLEAR")]
    [SerializeField]
    private UnityEvent onClearPuzzle;

    [SerializeField]
    Pair<DigitalNumber, DigitalNumber> targetValueObject;

    #region PROPERTY
    public int FirstNumber => plateGroups[0].Value;
    public OperatorType Operator => plateGroups[1].OperatorType;
    public int SecondNumber => plateGroups[2].Value;
    #endregion

    private void Awake()
    {
        plateGroups = GetComponentsInChildren<PlateGroup>().ToList();
    }

    private void Start()
    {
        plateGroups.ForEach(x => x.ListeningOnSelected(CalculateResult));
        StartPuzzle();
    }

    private void StartPuzzle()
    {
        ResetPuzzle();

        targetValue = settings[curIdx].targetValue;
        SetTargetNumberObject();

        plateGroups[0].SetNumberPairs(settings[curIdx].firstNumbers);
        plateGroups[2].SetNumberPairs(settings[curIdx].secondNumbers);

        plateGroups[1].SetOperators(settings[curIdx].operatorTypes);

        curIdx++;
    }

    private void ResetPuzzle()
    {
        plateGroups.ForEach(x => x.ResetPuzzle());
    }

    private int CalculatedValue() => Operator switch
    {
        OperatorType.Plus => FirstNumber + SecondNumber,
        OperatorType.Minus => FirstNumber - SecondNumber,
        OperatorType.Multiply => FirstNumber * SecondNumber,
        OperatorType.Divide => FirstNumber * SecondNumber,
        _ => 0
    };

    public void CalculateResult()
    {
        if (FirstNumber == 0 || Operator == OperatorType.None || SecondNumber == 0)
        {
            return;
        }

        if (CalculatedValue() == targetValue)
        {
            for (int i = 1; i < plateGroups.Count; i++)
                plateGroups[i].Success(null);

            plateGroups[0].Success(() =>
            {
                StartPuzzle();
                onClearPuzzle?.Invoke();
            });
        }
        else
        {
            for (int i = 1; i < plateGroups.Count; i++)
                plateGroups[i].Fail(null);

            plateGroups[0].Fail(StartPuzzle);
        }
    }

    private void SetTargetNumberObject()
    {
        string sNumber = targetValue.ToString();

        if (sNumber.Length == 1)
        {
            targetValueObject.second.gameObject.SetActive(false);
            targetValueObject.first.SetNumber(sNumber[0] - '0');
        }
        else // 2
        {
            targetValueObject.second.gameObject.SetActive(true);

            targetValueObject.first.SetNumber(sNumber[0] - '0');
            targetValueObject.second.SetNumber(sNumber[1] - '0');
        }
    }
}
