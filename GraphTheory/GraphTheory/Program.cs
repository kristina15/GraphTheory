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
                    Console.Write("Выберите действие:\n\t1. Добавить вершину\n\t2. Удалить вершину\n\t3. Добавить ребро\n\t4. Удалить ребро\n\t5. Показать граф\n\t6. Выход\nВВОД: ");
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
                        graph.Print();
                        break;
                    case 6:
                        do
                        {
                            Console.Write("Хотите сохранить изменения?\n\t1.Да\n\t2.Нет\nВВОД:");
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
                    case 7:
                        return;
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
                Console.Write("Выберите тип графа:\n\t1. Взвешенный\n\t2. Невзвешенный\nВВОД:");
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
                Console.Write("Выберите тип графа:\n\t1. Ориентированный\n\t2. Неориентированный\nВВОД:");
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

        public void Print(Graph g)
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
