using System;
using System.Collections.Generic;
using System.Linq;

namespace NotAnAPI.ReKact.Core
{
    public static class KeyCodeToChar
    {
        private static Random _random = new();

        // List from https://keycode.info/ - Thanks to Wes Bos
        private static Dictionary<int, string[]> _keyCodes = new()
        {
            {0, new[] {"That key has no keycode"}},
            {3, new[] {"[BREAK]"}},
            {8, new[] {"[BACKSPACE]"}},
            {9, new[] {"[TAB]"}},
            {12, new[] {"[CLEAR]"}},
            {13, new[] {"[ENTER]"}},
            {16, new[] {"[SHIFT]"}},
            {17, new[] {"[CTRL]"}},
            {18, new[] {"[ALT]"}},
            {19, new[] {"[PAUSE]"}},
            {20, new[] {"[CAPS-LOCK]"}},
            {21, new[] {"[HANGUL]"}},
            {25, new[] {"[HANJA]"}},
            {27, new[] {"[ESCAPE]"}},
            {28, new[] {"[CONVERSION]"}},
            {29, new[] {"[NON-CONVERSION]"}},
            {32, new[] {" "}},
            {33, new[] {"[PAGE-UP]"}},
            {34, new[] {"[PAGE-DOWN]"}},
            {35, new[] {"[END]"}},
            {36, new[] {"[HOME]"}},
            {37, new[] {"[LEFT-ARROW]"}},
            {38, new[] {"[UP-ARROW]"}},
            {39, new[] {"[RIGHT-ARROW]"}},
            {40, new[] {"[DOWN-ARROW]"}},
            {41, new[] {"[SELECT]"}},
            {42, new[] {"[PRINT]"}},
            {43, new[] {"[EXECUTE]"}},
            {44, new[] {"[Print-Screen]"}},
            {45, new[] {"[INSERT]"}},
            {46, new[] {"[DELETE]"}},
            {47, new[] {"[HELP]"}},
            {48, new[] {"0", ")"}},
            {49, new[] {"1", "!"}},
            {50, new[] {"2", "@"}},
            {51, new[] {"3", "#"}},
            {52, new[] {"4", "$"}},
            {53, new[] {"5", "%"}},
            {54, new[] {"6", "^"}},
            {55, new[] {"7", "&"}},
            {56, new[] {"8", "*"}},
            {57, new[] {"9", "("}},
            {58, new[] {":"}},
            {59, new[] {","}},
            {60, new[] {"<"}},
            {61, new[] {"="}},
            {63, new[] {"ß"}},
            {64, new[] {"@"}},
            {65, new[] {"a"}},
            {66, new[] {"b"}},
            {67, new[] {"c"}},
            {68, new[] {"d"}},
            {69, new[] {"e"}},
            {70, new[] {"f"}},
            {71, new[] {"g"}},
            {72, new[] {"h"}},
            {73, new[] {"i"}},
            {74, new[] {"j"}},
            {75, new[] {"k"}},
            {76, new[] {"l"}},
            {77, new[] {"m"}},
            {78, new[] {"n"}},
            {79, new[] {"o"}},
            {80, new[] {"p"}},
            {81, new[] {"q"}},
            {82, new[] {"r"}},
            {83, new[] {"s"}},
            {84, new[] {"t"}},
            {85, new[] {"u"}},
            {86, new[] {"v"}},
            {87, new[] {"w"}},
            {88, new[] {"x"}},
            {89, new[] {"y"}},
            {90, new[] {"z"} },
            {91, new[] {"[Windows Key / Left ⌘ / Chromebook Search key]"}},
            {92, new[] {"[RIGHT-WINDOWS-KEY]"}},
            {93, new[] {"[Windows Menu / Right ⌘]"}},
            {95, new[] {"[SLEEP]"}},
            {96, new[] {"0", "[INSERT]"}},
            {97, new[] {"1", "[END]"}},
            {98, new[] {"2", "[ARROW-DOWN]"}},
            {99, new[] {"3", "[PAGE-DOWN]"}},
            {100, new[] {"4", "[ARROW-LEFT]"}},
            {101, new[] {"5", "[CLEAR]"}},
            {102, new[] {"6", "[ARROW-RIGHT]"}},
            {103, new[] {"7", "[HOME]"}},
            {104, new[] {"8", "[ARROW-UP]"}},
            {105, new[] {"9", "[PAGE-UP]"}},
            {106, new[] {"*"}},
            {107, new[] {"+"}},
            {108, new[] {"."}},
            {109, new[] {"-"}},
            {110, new[] {".", "[DELETE]"}},
            {111, new[] {"/"}},
            {112, new[] {"[F1]"}},
            {113, new[] {"[F2]"}},
            {114, new[] {"[F3]"}},
            {115, new[] {"[F4]"}},
            {116, new[] {"[F5]"}},
            {117, new[] {"[F6]"}},
            {118, new[] {"[F7]"}},
            {119, new[] {"[F8]"}},
            {120, new[] {"[F9]"}},
            {121, new[] {"[F10]"}},
            {122, new[] {"[F11]"}},
            {123, new[] {"[F12]"}},
            {124, new[] {"[F13]"}},
            {125, new[] {"[F14]"}},
            {126, new[] {"[F15]"}},
            {127, new[] {"[F16]"}},
            {128, new[] {"[F17]"}},
            {129, new[] {"[F18]"}},
            {130, new[] {"[F19]"}},
            {131, new[] {"[F20]"}},
            {132, new[] {"[F21]"}},
            {133, new[] {"[F22]"}},
            {134, new[] {"[F23]"}},
            {135, new[] {"[F24]"}},
            {136, new[] {"[F25]"}},
            {137, new[] {"[F26]"}},
            {138, new[] {"[F27]"}},
            {139, new[] {"[F28]"}},
            {140, new[] {"[F29]"}},
            {141, new[] {"[F30]"}},
            {142, new[] {"[F31]"}},
            {143, new[] {"[F32]"}},
            {144, new[] {"[NUM-LOCK]"}},
            {145, new[] {"[SCROLL-LOCK]"}},
            {151, new[] {"[AIRPLANE-MODE]"}},
            {160, new[] {"^"}},
            {161, new[] {"!"}},
            {162, new[] {"؛"}},
            {163, new[] {"#"}},
            {164, new[] {"$"}},
            {165, new[] {"ù"}},
            {166, new[] {"[PAGE-BACKWARD]"}},
            {167, new[] {"[PAGE-FORWARD]"}},
            {168, new[] {"[REFRESH]"}},
            {169, new[] {")", "°"}},
            {170, new[] {"*"}},
            {171, new[] {"~*"}},
            {172, new[] {"[HOME-KEY]"}},
            {173, new[] {"[MUTE/UNMUTE]"}},
            {174, new[] {"[DECREASE-VOLUME-LEVEL]"}},
            {175, new[] {"[INCREASE-VOLUME-LEVEL]"}},
            {176, new[] {"[NEXT]"}},
            {177, new[] {"[PREVIOUS]"}},
            {178, new[] {"[STOP]"}},
            {179, new[] {"[PLAY/PAUSE]"}},
            {180, new[] {"[E-MAIL]"}},
            {181, new[] {"[MUTE/UNMUTE(FireFox)]"}},
            {182, new[] {"[DECREASE-VOLUME-LEVEL(FireFox)]"}},
            {183, new[] {"[INCREASE-VOLUME-LEVEL(FireFox)]"}},
            {186, new[] {";", ":"}},
            {187, new[] {"=", "+"}},
            {188, new[] {",", "<"}},
            {189, new[] {"-", "_"}},
            {190, new[] {".", ">"}},
            {191, new[] {"/", "?"}},
            {192, new[] {"`", "~"}},
            {193, new[] {"?"}},
            {194, new[] {"."}},
            {219, new[] {"[", "{"}},
            {220, new[] {"\\", "|"}},
            {221, new[] {"]", "}"}},
            {222, new[] {"\'", "\""}},
            {223, new[] {"`"}},
            {224, new[] {"[LEFT-OR-RIGHT-⌘-KEY(FireFox)]"}},
            {225, new[] {"[ALTGR]"}},
            {226, new[] {"[< /git >, left back slash]"}},
            {230, new[] {"[GNOME-COMPOSE-KEY]"}},
            {231, new[] {"ç"}},
            {233, new[] {"[XF86Forward]"}},
            {234, new[] {"[XF86Back]"}},
            {235, new[] {"[NON-CONVERSION]"}},
            {240, new[] {"[ALPHANUMERIC]"}},
            {242, new[] {"[hiragana/katakana]"}},
            {243, new[] {"[HALF-WIDTH/FULL-WIDTH]"}},
            {244, new[] {"[KANJI]"}},
            {251, new[] {"[UNLOCK-TRACKPAD(Chrome/Edge)]"}},
            {255, new[] {"[TOGGLE-TOUCHPAD]"}}
        };

        private static Dictionary<int, string[]> _charReKactKeyCodes = new();
        private static Dictionary<int, string[]> _noneCharReKactKeyCodes = new();

        /// <summary>
        /// Get a printable charReKact by the provided <see cref="KeyAct"/>.
        /// </summary>
        /// <param name="keyAct">The Act.</param>
        /// <param name="needCharReKactValue"><c>true</c> to only return a charReKact, otherwise <c>false</c> to return a none charReKact like [ENTER].</param>
        /// <returns>string.</returns>
        public static string GetKeyByKeyCode(this KeyAct keyAct, bool needCharReKactValue = false)
        {
            _fillCharReKactKeyCodes();
            _fillNoneCharReKactKeyCodes();

            int keyCode = keyAct.KeyCode;

            if (keyCode == -2)
            {
                bool specialKeysAccepted = needCharReKactValue == false;
                keyCode = _getRandomKeyCodeInRange(specialKeysAccepted ? 32 : 48, specialKeysAccepted ? 127 : 91, needCharReKactValue ? _charReKactKeyCodes : _noneCharReKactKeyCodes);

            } else if (keyCode == -3)
            {
                keyCode = _getRandomKeyCodeInRange(33, 48, _noneCharReKactKeyCodes);
            } else if (keyCode == -4)
            {
                keyCode = _getRandomKeyCodeInRange(112, 124, _noneCharReKactKeyCodes);
            }

            if (_keyCodes.ContainsKey(keyCode))
            {
                if (keyAct.Shift && !keyAct.Ctrl && !keyAct.Meta && !keyAct.Alt)
                {
                    return keyCode is >= 65 and <= 90
                        ? _keyCodes[keyCode][0].ToUpper()
                        : _keyCodes[keyCode].LastOrDefault();
                }
                if (keyAct.Modifier > 0)
                {
                    List<string> modifiers = new();
                    if (keyAct.Shift) modifiers.Add("SHIFT");
                    if (keyAct.Ctrl) modifiers.Add("CTRL");
                    if (keyAct.Meta) modifiers.Add("META");
                    if (keyAct.Alt) modifiers.Add("ALT");

                    string joinedModifiers = String.Join("-", modifiers);

                    if (("[" + joinedModifiers + "]") == _keyCodes[keyCode][0]) return _keyCodes[keyCode][0];
                    return $"[{joinedModifiers}-{_keyCodes[keyCode][0]}]";
                }
                return _keyCodes[keyCode][0];
            }

            return "$" + keyCode + "$";
        }

        private static int _getRandomKeyCodeInRange(int firstKeyCode,
            int lastKeyCode, Dictionary<int, string[]> sourceDictionary = default)
        {
            sourceDictionary ??= _keyCodes;
            var keyCodesInRange = sourceDictionary.Where(x => x.Key >= firstKeyCode && x.Key <= lastKeyCode).ToList();
            return keyCodesInRange[_random.Next(0, keyCodesInRange.Count)].Key;
        }

        private static void _fillCharReKactKeyCodes()
        {
            if (_charReKactKeyCodes.Count == 0)
            {
                foreach (KeyValuePair<int, string[]> keyValuePair in _keyCodes)
                {
                    if (keyValuePair.Value.Length == 1 && keyValuePair.Value[0].StartsWith("[") == false && keyValuePair.Value[0].EndsWith("]") == false) _charReKactKeyCodes.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        private static void _fillNoneCharReKactKeyCodes()
        {
            if (_noneCharReKactKeyCodes.Count == 0)
            {
                foreach (KeyValuePair<int, string[]> keyValuePair in _keyCodes)
                {
                    if (keyValuePair.Value.Any(x => x.StartsWith("[") && x.EndsWith("]"))) _noneCharReKactKeyCodes.Add(keyValuePair.Key, new []{keyValuePair.Value.FirstOrDefault(x => x.StartsWith("[") && x.EndsWith("]"))});
                }
            }
        }
    }
}