using EZBooking.Application.Exceptions;
using EZBooking.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Infrastructure.Database
{
    internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
    {
        private readonly IPublisher m_publisher;
        public ApplicationDbContext(DbContextOptions options, IPublisher publisher)
            : base(options)
        {
            m_publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                await PublishDomainEventsAsync();
                return result;
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyException("Concurrency exception occured", ex);
            }
        }

        private async Task PublishDomainEventsAsync()
        {
            var domainEvents = ChangeTracker
                .Entries<Entity>()
                .Select(entry => entry.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await m_publisher.Publish(domainEvent);
            }
        }
    }
}
