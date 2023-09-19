using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPetMove : MonoBehaviour
{
    public Image slide;
    public Image birdTrait;
    public Image myImage;
    public List<Image>  birdTraitImages;
    RectTransform myRect;
    RectTransform birdRect;
    float xMin = -850f;
    float xMax = 850f;
    float xPos;
    float traitSpawnPosLimit = -10;
    float traitSpawnPosLimitIndex = 0;

    private void Start()
    {
        myRect = transform.GetComponent<RectTransform>(); //�ǽð����� GetComponent������ �ϸ� �����ɸ��� ����.
        birdRect = birdTrait.GetComponent<RectTransform>();




    }

    private void Update()
    {
       
        xPos = Mathf.Lerp(xMin, xMax, slide.fillAmount); //���⿡ Time.DeltaTime������ �ȵǳ�?
        Debug.Log(xPos);
        myRect.anchoredPosition = new Vector2(xPos, myRect.anchoredPosition.y); //anchordPos�� xPos yPos��  �ٵ� �� anchorPos���� �ڼ��� ��

        if(xPos > xMin + 100 * traitSpawnPosLimitIndex) // -850 > 850 + -10; 
        {
            float distnace = xMin + 100 * traitSpawnPosLimitIndex;
            traitSpawnPosLimitIndex++;

            

            Image birdTraitImage  = Instantiate(birdTrait);
            birdTraitImage.transform.SetParent(GameObject.Find("Loading Scene Canvas").transform, false);
            RectTransform rect = birdTraitImage.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector3(distnace, 45.8f);


            myImage.rectTransform.SetAsLastSibling();
            birdTraitImages.Add(birdTraitImage);



            if (xPos >= 850)
            {
                for(int i = 0; i < birdTraitImages.Count;i++)
                {
                    birdTraitImages[i].gameObject.SetActive(false);
                }
                //birdTraitImages.Clear();
                traitSpawnPosLimitIndex = 0;

            }

            StartCoroutine(ImagePaid(birdTraitImage));
        }

      
        //�������� -10 �ɶ����� Ʈ���� ����


    }

    //�� �̵��ҋ� �ٷ� �׳� �����ع���
    IEnumerator ImagePaid(Image birdTraitImage) //���Ѿ�ϱ� ���� �ִµ�? �׳� ����Ʈ�� �ְ� ������ �� �����ؾ߰ڴ�.
    {
        float fadeCount = 1;
        while (fadeCount > 0.0f)
        {
            fadeCount -= 0.02f;
            yield return new WaitForSeconds(0.01f);
            birdTraitImage.color = new Color(birdTraitImage.color.r, birdTraitImage.color.g, birdTraitImage.color.b, fadeCount);
        }
     

    }
}
                                                                             