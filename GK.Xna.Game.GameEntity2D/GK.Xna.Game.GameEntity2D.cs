/// GK.Xna.Game.GameEntity2D.cs
///
/// A game entity library, featuring event-based collision detection, for XNA 4.
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
    public class GameEntity2D : GK.Xna.Patterns.Observer
    {
        protected String _uuid;

        protected Vector2 _position;
        protected Vector2 _velocity;
        protected Double _rotation;
        protected Double _scale;
        protected Int64 _z;

        protected Int64 _health;

        protected Action<object, GK.Xna.Mechanics.Collision2D> _handleCollision;
        protected Boolean _hasCollided;

        protected List<GK.Xna.Game.EntityState2D> _registeredStates;


        public GameEntity2D(Vector2 position, Vector2 velocity, Double rotation, Double scale, Int64 z = 1)
        {
            Guid uuid = System.Guid.NewGuid();
            this._uuid = uuid.ToString();
            this.Position = position;
            this.Velocity = velocity;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Z = z;
            this._registeredStates = new List<GK.Xna.Game.EntityState2D>();
            GK.Xna.Logs.Debug.Log("Initialized new entity <<" + this.Uuid + ">> at <<" + this.Position.X + "," + this.Position.Y + ">>");
        }


        public void RegisterEntityState(GK.Xna.Game.EntityState2D state)
        {
            if (!this.EntityStateExistsWithName(state.Name))
            {
                this._registeredStates.Add(state);
                GK.Xna.Logs.Debug.Log("Registered new state <<" + state.Name + ">> for entity <<" + this.Uuid + ">>");
            }
            else
            {
                GK.Xna.Logs.Debug.Log("Not registering entity state <<" + state.Name + ">> for entity <<" + this.Uuid + ">> (already exists)");
            }
        }


        public GK.Xna.Game.EntityState2D EntityStateWithName(String name)
        {
            GK.Xna.Game.EntityState2D result = this._registeredStates.Find(state => state.Name == name);
            return result;
        }


        public Boolean EntityStateExistsWithName(String name)
        {
            Boolean result = false;
            if (this.EntityStateWithName(name) != null)
            {
                result = true;
            }
            return result;
        }


        public List<GK.Xna.Game.EntityState2D> ActiveEntityStates()
        {
            List<GK.Xna.Game.EntityState2D> result = this._registeredStates.FindAll(state => state.IsActive);
            return result;
        }


        public List<GK.Xna.Game.EntityState2D> EntityStates
        {
            get
            {
                return this._registeredStates;
            }
        }


        public Boolean HasCollided
        {
            get { return this._hasCollided; }
        }


        public Boolean HasCollidedWith(GK.Xna.Game.GameEntity2D target)
        {
            Boolean result = false;

            if (this.Z != target.Z) { return result; }

            List<Rectangle> sourceCollisionZones = new List<Rectangle>();
            List<Rectangle> targetCollisionZones = new List<Rectangle>();

            foreach (GK.Xna.Game.EntityState2D sourceState in this.ActiveEntityStates())
            {
                foreach (Rectangle collisionZone in sourceState.CollisionZones)
                {
                    Rectangle offsetCollisionZone = new Rectangle(collisionZone.X + (int)this.Position.X, collisionZone.Y + (int)this.Position.Y, collisionZone.Width, collisionZone.Height);
                    sourceCollisionZones.Add(offsetCollisionZone);
                }
            }
            foreach (GK.Xna.Game.EntityState2D targetState in target.ActiveEntityStates())
            {
                foreach (Rectangle collisionZone in targetState.CollisionZones)
                {
                    Rectangle offsetCollisionZone = new Rectangle(collisionZone.X + (int)target.Position.X, collisionZone.Y + (int)target.Position.Y, collisionZone.Width, collisionZone.Height);
                    targetCollisionZones.Add(offsetCollisionZone);
                }
            }

            foreach (Rectangle sourceRect in sourceCollisionZones)
            {
                foreach (Rectangle targetRect in targetCollisionZones)
                {
                    if (sourceRect.Intersects(targetRect))
                    {
                        this._hasCollided = true;
                        result = true;
                    }
                }
            }

            return result;
        }


        public Action<object, GK.Xna.Mechanics.Collision2D> HandleCollision
        {
            get
            {
                return this._handleCollision;
            }
            set
            {
                this._handleCollision = value;
            }
        }


        public override void HandleEvent(object sender, EventArgs args)
        {
            if (null != this.HandleCollision)
            {
                this.HandleCollision(sender, (GK.Xna.Mechanics.Collision2D)args);
            }
            else
            {
                GK.Xna.Logs.Debug.Log("No collision handler for entity <<" + this.Uuid + ">>");
            }
        }


        public String Uuid
        {
            get
            {
                return this._uuid;
            }
            set
            {
                GK.Xna.Logs.Debug.Log("Entity <<" + this.Uuid + ">> renamed to <<" + value + ">>");
                this._uuid = value;
            }
        }


        public Vector2 Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }


        public Vector2 Velocity
        {
            get
            {
                return this._velocity;
            }
            set
            {
                this._velocity = value;
            }
        }


        public Double Rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                this._rotation = value;
            }
        }


        public Double Scale
        {
            get
            {
                return this._scale;
            }
            set
            {
                this._scale = value;
            }
        }


        public Int64 Z
        {
            get
            {
                return this._z;
            }
            set
            {
                this._z = value;
            }
        }


        public Int64 Health
        {
            get
            {
                return this._health;
            }
            set
            {
                this._health = value;
            }
        }
    }
}