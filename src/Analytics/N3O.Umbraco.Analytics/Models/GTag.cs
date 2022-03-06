using System.Collections.Generic;

namespace N3O.Umbraco.Analytics.Models {
    public class GTag {
        private readonly List<(string, object)> _events = new();

        public void AddEvent(string name, object parameters = null) {
            _events.Add((name, parameters));
        }

        public IReadOnlyList<(string, object)> GetEvents() => _events;
    }
}