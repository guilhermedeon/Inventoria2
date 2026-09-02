using System;
using System.Collections.Generic;
using System.Text;
using SharedKernel;

namespace Domain.Common.DomainEvents;

public record EntityCreatedDomainEvent<T>(Guid Id) : IDomainEvent where T : Entity;

public record EntityDeletedDomainEvent<T>(Guid Id) : IDomainEvent where T : Entity;

public record EntityUpdatedDomainEvent<T>(Guid Id) : IDomainEvent where T : Entity;
