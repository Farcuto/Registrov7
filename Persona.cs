


using System;
using System.Collections.Generic;

public class Persona: IComparable<Persona>
{
    private int Hiddendata = 0;
    public string Id { get; set; }
    public string Name { get; }
    public string LastName { get; }
    public string FullName => $"{Name} {LastName}";

    public string Fullname => $"{Name} {LastName}";

    public int Age => Hiddendata >> 4;
    public Gender Gender => (Gender)((Hiddendata & 7) >>2);
    public EstadoCivil Estado => (EstadoCivil)((Hiddendata & 7)>>2);
    public GradoAcademico Grado => (GradoAcademico)(Hiddendata & 3);

    public decimal Ahorros {get;}
    public string Password{get;}
    public Persona(in string ced, in string name, in string lastname, in int datos, in decimal ahorros, in string password)
    {
        Id = ced;
        Name = name;
        LastName = lastname;
        Hiddendata = datos;
        Ahorros = ahorros;
        Password = password;
    }

    public static Persona CreateFromLine(string line)
    {
        string[] data = line.Split(";");
        return new Persona(data[0],data[1],data[2],int.Parse(data[3]),decimal.Parse(data[4]),data[5]);
    }

    public override string ToString()
    {
        return $"Cedula: {Id}; Nombre: {FullName}; Edad: {Age}; Genero: {Gender}; Estado: {Estado}; Grado: {Grado}; Ahorros: {Ahorros}; Contraseña: {Password}";
    }
    public override bool Equals(object obj)
    {
        if (obj is Persona other)
        {
            return obj.Equals(other.Id);
        }
        return false;
    }
    public string ToWrite()
    {
        return $"{Id};{Name};{LastName};{Hiddendata};{Ahorros};{Password}";
    }

    public override int GetHashCode()//new function
    {
        return Id[0].GetHashCode();
    }

    public int CompareTo(Persona other)// new function
    {
        return this.Id.CompareTo(other.Id);
    }


}

public enum Gender 
{
    Masculino = 0,Femenino = 1
}
public enum EstadoCivil
{
    Soltero=0,Casado=1
}
public enum GradoAcademico
{
    Inicial = 0,Medio=1,Grado=2,Postgrado=3
}