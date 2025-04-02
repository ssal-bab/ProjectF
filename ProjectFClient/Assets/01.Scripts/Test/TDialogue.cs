using System.Collections;
using System.Collections.Generic;
using ProjectF.Datas;
using UnityEngine;

namespace ProjectF
{
    public class TDialogue : MonoBehaviour
    {
        // Start is called before the first frame update

        private float lastTouchTime;
        void Start()
        {
            //DialogueManager.Instance.StartDialogue(Dialogues.ESpeakerType.Admin, "[테스트테스트테스트테스트테스트테스트테스트][테스트테스트테스트테스트테스트테스트테스트][테스트테스트테스트테스트테스트테스트테스트]", null);
            lastTouchTime = Time.time;
        }

        // Update is called once per frame
    //     void Update()
    //     {
    //         if(Input.touchCount == 1) {
    //         Touch touch = Input.GetTouch(0);

    //         switch (touch.phase)
    //         {
    //             case TouchPhase.Began:
    //                 if(Time.time - lastTouchTime < 0.5f) // 더블터치 판정
    //                 {
    //                     UserActionObserver.Invoke(EActionType.OwnCrop);
    //                     UserActionObserver.Invoke(EActionType.PlantSeed);
    //                     UserActionObserver.Invoke(EActionType.HarvestCrop);
    //                 }
                        
    //                 break;

    //             case TouchPhase.Moved:
    //                 break;

    //             case TouchPhase.Ended:
    //                 lastTouchTime = Time.time;
    //                 break;
    //         }
    //         }

    //     if(Input.GetKeyDown(KeyCode.A))
    //         {
    //             UserActionObserver.Invoke(EActionType.OwnCrop);
    //             UserActionObserver.Invoke(EActionType.PlantSeed);
    //             UserActionObserver.Invoke(EActionType.HarvestCrop);
    //         }
    // }
}
}