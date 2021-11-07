using GraphTheory.Entites.HelperEntites;
using System;
using System.Collections;
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
            dfs(u1, ref newGraph, ref dist);

            if (!dist.ContainsKey(u2))
            {
                Console.WriteLine("К сожалению, такого пути не существует");
            }
            else
            {
                Console.Write($"{u1} -> ");
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
                bfs(item, ref dict);
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
        /// Поиск каркаса минимального веса по алгоритму Прима
        /// </summary>
        /// <returns></returns>
        public Graph AlgorithmByPrim()
        {
            if (Oriented && !Weiting)
            {
                Console.WriteLine("Граф должен быть взвешенным и неориентированным");
                return null;
            }

            if (!_vertexWeight.Keys.All(v => IsNotIsolated(v)))
            {
                Console.WriteLine("Невозможно получить минимальное остовное дерево");
                return null;
            }

            Graph MST = new Graph(false);
            IDictionary<Vertex, Dictionary<Vertex, int>> notUsedEdges = GetCopy();
            List<Vertex> usedV = new List<Vertex>();
            List<Vertex> notUsedV = new List<Vertex>(_vertexWeight.Keys.ToList());
            Random rand = new Random();
            usedV.Add(notUsedV[rand.Next(0, _vertexWeight.Keys.Count)]);
            Console.WriteLine($"Начинаем поиск с вершины: {usedV[0]}");
            notUsedV.Remove(usedV[0]);
            while (notUsedV.Count > 0)
            {
                KeyValuePair<Vertex, Vertex> minW = new KeyValuePair<Vertex, Vertex>();
                //номер наименьшего ребра
                //поиск наименьшего ребра
                foreach (var v in notUsedEdges)
                {
                    foreach (var e in v.Value)
                    {
                        if ((usedV.Contains(v.Key) && notUsedV.Contains(e.Key)) || (usedV.Contains(e.Key) && notUsedV.Contains(v.Key)))
                        {
                            if (minW.Key != null)
                            {
                                if (e.Value < _vertexWeight[minW.Key][minW.Value])
                                {
                                    minW = new KeyValuePair<Vertex, Vertex>(v.Key, e.Key);
                                }
                            }
                            else
                            {
                                minW = new KeyValuePair<Vertex, Vertex>(v.Key, e.Key);
                            }
                        }
                    }
                }
                //заносим новую вершину в список использованных и удаляем ее из списка неиспользованных
                if (usedV.Contains(minW.Key))
                {
                    usedV.Add(minW.Value);
                    notUsedV.Remove(minW.Value);
                }
                else
                {
                    usedV.Add(minW.Key);
                    notUsedV.Remove(minW.Value);
                }
                //заносим новое ребро в дерево и удаляем его из списка неиспользованных
                if (!MST.ContainsVertex(minW.Key))
                {
                    MST.AddVertex(minW.Key);
                }

                if (!MST.ContainsVertex(minW.Value))
                {
                    MST.AddVertex(minW.Value);
                }

                if (!MST.ContainsEdge(minW.Key, minW.Value))
                {
                    MST.AddEdge(minW.Key, minW.Value, _vertexWeight[minW.Key][minW.Value]);
                }

                //удаляем ребро из неиспользованных ребер
                notUsedEdges[minW.Key].Remove(minW.Value);
                if (notUsedEdges[minW.Key].Count == 0)
                {
                    notUsedEdges.Remove(minW.Key);
                }
            }

            return MST;
        }

        public bool ContainsVertex(Vertex v) => _vertexWeight.ContainsKey(v);

        public bool ContainsEdge(Vertex u1, Vertex u2) => _vertexWeight[u1].ContainsKey(u2);

        public bool IsNotIsolated(Vertex v) => _vertexWeight.Values.Any(u => u.ContainsKey(v));

        public int GetWeightOfGraph()
        {
            var newGraph = new Graph(true);
            foreach (var item in _vertexWeight)
            {
                foreach (var item2 in item.Value)
                {
                    if (!newGraph.ContainsVertex(item.Key))
                    {
                        newGraph.AddVertex(item.Key);
                    }

                    if (!newGraph.ContainsVertex(item2.Key))
                    {
                        newGraph.AddVertex(item2.Key);
                    }

                    if (!newGraph.ContainsEdge(item.Key, item2.Key) && !newGraph.ContainsEdge(item2.Key, item.Key))
                    {
                        newGraph.AddEdge(item.Key, item2.Key, _vertexWeight[item.Key][item2.Key]);
                    }
                }
            }

            return newGraph._vertexWeight.Sum(u => u.Value.Values.Sum());
        }

        public void IVa(Vertex v)
        {
            Dictionary<Vertex, int> nodes = new Dictionary<Vertex, int>();
            _usedVertex.Clear();

            foreach (var key in _vertexWeight.Keys)
            {
                nodes.Add(key, int.MaxValue);
            }

            nodes[v] = 0;

            Dijkstra(v, ref nodes);

            foreach (var key in nodes.Keys)
            {
                if (key != v)
                {
                    if (nodes[key] != int.MaxValue)
                    {
                        Console.WriteLine($"{v} -> {key} = {nodes[key]}");
                    }
                    else
                    {
                        Console.WriteLine($"{v} -> {key} = маршрут отсутствует");
                    }
                }
            }
        }

        public void IVc(Vertex v)
        {
            if (!Oriented)
            {
                var dic = FordBellman(v);

                foreach (var key in dic.Keys)
                {
                    if (key != v)
                    {
                        if (dic[key] != int.MaxValue)
                        {
                            Console.WriteLine($"{key} -> {v} = {dic[key]}");
                        }
                        else
                        {
                            Console.WriteLine($"{key} -> {v} = маршрут отсутствует");
                        }
                    }
                }
            }
            else
            {
                var m = GetCopy();
                Floyd(ref m);

                foreach (var key in m.Keys)
                {
                    if (key != v)
                    {
                        if (m[key].ContainsKey(v))
                        {
                            Console.WriteLine($"{key} -> {v} = {m[key][v]}");
                        }
                        else
                        {
                            Console.WriteLine($"{key} -> {v} = маршрут отсутствует");
                        }
                    }
                }
            }
        }


        public void IVb()
        {
            if (!_vertexWeight.Keys.All(v => IsNotIsolated(v)))
            {
                Console.WriteLine("Граф должен быть связным");
                return;
            }

            var m = GetCopy();
            Floyd(ref m);

            Dictionary<Vertex, long> l = new Dictionary<Vertex, long>();
            foreach (var item in _vertexWeight.Keys)
            {
                var max = m[item].Values.Max();
                l.Add(item, max);
            }

            var min = l.Min(x => x.Value);
            var r = l.Where(x => x.Value == min).ToList();

            Console.Write($"Радиус = {min}\nЦентр графа: ");
            foreach (var item in r)
            {
                Console.Write($"{item.Key} ");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Алгоритм Форда-Беллмана
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Dictionary<Vertex, long> FordBellman(Vertex v)
        {
            Dictionary<Vertex, long> F = new Dictionary<Vertex, long>();
            foreach (var key in _vertexWeight.Keys)
            {
                F.Add(key, int.MaxValue);
            }
            F[v] = 0;

            for (int i = 0; i < _vertexWeight.Count; i++)
            {
                foreach (var node in _vertexWeight.Keys)
                {
                    foreach (var edge in _vertexWeight[node])
                    {
                        F[edge.Key] = Math.Min(F[edge.Key], F[node] + edge.Value);
                    }
                }
            }

            return F;
        }

        /// <summary>
        /// Алгоритм Дейкстра
        /// </summary>
        /// <param name="v"></param>
        /// <param name="nodes"></param>
        private void Dijkstra(Vertex v, ref Dictionary<Vertex, int> nodes)
        {
            _usedVertex.Add(v);

            foreach (var key in _vertexWeight[v])
            {
                if (!_usedVertex.Contains(key.Key))
                {
                    foreach (var keys in _vertexWeight[v])
                    {
                        nodes[keys.Key] = Math.Min(nodes[keys.Key], keys.Value + nodes[v]);
                    }
                }
            }

            int minWeight = int.MaxValue;
            Vertex minNode = null;
            foreach (var key in _vertexWeight[v])
            {
                if (nodes[key.Key] < minWeight && !_usedVertex.Contains(key.Key))
                {
                    minWeight = nodes[key.Key];
                    minNode = key.Key;
                }
            }

            if (minNode != null)
            {
                Dijkstra(minNode, ref nodes);
            }
        }

        /// <summary>
        /// Алгоритм Флойда
        /// </summary>
        /// <param name="v"></param>
        public void Floyd(ref Dictionary<Vertex, Dictionary<Vertex, int>> m)
        {
            foreach (var k in _vertexWeight.Keys)
            {
                foreach (var i in _vertexWeight.Keys)
                {
                    foreach (var j in _vertexWeight.Keys)
                    {
                        if (i == j && !m[i].ContainsKey(j))
                        {
                            m[i].Add(j, 0);
                        }
                        else if (m[i].ContainsKey(j) && m[i].ContainsKey(k) && m[k].ContainsKey(j))
                        {
                            m[i][j] = Math.Min(m[i][j], m[i][k] + m[k][j]);
                        }
                        else if (m[i].ContainsKey(k) && m[k].ContainsKey(j))
                        {
                            m[i].Add(j, m[i][k] + m[k][j]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Обход в ширину
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="newGraph"></param>
        /// <param name="dist"></param>
        private void bfs(Vertex u1, ref Dictionary<Vertex, int> dist)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            _usedVertex = new HashSet<Vertex> { u1 };
            queue.Enqueue(u1);
            while (queue.Count > 0)
            {
                var u = queue.Peek();
                queue.Dequeue();
                foreach (var item in _vertexWeight[u].Keys)
                {
                    if (!_usedVertex.Contains(item))
                    {
                        _usedVertex.Add(item);
                        queue.Enqueue(item);
                        dist.Add(item, dist[u] + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Поиск в глубину
        /// </summary>
        /// <param name="v"></param>
        /// <param name="dist"></param>
        private void dfs(Vertex v, ref Graph newGraph, ref Dictionary<Vertex, List<Vertex>> dist)
        {
            newGraph._usedVertex.Add(v);
            foreach (var item in newGraph._vertexWeight[v].Keys)
            {
                if (!newGraph._usedVertex.Contains(item))
                {
                    dist.Add(item, new List<Vertex> { v });
                    dfs(item, ref newGraph, ref dist);
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

        /// <summary>
        /// Копия словаря
        /// </summary>
        /// <returns></returns>
        private Dictionary<Vertex, Dictionary<Vertex, int>> GetCopy()
        {
            var copy = new Dictionary<Vertex, Dictionary<Vertex, int>>();
            foreach (var key in _vertexWeight.Keys)
            {
                copy.Add(key, new Dictionary<Vertex, int>());
                foreach (var value in _vertexWeight[key])
                {
                    copy[key].Add(value.Key, value.Value);
                }
            }

            return copy;
        }
    }
}
