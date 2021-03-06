/*
 * Copyright (c) 2013-2014 Tobias Schulz, Maximilian Reuter, Pascal Knodel,
 *                         Gerd Augsburg, Christina Erler, Daniel Warzel
 *
 * This source code file is part of Knot3. Copying, redistribution and
 * use of the source code in this file in source and binary forms,
 * with or without modification, are permitted provided that the conditions
 * of the MIT license are met:
 *
 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 *
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 *
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE.
 *
 * See the LICENSE file for full license details of the Knot3 project.
 */

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Knot3.Framework.Core;

namespace Knot3.Framework.Utilities
{
    [ExcludeFromCodeCoverageAttribute]
    public static class TextHelper
    {
        private static Keys lastKey = Keys.None;
        private static double lastMillis = 0;

        public static bool TryTextInput (ref string str, IScreen screen, GameTime time)
        {
            bool catched = false;
            if (lastKey != Keys.None) {
                if (screen.InputManager.CurrentKeyboardState.IsKeyUp (lastKey)) {
                    lastKey = Keys.None;
                }
                else if ((time.TotalGameTime.TotalMilliseconds - lastMillis) > 200) {
                    lastKey = Keys.None;
                }
            }
            Keys[] keys = screen.InputManager.CurrentKeyboardState.GetPressedKeys ();
            if (lastKey == Keys.None) {
                for (int i = 0; i < keys.Length; ++i) {
                    if (keys [i] != Keys.LeftShift && keys [i] != Keys.RightShift) {
                        lastKey = keys [i];
                    }
                }
                if (lastKey != Keys.None) {
                    if (lastKey == Keys.Back) {
                        if (str.Length != 0) {
                            str = str.Substring (0, str.Length - 1);
                        }
                        catched = true;
                    }
                    else if (str.Length < 100) {
                        char c;
                        if (TryConvertKey (lastKey, screen, out c)) {
                            str += c.ToString ();
                        }
                        catched = true;
                    }
                }

                lastMillis = time.TotalGameTime.TotalMilliseconds;
            }
            return catched;
        }

        private static bool TryConvertKey (Keys keyPressed, IScreen screen, out char key)
        {
            bool shift = screen.InputManager.KeyHeldDown (Keys.LeftShift) || screen.InputManager.KeyHeldDown (Keys.RightShift);

            char c = (char)keyPressed.GetHashCode ();
            if (c >= 'A' && c <= 'Z') {
                if (shift) {
                    key = char.ToUpper (c);
                }
                else {
                    key = char.ToLower (c);
                }
                return true;
            }

            switch (keyPressed) {
                //Decimal keys
            case Keys.D0:
                if (shift) {
                    key = ')';
                }
                else {
                    key = '0';
                }
                return true;
            case Keys.D1:
                if (shift) {
                    key = '!';
                }
                else {
                    key = '1';
                }
                return true;
            case Keys.D2:
                if (shift) {
                    key = '@';
                }
                else {
                    key = '2';
                }
                return true;
            case Keys.D3:
                if (shift) {
                    key = '#';
                }
                else {
                    key = '3';
                }
                return true;
            case Keys.D4:
                if (shift) {
                    key = '$';
                }
                else {
                    key = '4';
                }
                return true;
            case Keys.D5:
                if (shift) {
                    key = '%';
                }
                else {
                    key = '5';
                }
                return true;
            case Keys.D6:
                if (shift) {
                    key = '^';
                }
                else {
                    key = '6';
                }
                return true;
            case Keys.D7:
                if (shift) {
                    key = '&';
                }
                else {
                    key = '7';
                }
                return true;
            case Keys.D8:
                if (shift) {
                    key = '*';
                }
                else {
                    key = '8';
                }
                return true;
            case Keys.D9:
                if (shift) {
                    key = '(';
                }
                else {
                    key = '9';
                }
                return true;

                //Decimal numpad keys
            case Keys.NumPad0:
                key = '0';
                return true;
            case Keys.NumPad1:
                key = '1';
                return true;
            case Keys.NumPad2:
                key = '2';
                return true;
            case Keys.NumPad3:
                key = '3';
                return true;
            case Keys.NumPad4:
                key = '4';
                return true;
            case Keys.NumPad5:
                key = '5';
                return true;
            case Keys.NumPad6:
                key = '6';
                return true;
            case Keys.NumPad7:
                key = '7';
                return true;
            case Keys.NumPad8:
                key = '8';
                return true;
            case Keys.NumPad9:
                key = '9';
                return true;

                //Special keys
            case Keys.OemTilde:
                if (shift) {
                    key = '~';
                }
                else {
                    key = '`';
                }
                return true;
            case Keys.OemSemicolon:
                if (shift) {
                    key = ':';
                }
                else {
                    key = ';';
                }
                return true;
            case Keys.OemQuotes:
                if (shift) {
                    key = '"';
                }
                else {
                    key = '\'';
                }
                return true;
            case Keys.OemQuestion:
                if (shift) {
                    key = '?';
                }
                else {
                    key = '/';
                }
                return true;
            case Keys.OemPlus:
                if (shift) {
                    key = '+';
                }
                else {
                    key = '=';
                }
                return true;
            case Keys.OemPipe:
                if (shift) {
                    key = '|';
                }
                else {
                    key = '\\';
                }
                return true;
            case Keys.OemPeriod:
                if (shift) {
                    key = '>';
                }
                else {
                    key = '.';
                }
                return true;
            case Keys.OemOpenBrackets:
                if (shift) {
                    key = '{';
                }
                else {
                    key = '[';
                }
                return true;
            case Keys.OemCloseBrackets:
                if (shift) {
                    key = '}';
                }
                else {
                    key = ']';
                }
                return true;
            case Keys.OemMinus:
                if (shift) {
                    key = '_';
                }
                else {
                    key = '-';
                }
                return true;
            case Keys.OemComma:
                if (shift) {
                    key = '<';
                }
                else {
                    key = ',';
                }
                return true;
            case Keys.Space:
                key = ' ';
                return true;
            }

            key = (char)0;
            return false;
        }

        public static List<Keys> ValidKeys = new List<Keys> (
        new Keys[] {
            Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K,
            Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V,
            Keys.W, Keys.X, Keys.Y, Keys.Z,
            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
            Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5,
            Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
            Keys.OemTilde, Keys.OemSemicolon, Keys.OemQuotes, Keys.OemQuestion, Keys.OemPlus,
            Keys.OemPipe, Keys.OemPeriod, Keys.OemOpenBrackets, Keys.OemCloseBrackets, Keys.OemMinus,
            Keys.OemComma, Keys.Space, Keys.Back
        }
        );
    }
}
