using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D backgroundTexture, shipTexture;
        Rectangle shipRect;

        MouseState mouseState, prevMouseState;
        Vector2 translatedMousePos, prevTranslatedMousePos;
        KeyboardState keyboardState, preKeyboardState;

        Rectangle virtualWindow;    // This is the size we will design our game window to be
        Matrix scaleMatrix;

        Vector2 screenOffset;
        Vector2 scale;

        int monitorWidth; 
        int monitorHeight; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // We will design our game based on this size
            virtualWindow = new Rectangle(0, 0, 800, 600);

            // Gets the size of the current monitor
            monitorWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            monitorHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // Scales the game screen to fit the current monitor
            _graphics.PreferredBackBufferWidth = monitorWidth;
            _graphics.PreferredBackBufferHeight = monitorHeight;
            _graphics.ApplyChanges();


            // Calculate Scale Factor
            float scaleX = (float)monitorWidth / (float)virtualWindow.Width;
            float scaleY = (float)monitorHeight / (float)virtualWindow.Height;
            scale = new Vector2(scaleX, scaleY);

            // Creates transformatin matrix
            scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            // For Letterboxing
            // float scale = Math.Min(scaleX, scaleY);


            // Translating mouse coordinates to match sreen
            //float viewportWidth = virtualWindow.Width * scale;
            //float viewportHeight = virtualWindow.Height * scale;
            //screenOffset = new Vector2((monitorWidth - viewportWidth) / 2f,(monitorHeight - viewportHeight) / 2f);
            // This is used of you use letterbox scaling
            //scaleMatrix = Matrix.CreateTranslation(new Vector3(screenOffset, 0)) * Matrix.CreateScale(scale, scale, 1f);




            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

            shipRect = new Rectangle(0, 0, 20, 20);

            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("Images/sunset_background");
            shipTexture = Content.Load<Texture2D>("Images/UFO");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            preKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            translatedMousePos = mouseState.Position.ToVector2();
            
            // screenPos -= screenOffset;
            // Un-scale
            translatedMousePos /= scale;
            shipRect.X = (int)translatedMousePos.X - 10;
            shipRect.Y = (int)translatedMousePos.Y - 10;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: scaleMatrix);
            _spriteBatch.Draw(backgroundTexture, virtualWindow, Color.White);

            _spriteBatch.Draw(shipTexture,shipRect, Color.White);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
