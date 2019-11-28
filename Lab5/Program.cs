using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Lab5
{

    [Serializable]
    public class Point
    {
        public float X, Y, Z;

        public Point() { }

        public Point(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}";
        }
    }

    [Serializable]
    public class Triangle
    {
        
        public Point A, B, C;

        public Triangle() { }

        public Triangle(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;
        }

        public override string ToString()
        {
            return $"{nameof(A)}: {A}, {nameof(B)}: {B}, {nameof(C)}: {C}";
        }
    }

    public abstract class CustomSerializer<T>
    {
        public void Write(String file, T value)
        {
            var stream = File.Create(file);
            Serialize(stream, value);
            stream.Close();
        }

        public T Read(String file)
        {
            var stream = File.OpenRead(file);
            var value = Deserialize(stream);
            stream.Close();
            return (T)value;
        }

        public abstract T Deserialize(Stream file);

        public abstract void Serialize(Stream stream, T value);
    }

    public class XmlCustomSerializer<T> : CustomSerializer<T>
    {
        public override T Deserialize(Stream file)
        {
            return (T) new XmlSerializer(typeof(T)).Deserialize(file);
        }

        public override void Serialize(Stream stream, T value)
        {
            new XmlSerializer(typeof(T)).Serialize(stream, value);
        }
    }

    public class BinarySerializer<T> : CustomSerializer<T>
    {
        public override T Deserialize(Stream file)
        {
            return (T) new BinaryFormatter().Deserialize(file);
        }

        public override void Serialize(Stream stream, T value)
        {
            new BinaryFormatter().Serialize(stream, value);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var triangle = new Triangle(new Point(0, 0, 0), 
                new Point(0, 1, 0), 
                new Point(1, 0, 0) );

            var serializer = new XmlCustomSerializer<Triangle>();
            serializer.Write("Triangle.xml", triangle);
            var deserialized = serializer.Read("Triangle.xml");
            
            Console.WriteLine(deserialized);
        }
    }
}