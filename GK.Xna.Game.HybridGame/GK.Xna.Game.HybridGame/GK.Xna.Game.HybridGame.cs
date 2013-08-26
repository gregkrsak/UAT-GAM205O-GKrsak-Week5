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
        protected Dictionary<String, GK.Xna.Graphics.Sprite2D> _spriteSheets;
        protected Dictionary<String, GK.Xna.Models.Model> _models;
        // Entities
        protected Dictionary<String, GK.Xna.Game.GameEntityHybrid> _entities;
        // Entity states
        protected Dictionary<String, EntityStateHybrid> _entityStates;
        // Cameras 
        protected Dictionary<String, GK.Xna.Cameras.Camera> _cameras;
        // Entity managers
        GK.Xna.Graphics.AnimationManager2D _animationManager;
        GK.Xna.Graphics.RenderManagerHybrid _renderManager;
        GK.Xna.Mechanics.CollisionManager2D _collisionManager;
        GK.Xna.Mechanics.MovementManagerHybrid _movementManager;

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

            // Initialize content containers
            this._songs = new Dictionary<byte, Song>();
            this._spriteSheets = new Dictionary<String, GK.Xna.Graphics.Sprite2D>();
            this._models = new Dictionary<String, GK.Xna.Models.Model>();
            // Initialize entity container
            this._entities = new Dictionary<String, GK.Xna.Game.GameEntityHybrid>();
            // Initialize entity state container
            this._entityStates = new Dictionary<string, EntityStateHybrid>();
            // Initialize cameras
            this._cameras = new Dictionary<string, GK.Xna.Cameras.Camera>();
            this._cameras["perspectiveCamera"] = new GK.Xna.Cameras.Camera(new Vector3(Vector2.Zero, -500.0f), Vector3.Zero, Vector3.Up, this);
            this._cameras["orthographicCamera"] = GK.Xna.Cameras.Camera.Orthographic(new Vector2(this.GraphicsDevice.Viewport.Width * 0.5f, this.GraphicsDevice.Viewport.Height * 0.5f));
            this._cameras["active"] = this._cameras["perspectiveCamera"];
            // Initialize entity managers
            this._animationManager = new GK.Xna.Graphics.AnimationManager2D();
            this._renderManager = new Graphics.RenderManagerHybrid(this._cameras["active"]);
            this._collisionManager = new GK.Xna.Mechanics.CollisionManager2D();
            this._movementManager = new GK.Xna.Mechanics.MovementManagerHybrid();

            // Set the initial game state
            GameState = GameStateStartup;

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

            // The player widget
            Vector2 widgetPosition = new Vector2(0.0f, 0.0f);
            Vector2 widgetVelocity = Vector2.Zero;
            Double widgetRotation = MathHelper.ToRadians(0.0f);
            Double widgetScale = 1.0;
            Rectangle widgetCollision_stationary = new Rectangle(0, 0, 64, 32);
            List<Rectangle> widgetCollisions = new List<Rectangle>();
            widgetCollisions.Add(widgetCollision_stationary);
            CreateModel("Widget", widgetPosition);
            CreateEntity("playerWidget", widgetPosition, widgetVelocity, widgetRotation, widgetScale);
            Entity("playerWidget").Uuid = "playerWidget";
            CreateEntityState("playerWidget_default", null, "Widget", widgetCollisions);
            AssignEntityStateToEntity("playerWidget_default", "playerWidget");
            RenderManager.ManageRenderingForEntity(Entity("playerWidget"));
            MovementManager.ManageMovementForEntity(Entity("playerWidget"));
            CollisionManager.ManageCollisionsForEntity(Entity("playerWidget"));
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

            // Startup
            if (GameState == GameStateStartup)
            {
                GameState = GameStatePlayingAlive;
            }
            // Attract
            if (GameState == GameStateAttract)
            {
            }
            // Tutorial
            if (GameState == GameStateTutorial)
            {
            }
            // PlayingAlive
            if (GameState == GameStatePlayingAlive)
            {
                AnimationManager.Animate(gameTime);
                MovementManager.Animate(gameTime);
                CollisionManager.Animate(gameTime);
            }
            // PlayingDead
            if (GameState == GameStatePlayingDead)
            {
            }
            // GameOverWon
            if (GameState == GameStateGameOverWon)
            {
            }
            // GameOverLost
            if (GameState == GameStateGameOverLost)
            {
            }
            // Shutdown
            if (GameState == GameStateShutdown)
            {
            }
            // Off
            if (GameState == GameStateOff)
            {
            }
            // Error
            if (GameState == GameStateError)
            {
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            // Startup
            if (GameState == GameStateStartup)
            {
            }
            // Attract
            if (GameState == GameStateAttract)
            {
            }
            // Tutorial
            if (GameState == GameStateTutorial)
            {
            }
            // PlayingAlive
            if (GameState == GameStatePlayingAlive)
            {
                RenderManager.Render(this._spriteBatch);
            }
            // PlayingDead
            if (GameState == GameStatePlayingDead)
            {
            }
            // GameOverWon
            if (GameState == GameStateGameOverWon)
            {
            }
            // GameOverLost
            if (GameState == GameStateGameOverLost)
            {
            }
            // Shutdown
            if (GameState == GameStateShutdown)
            {
            }
            // Off
            if (GameState == GameStateOff)
            {
            }
            // Error
            if (GameState == GameStateError)
            {
            }
            

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
        public Byte GameStateError { get { return GK.Xna.Game.GameState.Error; } }


        /// <summary>
        /// Manages entity sprite animations.
        /// </summary>
        public GK.Xna.Graphics.AnimationManager2D AnimationManager
        {
            get { return this._animationManager; }
        }


        /// <summary>
        /// Manages entity collisions.
        /// </summary>
        public GK.Xna.Mechanics.CollisionManager2D CollisionManager
        {
            get { return this._collisionManager; }
        }


        /// <summary>
        /// Manages entity movement.
        /// </summary>
        public GK.Xna.Mechanics.MovementManagerHybrid MovementManager
        {
            get { return this._movementManager; }
        }


        /// <summary>
        /// Managers entity rendering.
        /// </summary>
        public GK.Xna.Graphics.RenderManagerHybrid RenderManager
        {
            get { return this._renderManager; }
        }


        /// <summary>
        /// The active camera.
        /// </summary>
        public GK.Xna.Cameras.Camera ActiveCamera
        {
            get { return this._cameras["active"]; }
        }


        /// <summary>
        /// Creates a sprite sheet.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="framesWide"></param>
        /// <param name="framesHigh"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        /// <param name="position"></param>
        /// <param name="frameDelay"></param>
        /// <param name="shouldLoop"></param>
        public void CreateSpriteSheet(String nameOfTextureAsset, Vector2 initialPosition, Int64 framesWide, Int64 framesHigh, Int64 frameWidth, Int64 frameHeight, Double frameDelay, Boolean shouldLoop)
        {
            Texture2D texture = this.Content.Load<Texture2D>(nameOfTextureAsset);
            this._spriteSheets[nameOfTextureAsset] = new GK.Xna.Graphics.Sprite2D(texture, framesWide, framesHigh, frameWidth, frameHeight, initialPosition, frameDelay, shouldLoop);
        }


        /// <summary>
        /// Creates a model asset.
        /// </summary>
        /// <param name="nameOfModelAsset"></param>
        /// <param name="initialPosition"></param>
        public void CreateModel(String nameOfModelAsset, Vector2 initialPosition)
        {
            this._models[nameOfModelAsset] = new GK.Xna.Models.Model(nameOfModelAsset, new Vector3(initialPosition, 0.0f), this);
        }


        /// <summary>
        /// Creates a game entity.
        /// </summary>
        /// <param name="initialPosition"></param>
        /// <param name="initialVelocity"></param>
        /// <param name="initialRotation"></param>
        /// <param name="initialScale"></param>
        /// <param name="z"></param>
        public void CreateEntity(String name, Vector2 initialPosition, Vector2 initialVelocity, Double initialRotation, Double initialScale, Int64 z = 1)
        {
            this._entities[name] = new GK.Xna.Game.GameEntityHybrid(initialPosition, initialVelocity, initialRotation, initialScale, z);
        }


        /// <summary>
        /// Returns the game entity having the provided name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GK.Xna.Game.GameEntityHybrid Entity(String name)
        {
            return this._entities[name];
        }


        /// <summary>
        /// Creates an entity state (name, sprite, model, collision zones)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sprite"></param>
        /// <param name="model"></param>
        /// <param name="collisionZones"></param>
        public void CreateEntityState(String stateName, String spriteSheetName, String modelName, List<Rectangle> collisionZones)
        {
            GK.Xna.Graphics.Sprite2D sprite = null;
            GK.Xna.Models.Model model = null;
            if (null != spriteSheetName)
            {
                sprite = this._spriteSheets[spriteSheetName];
            }
            if (null != modelName)
            {
                model = this._models[modelName];
            }
            this._entityStates[stateName] = new GK.Xna.Game.EntityStateHybrid(stateName, sprite, model, collisionZones);
            this._entityStates[stateName].Activate();
        }


        /// <summary>
        /// Assigns an entity state to a game entity.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="entity"></param>
        public void AssignEntityStateToEntity(String stateName, String entityName)
        {
            try
            {
                GK.Xna.Game.GameEntityHybrid entity = this._entities[entityName];
                GK.Xna.Game.EntityStateHybrid state = this._entityStates[stateName];
                entity.RegisterEntityState((GK.Xna.Game.EntityState2D)state);
            }
            catch (Exception)
            {
            }
        }
    }
}
