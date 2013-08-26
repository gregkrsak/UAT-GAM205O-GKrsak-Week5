/// GK.Xna.Graphics.RenderManagerHybrid.cs
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
    public class RenderManagerHybrid : GK.Xna.Graphics.RenderManager2D
    {
        GK.Xna.Cameras.Camera _targetCamera;


        public RenderManagerHybrid(GK.Xna.Cameras.Camera targetCamera) : base()
        {
            this._targetCamera = targetCamera;
        }


        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            // draw the model
            foreach (GK.Xna.Game.GameEntityHybrid entity in this._managedEntities)
            {
                foreach (GK.Xna.Game.EntityStateHybrid state in entity.ActiveEntityStates())
                {
                    if (null != state.Model)
                    {
                        state.Model.Draw(this._targetCamera);
                    }
                    else
                    {
                        //GK.Xna.Logs.Debug.Log("No model for entity <<" + entity.Uuid + ">> state <<" + state.Name + ">>");
                    }
                }
            }
        }


        public GK.Xna.Cameras.Camera TargetCamera
        {
            get { return this._targetCamera; }
            set { this._targetCamera = value; }
        }
    }
}
