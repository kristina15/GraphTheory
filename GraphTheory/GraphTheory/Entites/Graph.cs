using GraphTheory.Entites.HelperEntites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphTheory.Entites
{
    public class Graph
    {
        public IDictionary<Vertex, Dictionary<Vertex, int>> _vertexWeight;
        public bool Oriented { get; set; }
        public bool Weiting { get; set; }

        public Graph()
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            Oriented = true;
            Weiting = false;
        }

        /// <summary>
        /// Конструктор, считывающий с файла
        /// </summary>
        /// <param name="path"></param>
        public Graph(string path) : this()
        {
            using (StreamReader file = new StreamReader(path))
            {
                Oriented = file.ReadLine() == "1";
                Weiting = file.ReadLine() == "1";
                string[] vertex;
                string[] StrFromFile = file.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var s in StrFromFile)
                {
                    vertex = s.Split(' ');
                    AddVertex(new Vertex(vertex[0]));
                }

                foreach (var s in StrFromFile)
                {
                    vertex = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var from = new Vertex(vertex[0]);
                    for (int k = 1; k < vertex.Length; k += 2)
                    {
                        var to = new Vertex(vertex[k]);
                        int weight = int.Parse(vertex[k + 1]);
                        AddEdge(from, to, weight);
                    }
                }
            }
        }

        public Graph(bool oriented)
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            Oriented = oriented;
            Weiting = false;
        }

        /// <summary>
        /// Конструктор - копия
        /// </summary>
        /// <param name="G"></param>
        public Graph(Graph G)
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>(G._vertexWeight);
            Oriented = G.Oriented;
            Weiting = G.Weiting;
        }

        /// <summary>
        /// Добавление вершины
        /// </summary>
        /// <param name="value"></param>
        public void AddVertex(Vertex value)
        {
            if (_vertexWeight.ContainsKey(value))
            {
                Console.WriteLine("Vertex already exists");
                return;
            }

            _vertexWeight.Add(value, new Dictionary<Vertex, int>());

        }

        /// <summary>
        /// Добавление ребра
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oriented"></param>
        public void AddEdge(Vertex from, Vertex to, int weight = 1, bool oriented = false)
        {
            if (!ValidateFromAndToVertex(from, to))
            {
                return;
            }

            if (_vertexWeight[from].ContainsKey(to))
            {
                Console.WriteLine("Such edge already exists");
                return;
            }

            _vertexWeight[from].Add(to, weight);

            if (!Oriented && !oriented)
            {
                AddEdge(to, from, weight, true);
            }
        }

        public void GetGraphWithoutVertexWithSameDegree()
        {
            foreach (var item in _vertexWeight)
            {
                var sameDegree = item.Value.Where(v => _vertexWeight[v.Key].ContainsKey(item.Key)).Where(v=> _vertexWeight[v.Key].Values.Count == _vertexWeight[item.Key].Values.Count);

                if (sameDegree.Any())
                {
                    DeleteEdge(item.Key, sameDegree.FirstOrDefault().Key);
                    return;
                }
            }
        }

        /// <summary>
        /// Удаление ребра
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oriented"></param>
        public void DeleteEdge(Vertex from, Vertex to, bool oriented = false)
        {
            if (!ValidateFromAndToVertex(from, to))
            {
                return;
            }

            if (!_vertexWeight[from].ContainsKey(to))
            {
                Console.WriteLine($"Invalid way {from} -> {to}");
                return;
            }

            _vertexWeight[from].Remove(to);

            if (!Oriented && !oriented)
            {
                DeleteEdge(to, from, true);
            }
        }

        /// <summary>
        /// Удаление вершины
        /// </summary>
        /// <param name="value"></param>
        public void DeleteVertex(Vertex value)
        {
            if (!_vertexWeight.ContainsKey(value))
            {
                Console.WriteLine("Invalid vertex");
                return;
            }

            _vertexWeight.Remove(value);

            foreach (var v in _vertexWeight)
            {
                v.Value.Remove(value);
            }
        }

        /// <summary>
        /// Сохранение в файл
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(Oriented ? "1" : "0");
                sw.WriteLine(Weiting ? "1" : "0");

                foreach (var key in _vertexWeight)
                {
                    StringBuilder builder = new StringBuilder($"{key.Key.Id} ");
                    foreach (var v in key.Value)
                    {
                        builder.Append($"{v.Key} {v.Value} ");
                    }

                    sw.WriteLine(builder);
                }
            }
        }

        public void GetHengingVertex_TaskLa6()
        {
            var hangingVertex = _vertexWeight.Where(v => v.Value.Count == 1);
            foreach (var item in hangingVertex)
            {
                Console.Write($"{item.Key}, ");
            }

            Console.WriteLine();
        }

        public void GetInAndOutVertex(Vertex v)
        {
            if (!_vertexWeight.ContainsKey(v))
            {
                Console.WriteLine($"Invalid vertex {v}");
                return;
            }

            foreach (var item in _vertexWeight[v])
            {
                if (_vertexWeight[item.Key].ContainsKey(v))
                {
                    Console.Write($"{item.Key}, ");
                }
            }

            Console.WriteLine();
        }


        private bool ValidateFromAndToVertex(Vertex from, Vertex to)
        {
            if (!_vertexWeight.ContainsKey(from))
            {
                Console.WriteLine($"Invalid vertex {from}");
                return false;
            }

            if (!_vertexWeight.ContainsKey(to))
            {
                Console.WriteLine($"Invalid vertex {to}");
                return false;
            }

            return true;
        }
    }
}
