using GraphTheory.Entites;
using GraphTheory.Entites.HelperEntites;
using System;

namespace GraphTheory
{
    public class Program
    {
        public static void Main()
        {
            Graph graph = null;
            bool flag;
            int choice;
            do
            {
                Console.Write("Выберите действие:\n\t1. Создать новый граф\n\t2. Считать из файла\nВВОД: ");
                flag = int.TryParse(Console.ReadLine(), out choice);
            } while (!flag);
            switch (choice)
            {
                case 1:
                    graph = new Graph();
                    GetOriented(ref graph);
                    GetWeiting(ref graph);
                    break;
                case 2:
                    graph = new Graph(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph.txt");
                    break;
            }
            while (true)
            {
                do
                {
                    Console.Write("Выберите действие:\n\t1. Добавить вершину\n\t2. Удалить вершину\n\t3. Добавить ребро\n\t4. Удалить ребро\n\t5. Показать граф\n\t6. Вывести все висячие вершины\n\t7. Вывести те вершины орграфа, которые являются одновременно заходящими и выходящими для заданной вершины.\n\t8. Построить граф, полученный однократным удалением рёбер, соединяющих вершины одинаковой степени.\n\t9. Найти путь, соединяющий вершины u1 и u2 и не проходящий через вершину v.\n\t10. Вывести все вершины, длины кратчайших (по числу дуг) путей от которых до всех остальных не превосходят k\n\t11. Выход\nВВОД: ");
                    flag = int.TryParse(Console.ReadLine(), out choice);
                } while (!flag);
                string name;
                string from;
                string to;
                string weight;
                switch (choice)
                {
                    case 1:
                        Console.Write("Введите имя вершины: ");
                        name = Console.ReadLine();
                        graph.AddVertex(new Vertex(name));
                        break;
                    case 2:
                        Console.Write("Введите имя вершины: ");
                        name = Console.ReadLine();
                        graph.DeleteVertex(new Vertex(name));
                        break;
                    case 3:
                        Console.Write("Введите имя вершины \"откуда\": ");
                        from = Console.ReadLine();
                        Console.Write("Введите имя вершины \"куда\": ");
                        to = Console.ReadLine();
                        if (graph.Weiting == true)
                        {
                            Console.Write("Введите вес вершины: ");
                            weight = Console.ReadLine();
                            if (!string.IsNullOrEmpty(weight))
                            {
                                graph.AddEdge(new Vertex(from), new Vertex(to), int.Parse(weight), graph.Oriented);
                            }
                        }
                        else
                        {
                            graph.AddEdge(new Vertex(from), new Vertex(to), 1, graph.Oriented);
                        }
                        break;
                    case 4:
                        Console.Write("Введите имя вершины \"откуда\": ");
                        from = Console.ReadLine();
                        Console.Write("Введите имя вершины \"куда\": ");
                        to = Console.ReadLine();
                        graph.DeleteEdge(new Vertex(from), new Vertex(to), graph.Oriented);
                        break;
                    case 5:
                        Print(graph);
                        break;
                    case 6:
                        graph.GetHengingVertex_TaskLa6();
                        break;
                    case 7:
                        Console.Write("Введите вершину: ");
                        graph.GetInAndOutVertex(new Vertex(Console.ReadLine()));
                        break;
                    case 8:
                        graph.GetGraphWithoutVertexWithSameDegree();
                        break;
                    case 9:
                        Console.Write("Введите имя вершины \"откуда\": ");
                        var u1 = Console.ReadLine();
                        Console.Write("Введите имя вершины \"куда\": ");
                        var u2 = Console.ReadLine();
                        Console.Write("Введите имя вершины, через которую не нужно проходить : ");
                        var v = Console.ReadLine();
                        graph.GetWay_Task_II_8(new Vertex(u1), new Vertex(u2), new Vertex(v));
                        break;
                    case 10:
                        do
                        {
                            Console.Write("Введите k = ");
                            flag = int.TryParse(Console.ReadLine(), out choice);
                        } while (!flag);
                        graph.GetVertex_II_34(choice);
                        break;
                    case 11:
                        do
                        {
                            Console.Write("Хотите сохранить изменения?\n\t1.Да\n\t2.Нет\nВВОД: ");
                            flag = int.TryParse(Console.ReadLine(), out choice);
                        } while (!flag);
                        switch (choice)
                        {
                            case 1:
                                graph.Save(@"C:\Users\krisy\OneDrive\Documents\GraphTheory\GraphTheory\graph.txt");
                                return;
                            case 2:
                                return;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void GetWeiting(ref Graph graph9)
        {
            bool flag;
            int choice;
            do
            {
                Console.Write("Выберите тип графа:\n\t1. Взвешенный\n\t2. Невзвешенный\nВВОД: ");
                flag = int.TryParse(Console.ReadLine(), out choice);
            } while (!flag);
            switch (choice)
            {
                case 1:
                    graph9.Weiting = true;
                    break;
                case 2:
                    graph9.Weiting = false;
                    break;
            }
        }

        private static void GetOriented(ref Graph graph9)
        {
            bool flag;
            int choice;
            do
            {
                Console.Write("Выберите тип графа:\n\t1. Ориентированный\n\t2. Неориентированный\nВВОД: ");
                flag = int.TryParse(Console.ReadLine(), out choice);
            } while (!flag);
            switch (choice)
            {
                case 1:
                    graph9.Oriented = true;
                    break;
                case 2:
                    graph9.Oriented = false;
                    break;
            }
        }

        /// <summary>
        /// Вывод на консоль
        /// </summary>
        public static void Print(Graph g)
        {
            foreach (var item in g._vertexWeight)
            {
                Console.Write(item.Key.ToString() + ": ");
                if (g.Weiting == false)
                {
                    foreach (var item2 in item.Value)
                    {
                        Console.Write(item2.Key + " ");
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
    }
}
