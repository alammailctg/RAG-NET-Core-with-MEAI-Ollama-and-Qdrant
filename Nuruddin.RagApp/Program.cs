using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Nuruddin.RagApp;
using OllamaSharp;
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

        IChatClient chatClient = new OllamaApiClient(ollamaEndpoint, chatModelId);

        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = new OllamaApiClient(ollamaEndpoint, embeddingModelId);

        var qdrantClient = new QdrantClient(qdrantEndpoint);

        var vectorStore = new QdrantVectorStore(qdrantClient, ownsClient: false, new QdrantVectorStoreOptions
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
            product.DescriptionEmbedding = await embeddingGenerator.GenerateVectorAsync(product.Description ?? string.Empty);

            await products.UpsertAsync(product);

            Console.WriteLine($"Inserted: {product.Name}");
        }

        Console.WriteLine("All products inserted successfully.");

        var systemMessage = new ChatMessage(ChatRole.System, "You are a helpful assistant.");

        while (true)
        {
            Console.Write("\nQuery: ");
            var query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
                continue;

            if (query.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            // Generate embedding for user query
            ReadOnlyMemory<float> queryEmbedding = await embeddingGenerator.GenerateVectorAsync(query);

            // Vector search
            var results = products.SearchAsync(
                queryEmbedding,
                top: 10,
                new VectorSearchOptions<Product>
                {
                    VectorProperty = product => product.DescriptionEmbedding
                });

            var searchedResult = new HashSet<string>();
            var references = new HashSet<string>();

            // Process search results
            await foreach (var result in results)
            {
                searchedResult.Add($"{result.Record.Name}: {result.Record.Description}");

                var score = result.Score ?? 0;
                var percent = (score * 100).ToString("F2");

                references.Add($"[{percent}%] {result.Record.Name}");
            }

            // Build context
            var context = string.Join(Environment.NewLine, searchedResult);

            var prompt = $"""
                Context:
                {context}

                Based on the context above, please answer the following question.
                If the context doesn't provide the answer, say you don't know based on the provided information.

                User question: {query}

                Answer:
                """;

            var userMessage = new ChatMessage(ChatRole.User, prompt);

            // Stream LLM response
            var response = chatClient.GetStreamingResponseAsync(
                [systemMessage, userMessage]);

            await foreach (var r in response)
            {
                Console.Write(r.Text);
            }

            Console.WriteLine("\n");

            // References
            if (references.Count > 0)
            {
                Console.WriteLine("References:");

                foreach (var reference in references)
                {
                    Console.WriteLine(reference);
                }
            }
        }
    }
}