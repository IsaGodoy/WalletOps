using WalletOps.Application.DTOs;
using WalletOps.Application.Interfaces;
using WalletOps.Domain.Entities;

namespace WalletOps.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null)
            {
                throw new InvalidOperationException("Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                throw new InvalidOperationException("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.DocumentNumber))
            {
                throw new InvalidOperationException("Document number is required.");
            }

            bool documentExists = await _customerRepository.ExistsByDocumentNumberAsync(request.DocumentNumber, cancellationToken);

            if (documentExists)
                throw new InvalidOperationException("A customer with the same document number already exists.");

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                DocumentNumber = request.DocumentNumber,
                UserId = request.UserId
            };

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }

        public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _customerRepository.GetAllAsync(cancellationToken);
        }
    }
}
