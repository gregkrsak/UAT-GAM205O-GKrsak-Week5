/// GK.Xna.Mechanics.MovementManagerHybrid.cs
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
    public class MovementManagerHybrid : GK.Xna.Mechanics.MovementManager2D
    {
        public MovementManagerHybrid()
            : base()
        {
        }


        public override void Animate(GameTime gameTime)
        {
            base.Animate(gameTime);
            foreach (GK.Xna.Game.GameEntity2D entity in this.ManagedEntities)
            {
                // Move the entity's models
                foreach (GK.Xna.Game.EntityState2D state in entity.EntityStates)
                {
                    //state.Model.
                }
            }
        }
    }
}
