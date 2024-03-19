namespace BioFuelExpress.WebApplication.Controllers
{
    public class UpdateBioFuelExpressRequest
    {
        public string Name { get; }

        public UpdateBioFuelExpressRequest(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
            Name = name;
        }
    }
}
