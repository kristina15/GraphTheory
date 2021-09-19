using System.Collections.Generic;

namespace GraphTheory.Entites.HelperEntites
{
    public class Edge
    {
        public Vertex From { get; set; }

        public Vertex To { get; set; }

        public int _weight = 1;

        public Edge(Vertex from, Vertex to)
        {
            From = from;
            To = to;
        }

        public Edge(Vertex from, Vertex to, int weight)
        {
            From = from;
            To = to;
            _weight = weight;
        }

        public Edge(int fromValue, int toValue, int weight, Dictionary<Vertex, List<Edge>> VertexWeight)
        {
            Vertex from = null;
            Vertex to = null;
            foreach (var v in VertexWeight)
            {
                if (v.Key.Id == fromValue) from = v.Key;
                if (v.Key.Id == toValue) to = v.Key;
            }
            From = from;
            To = to;
            _weight = weight;
        }
    }
}
