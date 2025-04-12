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

        public static readonly float DialogueSpeakerImageSizeWidth = 400.0f;

        private static Transform mainCanvas = null;
        public static Transform MainCanvas {
            get {
                if(mainCanvas == null)
                {
                    GameObject mainCanvasObject = GameObject.Find("MainCanvas");
                    if(mainCanvasObject != null)
                        mainCanvas = mainCanvasObject.transform;
                }

                return mainCanvas;
            }
        }

        private static Transform mainPopupFrame = null;
        public static Transform MainPopupFrame {
            get {
                if(mainPopupFrame == null)
                {
                    GameObject mainPopupCanvas = GameObject.Find("MainPopupCanvas");
                    if(mainPopupCanvas != null)
                        mainPopupFrame = mainPopupCanvas.transform;
                }

                return mainPopupFrame;
            }
        }

        private static Transform contentPopupFrame = null;
        public static Transform ContentPopupFrame {
            get {
                if(contentPopupFrame == null)
                {
                    GameObject contentPopupCanvas = GameObject.Find("ContentPopupCanvas");
                    if(contentPopupCanvas != null)
                        contentPopupFrame = contentPopupCanvas.transform;
                }

                return contentPopupFrame;
            }
        }

        private static Transform topPopupFrame = null;
        public static Transform TopPopupFrame {
            get {
                if(topPopupFrame == null)
                {
                    GameObject topPopupCanvas = GameObject.Find("TopPopupCanvas");
                    if(topPopupCanvas != null)
                        topPopupFrame = topPopupCanvas.transform;
                }

                return topPopupFrame;
            }
        }

        private static Transform highestPopupFrame = null;
        public static Transform HighestPopupFrame {
            get {
                if(highestPopupFrame == null)
                {
                    GameObject highestPopupCanvas = GameObject.Find("HighestPopupCanvas");
                    if(highestPopupCanvas != null)
                        highestPopupFrame = highestPopupCanvas.transform;
                }

                return highestPopupFrame;
            }
        }
    }
}