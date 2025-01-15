using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace N3O.Umbraco.Crowdfunding.Attributes;

public class CrowdfundingApiHeaderOperationProcessor : IOperationProcessor {
    public bool Process(OperationProcessorContext context) {
        context.OperationDescription.Operation.Parameters.Add(new OpenApiParameter {
            Name = CrowdfundingConstants.Http.Headers.ApiHeaderKey,
            Kind = OpenApiParameterKind.Header,
            Type = NJsonSchema.JsonObjectType.String,
            IsRequired = false
        });

        return true;
    }
}