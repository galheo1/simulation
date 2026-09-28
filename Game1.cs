using System;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace simulation;

public class Game1 : Core
{
    private Close _closeButton;
    private Update _updateManager;
    // The Sprite Font reference to draw with
    private Menu _menu;
    private SpriteFont font1;
    private SpriteFont font2;

    // The position to draw the text
    private Vector2 fontPos;

    private bool menu = true;

    public Game1() : base("Simulation", 1920, 1020, true)
    {
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // TODO: use this.Content to load your game content here
        Console.WriteLine("loaded content");
        _closeButton = new Close
        {
            ExitTexture = Content.Load<Texture2D>("assets/exit")
        };
        _closeButton.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);
        
        loadMenu(1, 1);

        Console.WriteLine("loaded content2");
        // Create a new SpriteBatch, which can be used to draw textures.
        font1 = Content.Load<SpriteFont>("font/fontss");
        Console.WriteLine("loaded content3");
        font2 = Content.Load<SpriteFont>("font/Weghorst");
        Console.WriteLine("loaded content4");
        

        base.LoadContent();
    }
    private void loadMenu(int lap, int setFeed)
    {
        _menu = new Menu
        {
            setLap = lap,
            setSponeFeed = setFeed
        };
        _menu.loadContent(Content, Window);
    }
    private void loadUpdateManager()
    {
        _updateManager = new Update
        {
            lapMax = _menu.setLap,
            reloadFeed = _menu.setSponeFeed,
            countdown = 10
        };
        _updateManager.loadContent(Content, Window);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        _closeButton.Update(gameTime);

        if (menu)
        {
            if (_menu.Update(gameTime))
            {
                menu = false;
                loadUpdateManager();
            }
        }
        else if (_updateManager.UpdateAll(gameTime,Content, Window))
        {
            foreach(Historic i in _updateManager.historic)
            {
                Console.WriteLine(i.day);
                Console.WriteLine(i.nbrSurvivors+" "+i.nbrDied);
            }
            menu = true;
        }
        

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        SpriteBatch.Begin();
        if(menu)
        {
            _menu.Draw(SpriteBatch);
        }else{
            _updateManager.Draw(SpriteBatch, font1);}
        


        _closeButton.Draw(SpriteBatch);
        SpriteBatch.End();


        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
