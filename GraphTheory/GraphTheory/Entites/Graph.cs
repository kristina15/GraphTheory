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
        private const int MaxSize = int.MaxValue;
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
                if (Oriented)
                {
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
                else
                {
                    foreach (var s in StrFromFile)
                    {
                        vertex = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var from = new Vertex(vertex[0]);
                        for (int k = 1; k < vertex.Length; k += 2)
                        {
                            var to = new Vertex(vertex[k]);
                            int weight = int.Parse(vertex[k + 1]);
                            if (!_vertexWeight[from].ContainsKey(to))
                            {
                                AddEdge(from, to, weight);
                            }
                        }
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
            if (_vertexWeight.ContainsKey(value) && !Oriented)
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
                if (!Weiting)
                {
                    Console.WriteLine($"Already exists qdge {from}->{to}");
                }
                else
                {
                    Console.WriteLine($"Already exists edge {from}->{to} with weight {_vertexWeight[from][to]}");
                }

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
            var list = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            foreach (var key in _vertexWeight.Keys)
            {
                foreach (var value in _vertexWeight[key])
                {
                    if (!list.ContainsKey(key))
                    {
                        list.Add(key, new Dictionary<Vertex, int> { [value.Key] = value.Value });
                    }
                    else
                    {
                        list[key].Add(value.Key, value.Value);
                    }
                }
            }

            foreach (var item in list)
            {
                var sameDegree = new List<Vertex>();
                foreach (var item2 in list)
                {
                    if (item.Key != item2.Key && item.Value.Count == item2.Value.Count)
                    {
                        sameDegree.Add(item2.Key);
                    }
                }

                foreach (var item2 in sameDegree)
                {
                    if (_vertexWeight[item.Key].ContainsKey(item2))
                    {
                        DeleteEdge(item.Key, item2);
                    }
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

        public void GetVertex(Vertex value)
        {
            var list = _vertexWeight.Where(v => !v.Value.ContainsKey(value)).Select(v => v.Key);
            var s = list.Where(v => !_vertexWeight[value].ContainsKey(v));
            foreach (var item in s)
            {
                Console.WriteLine(item);
            }
        }

        //public void GetMinWay(Vertex u1, Vertex u2, Vertex v)
        //{
        //    if (!ValidateFromAndToVertex(u1, u2))
        //    {
        //        return;
        //    }

        //    if (!_vertexWeight.ContainsKey(v))
        //    {
        //        Console.WriteLine($"Invalid vertex {v}");
        //        return;
        //    }

        //    Dijkstra(u1, u2, v);
        //}

        //private void Dijkstra(Vertex u1, Vertex u2, Vertex v)
        //{
        //    int matrixSize = _vertexWeight.Keys.Count;
        //    var dist = new Dictionary<Vertex, int>();
        //    var path = new Dictionary<Vertex, int>();
        //    var checkPoint = new Dictionary<Vertex, bool>();

        //    foreach (var item in _vertexWeight.Keys)
        //    {
        //        dist.Add(item, int.MaxValue);
        //        checkPoint.Add(item, false);
        //    }

        //    dist[u1] = 0;

        //    foreach (var item in _vertexWeight.Keys)
        //    {
        //        var minDist = MinDistance(dist, checkPoint);

        //        checkPoint[minDist] = true;

        //        foreach (var item2 in _vertexWeight.Keys)
        //        {
        //            if (!checkPoint[item2] && _vertexWeight[minDist].TryGetValue(item2, out int n) && dist[minDist] != int.MaxValue && dist[minDist] + _vertexWeight[minDist][item2] < dist[item2])
        //            {
        //                dist[item2] = dist[minDist] + _vertexWeight[minDist][item2];
        //                path[item2] = minDist; //Заполняется массив предков
        //            }
        //        }
        //    }

        //    Console.WriteLine("Данные о путях(по Дейкстре):");
        //    Console.WriteLine();
        //    Console.WriteLine($"Наша начальная точка 1");

        //    for (int i = 1; i < matrixSize; i++)
        //    {
        //        if (path[i] == 0)
        //            Console.WriteLine($"Кратчайший путь: из 1 -> {i + 1} прямой | Мин.Расстояние: {dist[i]}");
        //        else
        //        {
        //            var stack = new Stack<int>(); //Используем стэк для сохранения пути. Так как мы идем в обратном порядке, путь по итогу должен выводиться перевернтный
        //            stack.Push(path[i] + 1);

        //            Console.Write($"Кратчайший путь: из 1 -> ");

        //            for (int j = path[i]; j != 0; j = path[j])
        //            {
        //                if (path[j] == 0)
        //                    break;
        //                else
        //                {
        //                    stack.Push(path[j]);
        //                    j = path[j];
        //                }

        //            }

        //            for (int j = 0; j <= stack.Count; j++)
        //            {
        //                if (j == stack.Count)
        //                    Console.Write($"{i + 1} | Мин.Расстояние: {dist[i]}");
        //                else
        //                {
        //                    Console.Write(stack.Pop() + " -> ");
        //                    j = -1;
        //                }
        //            }

        //            Console.WriteLine();

        //        }
        //    }
        //}

        //private Vertex MinDistance(Dictionary<Vertex, int> dist, Dictionary<Vertex, bool> sptSet)
        //{
        //    var min = int.MaxValue;
        //    Vertex minIndex = null;

        //    foreach (var item in dist.Keys)
        //    {
        //        if (!sptSet[item] && dist[item] <= min)
        //        {
        //            min = dist[item];
        //            minIndex = item;
        //        }
        //    }

        //    return minIndex;
        //}

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
