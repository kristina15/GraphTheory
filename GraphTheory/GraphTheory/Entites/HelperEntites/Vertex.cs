namespace GraphTheory.Entites.HelperEntites
{
    public class Vertex
    {
        private bool mark = false;
        private int subTree = 0;

        public string Id { get; set; }

        public bool Mark
        {
            get => mark;
            set
            {
                mark = value;
            }
        }

        public int SubTree
        {
            get => subTree;
            set
            {
                subTree = value;
            }
        }

        public Vertex(string id)
        {
            Id = id;
        }

        public Vertex(int id)
        {
            Id = id.ToString();
        }

        public override string ToString()
        {
            return $"{Id}";
        }
    }
}
