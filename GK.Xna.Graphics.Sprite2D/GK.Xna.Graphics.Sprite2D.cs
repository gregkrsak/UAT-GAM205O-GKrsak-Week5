/// GK.Xna.Graphics.Sprite2D.cs
/// 
/// Sprite library for XNA 4.
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
    public class Sprite2D
    {
        protected String _uuid;

        protected Boolean _isActive;
        protected Boolean _shouldLoop;
        protected Boolean _shouldDeactivateWhenAnimationIsFinished;

        protected Texture2D _texture;
        protected Rectangle _sourceRect;
        protected Vector2 _position;

        protected Double _timeNow;
        protected Double _timeOfPreviousAnimation;
        protected Double _frameDelay;
        protected Double _rotation;
        protected Double _scale;

        protected Int64 _frameCount;
        protected Int64 _frameX;
        protected Int64 _frameY;
        protected Int64 _textureWidth;
        protected Int64 _textureHeight;
        protected Int64 _framesWide;
        protected Int64 _framesHigh;
        protected Int64 _frameWidth;
        protected Int64 _frameHeight;
        protected Int64 _animationLoopStartFrame;
        protected Int64 _z;


        public Sprite2D(Texture2D texture, Int64 framesWide, Int64 framesHigh, Int64 frameWidth, Int64 frameHeight, Vector2 position, Double frameDelay, Boolean shouldLoop)
        {
            Guid uuid = System.Guid.NewGuid();
            this._uuid = uuid.ToString();

            this._texture = texture;
            this._position = position;
            this._frameDelay = frameDelay;
            this._shouldLoop = shouldLoop;
            this._framesWide = framesWide;
            this._framesHigh = framesHigh;
            this._frameWidth = frameWidth;
            this._frameHeight = frameHeight;
            this._frameCount = this._framesWide * _framesHigh;

            this._frameX = 0;
            this._frameY = 0;
            this._animationLoopStartFrame = 0;
            this._timeOfPreviousAnimation = 0.0;

            if (this._shouldLoop)
            {
                this.AnimationOneShot = false;
            }
            else
            {
                this.AnimationOneShot = true;
            }

            this.Rotation = 0.0;
            this.Scale = 1.0;
            this.Z = 1;
            this.Activate();

            GK.Xna.Logs.Debug.Log("Initialized sprite <<" + this.Uuid + ">>");
        }


        public void Animate(GameTime time)
        {
            this._timeNow += time.ElapsedGameTime.TotalMilliseconds;
            if (this.IsActive)
            {                
                this._sourceRect = new Rectangle((int)(this._frameX * this._frameWidth), (int)(this._frameY * this._frameHeight), (int)this._frameWidth, (int)this._frameHeight);

                if (this.WillAnimate)
                {
                    if (this.AnimationCompleted)
                    {
                        if (this.AnimationWillLoop)
                        {
                            this.ResetAnimation();
                        }
                        else
                        {
                            if (this.AnimationOneShot)
                            {
                                this.Deactivate();
                            }
                        }
                    }
                    else
                    {
                        this._frameX++;
                        if (this._frameX > this._framesWide - 1)
                        {
                            this._frameX = 0;
                            this._frameY++;
                            if (this.AnimationCompleted)
                            {
                                if (this.AnimationWillLoop)
                                {
                                    this.ResetAnimation();
                                }
                                else
                                {
                                    if (this.AnimationOneShot)
                                    {
                                        this.Deactivate();
                                    }
                                }
                            }
                        }
                    }
                    this._timeOfPreviousAnimation = this._timeNow;
                }
            }
            
        }


        public Boolean WillAnimate
        {
            get
            {
                if (this._timeNow > (this._timeOfPreviousAnimation + this._frameDelay))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public Boolean AnimationCompleted
        {
            get { return this._frameY > this._framesHigh - 1; }
        }


        public Boolean AnimationWillLoop
        {
            get { return this._shouldLoop; }
        }


        public Boolean AnimationOneShot
        {
            get { return this._shouldDeactivateWhenAnimationIsFinished; }
            set { this._shouldDeactivateWhenAnimationIsFinished = value; }
        }


        public void Render(SpriteBatch batch)
        {
            if (this.IsActive)
            {
                batch.Draw(this._texture, this._position, this._sourceRect, Color.White, (float)this.Rotation, Vector2.Zero, (float)this.Scale, SpriteEffects.None, (float)this.Z);
            }
        }


        public void Activate()
        {
            this._isActive = true;
        }


        public void Deactivate()
        {
            this._isActive = false;
        }


        public Boolean IsActive
        {
            get { return this._isActive; }
        }


        public void ResetAnimation()
        {
            this._frameX = 0;
            this._frameY = 0;
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


        public String Uuid
        {
            get
            {
                return this._uuid;
            }
            set
            {
                GK.Xna.Logs.Debug.Log("Sprite <<" + this.Uuid + ">> renamed to <<" + value + ">>");
                this._uuid = value;
            }
        }
    }
}