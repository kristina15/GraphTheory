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

        public static bool operator ==(Vertex v1, Vertex v2)
        {
            if (v1 is null || v2 is null)
            {
                return false;
            }

            return v1.Id.Equals(v2.Id);
        }

        public static bool operator !=(Vertex v1, Vertex v2)
        {
            if (v1 is null && v2 is null)
            {
                return false;
            }

            if(v1 is null || v2 is null)
            {
                return true;
            }

            return !(v1.Id.Equals(v2.Id));
        }

        public override bool Equals(object v2)
        {
            return this == (Vertex)v2;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
