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
        myRect = transform.GetComponent<RectTransform>(); //실시간으로 GetComponent받으려 하면 오류걸리는 구나.
        birdRect = birdTrait.GetComponent<RectTransform>();




    }

    private void Update()
    {
       
        xPos = Mathf.Lerp(xMin, xMax, slide.fillAmount); //여기에 Time.DeltaTime넣으면 안되네?
        Debug.Log(xPos);
        myRect.anchoredPosition = new Vector2(xPos, myRect.anchoredPosition.y); //anchordPos는 xPos yPos임  근데 왜 anchorPos인지 자세히 모름

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

      
        //포지션이 -10 될때마다 트레잇 생성


    }

    //씬 이동할떄 바로 그냥 삭제해버려
    IEnumerator ImagePaid(Image birdTraitImage) //씬넘어가니까 저게 있는데? 그냥 리스트에 넣고 끝나면 다 삭제해야겠다.
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
                                                                             