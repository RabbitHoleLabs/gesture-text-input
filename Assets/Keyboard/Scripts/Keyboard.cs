using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Normal.UI {
    public class Keyboard : MonoBehaviour {
        public delegate void KeyPressedDelegate(Keyboard keyboard, string keyPress);
        public event KeyPressedDelegate keyPressed;

        [SerializeField]
        private GameObject  _letters;

        [SerializeField]
        private GameObject  _numbers;

        [SerializeField]
        private KeyboardKey _layoutSwapKey;

        private KeyboardMallet[] _mallets;
        private KeyboardKey[]    _keys;

        private bool _shift = false;
        public  bool  shift { get { return _shift; } set { SetShift(value); } }

        public enum Layout {
            Letters,
            Numbers
        };

        private Layout _layout = Layout.Letters;
        public  Layout  layout { get { return _layout; } set { SetLayout(value); } }

        // Helpers for gesture-based input
        private KeyboardKey prevKey = null;
        private string KeySequence = "";
        //private List<string> wordSequence = new List<string>();
        private bool isRightTrigger = false;

        void Awake() {
            Debug.Log(new DirectoryInfo("./Resources/dictionary.txt").FullName);
            Debug.Log(new DirectoryInfo("./Resources").Exists);

            TextAsset dictFile = (TextAsset)Resources.Load("dictionary.txt", typeof(TextAsset));
            Debug.Log(dictFile);
            Debug.Log(dictFile.text);

            RabbitHoleGestureKeyboard RHGK = new RabbitHoleGestureKeyboard("./Resources/dictionary.txt");
            string finalPath = "bvcxasdfttrewr";
            List<string> suggestions = RHGK.getSuggestions(finalPath);
            string output = string.Join(",", suggestions.ToArray());
            Debug.Log(output);



            _mallets = GetComponentsInChildren<KeyboardMallet>(true);
            _keys    = GetComponentsInChildren<KeyboardKey>(true);

            foreach (KeyboardMallet mallet in _mallets)
                mallet._keyboard = this;

            foreach (KeyboardKey key in _keys)
                key._keyboard = this;
        }

        void Update()
        {
            // is right trigger down? 
            float rightTriggerVal = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
            if (rightTriggerVal > 0.35f) // passes (somewhat arbitrary) float value threshold 
            {
                isRightTrigger = true;
            }
            else
            {
                // releasing trigger 
                if (isRightTrigger) // check previous value 
                {
                    // start new word 
                    //wordSequence.Add(KeySequence);
                    //Debug.Log(wordSequence);
                    Debug.Log(KeySequence);
                    KeySequence = "";
                }

                isRightTrigger = false; 
            }
        }

        // Internal
        public void _MalletStruckKeyboardKey(KeyboardMallet mallet, KeyboardKey key) {
            // Did we hit the key for another keyboard?
            if (key._keyboard != this)
                return;

            GameObject grandparent = key.transform.parent.gameObject.transform.parent.gameObject;
            bool isLetter = grandparent == _letters;
            if (isLetter) {
                // Gesture Mode: Don't trigger the same key twice
                if (key == prevKey)
                    return;
                prevKey = key;

                // log keys 
                KeySequence += key.character;

            }

            // Trigger key press animation
            key.KeyPressed();

            // Fire key press event
            if (keyPressed != null) {
                string keyPress = key.GetCharacter();

                bool shouldFireKeyPressEvent = true;

                if (keyPress == "\\s") {
                    // Shift
                    shift = !shift;
                    shouldFireKeyPressEvent = false;
                } else if (keyPress == "\\l") {
                    // Layout swap
                    if (layout == Layout.Letters)
                        layout = Layout.Numbers;
                    else if (layout == Layout.Numbers)
                        layout = Layout.Letters;

                    shouldFireKeyPressEvent = false;
                } else if (keyPress == "\\b") {
                    // Backspace
                    keyPress = "\b";
                } else {
                    // Turn off shift after typing a letter
                    if (shift && layout == Layout.Letters)
                        shift = false;
                }

                if (shouldFireKeyPressEvent)
                    keyPressed(this, keyPress);
            }
        }

        void SetShift(bool shift) {
            if (shift == _shift)
                return;

            foreach (KeyboardKey key in _keys)
                key.shift = shift;
            
            _shift = shift;
        }

        void SetLayout(Layout layout) {
            if (layout == _layout)
                return;

            shift = false;

            if (layout == Layout.Letters) {
                // Swap layouts
                _letters.SetActive(true);
                _numbers.SetActive(false);

                // Update layout swap key
                _layoutSwapKey.displayCharacter      = "123";
                _layoutSwapKey.shiftDisplayCharacter = "123";
                _layoutSwapKey.RefreshDisplayCharacter();
            } else if (layout == Layout.Numbers) {
                // Swap layouts
                _letters.SetActive(false);
                _numbers.SetActive(true);

                // Update layout swap key
                _layoutSwapKey.displayCharacter      = "abc";
                _layoutSwapKey.shiftDisplayCharacter = "abc";
                _layoutSwapKey.RefreshDisplayCharacter();
            }

            _layout = layout;
        }
    }

}
