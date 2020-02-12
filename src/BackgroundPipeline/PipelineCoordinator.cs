using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Jpp.BackgroundPipeline.Storage;

namespace Jpp.BackgroundPipeline
{
    /// <summary>
    /// Coordinates pipelines, storing them and providing access methods
    /// </summary>
    public class PipelineCoordinator
    {
        /// <summary>
        /// Delay in milliseconds between polling for work
        /// </summary>
        public int MillisecondsDelay { get; set; }

        public IReadOnlyCollection<IPipelineFactory> Factories
        {
            get { return _factories; }
        }

        private readonly List<IPipelineFactory> _factories;
        private readonly IPipelineStorage _pipelines;

        /// <summary>
        /// Create new pipeline coordinator
        /// </summary>
        /// <param name="storage">Backage storage system for pipeline persistence</param>
        public PipelineCoordinator(IPipelineStorage storage, IServiceProvider provider)
        {
            _pipelines = storage;
            _factories = new List<IPipelineFactory>();

            var factoryTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IPipelineFactory).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (Type factoryType in factoryTypes)
            {
                RegisterFactory((IPipelineFactory)provider.GetService(factoryType));
            }
#if DEBUG
            MillisecondsDelay = 10000;
#else
            MillisecondsDelay = 100;
#endif
        }

        /// <summary>
        /// Get specific instance of pipeline
        /// </summary>
        /// <param name="id">ID of pipeline</param>
        /// <returns>Requested pipeline instance</returns>
        public async Task<Pipeline> GetPipelineAsync(Guid id)
        {
            // TODO: Review unfound id/param validation
            return await _pipelines.GetPipelineAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all pipelines
        /// </summary>
        /// <returns>Enumerable collection of all pipelines</returns>
        public async Task<IEnumerable<Pipeline>> GetAllPipelinesAsync()
        {
            return await _pipelines.GetAllPipelinesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Add a new pipeline to the coordinator
        /// </summary>
        /// <param name="pipeline">Pipeline to add</param>
        /// <returns>Awaitable</returns>
        public async Task QueuePipelineAsync(Pipeline pipeline)
        {
            pipeline.QueuedDate = DateTime.Now;
            pipeline.Status = Status.Queued;
            await _pipelines.SavePipelineAsync(pipeline).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the pipeline next in the queue
        /// </summary>
        /// <returns>The next pipeline</returns>
        public async Task<Pipeline> GetNextQueuedAsync()
        {
            IOrderedEnumerable<Pipeline> queue;
            bool workFound = false;
            do
            {
                IEnumerable<Pipeline> pipelines = await _pipelines.GetAllPipelinesAsync().ConfigureAwait(false);
                // TODO: Review priority rules are appropriate
                queue = pipelines.Where(p => p.Status == Status.Queued).OrderByDescending(p => p.Priority)
                    .ThenByDescending(p => p.RequiredDate).ThenByDescending(p => p.QueuedDate);

                workFound = queue.Any();
                if(!workFound)
                    await Task.Delay(MillisecondsDelay).ConfigureAwait(false);
            } while (!workFound);

            Pipeline nextPipeline = queue.First();
            return nextPipeline;
        }

        private void RegisterFactory(IPipelineFactory factory)
        {
            _factories.Add(factory);
        }
    }
}
