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
        // Keyboard manager
        GK.Xna.Input.KeyboardManager _keyboardManager;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        // Terrain
        List<GK.Xna.Game.GameEntityHybrid> _terrainSegments = new List<GameEntityHybrid>();
        Int64 _totalTerrainSegments = 0;
        Int16 degrees = 0;
        Random _rand = new Random();

        // Score
        Int64 _score = 0;
        SpriteFont _font;


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
            this._cameras["active"] = this._cameras["perspectiveCamera"];
            // Initialize entity managers
            this._animationManager = new GK.Xna.Graphics.AnimationManager2D();
            this._renderManager = new Graphics.RenderManagerHybrid(this._cameras["active"]);
            this._collisionManager = new GK.Xna.Mechanics.CollisionManager2D();
            this._movementManager = new GK.Xna.Mechanics.MovementManagerHybrid();
            this._keyboardManager = new GK.Xna.Input.KeyboardManager();

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

            // Background music
            _songs[GameStateStartup] = null;
            _songs[GameStateAttract] = null;
            _songs[GameStateTutorial] = null;
            _songs[GameStatePlayingAlive] = null;
            _songs[GameStatePlayingDead] = null;
            _songs[GameStateGameOverWon] = null;
            _songs[GameStateGameOverLost] = null;
            _songs[GameStateShutdown] = null;
            _songs[GameStateOff] = null;
            _songs[GameStateError] = null;
            AssignSongAssetToGameState("danosongs.com-junk-ship-gold", GameStatePlayingAlive);

            // Set the initial game state
            GameState = GameStateStartup;

            // The player widget
            Vector2 widgetPosition = new Vector2(0.0f, 0.0f);
            Vector2 widgetVelocity = Vector2.Zero;
            Double widgetRotation = MathHelper.ToRadians(0.0f);
            Double widgetScale = 1.0;
            Rectangle widgetCollision_stationary = new Rectangle(0, 0, 32, 48);
            List<Rectangle> widgetCollisions = new List<Rectangle>();
            widgetCollisions.Add(widgetCollision_stationary);
            CreateModel("Widget", widgetPosition);
            CreateEntity("playerWidget", widgetPosition, widgetVelocity, widgetRotation, widgetScale);
            Entity("playerWidget").Z = 1;
            Entity("playerWidget").IgnoreZ = -1;
            Entity("playerWidget").Health = 100;
            CreateEntityState("playerWidget_default", null, "Widget", widgetCollisions);
            AssignEntityStateToEntity("playerWidget_default", "playerWidget");
            // begin player collision handler
            Entity("playerWidget").HandleCollision = (sender, collision) =>
            {
                GK.Xna.Game.GameEntityHybrid collider = (GK.Xna.Game.GameEntityHybrid)collision.Observer1;
                if (collider == Entity("playerWidget"))
                {
                    GK.Xna.Logs.Debug.Log("Handling collision for <<" + collider.Uuid + ">>");
                    if (GameState == GameStatePlayingAlive)
                    {
                        GK.Xna.Logs.Debug.Log("Killing <<" + collider.Uuid + ">>");
                        System.Console.WriteLine("Killing <<" + collider.Uuid + ">>");
                        collider.Health = 0;
                        GameState = GameStatePlayingDead;
                    }
                }
            };
            // end player collision handler
            RenderManager.ManageRenderingForEntity(Entity("playerWidget"));
            MovementManager.ManageMovementForEntity(Entity("playerWidget"));
            CollisionManager.ManageCollisionsForEntity(Entity("playerWidget"));

            // Score
            this._font = this.Content.Load<SpriteFont>("GenericFont");
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
            KeyboardManager.Animate(gameTime);

            // Allows the game to exit
            if (KeyboardManager.KeyJustReleased(Keys.Escape))
            {
                GameState = GameStateShutdown;
            }

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
                if (Entity("playerWidget").Health > 0)
                {
                    // Drift left
                    if (KeyboardManager.KeyJustPressed(Keys.A))
                    {
                        Vector2 v = new Vector2(0.5f, 0.0f);
                        Entity("playerWidget").Velocity = Vector2.Add(Entity("playerWidget").Velocity, v);
                    }
                    // Drift right
                    if (KeyboardManager.KeyJustPressed(Keys.D))
                    {
                        Vector2 v = new Vector2(-0.5f, 0.0f);
                        Entity("playerWidget").Velocity = Vector2.Add(Entity("playerWidget").Velocity, v);
                    }
                    // Space
                    if (KeyboardManager.KeyJustPressed(Keys.Space))
                    {
                        GameState = GameStateError;
                    }
                    GenerateTerrain();
                    AnimationManager.Animate(gameTime);
                    MovementManager.Animate(gameTime);
                    CollisionManager.Animate(gameTime);
                    this._score++;
                    RotateCamera();
                }
                else
                {
                    GameState = GameStatePlayingDead;
                }  
            }
            // PlayingDead
            if (GameState == GameStatePlayingDead)
            {
                GenerateTerrain();
                AnimationManager.Animate(gameTime);
                MovementManager.Animate(gameTime);
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
                GameState = GameStateOff;
            }
            // Off
            if (GameState == GameStateOff)
            {
                this.Exit();
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
                DisplayMessage(-Entity("playerWidget").Position.X + 350, 0, String.Format("Score: {0}", this._score));
            }
            // PlayingDead
            if (GameState == GameStatePlayingDead)
            {
                RenderManager.Render(this._spriteBatch);
                DisplayMessage(-Entity("playerWidget").Position.X + 350, 0, String.Format("Score: {0}", this._score));
                DisplayMessage(-Entity("playerWidget").Position.X + 350, 20,"  SORRY!");
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
                GameState = GameStatePlayingAlive;
            }
            

            base.Draw(gameTime);
        }


        /// <summary>
        /// Displays a sprite font message.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="message"></param>
        private void DisplayMessage(Double x, Double y, String message)
        {
            this._spriteBatch.Begin();
            this._spriteBatch.DrawString(this._font, message, new Vector2((float)x, (float)y), Color.White);
            this._spriteBatch.End();
        }


        /// <summary>
        /// Rotates the active camera around the Z axis.
        /// </summary>
        private void RotateCamera()
        {
            this._cameras["active"].ViewMatrix *= Matrix.CreateRotationZ(MathHelper.ToRadians(-5f) * (float)0.25);
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
            set
            { 
                GK.Xna.Game.GameState.State = value;
                if (_songs[value] != null)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(_songs[GameState]);
                }
            }
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
        /// Manages keyboard input.
        /// </summary>
        public GK.Xna.Input.KeyboardManager KeyboardManager
        {
            get { return this._keyboardManager; }
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
            this._entities[name].Uuid = name;
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


        /// <summary>
        /// Generates a universally-unique identifier string.
        /// </summary>
        public String NewUuid
        {
            get
            {
                Guid uuid = System.Guid.NewGuid();
                return uuid.ToString();
            }
        }


        /// <summary>
        /// Generates the terrain.
        /// </summary>
        private void GenerateTerrain()
        {
            Double leftX;
            Double rightX;

            // -- GENERATE NEW SEGMENTS

            if ((_totalTerrainSegments < 150) && (_rand.Next(3) == 1))
            {
                if ((degrees += 1) > 359) { degrees -= 360; }
                Double sineWaveA = 200 * Math.Sin(MathHelper.ToRadians(degrees));
                Double sineWaveB = 200 * Math.Sin(MathHelper.ToRadians(degrees));
                leftX = -150 + sineWaveA;
                rightX = 150 + sineWaveB;

                Vector2 leftSegmentPos = new Vector2((float)leftX, 380.0f);
                Vector2 rightSegmentPos = new Vector2((float)rightX, 380.0f);

                Vector2 segmentVelocity = new Vector2(0.0f, -2.0f);

                Rectangle segmentZone = new Rectangle(0, 0, 32, 48);
                List<Rectangle> zones = new List<Rectangle>();
                zones.Add(segmentZone);

                String leftName = NewUuid;
                String rightName = NewUuid;

                // left segment
                this._models[leftName] = new GK.Xna.Models.Model("Widget", new Vector3(leftSegmentPos, 0.0f), this);
                CreateEntity(leftName, leftSegmentPos, segmentVelocity, 0.0, 1.0);
                Entity(leftName).Z = 0;
                Entity(leftName).IgnoreZ = 0;
                CreateEntityState(leftName, null, leftName, zones);
                AssignEntityStateToEntity(leftName, leftName);
                // right segment
                this._models[rightName] = new GK.Xna.Models.Model("Widget", new Vector3(rightSegmentPos, 0.0f), this);
                CreateEntity(rightName, rightSegmentPos, segmentVelocity, 0.0, 1.0);
                Entity(rightName).Z = 0;
                Entity(rightName).IgnoreZ = 0;
                CreateEntityState(rightName, null, rightName, zones);
                AssignEntityStateToEntity(rightName, rightName);
                // Manage the segments
                MovementManager.ManageMovementForEntity(Entity(leftName));
                MovementManager.ManageMovementForEntity(Entity(rightName));
                RenderManager.ManageRenderingForEntity(Entity(leftName));
                RenderManager.ManageRenderingForEntity(Entity(rightName));
                CollisionManager.ManageCollisionsForEntity(Entity(leftName));
                CollisionManager.ManageCollisionsForEntity(Entity(rightName));

                _terrainSegments.Add(Entity(leftName));
                _terrainSegments.Add(Entity(rightName));
                _totalTerrainSegments++;
            }

            // -- DESTROY OFF SCREEN SEGMENTS

            for (Int64 x = 0; x < _terrainSegments.Count; x++)
            {
                GK.Xna.Game.GameEntityHybrid segment = _terrainSegments.ElementAt((int)x);
                if (segment.Position.Y < -380)
                {
                    RenderManager.UnmanageRenderingForEntity(segment);
                    MovementManager.UnmanageMovementForEntity(segment);
                    CollisionManager.UnmanageCollisionsForEntity(segment);
                    _terrainSegments.Remove(segment);
                    _totalTerrainSegments--;
                }
            }
        }
    }
}
