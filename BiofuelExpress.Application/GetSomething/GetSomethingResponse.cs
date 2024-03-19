namespace BioFuelExpress.Application.GetSomething
{
    public class GetSomethingResponse
    {
        public Guid Id { get; }

        public string Title { get; set; }

        public GetSomethingResponse(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
