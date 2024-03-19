using Insight.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.BuildingBlocks.Tests.Infrastructure
{
    public sealed class CarId : ValueObject
    {
        public Guid Value { get; }

        private CarId(Guid value)
        {
            Value = value;
        }

        public static CarId Create(Guid value)
        {
            return new CarId(value);
        }
    }

    public sealed class Car : Entity
    {
        public CarId CarId { get; }
        public Owner Owner { get; }

        private Car()
        {
            CarId = CarId.Create(Guid.Empty);
            Owner = Owner.Create();
        }

        private Car(CarId carId, Owner owner)
        {
            CarId = carId;
            Id = CarId.Value;
            Owner = owner;
        }

        public static Car Create()
        {
            var owner = Owner.Create();
            return new Car(CarId.Create(Guid.NewGuid()), owner);
        }
    }

    public sealed class OwnerId : ValueObject
    {
        public Guid Value { get; }

        private OwnerId(Guid value)
        {
            Value = value;
        }

        public static OwnerId Create(Guid value)
        {
            return new OwnerId(value);
        }
    }

    public sealed class Owner : Entity
    {
        public OwnerId OwnerId { get; }

        private Owner(OwnerId ownerId)
        {
            OwnerId = ownerId;
            Id = ownerId.Value;
        }

        public static Owner Create()
        {
            return new Owner(OwnerId.Create(Guid.NewGuid()));
        }
    }
}



