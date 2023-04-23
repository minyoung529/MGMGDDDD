using UnityEngine;

[ExecuteAlways] // 에디터 모드에서도 실행되어 테스트가 쉽다.
public class Reveal : MonoBehaviour
{
    [SerializeField] Light spotLight;

    private Material m_Mat;
    private ChangeShaderFloat shaderFloat;

    private void Start()
    {
        m_Mat = GetComponent<Renderer>().sharedMaterial;
        shaderFloat= GetComponent<ChangeShaderFloat>();

        SetLight(false);
    }

    private void Update()
    {
        m_Mat.SetVector("_MyLightPosition", spotLight.transform.position);
        m_Mat.SetVector("_MyLightDirection", -spotLight.transform.forward);
    }

    public void SetLight(bool value)
    {
        spotLight.gameObject.SetActive(value);

        if(value)
        {
        shaderFloat.Active();
        }
        else
        {
        shaderFloat.Inactive();
        }
    }
    public void SwitchLight()
    {
        if(spotLight.gameObject.activeSelf) SetLight(false);
        else SetLight(true);
    }

}