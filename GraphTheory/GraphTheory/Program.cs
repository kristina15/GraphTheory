using GraphTheory.Entites;
using GraphTheory.Entites.HelperEntites;
using System;

namespace GraphTheory
{
    public class Program
    {
        public static void Main()
        {
            var graph = new Graph();

            #region Добавление вершин ориентированного графа
            var a = new Vertex(1);
            var b = new Vertex(2);
            var c = new Vertex(3);
            var d = new Vertex(4);
            var e = new Vertex(5);
            var f = new Vertex(6);
            var h = new Vertex(7);
            var k = new Vertex(8);
            AddVertex(graph, a, b, c, d, e, f, h, k);
            #endregion

            #region Добавление ребер ориентированного графа
            graph.AddEdge(a, b);
            graph.AddEdge(a, c, 1);
            graph.AddEdge(a, d);
            graph.AddEdge(b, c);
            graph.AddEdge(b, e);
            graph.AddEdge(b, k);
            graph.AddEdge(c, d);
            graph.AddEdge(c, f);
            graph.AddEdge(c, k);
            graph.AddEdge(d, f);
            graph.AddEdge(d, k);
            graph.AddEdge(f, k);
            graph.AddEdge(e, f);
            #endregion

            Console.WriteLine("Исходный граф (ориентированный):");
            graph.Print();
            graph.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph1.txt");

            var graph2 = new Graph(@"C:\Users\krisy\source\repos\GraphTheory\graph1.txt");
            Console.WriteLine("\nГраф-копия:");
            graph2.Print();

            graph2.DeleteVertex(b);
            Console.WriteLine("\nГраф после удаления вершины 2:");
            graph2.Print();

            graph2.DeleteEdge(f, k, 1);
            Console.WriteLine("\nГраф после удаления ребра (6, 8):");
            graph2.Print();

            graph2.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph2.txt");

            var graph3 = new Graph(0.2);
            Console.WriteLine("\nСлучайный граф:");
            graph3.Print();
            graph3.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph3.txt");

            var graph4 = new Graph(false);
            AddVertex(graph4, a, b, c, d, e, f, h, k);

            #region Добавление ребер неориентированного графа (с петлями)
            graph4.AddEdge(a, b, 1);
            graph4.AddEdge(a, c, 1);
            graph4.AddEdge(a, d, 1);
            graph4.AddEdge(a, a);
            graph4.AddEdge(b, e, 1);
            graph4.AddEdge(k, k);
            graph4.AddEdge(c, f, 1);
            graph4.AddEdge(c, k, 1);
            graph4.AddEdge(b, k, 1);
            graph4.AddEdge(f, k, 1);
            #endregion

            Console.WriteLine("\nНеориентированный граф (c петлями):");
            graph4.Print();
            graph4.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph4.txt");

            var graph5 = new Graph();
            AddVertex(graph5, a, b, c, d, e, f, h, k);

            #region Добавление ребер взвешенного графа
            graph5.AddEdge(a, b, 45);
            graph5.AddEdge(a, c, 65);
            graph5.AddEdge(a, d, 76);
            graph5.AddEdge(b, e, 32);
            graph5.AddEdge(b, k, 75);
            graph5.AddEdge(c, f, 4);
            graph5.AddEdge(c, k, 12);
            graph5.AddEdge(f, k, 10);
            #endregion

            Console.WriteLine("\nВзвешенный граф:");
            graph5.Print();
            graph5.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph5.txt");

            #region Проверки на некоректный ввод
            Console.Write("\nУдаление несуществующей вершины: ");
            graph4.DeleteVertex(null);

            Console.Write("\nУдаление ребра без вершины 'from': ");
            graph4.DeleteEdge(null, c);

            Console.Write("\nУдаление ребра без вершины 'to': ");
            graph4.DeleteEdge(c, null);

            Console.Write("\nУдаление несуществующего ребра: ");
            graph4.DeleteEdge(null, null);
            #endregion
        }

        private static void AddVertex(Graph graph, Vertex a, Vertex b, Vertex c, Vertex d, Vertex e, Vertex f, Vertex h, Vertex k)
        {
            graph.AddVertex(a);
            graph.AddVertex(b);
            graph.AddVertex(c);
            graph.AddVertex(d);
            graph.AddVertex(e);
            graph.AddVertex(f);
            graph.AddVertex(h);
            graph.AddVertex(k);
        }
    }
}
