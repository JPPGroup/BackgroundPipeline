using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Jpp.BackgroundPipeline.Storage
{
    public class DatabasePipelineStorage : IPipelineStorage
    {
        readonly IServiceScopeFactory _factory;
        readonly IServiceScope _scope;
        readonly IPipelineDBContext _context;

        public DatabasePipelineStorage(IServiceScopeFactory factory)
        {
            _factory = factory;
            _scope = _factory.CreateScope();
            _context = _scope.ServiceProvider.GetService<IPipelineDBContext>();
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelinesAsync()
        {

            return _context.Pipelines.Include(pipeline => pipeline.Stages).ToList();
        }

        public async Task<Pipeline> GetPipelineAsync(Guid id)
        {
            return await _context.Pipelines.FindAsync(id);

        }

        public async Task SavePipelineAsync(Pipeline pipeline)
        {
            await Task.Run(() =>
            {
                _context.Pipelines.Add(pipeline);
                _context.SaveChanges();
            }).ConfigureAwait(false);
        }

        ~DatabasePipelineStorage()
        {
            _scope.Dispose();
        }
    }
}
