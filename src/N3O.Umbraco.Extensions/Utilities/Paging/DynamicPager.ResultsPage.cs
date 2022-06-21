using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Utilities;

public partial class DynamicPager<T> {
    public class ResultsPage {
        private readonly Func<int, int, IEnumerable<T>> _getResults;
        private List<T> _results;

        public ResultsPage(Func<int, int, IEnumerable<T>> getResults, int pageNumber, int start, int num) {
            PageNumber = pageNumber;
            Start = start;
            Number = num;
            _getResults = getResults;
        }

        public int PageNumber { get; }
        public int Start { get; }
        public int Number { get; }

        public IEnumerable<T> Results {
            get {
                if (_results == null) {
                    _results = _getResults(Start, Number).ToList();
                }

                return _results;
            }
        }
    }
}
