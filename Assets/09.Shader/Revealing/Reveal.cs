using UnityEngine;

[ExecuteAlways] // 에디터 모드에서도 실행되어 테스트가 쉽다.
public class Reveal : MonoBehaviour
{
    [SerializeField] Light spotLight;

    private Material m_Mat;

    private void Start()
    {
        m_Mat = GetComponent<Renderer>().sharedMaterial;

        SetLight(false);
    }

    private void Update()
    {
        m_Mat.SetVector("_MyLightPosition", spotLight.transform.position);
        m_Mat.SetVector("_MyLightDirection", -spotLight.transform.forward);
    }

    public void SetLight(bool value)
    {
        int rot = 0;
        if (value) rot = 90;

        spotLight.gameObject.SetActive(value);
        spotLight.transform.eulerAngles = new Vector3(rot, -90, 0);
    }
    public void SwitchLight()
    {
        if(spotLight.gameObject.activeSelf) SetLight(false);
        else SetLight(true);
    }

}