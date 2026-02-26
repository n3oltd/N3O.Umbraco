using Umbraco.Engage.Infrastructure.Personalization.Segments;
using Umbraco.Engage.Web.Cockpit.Segments;

namespace N3O.Umbraco.Cloud.Platforms.Marketing;

public class TelethonOnAirCockpitSegmentRuleFactory : ICockpitSegmentRuleFactory {
    public bool TryCreate(ISegmentRule segmentRule, bool isSatisfied, out CockpitSegmentRule cockpitSegmentRule) {
        cockpitSegmentRule = null;

        if (segmentRule is TelethonOnAirSegmentRule telethonOnAirRule) {
            cockpitSegmentRule = new CockpitSegmentRule {
                Name = "Telethon On Air",
                Icon = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAB2wAAAdsBV+WHHwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANISURBVHic7Zu/axRBFMc/74gggQhRTIgRlEBIUiUopgmYRkzaIEELsVSUgJV1/oWkEi1FBAuRlFpeZ9BoOkVIGk+JhxEs5JAkz+L2YDO7e7nZu5vZ3N4XHty9me+8Hzf7dmZ2D1UljQALwPtAFtKO49u+BINZQUROAt+B/kD1GzinqhXrwVKglfYLKX2YCRkn+DyTciyv9tMm4ESDunahZfbTJqBjkPsE9CQ1iMhZ4ExC8/k4nYiMt8Sro2Fr/5eqlmNbjFvLBPAC2Aa0w2Q7iG3iUMxB4AI8AioZcLTdUglilXACljLgmGtZUlUEGAU+Ab3kC3+BqR7gAdHg/wDPga8J5HHgnqF7AnxupYd1YGt/FLgNnArpeqnGTpHDU+MfMHzEOnye6JSad7gPsLYPDAexhTnFAjBlZGtNVUuN/hTHBUFMa4Z6qgD0GcqOCz4EM7a+3K8E0yZgE9gLfd8LdK7QMvupEqCqP4DVwPAesBronKDV9s1qumJRjYeAIdenQWntAytmvImboYYy5/BXb5f9bhH07YBvxF0C/Q739a7RbyqEajHILXJ/CXQT4NsB34grgkXgtWtHHGEBuBpWxCXgo6quuPHHLUTkIkYCcn8JdBPg2wHfsE6AiCyLSElE1JCSiCxnnR+HhrfDwGxMf1NmM8yPbIdtZ8ClJvv45sfCZgYMUv/xWQUYzDC/uQMRVd0RkQvAHarn7GGUgGequpNVfuK4NDgDjrvQghrQcegmwLcDvmF9KiwiN4G7xBehp6r6Msv8ONjcBidj+psymWF+00XwepN9fPMjsE3AK+CgTvtB0Cer/AhsF0JbIjIN3Cf6qto34LGqbmWVnziuId2FUJ7QTYBvB3wjzULoMvWL0Ics8+NgsxAaAfZjODXZB0YyzG+6CN6g/mVTCPpklR9LsMHbJvv45kdguxDaFJFb1N+MJL6t5ZufOC4N1oDjLnQXQlEUgF1DN+bDEUcwY9stABuGck5ErjhyyBmCmOYM9UYPsA5cC/cF3onIG+CLI//ajTGqwYuhXwcYAMocfdLSaVIGBmrVcTEDDrmWRVUlfItYzMlMKNeCVzX+PS4iA8BDYJrqQ8bTdAZ2qRb7dapvlv+sNfwHCNoge2U5h7MAAAAASUVORK5CYII=",
                Config = telethonOnAirRule.Config,
                IsNegation = segmentRule.IsNegation,
                IsSatisfied = isSatisfied,
                Type = segmentRule.Type,
            };

            return true;
        } else {
            return false;
        }
    }
}