using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace simulation;

public class Individu : Update
{
    public Texture2D ExitTexture;
    public Texture2D lifeBar;
    public Vector2 Position;
    public Vector2 Scale;
    private Color color = Color.Cyan;

    private Food goEat;
    private float estomact;
    public float estomactMax;
    public float velocity;
    public float energy;
    public int size;
    private float endurance;
    public bool live;
    public int reproduction;
    public List<float> pos = new List<float>(2);

    private Random rnd = new Random();

    public void Initialize(int windowWidth, int windowHeight)
    {
        
        estomact = estomactMax;//diminue à force de se déplacer, augmente en mangeant
        endurance = (velocity*5+size)/25;//plus la vélocitée et la taille sont grandes, plus la barre de faim diminue vite
        //endurance = (velocity*25)/50;
        //Console.WriteLine(endurance);
        live = true;
        pos.Add(0);
        pos.Add(0);
        //Console.WriteLine("   a   "+pos.Count+"   a   ");

        float r = (float)size / ExitTexture.Width;
        Scale = new Vector2(r, r);
            Position = new Vector2(rnd.Next(0, windowWidth - (int)(Scale.X * ExitTexture.Width)),
                                    rnd.Next(0, windowHeight - (int)(Scale.Y * ExitTexture.Height)));
        
        }
    

    public void eat()
    {
        if (estomact + 50 > estomactMax)
        {
            estomact = estomactMax;
        }
        else
        {
            estomact += 50;
        }

    }

    public void Update(GameTime gameTime, List<Food> food)
    {
        if (food.Count > 0)
        {
        if (!(food.Contains(goEat)))
        {
            float distances = Vector2.Distance(Position, food[0].Position);
            Food plusProche = food[0];
            //Console.Write(true);
            foreach (Food f in food)            {
                if(Vector2.Distance(Position, f.Position) < distances)
                {
                    distances = Math.Abs(Vector2.Distance(Position, f.Position));
                    plusProche = f;
                }
            }
            goEat = plusProche;
            //Console.WriteLine(goEat.Position.X.GetType()+" "+(goEat.Position.X + (Scale.X * ExitTexture.Width)/2));
            pos[0] = (float)(goEat.Position.X - (Scale.X * ExitTexture.Width)/2);
            pos[1] = (float)(goEat.Position.Y - (Scale.Y * ExitTexture.Height)/2);

            
        }
        move();
        }
        else
        {
            pos[0] = (float)(Position.X);
            pos[1] = (float)(Position.Y);
        }

    }

    private void move()
    {
        if (estomact > 0)
        {
            //float xtest = cos(new Vector2(1, 0), goEat.Position - Position);
            //float ytest = sin(new Vector2(1, 0), goEat.Position - Position);
            //float xtest = (float)(Math.Cos(Math.Atan2(goEat.Position.Y - Position.Y, goEat.Position.X - Position.X))*velocity);
            //float ytest = (float)(Math.Sin(Math.Atan2(goEat.Position.Y - Position.Y, goEat.Position.X - Position.X))*velocity);
            float xtest = (float)(Math.Cos(Math.Atan2(pos[1] - Position.Y,pos[0]  - Position.X))*velocity);
            float ytest = (float)(Math.Sin(Math.Atan2(pos[1] - Position.Y, pos[0] - Position.X))*velocity);
            //Console.Write("("+xtest+" "+ytest+")  "+Math.Acos(xtest)+" "+Math.Asin(ytest)+"  ");
            //xtest = (float)(Math.Cos(DegreeToRadian(xtest)));
            //ytest = (float)(Math.Sin(DegreeToRadian(ytest)));
            Position.X += xtest;
            Position.Y += ytest;
            //Console.Write("("+Position+" "+ goEat.Position+" "+(goEat.Position - Position)+") ");
            //Console.Write("("+xtest+" "+ytest+") ("+Position.X+" "+Position.Y+") ("+ (int)(Position.X - (int)goEat.Position.X) +" "+(int)(Position.Y - (int)goEat.Position.Y)+") ("+goEat.Position.X+" "+goEat.Position.Y+")");
            estomact -= endurance;
        }
        else
        {
            //Console.Write("dead");
            color = Color.Red;
            live = false;
        }
    }

    
    

    public float DegreeToRadian(float angle)
    {
    return (float)(Math.PI * angle / 180.0);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            ExitTexture,
            Position,
            null,
            color,
            0f,
            Scale * 0.5f,
            Scale,
            SpriteEffects.None,
            0f);
        spriteBatch.Draw(
            lifeBar,
            new Vector2(Position.X, Position.Y - 10),
            null,
            Color.DarkBlue,
            0f,
            new Vector2(0f, 0f),
            new Vector2(ExitTexture.Width*Scale.X, 3f),
            SpriteEffects.None,
            0f);
        spriteBatch.Draw(
            lifeBar,
            new Vector2(Position.X, Position.Y - 10),
            null,
            Color.Red,
            0f,
            new Vector2(0f, 0f),
            new Vector2(ExitTexture.Width*Scale.X*(estomact/(float)estomactMax), 3f),
            SpriteEffects.None,
            0f);
        
    }
}