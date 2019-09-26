using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Jpp.BackgroundPipeline
{
    public class DatabasePipelineStorage : IPipelineStorage
    {
        IServiceScopeFactory _factory;
        IServiceScope _scope;
        IPipelineDBContext _context;

        public DatabasePipelineStorage(IServiceScopeFactory factory)
        {
            _factory = factory;
            _scope = _factory.CreateScope();
            _context = _scope.ServiceProvider.GetService<IPipelineDBContext>();
        }

        public async Task<IEnumerable<Pipeline>> GetAllPipelines()
        {

            return _context.Pipelines.Include(pipeline => pipeline.Stages).ToList();
        }

        public async Task<Pipeline> GetPipeline(Guid id)
        {
            return await _context.Pipelines.FindAsync(id);

        }

        public async Task SavePipeline(Pipeline pipeline)
        {
            _context.Pipelines.Add(pipeline);
            _context.SaveChanges();
        }

        ~DatabasePipelineStorage()
        {
            _scope.Dispose();
        }
    }
}
