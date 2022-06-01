namespace WebAPI.Repositories
{
    public interface ITopDestinationsRepository
    {
        public IEnumerable<string> GetTopDestinations(int number);
        public void AddDestination(string destination);
    }
}
