using Microsoft.Extensions.VectorData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nuruddin.RagApp
{
    public class Product
    {
        [VectorStoreKey]
        public int Id { get; set; }

        [VectorStoreData]
        public string? Name { get; set; }

        [VectorStoreData]
        public string? Description { get; set; }

        [VectorStoreVector(Dimensions:768, DistanceFunction =DistanceFunction.CosineSimilarity)]
        public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }
    }
}
