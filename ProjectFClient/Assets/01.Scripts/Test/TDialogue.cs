using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectF
{
    public class TDialogue : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DialogueManager.Instance.StartDialogue(Dialogues.ESpeakerType.Admin, "[테스트테스트테스트테스트테스트테스트테스트][테스트테스트테스트테스트테스트테스트테스트][테스트테스트테스트테스트테스트테스트테스트]", null);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
