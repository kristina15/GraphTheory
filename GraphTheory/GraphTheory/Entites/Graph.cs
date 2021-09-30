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
        public bool Oriented { get; set; }
        public bool Weiting { get; set; }

        public Graph()
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            _randomNumber = new Random();
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
                Oriented = file.ReadLine()=="1"?true:false;
                Weiting = file.ReadLine() == "1" ? true : false;
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
            _randomNumber = new Random();
            Oriented = oriented;
            Weiting = false;
        }

        /// <summary>
        /// Вывод на консоль
        /// </summary>
        public void Print()
        {
            foreach (var item in _vertexWeight)
            {
                Console.Write(item.Key.ToString() + ": ");
                if (Weiting == false)
                {
                    foreach (var item2 in item.Value)
                    {
                            Console.Write(item2.Key+" ");
                    }
                }
                else
                {
                    foreach (var item2 in item.Value)
                    {
                            Console.Write("(" + item2.Key + ",");
                            Console.Write(item2.Value + ") ");
                        
                    }
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Конструктор - копия
        /// </summary>
        /// <param name="G"></param>
        public Graph(Graph G)
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>(G._vertexWeight);
            _randomNumber = new Random();
            Oriented = G.Oriented;
            Weiting = G.Weiting;
        }

        /// <summary>
        /// Добавление вершины
        /// </summary>
        /// <param name="value"></param>
        public void AddVertex(Vertex value)
        {
            try
            {
                if (_vertexWeight.Where(v => v.Key.Id == value.Id).Count() == 0)
                {
                    _vertexWeight.Add(value, new Dictionary<Vertex, int>());
                }
                else
                {
                    Console.WriteLine("Vertex already exists");
                }
            }
            catch
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
            if (_vertexWeight.Keys.Any(v => v.Id == from.Id) && _vertexWeight.Keys.Any(v => v.Id == to.Id))
            {
                foreach (var item in _vertexWeight)
                {
                    if (from.Id == item.Key.Id && _vertexWeight.Keys.Where(v => v == to).Count() != 0 && item.Value.Where(v => v.Key == to).Count() == 0)
                    {
                        item.Value.Add(to, weight);
                    }
                }

                if (!Oriented && !oriented)
                {
                    AddEdge(to, from, weight, true);
                }
            }
            else
            {
                Console.WriteLine("Uncorrect edge");
            }
        }

        /// <summary>
        /// Удаление ребра
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oriented"></param>
        public void DeleteEdge(Vertex from, Vertex to, int weight = 1, bool oriented = false)
        {
            if (_vertexWeight.Keys.Any(v => v.Id == from.Id) && _vertexWeight.Keys.Any(v => v.Id == to.Id))
            { 
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

                if (!Oriented && !oriented)
                {
                    DeleteEdge(to, from, weight, true);
                }
            }
            else
            {
                Console.WriteLine("Uncorrect edge");
            }
        }

        /// <summary>
        /// Удаление вершины
        /// </summary>
        /// <param name="value"></param>
        public void DeleteVertex(Vertex value)
        {
            if (_vertexWeight.Keys.Any(v => v.Id == value.Id))
            { foreach (var v in _vertexWeight)
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
    }
}
