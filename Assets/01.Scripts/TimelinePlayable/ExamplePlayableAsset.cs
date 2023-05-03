using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using 필수
using UnityEngine.Playables;

public class ExamplePlayableAsset : PlayableAsset
{
	// duration : Playable 재생시간

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        throw new System.NotImplementedException();
    }

 //   // 주의: 이 함수는 Play모드가 아닌 Edit모드에서도(플레이 버튼을 누르지 않았을 때도) 호출됨.
 //   public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	//{
	//	// 트랙에 존재하는 클립의 갯수
	//	int inputCount = playable.GetInputCount();
	//	int currentInputCount = 0;
		
	//	for (int i = 0; i < inputCount; i++)
	//	{
	//		// 클립의 blending에 비례하여 계산하는 트랙 점유율(0~1)
	//		float inputWeight = playable.GetInputWeight(i);
			
	//		if (inputWeight > 0f)
	//			currentInputCount++;
	//	}

 //       // 클립이 없는 곳을 지나는 중
 //       if (currentInputCount == 0)
 //       {
 //           Debug.Log("없음");
 //       }
 //       // 클립이 1개인 곳을 지나는 중
 //       else if(currentInputCount == 1)
 //       {
 //           Debug.Log("1개");
 //       }
	//// 클립이 2개 이상인 블렌딩되고 있는 곳을 지나는 중
 //       else
 //       {
 //           Debug.Log("블렌딩");
 //       }
	//}


}
