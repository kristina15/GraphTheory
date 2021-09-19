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
            var edge1 = new Edge(a, b);
            var edge2 = new Edge(a, c);
            var edge3 = new Edge(a, d);
            var edge4 = new Edge(b, c);
            var edge5 = new Edge(b, e);
            var edge6 = new Edge(b, k);
            var edge7 = new Edge(c, d);
            var edge8 = new Edge(c, f);
            var edge9 = new Edge(c, k);
            var edge10 = new Edge(d, f);
            var edge11 = new Edge(d, k);
            var edge12 = new Edge(f, k);
            var edge13 = new Edge(e, f);

            graph.AddEdge(edge1);
            graph.AddEdge(edge2, false);
            graph.AddEdge(edge3);
            graph.AddEdge(edge4);
            graph.AddEdge(edge5);
            graph.AddEdge(edge6);
            graph.AddEdge(edge7);
            graph.AddEdge(edge8);
            graph.AddEdge(edge9);
            graph.AddEdge(edge10);
            graph.AddEdge(edge11);
            graph.AddEdge(edge12);
            graph.AddEdge(edge13);
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

            graph2.DeleteEdge(edge12);
            Console.WriteLine("\nГраф после удаления ребра (6, 8):");
            graph2.Print();

            graph2.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph2.txt");

            var graph3 = new Graph(0.2);
            Console.WriteLine("\nСлучайный граф:");
            graph3.Print();
            graph3.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph3.txt");

            var graph4 = new Graph();
            AddVertex(graph4, a, b, c, d, e, f, h, k);

            #region Добавление ребер неориентированного графа (с петлями)
            var edge41 = new Edge(a, b);
            var edge42 = new Edge(a, c);
            var edge43 = new Edge(a, d);
            var edge44 = new Edge(a, a);
            var edge45 = new Edge(b, e);
            var edge46 = new Edge(b, k);
            var edge47 = new Edge(k, k);
            var edge48 = new Edge(c, f);
            var edge49 = new Edge(c, k);
            var edge412 = new Edge(f, k);

            graph4.AddEdge(edge41, false);
            graph4.AddEdge(edge42, false);
            graph4.AddEdge(edge43, false);
            graph4.AddEdge(edge44);
            graph4.AddEdge(edge45, false);
            graph4.AddEdge(edge47);
            graph4.AddEdge(edge48, false);
            graph4.AddEdge(edge46, false);
            graph4.AddEdge(edge49, false);
            graph4.AddEdge(edge412, false);
            #endregion

            Console.WriteLine("\nНеориентированный граф (c петлями):");
            graph4.Print();
            graph4.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph4.txt");

            var graph5 = new Graph();
            AddVertex(graph5, a, b, c, d, e, f, h, k);

            #region Добавление ребер взвешенного графа
            var edge51 = new Edge(a, b, 45);
            var edge52 = new Edge(a, c, 65);
            var edge53 = new Edge(a, d, 76);
            var edge55 = new Edge(b, e, 32);
            var edge56 = new Edge(b, k, 75);
            var edge58 = new Edge(c, f, 4);
            var edge59 = new Edge(c, k, 12);
            var edge512 = new Edge(f, k, 10);

            graph5.AddEdge(edge51);
            graph5.AddEdge(edge52);
            graph5.AddEdge(edge53);
            graph5.AddEdge(edge55);
            graph5.AddEdge(edge58);
            graph5.AddEdge(edge56);
            graph5.AddEdge(edge59);
            graph5.AddEdge(edge512);
            #endregion

            Console.WriteLine("\nВзвешенный граф:");
            graph5.Print();
            graph5.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph5.txt");

            #region Проверки на некоректный ввод
            Console.Write("\nУдаление несуществующей вершины: ");
            graph4.DeleteVertex(null);

            Console.Write("\nУдаление ребра без вершины 'from': ");
            graph4.DeleteEdge(new Edge(null, c));

            Console.Write("\nУдаление ребра без вершины 'to': ");
            graph4.DeleteEdge(new Edge(c, null));

            Console.Write("\nУдаление несуществующего ребра: ");
            graph4.DeleteEdge(null);
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
