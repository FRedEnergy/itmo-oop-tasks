using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


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
            return $"{nameof(A)}: {A}\n, {nameof(B)}: {B}\n, {nameof(C)}: {C}\n";
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

    class SqlTriangleSerializer
    {
        public readonly MySqlConnection Connection;

        public SqlTriangleSerializer(MySqlConnection connection)
        {
            this.Connection = connection;
            new MySqlCommand(@"create table if not exists triangles
            (
                id int auto_increment,
                ax float null,
            ay float null,
            az float null,
            bx float null,
           `by` float null,
            bz float null,
            cx float null,
            cy float null,
            cz float null,
            constraint triangles_pk
            primary key (id)
                );

            ", connection).ExecuteNonQuery();
        }
        public void AddTriangle(Triangle triangle)
        {
            new MySqlCommand($@"INSERT INTO 
             triangles(ax,ay,az,bx,`by`,bz,cx,cy,cz) 
             VALUES({triangle.A.X},{triangle.A.Y},{triangle.A.Z},
            {triangle.B.X},{triangle.B.Y},{triangle.B.Z},
            {triangle.C.X},{triangle.C.Y},{triangle.C.Z})", Connection).ExecuteNonQuery();
        }

        public List<Triangle> ReadAllTriangles()
        {
            var reader = new MySqlCommand(@"SELECT * FROM triangles", 
                Connection).ExecuteReader();
            var results = new List<Triangle>();
            while (reader.Read())
            {
                results.Add(new Triangle(new Point(reader.GetFloat("ax"),
                        reader.GetFloat("ay"),reader.GetFloat("az" )),
                    new Point(reader.GetFloat("bx"),reader.GetFloat("by"),
                        reader.GetFloat("bz" )),
                    new Point(reader.GetFloat("cx"),reader.GetFloat("cy"),
                        reader.GetFloat("cz" ))));
            }

            return results;
        }
    }
  
    
    class Program
    {
        
        static void Main(string[] args)
        {
            var triangle = new Triangle(new Point(0, 0, 0), 
                new Point(0, 1, 0), 
                new Point(1, 0, 0) );

            Console.WriteLine("Writing to xml:");
            var serializer = new XmlCustomSerializer<Triangle>();
            serializer.Write("Triangle.xml", triangle);
            var deserialized = serializer.Read("Triangle.xml");
            Console.WriteLine(deserialized);
            
            Console.WriteLine("Writing to binary:");
            var binarySerializer = new BinarySerializer<Triangle>();
            binarySerializer.Write("Triangle.bin", triangle);
            var deserializedBinary = binarySerializer.Read("Triangle.bin");
            Console.WriteLine(deserializedBinary);

            Console.WriteLine("Writing to database:");
            var connection = new MySqlConnection($@"Server=localhost; database=labs; UID=root; password=918273645;");
            connection.Open();
            var sqlTriangleSerializer = new SqlTriangleSerializer(connection);
            sqlTriangleSerializer.AddTriangle(triangle);
            var triangles = sqlTriangleSerializer.ReadAllTriangles();
            foreach (var loadedFromSql in triangles)
            {
                Console.WriteLine(loadedFromSql);
            }
        }
    }
}