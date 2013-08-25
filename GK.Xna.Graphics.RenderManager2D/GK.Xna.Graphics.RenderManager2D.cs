/// GK.Xna.Graphics.RenderManager2D.cs
///
/// Entity rendering manager library for XNA 4.
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

namespace GK.Xna.Graphics
{
    public class RenderManager2D
    {
        protected List<GK.Xna.Game.GameEntity2D> _managedEntities;


        public RenderManager2D()
        {
            this._managedEntities = new List<GK.Xna.Game.GameEntity2D>();
        }


        public void ManageRenderingForEntity(GK.Xna.Game.GameEntity2D entity)
        {
            this._managedEntities.Add(entity);
            GK.Xna.Logs.Debug.Log("Managing rendering for entity <<" + entity.Uuid + ">>");
        }


        public void UnmanageRenderingForEntity(GK.Xna.Game.GameEntity2D entity)
        {
            this._managedEntities.Remove(entity);
            GK.Xna.Logs.Debug.Log("No longer managing rendering for entity <<" + entity.Uuid + ">>");
        }


        public virtual void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (GK.Xna.Game.GameEntity2D entity in this._managedEntities)
            {
                foreach (GK.Xna.Game.EntityState2D state in entity.ActiveEntityStates())
                {
                    if (null != state.Sprite)
                    {
                        state.Sprite.Render(spriteBatch);
                    }
                    else
                    {
                        GK.Xna.Logs.Debug.Log("No sprite for entity <<" + entity.Uuid + ">> state <<" + state.Name + ">>");
                    }
                }
            }
            spriteBatch.End();
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