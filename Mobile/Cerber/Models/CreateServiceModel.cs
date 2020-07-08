namespace Cerber.Models
{
    public class CreateServiceModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public int ExpectedStatusCode { get; set; }

        public string ExpectedResponse { get; set; }

        public int LifetimeInHours { get; set; }
    }
}
