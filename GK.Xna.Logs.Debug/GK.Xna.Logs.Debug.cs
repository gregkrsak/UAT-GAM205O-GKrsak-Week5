/// GK.Xna.Logs.Debug.cs
/// 
/// Debug logging library for XNA 4.
/// Uncomment the bulk of Log(...) to view output (it will slow your game down).
///
/// Copyright 2013 Greg M. Krsak (greg.krsak@gmail.com)
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// 
///   http://www.apache.org/licenses/LICENSE-2.0
/// 
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GK.Xna.Logs
{
    public class Debug
    {
        static UInt64 _key;
        static Dictionary<UInt64, Dictionary<Double, String>> _console;


        static public void Log(String text)
        {/*
            if (null == Debug._console)
            {
                Debug._key = 0;
                Debug._console = new Dictionary<UInt64, Dictionary<Double, String>>();
            }
            //#if DEBUG
                Dictionary<Double, String> value = new Dictionary<Double, String>();
                Double timestamp = Debug.UnixFromDateTime(DateTime.Now);
                value.Add(timestamp, text);
                Debug._console.Add(Debug._key++, value);
                System.Console.WriteLine("[" + timestamp + "] " + text);
            //#endif*/
        }


        static protected Double UnixFromDateTime(DateTime time)
        {
            return (Double)(time - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
    }
}