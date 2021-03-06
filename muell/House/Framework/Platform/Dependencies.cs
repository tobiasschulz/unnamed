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

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Windows.Forms;

using Knot3.Framework.Storage;

namespace Knot3.Framework.Platform
{
    [ExcludeFromCodeCoverageAttribute]
    public static class Dependencies
    {

        public static void CatchExtractExceptions (Action action)
        {
            try {
                action ();
            } catch (Exception ex) {
                Log.Error (ex);
            }
        }

        private static string MessageBoxTitle = "Dependency missing";

        private static string DownloadErrorMessage (string package)
        {
            return package + " could not be downloaded. Please contact the developers.";
        }

        public static void CatchDllExceptions (Action action)
        {
            Application.EnableVisualStyles ();

            if (SystemInfo.IsRunningOnWindows () && SystemInfo.IsRunningOnMonogame ()) {
                /*
                if (!Dependencies.DownloadSDL2 ()) {
                    Log.ShowMessageBox (DownloadErrorMessage ("SDL2"), MessageBoxTitle);
                    return;
                }
                if (!Dependencies.DownloadSDL2_image ()) {
                    Log.ShowMessageBox (DownloadErrorMessage ("SDL2_image"), MessageBoxTitle);
                    return;
                }
                if (!Dependencies.DownloadOpenALSoft ()) {
                    Log.ShowMessageBox (DownloadErrorMessage ("OpenAL Soft"), MessageBoxTitle);
                    return;
                }
                */
            }

            try {
                action ();
            } catch (DllNotFoundException ex) {
                Log.Message ();
                Log.Error (ex);
                Log.Message ();
                string dllMessage = ex.ToString ().Split ('(') [0].Split ('\n') [0];
                Log.ShowMessageBox (dllMessage, MessageBoxTitle);
            }
        }
    }
}
