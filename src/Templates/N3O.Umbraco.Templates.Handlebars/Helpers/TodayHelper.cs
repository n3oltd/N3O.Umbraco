using HandlebarsDotNet;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates.Handlebars.Helpers {
    public class TodayHelper : IHelper {
        private readonly ILocalClock _localClock;
        private readonly IFormatter _formatter;

        public TodayHelper(ILocalClock localClock, IFormatter formatter) {
            _localClock = localClock;
            _formatter = formatter;
        }
        
        public void Execute(EncodedTextWriter writer, HandlebarsDotNet.Context context, Arguments args) {
            var today = _localClock.GetLocalToday();
            var output = _formatter.DateTime.FormatDate(today);
            
            writer.Write(output);
        }
        
        public string Name => "today";
    }
}