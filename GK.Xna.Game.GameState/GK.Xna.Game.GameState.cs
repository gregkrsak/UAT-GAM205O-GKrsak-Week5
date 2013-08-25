/// GK.Xna.Game.GameState.cs
/// 
/// Global-Mutable game state library for XNA 4.
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

namespace GK.Xna.Game
{
    public static class GameState
    {
        public const Byte Startup = 1;
        public const Byte Attract = 2;
        public const Byte Tutorial = 3;
        public const Byte PlayingAlive = 4;
        public const Byte PlayingDead = 5;
        public const Byte GameOverWon = 6;
        public const Byte GameOverLost = 7;
        public const Byte Shutdown = 8;
        public const Byte Off = 9;
        public const Byte Error = 10;

        static private Byte _state = GameState.Startup;

        static public Byte State
        {
            get
            {
                return GameState._state;
            }
            set
            {
                Byte result;
                if ((value < GameState.Startup) || (value > GameState.Off))
                {
                    result = GameState.Error;
                }
                else
                {
                    result = value;
                }
                GameState._state = result;
            }
        }
    }
}