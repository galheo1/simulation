using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace simulation;

public class Historic : Update
{
    public string day;
    public int nbrSurvivors;
    public int nbrDied;
    public List<Individu> sorvivors;

}