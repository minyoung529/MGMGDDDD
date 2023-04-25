using DG.Tweening;
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
            spotLight.transform.position = new Vector3(spotLight.transform.position.x, 0f, spotLight.transform.position.z);
            spotLight.transform.DOMoveY(30f, 1f);
            shaderFloat.Active();
        }
        else
        {
            spotLight.transform.DOMoveY(0f, 1f);
            shaderFloat.Inactive();
        }
    }
    public void SwitchLight()
    {
        if(spotLight.gameObject.activeSelf) SetLight(false);
        else SetLight(true);
    }

}