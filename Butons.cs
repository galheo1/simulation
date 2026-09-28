using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace simulation;

public class Butons : Menu
{
    public Texture2D butonTexture;
    public SpriteFont font;
    public string text;
    public Vector2 Position;
    public Vector2 Scale;
    public Color color;
    public Color fontColor;
    private bool pressed = true;
    public void Initialize(int windowWidth, int windowHeight)
    {
        
    }

    public void Update(GameTime gameTime)
    {
    }
    public bool onPressed(GameTime gameTime)
    {
        
        var mouse = Mouse.GetState();
        //if (
        //    mouse.Position.X > Position.X-Scale.X * butonTexture.Width/2 && mouse.Position.X < Position.X + Scale.X * butonTexture.Width/2 &&
        //    mouse.Position.Y > Position.Y-Scale.Y * butonTexture.Height/2 && mouse.Position.Y < Position.Y + Scale.Y * butonTexture.Height/2)
        //{
        //    Console.WriteLine(true);
        //}
        //else
        //{
        //    Console.WriteLine(false);
        //}
        if (mouse.LeftButton == ButtonState.Pressed &&
            mouse.Position.X > Position.X-Scale.X * butonTexture.Width/2 && mouse.Position.X < Position.X + Scale.X * butonTexture.Width/2 &&
            mouse.Position.Y > Position.Y-Scale.Y * butonTexture.Height/2 && mouse.Position.Y < Position.Y + Scale.Y * butonTexture.Height/2 &&
            pressed)
        {
            pressed = false;
            return true;
        }
        else if (!(mouse.LeftButton == ButtonState.Pressed)&&!pressed)
        {
            pressed = true;
        }
        return false;
    }
    public Vector2 pos()
    {
        return new Vector2(Position.X-Scale.X * butonTexture.Width/2, Position.Y-Scale.Y * butonTexture.Height/2);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            butonTexture,
            pos(),
            null,
            color,
            0f,
            new Vector2(0f, 0f),
            Scale,
            SpriteEffects.None,
            0f);

        spriteBatch.DrawString(font, text,
        new Vector2(Position.X- font.MeasureString(text).X/2,Position.Y-font.MeasureString(text).Y/2),
        fontColor);
    }
}