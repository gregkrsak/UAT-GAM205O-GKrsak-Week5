/// GK.Xna.Input.KeyboardManager.cs
///
/// Keyboard input library for XNA 4.
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

namespace GK.Xna.Input
{
    public class KeyboardManager
    {
        protected KeyboardState _previousKeyboardState;
        protected KeyboardState _currentKeyboardState;


        public KeyboardManager()
        {
            this._previousKeyboardState = new KeyboardState();
            this._currentKeyboardState = new KeyboardState();
        }


        public void Animate(GameTime gameTime)
        {
            this._previousKeyboardState = this.CurrentKeyboardState;
            this._currentKeyboardState = Keyboard.GetState();
        }


        public KeyboardState PreviousKeyboardState
        {
            get
            {
                return this._previousKeyboardState;
            }
        }


        public KeyboardState CurrentKeyboardState
        {
            get
            {
                return this._currentKeyboardState;
            }
        }


        public Boolean KeyJustPressed(Keys key)
        {
            Boolean result = false;
            if (this.CurrentKeyboardState.IsKeyDown(key) && this.PreviousKeyboardState.IsKeyUp(key))
            {
                result = true;
            }
            return result;
        }


        public Boolean KeyJustReleased(Keys key)
        {
            Boolean result = false;
            if (this.CurrentKeyboardState.IsKeyUp(key) && this.PreviousKeyboardState.IsKeyDown(key))
            {
                result = true;
            }
            return result;
        }
    }
}