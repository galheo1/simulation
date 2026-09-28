using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace simulation;

public class Food : Update
{
    public Texture2D ExitTexture;
    public Vector2 Position;
    public Vector2 Scale;

    public Random rdn = new Random();

    public void Initialize(int windowWidth, int windowHeight)
    {
        float r = 5f / ExitTexture.Width;
        Scale = new Vector2(r, r);
        Position = new Vector2(rdn.Next(0, windowWidth - (int)(Scale.X * ExitTexture.Width)), rdn.Next(0, windowHeight - (int)(Scale.Y * ExitTexture.Height)));
    }

    public void Update(GameTime gameTime)
    {
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