using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Security.Cryptography.X509Certificates;

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
        Matrix scaleMatrix;         // We will use this to scale our game window to fill the screen when it is drawn

        Vector2 screenOffset;

        Vector2 scale;  // Use when not maintaining aspect ratio
        float scaleAspect;  // Use when maintaining aspect ratio and adding borders (LetterBoxing)

        float viewportWidth, viewportHeight; // Needed when LetterBoxing

        int monitorWidth; 
        int monitorHeight; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
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

            // Scales the window to fit the current monitor
            _graphics.PreferredBackBufferWidth = monitorWidth;
            _graphics.PreferredBackBufferHeight = monitorHeight;
            _graphics.ApplyChanges();


            // Calculate Scale Factor
            float scaleX = (float)monitorWidth / (float)virtualWindow.Width;
            float scaleY = (float)monitorHeight / (float)virtualWindow.Height;
            
            // ** For non letterboxing
            //scale = new Vector2(scaleX, scaleY);
            // Creates transformatin matrix for non LetterBox scaling
            //scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            // ** For Letterboxing
            scaleAspect = Math.Min(scaleX, scaleY);
            // Calculates dimensions of the scaled game window on the monitor
            viewportWidth = virtualWindow.Width * scaleAspect;
            viewportHeight = virtualWindow.Height * scaleAspect;
            //Calculates the offset of our game screen so it is centered within the LetterBox bars
            screenOffset = new Vector2((monitorWidth - viewportWidth) / 2f,(monitorHeight - viewportHeight) / 2f) / scaleAspect; // Divide by scale offset here so when it is applied later by the transformation matrix it is correct
            // This is used of you use letterbox scaling
            scaleMatrix = Matrix.CreateTranslation(new Vector3(screenOffset, 0)) * Matrix.CreateScale(scaleAspect, scaleAspect, 1f);



            shipRect = new Rectangle(0, 0, 20, 20);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            


            // TODO: use this.Content to load your game content here
            backgroundTexture = Content.Load<Texture2D>("Images/sunset_background");
            shipTexture = Content.Load<Texture2D>("Images/UFO");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Updates the mouse each frame
            preKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            prevMouseState = mouseState;
            prevTranslatedMousePos = translatedMousePos;

            mouseState = Mouse.GetState();
            translatedMousePos = mouseState.Position.ToVector2();
            

            // Un-scale cursor position to get its real position in our game
            translatedMousePos /= scaleAspect;
            // Remove Offset applied to senter game window
            translatedMousePos -= screenOffset;

            // ** Un-scale cursor position Non LetterBoxing to get its real position in our game
            // translatedMousePos /= scale;

            // Centers ship around cursor
            shipRect.X = (int)translatedMousePos.X - 10;
            if (shipRect.X < 0)
                shipRect.X = 0;
            if (shipRect.Right > virtualWindow.Width)
                shipRect.X = virtualWindow.Width - shipRect.Width;

            shipRect.Y = (int)translatedMousePos.Y - 10;
            if (shipRect.Y < 0)
                shipRect.Y = 0;
            if (shipRect.Bottom > virtualWindow.Height)
                shipRect.X = virtualWindow.Height - shipRect.Height;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(transformMatrix: scaleMatrix);

            _spriteBatch.Draw(backgroundTexture, virtualWindow, Color.White);
            _spriteBatch.Draw(shipTexture,shipRect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
