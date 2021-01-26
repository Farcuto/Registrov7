using System;
using System.IO;
using System.Collections.Generic;
using Registro_V6;

namespace Registro_V6
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Environment.Exit(0);
            }

            while(true){
                FileExists(args[0]);

                StreamReader reader = new StreamReader(args[0]);
                string[] read = reader.ReadToEnd().Split(Environment.NewLine);
                reader.Close();

                Ins214Set people = new Ins214Set();
                foreach (var i in read)
                {
                    if(i != "") people.Add(Persona.CreateFromLine(i));
                }
                
                System.Console.WriteLine("\n1- Capturar\n2- Listar\n3- Buscar\n4- Modificar\n5- Eliminar\n6-salir");
                System.Console.Write("Selecione una opciona: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Capturar(args[0], people);
                        break;
                    
                    case "2":
                        Listar(people);
                        break;

                    case "3":
                        Buscar(people);
                        break;
                    
                    case "4":
                        Editar(args[0], people);
                        break;
                    
                    case "5":
                        Eliminar(args[0], people);
                        break;

                    case "6":
                        Environment.Exit(0);
                        break;
                    
                    default:
                        System.Console.WriteLine("Esta opcion no existe!!!");
                        break;
                }

            }
        }

        static void FileExists(string path)
        {
            if(!File.Exists(path))
            {
                var creator = File.Create(path);
                creator.Close();
            }
        }
        static void Capturar(string path, Ins214Set people)
        {
            while(true){
                string ced = ReadCedula("\nCedula: ");
                Console.Write("\nNombre: ");
                string name = Console.ReadLine();
                Console.Write("Apellidos: ");
                string ape = Console.ReadLine();

                if (name == "" && ape == "")
                    break;

                int age = ReadAge("Edad (7 - 120): ");;
                while (age < 7 || age > 120){
                    age = ReadAge("\nEdad (7 - 120): ");
                }

                char gender, state, grade;
                do{
                    gender = ReadChar("\nGénero (M/F): ");
                } while (gender != 'M' && gender != 'F');

                do{
                    state = ReadChar("\nEstado Civil (S/C): ");
                } while (state != 'S' && state != 'C');

                do{
                    grade = ReadChar("\nGrado Académico (I/M/G/P): ");
                } while (grade != 'I' && grade != 'M' && grade != 'G' && grade != 'P');

                decimal ahorros = ReadMoney("\nAhorros: ");
                string password = ReadPassword("\nContraseña: ");

                bool success = password == ReadPassword("\nConfirme contraseña: ");

                int datos = ToBits(age, gender, state, grade);

                Console.WriteLine();
                if (!success) continue;

                Persona nuevo = new Persona(ced, name, ape, datos, ahorros, password);
                if (people.Contains(nuevo)){
                    Console.WriteLine("La cédula ya existe!!");
                } else{
                    while (true){
                        Console.WriteLine("\nGuardar (G); Rehacer (R); Salir (S)");
                        string opt = Console.ReadLine().ToUpper();

                        if (opt == "G"){
                            StreamWriter writer = File.AppendText(path);
                            writer.WriteLine(nuevo.ToWrite());
                            writer.Close();
                            
                            break;
                        } else if (opt == "R") break;
                        else if (opt == "S") Environment.Exit(0);
                        else continue;
                    }
                }

            }
        }
        

        
        static void Listar(Ins214Set people)
        {// Se devuelve la lista sorteada por Id
            foreach (var i in people.ToSortedArray()){
                Console.WriteLine(i);
            }
        }

        static Persona Buscar(Ins214Set people)
        {
            string ced = ReadCedula("\nIntroduzca la cédula a buscar: ");
            Persona persona = new Persona("","","",0,0,"");

            Console.WriteLine();
            foreach (var i in people.ToSortedArray()){
                if (i.Id == ced){
                    persona = i;
                    Console.WriteLine(persona);
                }
            }

            if (persona.Id == "") 
            {
                Console.WriteLine("No se ha podido encontrar la persona!!");
            }
            return persona;
        }

        
        static void Editar(string path, Ins214Set people)
        {
            Persona persona = Buscar(people);

            if (persona.Id == "") return;

            while (true){
                string ced = ReadCedula("\nCedula: ");
                Console.Write("\nNombre: ");
                string name = Console.ReadLine();
                Console.Write("Apellidos: ");
                string ape = Console.ReadLine();

                if (name == "" && ape == "")
                    break;

                int age = ReadAge("Edad (7 - 120): ");;
                while (age < 7 || age > 120){
                    age = ReadAge("\nEdad (7 - 120): ");
                }

                char gender, state, grade;
                do{
                    gender = ReadChar("\nGénero (M/F): ");
                } while (gender != 'M' && gender != 'F');

                do{
                    state = ReadChar("\nEstado Civil (S/C): ");
                } while (state != 'S' && state != 'C');

                do{
                    grade = ReadChar("\nGrado Académico (I/M/G/P): ");
                } while (grade != 'I' && grade != 'M' && grade != 'G' && grade != 'P');

                decimal ahorros = ReadMoney("\nAhorros: ");
                string password = ReadPassword("\nContraseña: ");

                bool success = password == ReadPassword("\nConfirme contraseña: ");

                int datos = ToBits(age, gender, state, grade);

                Console.WriteLine();
                if (!success) continue;

                Persona nuevo = new Persona(ced, name, ape, datos, ahorros, password);
                if (nuevo.Equals(persona)){
                    File.Delete(path);
                    foreach (var i in people.ToSortedArray()){
                        Persona line = i;
                        if (line.Equals(nuevo)) line = nuevo;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(line.ToWrite());
                        writer.Close();
                    }
                    Console.WriteLine();
                    break;
                } else if (people.Contains(nuevo)) Console.WriteLine("\nLa cédula ya existe!!");
                else {
                    File.Delete(path);
                    foreach (var i in people.ToSortedArray()){
                        Persona line = i;
                        if(line.Equals(persona)) line = nuevo;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(line.ToWrite());
                        writer.Close();
                    }
                    Console.WriteLine();
                    break;
                }
            }
        }

        static void Eliminar(string path, Ins214Set people)
        {
            Persona persona = Buscar(people);

            if (persona.Id == "") return;

            while (true){
                Console.WriteLine("¿Desea eliminarlo (Y/N)?");
                string opt = Console.ReadLine().ToUpper();

                if (opt == "Y"){
                    people.Remove(persona);//Se remueve la persona 
                    File.Delete(path);

                    foreach(var i in people.ToSortedArray()){
                        if (i.Equals(persona)) continue;

                        StreamWriter writer = File.AppendText(path);
                        writer.WriteLine(i.ToWrite());
                        writer.Close();
                    }
                    Console.WriteLine();
                    break;
                } else if (opt == "N") break;
                else continue;
            }
        }
        

        #region Read char by char
        static string ReadPassword(string text)
        {
            Console.Write(text);

            while(true){
                string password = "";
                ConsoleKey key;

                do{
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if(key == ConsoleKey.Backspace && password.Length > 0){
                        Console.Write("\b \b");
                        password = password.Remove(password.Length - 1);
                    }
                    else if(!char.IsControl(keyInfo.KeyChar)){
                        Console.Write("*");
                        password += keyInfo.KeyChar;
                    }
                } while(key != ConsoleKey.Enter);

                if (password == "")
                    continue;

                return password;
            }
        }

        static string ReadCedula(string text)
        {
            Console.Write(text);
            while(true){
                string data = "";
                ConsoleKey key;

                do{
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value);

                    if(key == ConsoleKey.Backspace && data.Length > 0){
                        Console.Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    } else if(!char.IsControl(keyInfo.KeyChar) && success){
                        Console.Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }

                } while(key != ConsoleKey.Enter);

                if(data == "")
                    continue;

                return data;
            }
        }

        static int ReadAge(string text)
        {
            Console.Write(text);
            while (true){
                string data = "";
                ConsoleKey key;

                do{
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value);

                    if(key == ConsoleKey.Backspace && data.Length > 0){
                        Console.Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    } else if(!char.IsControl(keyInfo.KeyChar) && success){
                        Console.Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }
                } while(key != ConsoleKey.Enter);

                if(data == "")
                    continue;

                return int.Parse(data);
            }
        }

        static decimal ReadMoney(string text)
        {
            Console.Write(text);
            while (true){
                string data = "";
                ConsoleKey key;
                int c = 0;

                do{
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    int value;
                    bool success = int.TryParse(keyInfo.KeyChar.ToString(), out value) || (keyInfo.KeyChar == '.' && c == 0);

                    if(keyInfo.KeyChar == '.')
                        c++;

                    if(key == ConsoleKey.Backspace && data.Length > 0){
                        Console.Write("\b \b");
                        data = data.Remove(data.Length - 1);
                    } else if(!char.IsControl(keyInfo.KeyChar) && success){
                        Console.Write(keyInfo.KeyChar);
                        data += keyInfo.KeyChar;
                    }
                } while(key != ConsoleKey.Enter);
                if(data == "")
                    continue;
                return Math.Round(decimal.Parse(data), 2);
            }
        }
        #endregion

        
        static int ToBits(int edad, char genero, char estado, char grado)
        {
            int datos = edad << 4;
            if (genero == 'F') datos = datos | 8;
            if (estado == 'C') datos = datos | 4;
            if (grado == 'M') datos = datos | 1;
            else if (grado == 'G') datos = datos | 2;
            else if (grado == 'P') datos = datos | 3;
            return datos;
        }

        static void ReadBits(int datos, out int edad, out char genero, out char estado, out char grado)
        {
            edad = datos >> 4;
            datos = datos & 15;
            if ((datos >> 3) == 1) genero = 'F';
            else genero = 'M';
            datos = datos & 7;
            if ((datos >> 2) == 1) estado = 'C';
            else estado = 'S';
            datos = datos & 3;
            if (datos == 0) grado = 'I';
            else if (datos == 1) grado = 'M';
            else if (datos == 2) grado = 'G';
            else grado = 'P';
        }
        static char ReadChar(string text)
        {
            Console.Write(text);
            string value = "";
            ConsoleKey key;
            do{
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;
                if(key == ConsoleKey.Backspace && value.Length > 0){
                    Console.Write("\b \b");
                    value = value.Remove(value.Length - 1);
                }
                else if(!char.IsControl(keyInfo.KeyChar) && value.Length < 1){
                    Console.Write(keyInfo.KeyChar);
                    value += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return Convert.ToChar(value);
        }
            
    }
}