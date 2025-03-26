using H00N.OptOptions;
using UnityEngine;

namespace ProjectF
{
    public static class GameDefine
    {
        public static readonly OptOption<string> DefaultColorOption = new OptOption<string>("#F2F2F2", "#F30000");
        public static readonly string DefaultGoldColor = "#FFCF4A";
        public static readonly string AbleBehavioutColor = "#64A980";
        public static readonly string UnAbleBehaviourColor = "#FF6E6E";

        private static Transform mainCanvas = null;
        public static Transform MainCanvas {
            get {
                if(mainCanvas == null)
                    mainCanvas = GameObject.Find("MainCanvas").transform;

                return mainCanvas;
            }
        }

        private static Transform mainPopupFrame = null;
        public static Transform MainPopupFrame {
            get {
                if(mainPopupFrame == null)
                    mainPopupFrame = MainCanvas.Find("MainPopupFrame");

                return mainPopupFrame;
            }
        }

        private static Transform contentsPopupFrame = null;
        public static Transform ContentsPopupFrame {
            get {
                if(contentsPopupFrame == null)
                    contentsPopupFrame = MainCanvas.Find("ContentsPopupFrame");

                return contentsPopupFrame;
            }
        }

        private static Transform topPopupFrame = null;
        public static Transform TopPopupFrame {
            get {
                if(topPopupFrame == null)
                    topPopupFrame = MainCanvas.Find("TopPopupFrame");

                return topPopupFrame;
            }
        }
    }
}