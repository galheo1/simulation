using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content; 
using MonoGameLibrary;
using System.Linq.Expressions;
using System;
using System.Timers;
using Microsoft.VisualBasic;
namespace simulation;

public class Menu
{
    private Butons butonMenu; 

    public SpriteFont fontChiffre;
    public SpriteFont font;

    private Butons butonLapPlus; 
    private Butons butonLapMoins; 
    private Texture2D lapSet;
    private Vector2 lapSetPosition;
    private Vector2 lapSetScale;
    public int setLap;


    private Butons butonSponeFeedPlus; 
    private Butons butonSponeFeedMoins; 
    private Texture2D sponeFeedSet;
    private Vector2 sponeFeedPosition;
    private Vector2 sponeFeedScale;
    private SpriteFont sponeFeedFont;
    public int setSponeFeed;





    public void loadContent(ContentManager Content, GameWindow Window)
    {
        butonMenu = new Butons
        {
                butonTexture = Content.Load<Texture2D>("assets/rect"),
                font =  Content.Load<SpriteFont>("font/fontss"),
                Position = new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2-100),
                Scale = new Vector2(250, 100),
                text = "start",
                color = Color.Orange
        };
        butonMenu.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);



        (butonLapPlus, butonLapMoins) = PlusMoinsButton(Content, Window, 0, 0);
        butonLapPlus.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);
        butonLapMoins.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);

        lapSet = Content.Load<Texture2D>("assets/rect");
        lapSetPosition = new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2);
        lapSetScale = new Vector2(150, 50);


        fontChiffre = Content.Load<SpriteFont>("font/fontss");
        font = Content.Load<SpriteFont>("font/Weghorst");


        (butonSponeFeedPlus, butonSponeFeedMoins) = PlusMoinsButton(Content, Window, 0, 125);
        butonSponeFeedPlus.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);
        butonSponeFeedMoins.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);

        sponeFeedSet = Content.Load<Texture2D>("assets/rect");
        sponeFeedPosition = new Vector2(Window.ClientBounds.Width/2, Window.ClientBounds.Height/2+125);
        sponeFeedScale = new Vector2(150, 50);
        sponeFeedFont = Content.Load<SpriteFont>("font/fontss");


        
    }
    private (Butons, Butons) PlusMoinsButton(ContentManager Content, GameWindow Window ,int x, int y)
    {   Butons P = new Butons
        {
                butonTexture = Content.Load<Texture2D>("assets/rect"),
                font =  Content.Load<SpriteFont>("font/fontss"),
                Position = new Vector2(Window.ClientBounds.Width/2-125+x, Window.ClientBounds.Height/2+y),
                Scale = new Vector2(50, 50),
                text = "+",
                color = Color.Orange
        };
        Butons M = new Butons
        {
                butonTexture = Content.Load<Texture2D>("assets/rect"),
                font =  Content.Load<SpriteFont>("font/fontss"),
                Position = new Vector2(Window.ClientBounds.Width/2+125+x, Window.ClientBounds.Height/2+y),
                Scale = new Vector2(50, 50),
                text = "-",
                color = Color.Orange
        };

        return (P, M);
    }
    

    public bool Update(GameTime gameTime)
    {
        SetLapButtons(gameTime);
        SetSponeFeedButtons(gameTime);



        if (butonMenu.onPressed(gameTime))
        {
            return true;
        }
        return false;
    }

    private void SetSponeFeedButtons(GameTime gameTime)
    {
        
        bool pressedP = butonSponeFeedPlus.onPressed(gameTime);
        bool pressedM = butonSponeFeedMoins.onPressed(gameTime);
        if(pressedP)
        {
            setSponeFeed++;
        }
        else if(pressedM && setSponeFeed > 1)
        {
            setSponeFeed--;
        }
    }

    private void SetLapButtons(GameTime gameTime)
    {
        bool pressedP = butonLapPlus.onPressed(gameTime);
        bool pressedM = butonLapMoins.onPressed(gameTime);
        if(pressedP)
        {
            setLap++;
        }
        else if(pressedM && setLap > 1)
        {
            setLap--;
        }
    }

    public void DrawModifs(SpriteBatch spriteBatch, Vector2 pos,Vector2 scale, Texture2D texture, string variable, string text, Vector2 posText)
    {
        spriteBatch.Draw(
            texture,
            new Vector2(pos.X-scale.X * texture.Width/2, pos.Y-scale.Y * texture.Height/2),
            null,
            Color.Orange,
            0f,
            new Vector2(0f, 0f),
            scale,
            SpriteEffects.None,
            0f);
        spriteBatch.DrawString(fontChiffre, variable,
        new Vector2(pos.X- fontChiffre.MeasureString(variable).X/2,pos.Y-fontChiffre.MeasureString(variable).Y/2),
        Color.Black);
        spriteBatch.DrawString(font, text,
        new Vector2(posText.X-10- font.MeasureString(text).X,posText.Y),
        Color.White);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        butonMenu.Draw(spriteBatch);
        butonLapPlus.Draw(spriteBatch);
        butonLapMoins.Draw(spriteBatch);
        DrawModifs(spriteBatch, lapSetPosition, lapSetScale, lapSet, setLap.ToString(), "laps", butonLapPlus.pos());
        
        butonSponeFeedPlus.Draw(spriteBatch);
        butonSponeFeedMoins.Draw(spriteBatch);
        DrawModifs(spriteBatch, sponeFeedPosition, sponeFeedScale, sponeFeedSet, setSponeFeed.ToString(), "feed taux",
            butonSponeFeedPlus.pos());
        //lapSetPosition.X- lapFont.MeasureString("setLap.ToString()").X/2,lapSetPosition.Y-lapFont.MeasureString("setLap.ToString()").Y/2
        //Position.X- font.MeasureString(text).X/2,Position.Y-font.MeasureString(text).Y/2
    }
}