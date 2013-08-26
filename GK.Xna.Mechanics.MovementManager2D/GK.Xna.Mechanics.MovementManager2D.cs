/// GK.Xna.Mechanics.MovementManager2D.cs
///
/// Entity movement manager library for XNA 4.
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

namespace GK.Xna.Mechanics
{
    public class MovementManager2D
    {
        protected List<GK.Xna.Game.GameEntity2D> _managedEntities;


        public MovementManager2D()
        {
            this._managedEntities = new List<GK.Xna.Game.GameEntity2D>();
        }


        public void ManageMovementForEntity(GK.Xna.Game.GameEntity2D entity)
        {
            this._managedEntities.Add(entity);
            GK.Xna.Logs.Debug.Log("Managing movement for entity <<" + entity.Uuid + ">>");
        }


        public void UnmanageMovementForEntity(GK.Xna.Game.GameEntity2D entity)
        {
            this._managedEntities.Remove(entity);
            GK.Xna.Logs.Debug.Log("No longer managing movement for entity <<" + entity.Uuid + ">>");
        }


        public virtual void Animate(GameTime gameTime)
        {
            foreach (GK.Xna.Game.GameEntity2D entity in this.ManagedEntities)
            {
                // move entity
                entity.Position = Vector2.Add(entity.Position, entity.Velocity);
                // move entity's sprites
                foreach (GK.Xna.Game.EntityState2D state in entity.EntityStates)
                {
                    if (null != state.Sprite)
                    {
                        state.Sprite.Position = Vector2.Add(state.Sprite.Position, entity.Velocity);
                        state.Sprite.Rotation = entity.Rotation;
                        state.Sprite.Scale = entity.Scale;
                        state.Sprite.Z = entity.Z;
                    }
                }
            }
        }


        public List<GK.Xna.Game.GameEntity2D> ManagedEntities
        {
            get
            {
                return this._managedEntities;
            }
        }
    }
}