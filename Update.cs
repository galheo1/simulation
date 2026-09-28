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



public class Update
{
    private Butons pauseButton;


    private List<Individu> _individus = new List<Individu>();
    private List<Individu> _dead = new List<Individu>();
    public int reloadFeed;
    private List<Food> _feeds = new List<Food>();
    private Timer sponeFeed = new Timer();
    private Timer simuTime = new Timer();
    private Timer countdownS = new Timer();
    public int countdown;

    private bool addfood;
    private ContentManager content;
    private GameWindow window;
    private Random rnd = new Random();
    public int lap = 1;
    public int lapMax;

    private bool reloadLap = false;


    public List<Historic> historic = new List<Historic>();


    public void loadContent(ContentManager Content, GameWindow Window){
        content = Content;
        window = Window;
        addfood = true;
        pauseButton =new Butons
        {
                butonTexture = Content.Load<Texture2D>("assets/rect"),
                font =  Content.Load<SpriteFont>("font/fontss"),
                Position = new Vector2(Window.ClientBounds.Width-75, 100),
                Scale = new Vector2(150, 100),
                text = "Menu",
                color = Color.Transparent,
                fontColor = Color.White
        };

        for (int i=0; i<200; i++){
            var food = new Food
            {
                ExitTexture = Content.Load<Texture2D>("assets/feed")
            };
            food.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);
            _feeds.Add(food);
        }
        for (int i=0; i<30; i++){
            int size = rnd.Next(15, 80);
            var individu = new Individu
            {
                lifeBar = Content.Load<Texture2D>("assets/rect"),
                ExitTexture = Content.Load<Texture2D>("assets/minion"),
                size = size,
                estomactMax = (float)(rnd.NextDouble()*(size-5)+5)*1000/60,
                velocity = (float)rnd.NextDouble()*3+1,
                energy = rnd.Next(20, 100),
                reproduction = rnd.Next(1, 10)
            };
            individu.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);
            _individus.Add(individu);
        }

        sponeFeed.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        sponeFeed.Interval = 100;
        sponeFeed.Enabled = true;

        simuTime.Elapsed += new ElapsedEventHandler(Lap);
        simuTime.Interval = 10000;
        simuTime.Enabled = true;

        countdownS.Elapsed += new ElapsedEventHandler(Countdown);
        countdownS.Interval = 1000;
        countdownS.Enabled = true;
        
    }
    public void addFood(ContentManager Content, GameWindow Window){
        var food = new Food
            {
                ExitTexture = Content.Load<Texture2D>("assets/feed")
            };
            food.Initialize(Window.ClientBounds.Width, Window.ClientBounds.Height);
            _feeds.Add(food);
    }
    public bool UpdateAll(GameTime gameTime, ContentManager Content, GameWindow Window)
    {
        if (addfood)
        {
            //Console.WriteLine(reloadFeed);
            for(int i =0; i<reloadFeed; i++){
            addFood(Content, Window);
            }
            addfood = false;
        }

        List<Individu> toRemove = new List<Individu>();
        foreach(Individu individu in _individus){
            if (individu.live)
            {
            individu.Update(gameTime, _feeds);
            }
            else
            {
                toRemove.Add(individu);
            }
            }
        foreach(Individu individu in toRemove){
            _individus.Remove(individu);
            _dead.Add(individu);
        }
        //Console.WriteLine("");
        
        removeFeed();
        if (reloadLap)
        {
            Reload();
        }
        if(lap>lapMax)
        {
            return true;
        }
        else if (pauseButton.onPressed(gameTime))
        {
            return true;
        }
        {
            return false;
        }
        return false;
    }
    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        addfood = true;
    }
    private void Countdown(object source, ElapsedEventArgs e)
    {
        countdown--;
    }
    private void Lap(object source, ElapsedEventArgs e)
    {
        reloadLap = true;
    }

    private List<int> setChanges(int min, int max, int value, int range = 5)
    {
        int mini;
        int maxi;
        if (value-range<min)
        {
            mini = min;
            maxi = value+range;
        }else if(value+range>max)
        {
            mini = value-range;
            maxi = max;
        }
        else
        {
            mini = value-range;
            maxi = value+range;
        }

        return new List<int> { mini, maxi };
    }
    private Historic LastDay()
    {
        return new Historic
        {
            day = "day"+lap,
            nbrSurvivors = _individus.Count,
            nbrDied = _dead.Count,
            sorvivors = _individus
        };
    }
    private void Reload()
    {
        historic.Add(LastDay());
        lap ++;
        countdown = 10;



        _feeds.Clear();
        for (int i=0; i<200; i++){
            var food = new Food
            {
                ExitTexture = content.Load<Texture2D>("assets/feed")
            };
            food.Initialize(window.ClientBounds.Width, window.ClientBounds.Height);
            _feeds.Add(food);
        }
        List<Individu> oldGen = new List<Individu>();
        //Console.WriteLine(oldGen.Count+" "+_individus.Count);
        List<Individu> reproducers = new List<Individu>();
        if (_individus.Count > 0)
        {
            reproducers = _individus;
        }
        else
        {
            if (_dead.Count > 3)
            {
                
            reproducers = _dead[(_dead.Count-3).._dead.Count];
            }
            else
            {
                reproducers = _dead;
            }
        }
        //Console.Write("hola  ");
        foreach(Individu indiv in reproducers){
            //Console.Write("1");
            for (int i=0; i<indiv.reproduction; i++){
                //Console.Write("a");
                List<int> sizeChanges = setChanges(15, 80, indiv.size, 7);
                //Console.Write("b");
                List<int> estomachChanges = setChanges(50, 500, (int)indiv.estomactMax, 25);
                //Console.Write("c");
                List<int> energyChanges = setChanges(20, 100, (int)indiv.energy, 10);
                List<int> reproductionChanges = setChanges(0, 10, indiv.reproduction, 2);
                //Console.Write("d");

                double mini;
                double maxi;
                Console.Write(indiv.velocity+" ");
                if (indiv.velocity-0.1<1)
                {
                    mini = 1;
                    maxi = 1.5;
                }else if(indiv.velocity+0.1>=4)
                {
                    mini = 2.5;
                    maxi = 3;
                }
                else
                {
                    mini = indiv.velocity-0.1;;
                    maxi = indiv.velocity+0.1;
                }
                //Console.Write("a"+estomachChanges[0]+" "+estomachChanges[1]+" ");
                int size = rnd.Next(sizeChanges[0], sizeChanges[1]);
                var individu = new Individu
                {
                    lifeBar = content.Load<Texture2D>("assets/rect"),
                    ExitTexture = content.Load<Texture2D>("assets/minion"),
                    size = size,
                    //estomactMax = rnd.Next(estomachChanges[0], estomachChanges[1]),
                    estomactMax = (float)(rnd.NextDouble()*(size-5)+5)*500/60,
                    velocity = (float)(rnd.NextDouble()*maxi+mini),
                    energy = rnd.Next(energyChanges[0], energyChanges[1]),
                    reproduction = rnd.Next(reproductionChanges[0], reproductionChanges[1]),
                };
                //Console.Write("b");
                individu.Initialize(window.ClientBounds.Width, window.ClientBounds.Height);
                //Console.WriteLine("holla");
                oldGen.Add(individu);
                //Console.WriteLine(individu.velocity+" "+mini+" "+maxi);
            }
        }
        
        //Console.WriteLine("holla" + oldGen.Count);
        _individus.Clear();
        _dead.Clear();
        //Console.WriteLine("hollab" + oldGen.Count);
        _individus = oldGen;
        //Console.WriteLine("hollac" + oldGen.Count);
        reloadLap = false;
        
    }
    private void removeFeed()
    {

        List<Food> eated = new List<Food>();
        foreach(Food food in _feeds){
            foreach(Individu individu in _individus){
                Vector2 posIndividu = new Vector2(individu.Position.X + individu.Scale.X * individu.ExitTexture.Width / 2, individu.Position.Y + individu.Scale.Y * individu.ExitTexture.Height / 2);
                Vector2 posFood = new Vector2(food.Position.X + food.Scale.X * food.ExitTexture.Width / 2, food.Position.Y + food.Scale.Y * food.ExitTexture.Height / 2);
                if (Vector2.Distance(posIndividu, posFood) < individu.Scale.X * individu.ExitTexture.Width / 2 + food.Scale.X * food.ExitTexture.Width / 2)
                {
                    //Console.WriteLine("touched");
                    eated.Add(food);
                    individu.eat();
                }
                }
            }
        foreach(Food food in eated){
            _feeds.Remove(food);
        }
        
    }
    public void Draw(SpriteBatch spriteBatch, SpriteFont font)
    {
        foreach(Individu individu in _dead){
            individu.Draw(spriteBatch);
            }
        foreach(Individu individu in _individus){
            individu.Draw(spriteBatch);
            }
        foreach(Food food in _feeds){
            food.Draw(spriteBatch);
            }
        spriteBatch.DrawString(font, lap.ToString()+"/"+lapMax.ToString(), new Vector2(50, 50), Color.White);
        spriteBatch.DrawString(font, countdown.ToString()+"s", new Vector2(50, 150), Color.White);
        pauseButton.Draw(spriteBatch);
        
    }
}