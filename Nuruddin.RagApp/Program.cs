using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Nuruddin.RagApp;
using Qdrant.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var ollamaEndpoint = new Uri("http://localhost:11434");
        var qdrantEndpoint = new Uri("http://localhost:6334");

        const string chatModelId = "qwen3:4b";
        const string embeddingModelId = "nomic-embed-text:latest";
        const string collectionName = "products";

        IChatClient chatClient = new OllamaChatClient(ollamaEndpoint, chatModelId);

        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
            new OllamaEmbeddingGenerator(ollamaEndpoint, embeddingModelId);

        var qdrantClient = new QdrantClient(qdrantEndpoint);

        var vectorStore = new QdrantVectorStore(
            qdrantClient,
            ownsClient: false,
            new QdrantVectorStoreOptions
            {
                EmbeddingGenerator = embeddingGenerator
            });

        Console.WriteLine("Ollama + Qdrant initialized successfully.");

        var products = vectorStore.GetCollection<Guid, Product>(collectionName);

        await products.EnsureCollectionExistsAsync();

        Console.WriteLine($"Collection '{collectionName}' is ready.");

        var productData = ProductDatabase.GetProducts();

        foreach (var product in productData)
        {
            product.DescriptionEmbedding =
                await embeddingGenerator.GenerateVectorAsync(
                    product.Description ?? string.Empty);

            await products.UpsertAsync(product);

            Console.WriteLine($"Inserted: {product.Name}");
        }

        Console.WriteLine("All products inserted successfully.");
    }
}