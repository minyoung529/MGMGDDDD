using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using �ʼ�
using UnityEngine.Playables;

public class ExamplePlayableAsset : PlayableAsset
{
	// duration : Playable ����ð�

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        throw new System.NotImplementedException();
    }

 //   // ����: �� �Լ��� Play��尡 �ƴ� Edit��忡����(�÷��� ��ư�� ������ �ʾ��� ����) ȣ���.
 //   public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	//{
	//	// Ʈ���� �����ϴ� Ŭ���� ����
	//	int inputCount = playable.GetInputCount();
	//	int currentInputCount = 0;
		
	//	for (int i = 0; i < inputCount; i++)
	//	{
	//		// Ŭ���� blending�� ����Ͽ� ����ϴ� Ʈ�� ������(0~1)
	//		float inputWeight = playable.GetInputWeight(i);
			
	//		if (inputWeight > 0f)
	//			currentInputCount++;
	//	}

 //       // Ŭ���� ���� ���� ������ ��
 //       if (currentInputCount == 0)
 //       {
 //           Debug.Log("����");
 //       }
 //       // Ŭ���� 1���� ���� ������ ��
 //       else if(currentInputCount == 1)
 //       {
 //           Debug.Log("1��");
 //       }
	//// Ŭ���� 2�� �̻��� �����ǰ� �ִ� ���� ������ ��
 //       else
 //       {
 //           Debug.Log("����");
 //       }
	//}


}
