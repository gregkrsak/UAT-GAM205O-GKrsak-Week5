/// GK.Xna.Game.HybridGame.cs
/// 
/// Create 2D-looking 3D games in XNA 4.
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
    public class HybridGame : Microsoft.Xna.Framework.Game
    {
        // Content
        protected Dictionary<Byte, Song> _songs;
        // Entities
        protected List<GK.Xna.Game.GameEntityHybrid> _entities;
        // Entity states

        // Entity managers
        GK.Xna.Graphics.AnimationManager2D _animationManager;
        GK.Xna.Mechanics.CollisionManager2D _collisionManager;


        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;


        public HybridGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this._spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        /// <summary>
        /// Sets the music for the provided game state.
        /// </summary>
        /// <param name="songAssetName"></param>
        /// <param name="gameState"></param>
        public void AssignSongAssetToGameState(String songAssetName, Byte gameState)
        {
            this._songs[gameState] = null;
            this._songs[gameState] = this.Content.Load<Song>(songAssetName);
        }

        /*
        /// <summary>
        /// Returns the current game state.
        /// </summary>
        public Byte GameState
        {
            get { return GK.Xna.Game.GameState.State; }
            set { GK.Xna.Game.GameState.State = value; }
        }


        /// <summary>
        /// These are the available game states.
        /// </summary>
        public Byte GameStateStartup { get { return GK.Xna.Game.GameState.Startup; } }
        public Byte GameStateAttract { get { return GK.Xna.Game.GameState.Attract; } }
        public Byte GameStateTutorial { get { return GK.Xna.Game.GameState.Tutorial; } }
        public Byte GameStatePlayingAlive { get { return GK.Xna.Game.GameState.PlayingAlive; } }
        public Byte GameStatePlayingDead { get { return GK.Xna.Game.GameState.PlayingDead; } }
        public Byte GameStateGameOverWon { get { return GK.Xna.Game.GameState.GameOverWon; } }
        public Byte GameStateGameOverLost { get { return GK.Xna.Game.GameState.GameOverLost; } }
        public Byte GameStateShutdown { get { return GK.Xna.Game.GameState.Shutdown; } }
        public Byte GameStateOff { get { return GK.Xna.Game.GameState.Off; } }
        public Byte GameStateError { get { return GK.Xna.Game.GameState.Error; } }*/
    }
}
