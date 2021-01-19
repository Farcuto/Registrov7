using System;
using System.Collections.Generic;

public interface IReg
{
    bool Contains(Persona persona);// T = existe. F = No existe
    bool Add(Persona persona);// T = Agregado . F = existe

    bool Remove(Persona persona);// T = Removido. F = No existe

    bool Replace(Persona pOld, Persona pNew);// T = Reemplazado. F = No existe pOld, pNew existe

    Persona[] toSortArray();
}

public class Reg: IReg
{
    public int MaxElement = 2;
    public int Bucket { get; set; }
    public Persona[][] Regi{ get; }

    public Reg(List<Persona> people, int bucket)
    {
        Bucket = bucket;
        Regi = new Persona[Bucket][];

        for(int i = 0; i < Bucket; i = i + 1)
        {
            Regi[i] = new Persona[MaxElement];
            ToArray(people);
        }

    }

    public bool Contains(Persona persona)
    {
        int buck = Math.Abs(persona.GetHashCode()) % Bucket;
        int min = 0,max = Regi[buck].Length - 1;

        while (min <= max)
        {
            int middle = (min + max) / 2;

            if(Regi[buck][middle] != null)
            {
                if(Regi[buck][middle].Equals(persona))
                {
                    return true;
                }
                else if(persona.CompareTo(Regi[buck][middle]) == -1)
                {
                    max = middle - 1;
                }
                else
                {
                    min = middle + 1;
                }


            }
            else
            {
                max = max - 1;
            }
        }
        return false;
    }

    public bool Add(Persona persona)
    {
        if(!Contains(persona))
        {
            if(Space(persona))
            {
                int buck = Math.Abs(persona.GetHashCode()) % Bucket;
                int pas = Detect(persona);

                for(int i = Regi[buck].Length - 1; i > pas; i--)
                {
                    Regi[buck][i] = Regi[buck][i - 1];
                }

                Regi[buck][pas] = persona;

                return Contains(persona);

            }
            else
            {
                int buck = Math.Abs(persona.GetHashCode()) % Bucket;
                Persona[] backup = Regi[buck];
                Exp(persona);

                for(int i = 0; i < backup.Length; i = i + 1)
                {
                    Regi[buck][i] = backup[i];
                }

                int pas = Detect(persona);
                for(int i = Regi[buck].Length - 1; i > pas; i = i + 1)
                {
                    Regi[buck][i] = Regi[buck][i - 1];

                }

                Regi[buck][pas] = persona;

                return Contains(persona);

            }
        }

        return false;
    }

    public bool Remove(Persona persona)
    {
        if(Contains(persona))
        {
            int pas = BinaryS(persona);
            int buck = Math.Abs(persona.GetHashCode()) % Bucket;

            Regi[buck][pas] = null;

            for(int i = pas; i < Regi[buck].Length - 1; i = i + 1)
            {
                if(Regi[buck][i + 1] != null)
                {
                    Regi[buck][i] = Regi[buck][i + 1];
                }
                else if(Regi[buck][i +1] == null)
                {
                    Regi[buck][i] = null;
                }
                if(Regi[buck][i] == Regi[buck][i + 1])
                {
                    Regi[buck][i + 1] = null;
                }
            }

            return (!Contains(persona));
        }

        return false;
    }

    public bool Replace(Persona pOld, Persona pNew)
    {
        if(Contains(pOld))
        {
            if(pOld.Equals(pNew))
            {
                if(Remove(pOld))
                {
                    return Add(pNew);
                }
            }
            else if(!Contains(pNew))
            {
                if(Remove(pOld))
                {
                    return Add(pNew);
                }
            }
        }

        return false;
    }

    public Persona[] toSortArray()
    {
        Persona[] order = new Persona[1];

        foreach(var b in Regi)
        {
            foreach(var p in b)
            {
                if(SpaceSort(order))
                {
                    if(p != null)
                    {
                        for(int i = 0; i < order.Length; i = i +1)
                        {
                            if(order[i] == null)
                            {
                                order[i] = p;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if(p != null)
                    {
                        Persona[] backup = order;
                        int max = Convert.ToInt32(order.Length + order.Length * 0.50);
                        order = new Persona[max];
                    
                        for(int i = 0; i < backup.Length; i = i + 1 )
                        {
                            order[i] = backup[i];
                        }

                        for(int i = 0; i < order.Length; i = i + 1 )
                        {
                            if(order[i] == null)
                            {
                                order[i] = p;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if(order.Length == 1)
        {
            return order;
        }

        Array.Sort(order);
        
        return order;
    }

    private bool SpaceSort(Persona[] arr)
    {
        foreach(var i in arr)
        {
            if(i == null)
            {
                return true;
            }
        }
        return false;
    }
    public int BinaryS(Persona persona)
    {
        int buck = Math.Abs(persona.GetHashCode()) % Bucket;
        int min = 0, max = Regi[buck].Length - 1;

        while (min <= max)
        {
            int middle = (min + max) / 2;

            if(Regi[buck][middle] != null)
            {
                if(Regi[buck][middle].Equals(persona))
                {
                    return middle++;
                }
                else if(persona.CompareTo(Regi[buck][middle]) == -1)
                {
                    max = middle - 1;
                }
                else
                {
                    min = middle + 1;
                }


            }
            else
            {
                max = max - 1;
            }
        }
        return 0;
    }

    private int Detect(Persona persona)
    {
        int buck = Math.Abs(persona.GetHashCode()) % Bucket;
        int min = 0, max = Regi[buck].Length - 1;

        while (min <= max)
        {
            int middle = (min + max) / 2;

            if(Regi[buck][middle] != null)
            {
                if(persona.CompareTo(Regi[buck][max]) == 1)
                {
                    return max;
                }
                else if(persona.CompareTo(Regi[buck][0]) == -1)
                {
                    return 0;
                }
                else if(persona.CompareTo(Regi[buck][middle]) == 1 && persona.CompareTo(Regi[buck][++middle]) == -1)
                {
                    return middle++;
                }
                else if(persona.CompareTo(Regi[buck][middle]) == 1 && persona.CompareTo(Regi[buck][--middle]) == -1)
                {
                    return middle++;
                }
                else if(persona.CompareTo(Regi[buck][middle]) == -1)
                {
                    max = middle - 1;
                }
                else
                {
                    min = middle + 1;
                }

            }
            else
            {
                max = max - 1;
            }
        }

        return 0;

    }
    private bool Space(Persona persona)
    {
        int buck = Math.Abs(persona.GetHashCode()) % Bucket;
        foreach (var i in Regi[buck])
        {
            if(i == null)
            {
                return false;
            }
            
        }
        return false;
    }

    private void Exp(Persona persona)
    {
        int buck = Math.Abs(persona.GetHashCode()) % Bucket;
        int max = Convert.ToInt32(Regi[buck].Length + Regi[buck].Length * 0.50);
        Regi[buck] = new Persona[max];

    }
    private void ToArray(List<Persona> people)
    {
        foreach(var persona in people)
        {
            if(Space(persona))
            {
                if(!Contains(persona))
                {
                    int buck = Math.Abs(persona.GetHashCode()) % Bucket;
                    for(int i = 0; i < Regi[buck].Length; i = i + 1)
                    {
                        if(Regi[buck][i] == null)
                        {
                            Regi[buck][i] = persona;
                            break;
                        }
                    }
                }
            }
            else
            {
                if(!Contains(persona))
                {
                    int buck = Math.Abs(persona.GetHashCode()) % Bucket;
                    Persona[] backup = Regi[buck];
                    Exp(persona);
                    
                    for(int i = 0; i < backup.Length ; i = i + 1)
                    {
                        Regi[buck][i] = backup[i];
                    }

                    for(int i = 0; i < Regi[buck].Length; i = i + 1)
                    {
                        if(Regi[buck][i] == null)
                        {
                            Regi[buck][i] = persona;
                            break;
                        }
                    }
                }
            }
        }
    }
}