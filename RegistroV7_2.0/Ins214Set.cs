using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Registro_V6
{
    interface IIns214Set
    {
        bool Add(Persona p);
        bool Remove(Persona p);
        bool Contains(Persona p);
        bool Replace(Persona oldPerson, Persona newPerson);
        Persona[] ToSortedArray();
    }

    class Ins214Set:IIns214Set
    {
        public List<Persona> Items;
        public Ins214Set()
        {
            Items = new List<Persona>();
        }
    

        public bool Add(Persona p)
        {
            try
            {
                Items.Add(p);// Se agrega el objeto a la lista y se devuelve el booleano como true
                return true;
            }
            catch
            {
                return false;// Si occurre un error, devuelve un false
            }
        }

        public bool Remove(Persona p)
        {
            try
            {
                Items.Remove(p);// Si el item se encuentra y se remueve entonces devuele un true, si no esta en la lista se devuelve un false
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool Contains(Persona p)
        {
            return Items.Contains(p);//Si la lista contiene el objeto devuelve un true, de lo contrario devuelve un false
        }

        public bool Replace(Persona oldPerson, Persona newPerson)
        {
            try
            {
            
                if(Remove(oldPerson))// Se remueve la persona vieja, se remueve con exito, entonces se añade la persona nueva
                {
                    
                    Add(newPerson);
                    return true;
                }
                else
                {
                    return false;
                }
            
            }
            catch
            {
                return false;
            }
        }

        public Persona[] ToSortedArray()
        {
            //Se crea una lista vacia para sortear
            List<Persona> temp = new List<Persona>();
            //Se llena con los valores de la lista original
            foreach(Persona persona in Items)
            {
                temp.Add(persona);
            }

            //Se sortea por String Id
            temp.Sort
            (
                delegate(Persona p1, Persona p2)
                {
                    return p1.Id.CompareTo(p2.Id);
                }
            );
            //Se retorna el arreglo Sorteado
            return temp.ToArray();
        }
    }
}