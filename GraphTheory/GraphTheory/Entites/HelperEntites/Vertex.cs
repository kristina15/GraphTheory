namespace GraphTheory.Entites.HelperEntites
{
    public class Vertex
    {
        private bool mark = false;

        public string Id { get; set; }

        public bool Mark
        {
            get => mark;
            set
            {
                mark = value;
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
