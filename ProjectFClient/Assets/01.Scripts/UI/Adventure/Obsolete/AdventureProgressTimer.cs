// using System.Collections;
// using System.Collections.Generic;
// using ProjectF.UI.Adventure;
// using UnityEngine;
// using TMPro;

// namespace ProjectF
// {
//     public class AdventureProgressTimer : AdventureProgressContentUI
//     {
//         public GameObject progressTimer;
//         public GameObject adventureCancleButton;
//         public TextMeshProUGUI timerText;

//         private double _currentRemainSeconds;
//         private float _elapsedTime;

//         public void Initialize(double remainSeconds)
//         {
//             Debug.Log(remainSeconds);
//             _currentRemainSeconds = remainSeconds;
//         }

//         private void Update()
//         {
//             if (_currentRemainSeconds <= 0) return;

//             _elapsedTime += Time.deltaTime;

//             if (_elapsedTime >= 1f)
//             {
//                 int hours = (int)(_currentRemainSeconds / 3600);
//                 int minute = (int)((_currentRemainSeconds % 3600) / 60);
//                 int seconds = (int)(_currentRemainSeconds % 60);

//                 timerText.text = $"{hours} : {minute} : {seconds}";

//                 _elapsedTime = 0;
//                 _currentRemainSeconds -= 1;
//             }
//         }

//         public override void Active()
//         {
//             progressTimer.SetActive(true);
//             adventureCancleButton.SetActive(true);
//         }

//         public override void DeActive()
//         {
//             progressTimer.SetActive(false);
//             adventureCancleButton.SetActive(false);
//         }
//     }
// }
