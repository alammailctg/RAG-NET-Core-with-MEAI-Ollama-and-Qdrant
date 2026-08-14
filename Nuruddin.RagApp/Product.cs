using Microsoft.Extensions.VectorData;

namespace Nuruddin.RagApp
{
    public class Product
    {
        [VectorStoreKey]
        public Guid Id { get; set; }

        [VectorStoreData]
        public string? Name { get; set; }

        [VectorStoreData]
        public string? Description { get; set; }

        [VectorStoreVector(768, DistanceFunction = DistanceFunction.CosineSimilarity)]
        public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }
    }
}