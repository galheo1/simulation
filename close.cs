using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace simulation;

public class Close : Update
{
    public Texture2D ExitTexture;
    public Vector2 Position;
    public Vector2 Scale;

    public void Initialize(int windowWidth, int windowHeight)
    {
        float x = 100f / ExitTexture.Width;
        float y = x;
        Scale = new Vector2(x, y);
        Position = new Vector2(windowWidth - Scale.X * ExitTexture.Width, 0);
        //Console.Write("("+x.ToString()+" "+y.ToString()+") ("+Position.X.ToString()+" "+Position.Y.ToString()+") ("+windowWidth+" "+Scale.X+" "+ExitTexture.Width.ToString()+" )");
    }

    public void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        if (mouse.LeftButton == ButtonState.Pressed &&
            mouse.Position.X > Position.X && mouse.Position.X < Position.X + Scale.X * ExitTexture.Width &&
            mouse.Position.Y > Position.Y && mouse.Position.Y < Position.Y + Scale.Y * ExitTexture.Height)
        {
            Core.Instance?.Exit();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            ExitTexture,
            Position,
            null,
            Color.White,
            0f,
            Scale * 0.5f,
            Scale,
            SpriteEffects.None,
            0f);
    }
}