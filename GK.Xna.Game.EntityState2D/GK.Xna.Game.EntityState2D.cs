/// GK.Xna.Game.EntityState2D.cs
///
/// Represents a unique state (including spritesheet and collisions) of a GameEntity2D.
/// For XNA 4.
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
    public class EntityState2D
    {
        protected String _name;
        protected GK.Xna.Graphics.Sprite2D _sprite;
        protected Boolean _isActive;

        protected List<Rectangle> _collisionZones;


        public EntityState2D(String name, GK.Xna.Graphics.Sprite2D sprite, List<Rectangle> collisionZones)
        {
            this._name = name;
            this._sprite = sprite;
            this._collisionZones = collisionZones;
            this.Deactivate();
        }


        public void Activate()
        {
            this._isActive = true;
        }


        public void Deactivate()
        {
            this._isActive = false;
        }


        public String Name
        {
            get
            {
                return this._name;
            }
        }


        public GK.Xna.Graphics.Sprite2D Sprite
        {
            get
            {
                return this._sprite;
            }
        }


        public Boolean IsActive
        {
            get
            {
                return this._isActive;
            }
        }


        public List<Rectangle> CollisionZones
        {
            get
            {
                return this._collisionZones;
            }
        }
    }
}