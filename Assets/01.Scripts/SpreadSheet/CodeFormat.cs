using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
public class CodeFormat
{
    public static string CharFormat =
@"using UnityEngine;

public class {0} : MonoBehaviour
{{
    private const string _name = ""{1}"";
    private CharacterType _type = CharacterType.{2};

    public void Introduce()
    {{
        Debug.Log($""{{_name}} 이고 {{_type}} 입니다."");
    }}
}}";
}
#endif