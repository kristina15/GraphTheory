using GraphTheory.Entites.HelperEntites;
using GraphTheory.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphTheory.Entites
{
    public class Graph
    {
        private IDictionary<Vertex, List<Edge>> _vertexWeight;
        private IList<Edge> Edge;
        private Random _randomNumber;
        private EdgeValidator _edgeValidator;

        public Graph()
        {
            _vertexWeight = new Dictionary<Vertex, List<Edge>>();
            Edge = new List<Edge>();
            _randomNumber = new Random();
            _edgeValidator = new EdgeValidator();

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
                        AddEdge(new Edge(from, to, weight));
                    }
                }
            }
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
                    if (item.Value.Any(x=>x.To.Id == item2.Id))
                    {
                        Console.Write(item.Value.First(x => x.To.Id == item2.Id)._weight.ToString() + "\t");
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
            _vertexWeight = new Dictionary<Vertex, List<Edge>>(G._vertexWeight);
            Edge = new List<Edge>(G.Edge);
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
                _vertexWeight.Add(new Vertex(i + 1), new List<Edge>());
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
                            AddEdge(new Edge(i, j, 1));
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
                _vertexWeight.Add(value, new List<Edge>());
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
        public void AddEdge(Edge value, bool oriented = true)
        {
            if(value == null)
            {
                Console.WriteLine("Uncorrect edge");
                return;
            }

            var res = _edgeValidator.Validate(value);

            if (res.IsValid)
            {
                foreach (var item in _vertexWeight)
                {
                    if (value.From.Id == item.Key.Id)
                    {
                        item.Value.Add(value);
                    }

                    if (!oriented)
                    {
                        if (value.To.Id == item.Key.Id)
                        {
                            var value2 = new Edge(value.To, value.From, value._weight);
                            item.Value.Add(value2);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(res.Errors);
            }
        }

        /// <summary>
        /// Удаление ребра
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oriented"></param>
        public void DeleteEdge(Edge value, bool oriented = true)
        {
            if(value==null)
            {
                Console.WriteLine("Uncorrect edge");
                return;
            }

            var res = _edgeValidator.Validate(value);
            if (res.IsValid)
            {
                foreach (var v in _vertexWeight)
                {
                    if (v.Key.Id == value.From.Id)
                    {
                        foreach (var e in v.Value)
                        {
                            if (e.To.Id == value.To.Id)
                            {
                                v.Value.Remove(e);
                                break;
                            }
                        }
                    }
                }

                if (!oriented)
                {
                    DeleteEdge(new Edge(value.To, value.From, value._weight));
                }
            }
            else
            {
                foreach (var item in res.Errors)
                {
                    Console.WriteLine(item.ErrorMessage);
                }
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
                        if (e.To.Id == value.Id)
                        {
                            v.Value.Remove(e);
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
                        builder.Append($"{v.To} {v._weight} ");
                    }

                    sw.WriteLine(builder);
                }
            }
        }
    }
}
