/// GK.Xna.Mechanics.CollisionManager2D.cs
/// 
/// Event-based collision detection library for XNA 4.
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
    public class CollisionManager2D : GK.Xna.Patterns.Observable
    {
        protected List<GK.Xna.Game.GameEntity2D> _managedEntities;


        public CollisionManager2D()
        {
            this._managedEntities = new List<GK.Xna.Game.GameEntity2D>();
        }


        public void ManageCollisionsForEntity(GK.Xna.Game.GameEntity2D entity)
        {
            this._managedEntities.Add(entity);
            base.OnEvent += entity.HandleEvent;
            GK.Xna.Logs.Debug.Log("Managing collisions for entity <<" + entity.Uuid + ">>");
        }


        public void UnmanageCollisionsForEntity(GK.Xna.Game.GameEntity2D entity)
        {
            this._managedEntities.Remove(entity);
            base.OnEvent -= entity.HandleEvent;
            GK.Xna.Logs.Debug.Log("No longer managing collisions for entity <<" + entity.Uuid + ">>");
        }


        public void Animate(GameTime gameTime)
        {
            foreach (GK.Xna.Game.GameEntity2D sourceEntity in this._managedEntities)
            {
                foreach (GK.Xna.Game.GameEntity2D targetEntity in this._managedEntities)
                {
                    if (sourceEntity != targetEntity)
                    {
                        if (sourceEntity.HasCollidedWith(targetEntity))
                        {
                            GK.Xna.Logs.Debug.Log("Collision between entity <<" + sourceEntity.Uuid + ">> and entity <<" + targetEntity.Uuid + ">>");
                            //System.Console.WriteLine("Collision between entity <<" + sourceEntity.Uuid + ">> and entity <<" + targetEntity.Uuid + ">>");
                            base.EventFire(new Collision2D(sourceEntity, targetEntity));
                        }
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