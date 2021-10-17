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
        HashSet<Vertex> _usedVertex = new HashSet<Vertex>();
        private IDictionary<Vertex, Dictionary<Vertex, int>> vertexWeight;

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

        public Graph(IDictionary<Vertex, Dictionary<Vertex, int>> vertexWeight)
        {
            _vertexWeight = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            foreach (var key in vertexWeight.Keys)
            {
                foreach (var value in vertexWeight[key])
                {
                    if (!_vertexWeight.ContainsKey(key))
                    {
                        _vertexWeight.Add(key, new Dictionary<Vertex, int> { [value.Key] = value.Value });
                    }
                    else
                    {
                        _vertexWeight[key].Add(value.Key, value.Value);
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

        /// <summary>
        /// Поиск пути, соединяющего вершины u1 и u2 и не проходящий через вершину v.
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <param name="v"></param>
        public void GetWay_Task_II_8(Vertex u1, Vertex u2, Vertex v)
        {
            var newGraph = new Graph(_vertexWeight);
            newGraph.DeleteVertex(v);
            var dist = new Dictionary<Vertex, List<Vertex>>() { [u1] = new List<Vertex>() };
            bfs(u1, ref newGraph, ref dist);

            if (!dist.ContainsKey(u2))
            {
                Console.WriteLine("К сожалению, такого пути не существует");
            }
            else
            {
                for (int i = dist[u2].Count - 1; i > -1; i--)
                {
                    Console.Write($"{dist[u2][i]} -> ");
                }

                Console.WriteLine($"{u2} ");
            }
        }

        /// <summary>
        /// Вывести все вершины, длины кратчайших (по числу дуг) путей от которых до всех остальных не превосходят k
        /// </summary>
        /// <param name="k"></param>
        public void GetVertex_II_34(int k)
        {
            var dist = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            _usedVertex.Clear();
            foreach (var item in _vertexWeight.Keys)
            {
                var dict = new Dictionary<Vertex, int>() { [item] = 0 };
                dist.Add(item, dict);
                dfs(item, ref dict);
                _usedVertex.Clear();
            }

            var vs = dist.Where(v => v.Value.All(x => x.Value <= k)).Select(v => v.Key);
            foreach (var item in vs)
            {
                Console.Write($"{item} ");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Обход в ширину
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="newGraph"></param>
        /// <param name="dist"></param>
        private void bfs(Vertex u1, ref Graph newGraph, ref Dictionary<Vertex, List<Vertex>> dist)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            newGraph._usedVertex = new HashSet<Vertex> { u1 };
            queue.Enqueue(u1);
            while (queue.Count > 0)
            {
                var u = queue.Peek();
                queue.Dequeue();
                foreach (var item in newGraph._vertexWeight[u].Keys)
                {
                    if (!newGraph._usedVertex.Contains(item))
                    {
                        newGraph._usedVertex.Add(item);
                        queue.Enqueue(item);
                        dist.Add(item, new List<Vertex>() { u });
                        dist[item].AddRange(dist[u]);

                    }
                }
            }
        }

        /// <summary>
        /// Поиск в глубину
        /// </summary>
        /// <param name="v"></param>
        /// <param name="dist"></param>
        private void dfs(Vertex v, ref Dictionary<Vertex, int> dist)
        {
            _usedVertex.Add(v);
            foreach (var item in _vertexWeight[v].Keys)
            {
                if (!_usedVertex.Contains(item))
                {
                    dist.Add(item, dist[v] + 1);
                    dfs(item, ref dist);
                }
            }
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
