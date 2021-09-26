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
        private IDictionary<Vertex, Dictionary<Vertex, int>> _vertexWeight;
        private Random _randomNumber;
        private bool _oriented;

        public Graph()
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            _randomNumber = new Random();
            _oriented = true;
        }

        /// <summary>
        /// Конструктор, считывающий с файла
        /// </summary>
        /// <param name="path"></param>
        public Graph(string path) : this()
        {
            using (StreamReader file = new StreamReader(path))
            {
                string[] vertex;
                string[] StrFromFile = file.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var s in StrFromFile)
                {
                    vertex = s.Split(' ');
                    AddVertex(new Vertex(int.Parse(vertex[0])));
                }

                foreach (var s in StrFromFile)
                {
                    vertex = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var from = new Vertex(int.Parse(vertex[0]));
                    for (int k = 1; k < vertex.Length; k += 2)
                    {
                        var to = new Vertex(int.Parse(vertex[k]));
                        int weight = int.Parse(vertex[k + 1]);
                        AddEdge(from, to, weight);
                    }
                }
            }
        }

        public Graph(bool oriented)
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            _randomNumber = new Random();
            _oriented = oriented;
        }

        /// <summary>
        /// Вывод на консоль
        /// </summary>
        public void Print()
        {
            Console.Write(" \t");
            foreach (var item in _vertexWeight.Keys)
            {
                Console.Write(item + "\t");
            }

            Console.WriteLine();
            foreach (var item in _vertexWeight)
            {
                Console.Write(item.Key.ToString() + '\t');
                foreach (var item2 in _vertexWeight.Keys)
                {
                    if (item.Value.Any(x => x.Key.Id == item2.Id))
                    {
                        Console.Write(item.Value.First(x => x.Key.Id == item2.Id).Value.ToString() + "\t");
                    }
                    else
                    {
                        Console.Write("0\t");
                    }
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Конструктор - копия
        /// </summary>
        /// <param name="G"></param>
        public Graph(Graph G) : this()
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>(G._vertexWeight);
            _randomNumber = new Random();
        }

        /// <summary>
        /// Консутрктор случайного графа по вероятности
        /// </summary>
        /// <param name="p"></param>
        public Graph(double p) : this()
        {
            int count = _randomNumber.Next(7, 10);
            for (int i = 0; i < count; i++)
            {
                _vertexWeight.Add(new Vertex(i + 1), new Dictionary<Vertex, int>());
            }

            double r;
            foreach (var i in _vertexWeight.Keys)
            {
                foreach (var j in _vertexWeight.Keys)
                {
                    if (i != j)
                    {
                        r = _randomNumber.NextDouble();
                        if (r < p)
                        {
                            AddEdge(i, j, 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавление вершины
        /// </summary>
        /// <param name="value"></param>
        public void AddVertex(Vertex value)
        {
            if (value != null)
            {
                _vertexWeight.Add(value, new Dictionary<Vertex, int>());
            }
            else
            {
                Console.WriteLine("Uncorrect vertex");
            }
        }

        /// <summary>
        /// Добавление ребра
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oriented"></param>
        public void AddEdge(Vertex from, Vertex to, int weight = 1, bool oriented = false)
        {
            if (from == null || to == null)
            {
                Console.WriteLine("Uncorrect edge");
                return;
            }

            foreach (var item in _vertexWeight)
            {
                if (from.Id == item.Key.Id && !item.Value.ContainsKey(to))
                {
                    item.Value.Add(to, weight);
                }
            }

            if (!_oriented && !oriented)
            {
                AddEdge(to, from, weight, true);
            }
        }

        /// <summary>
        /// Удаление ребра
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oriented"></param>
        public void DeleteEdge(Vertex from, Vertex to, int weight = 1, bool oriented = false)
        {
            if (from == null || to == null)
            {
                Console.WriteLine("Uncorrect edge");
                return;
            }

            foreach (var v in _vertexWeight)
            {
                if (v.Key.Id == from.Id)
                {
                    foreach (var e in v.Value)
                    {
                        if (e.Key.Id == to.Id && e.Value == weight)
                        {
                            v.Value.Remove(e.Key);
                            break;
                        }
                    }
                }
            }

            if (!_oriented && !oriented)
            {
                DeleteEdge(to, from, weight, true);
            }
        }

        /// <summary>
        /// Удаление вершины
        /// </summary>
        /// <param name="value"></param>
        public void DeleteVertex(Vertex value)
        {
            if (value != null)
            {
                foreach (var v in _vertexWeight)
                {
                    foreach (var e in v.Value)
                    {
                        if (e.Key.Id == value.Id)
                        {
                            v.Value.Remove(e.Key);
                            break;
                        }
                    }
                }

                foreach (var v in _vertexWeight)
                {
                    if (v.Key.Id == value.Id)
                    {
                        _vertexWeight.Remove(v.Key);
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Uncorrect vertex");
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
    }
}
