namespace GraphTheory.Entites.HelperEntites
{
    public class Vertex
    {
        private bool mark = false;
        private int subTree = 0;

        public int Id { get; set; }

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

        public Vertex(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{Id}";
        }
    }
}
