namespace WebAPI.Repositories
{
    public class TopDestinationsRepository : ITopDestinationsRepository
    {
        private Dictionary<string, int> _destinations;
        public TopDestinationsRepository()
        {
            _destinations = new Dictionary<string, int>();
        }
        public IEnumerable<string> GetTopDestinations(int number)
        {
            var actualNumber = number;
            if(number > _destinations.Count())
            {
                actualNumber = _destinations.Count();
            }
            var topDestinations = _destinations.OrderByDescending(x => x.Value).Select(x => x.Key).Take(actualNumber).ToList();
            // add empty elements if number was greater than count
            for(int i = actualNumber; i < number; i++)
            {
                topDestinations.Add("");
            }
            return topDestinations;
        }
        public void AddDestination(string destination)
        {
            if(_destinations.ContainsKey(destination))
            {
                _destinations[destination] += 1;
            }
            else
            {
                _destinations.Add(destination, 1);
            }
        }
    }
}
