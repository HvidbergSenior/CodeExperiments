    using Insight.BuildingBlocks.Application;
    using Insight.BuildingBlocks.Application.Commands;
    using Insight.BuildingBlocks.Authorization;
    using Insight.BuildingBlocks.Exceptions;
    using Insight.BuildingBlocks.Infrastructure;
    using Insight.IncomingDeclarations.Domain.Incoming;
    using Insight.IncomingDeclarations.Domain.Parsing;
    using Marten;
    using Marten.Exceptions;
    using Microsoft.AspNetCore.Http;

    namespace Insight.IncomingDeclarations.Application.UploadIncomingDeclaration
    {
        public sealed class UploadIncomingDeclarationCommand : ICommand<UploadIncomingDeclarationCommandResponse>
        {
            public IncomingDeclarationSupplier IncomingDeclarationSupplier { get; private set; }
            public IFormFile ExcelFile { get; private set; }

            private UploadIncomingDeclarationCommand(IFormFile excelFile, IncomingDeclarationSupplier incomingDeclarationSupplier)
            {
                IncomingDeclarationSupplier = incomingDeclarationSupplier;
                ExcelFile = excelFile;
            }

            public static UploadIncomingDeclarationCommand Create(IFormFile excelFile, IncomingDeclarationSupplier incomingDeclarationSupplier)
            {
                return new UploadIncomingDeclarationCommand(excelFile, incomingDeclarationSupplier);
            }
        }

        internal class UploadIncomingDeclarationCommandHandler : ICommandHandler<UploadIncomingDeclarationCommand, UploadIncomingDeclarationCommandResponse>
        {
            private readonly IIncomingDeclarationRepository incomingDeclarationRepository;
            private readonly IUnitOfWork unitOfWork;
            private readonly IEnumerable<IIncomingDeclarationParser> incomingDeclarationParsers;

            public UploadIncomingDeclarationCommandHandler(IIncomingDeclarationRepository incomingDeclarationRepository, IUnitOfWork unitOfWork, IEnumerable<IIncomingDeclarationParser> incomingDeclarationParsers)
            {
                this.incomingDeclarationRepository = incomingDeclarationRepository;
                this.unitOfWork = unitOfWork;
                this.incomingDeclarationParsers = incomingDeclarationParsers;
            }
            public async Task<UploadIncomingDeclarationCommandResponse> Handle(UploadIncomingDeclarationCommand request, CancellationToken cancellationToken)
            {
                var parser = incomingDeclarationParsers.FirstOrDefault(x => x.CanParseDocument(request.IncomingDeclarationSupplier));
                
                if (parser == null)
                    throw new InvalidOperationException($"No parser found for supplier {request.IncomingDeclarationSupplier}");

                var (errors, declarations) = await parser.ParseDeclarationDocumentAsync(request.ExcelFile.OpenReadStream(), cancellationToken);

                if (declarations.IsEmpty() && errors.IsEmpty() )
                {
                    throw new BusinessException("No declarations in document");
                }
                
                var incomingDeclarationUploadId = IncomingDeclarationUploadId.Create(Guid.NewGuid());

                var response = new List<IncomingDeclarationParseResponse>();

                foreach (var error in errors)
                {
                    response.Add(new IncomingDeclarationParseResponse(error.Row, error.PosNumber, error.ErrorMessage, false));
                }

                foreach (var declaration in declarations)
                {
                    declaration.SetIncomingDeclarationUploadId(incomingDeclarationUploadId);
                    await incomingDeclarationRepository.Add(declaration, cancellationToken);
                    response.Add(new IncomingDeclarationParseResponse(declaration.DeclarationRowNumber.Value, declaration.PosNumber.Value, string.Empty, true));
                }
                                                                                           
                await incomingDeclarationRepository.SaveChanges(cancellationToken);

                try
                {
                    await unitOfWork.Commit(cancellationToken);
                }
                catch (DocumentAlreadyExistsException ex)
                {
                    var groupings = declarations.GroupBy(c => c.ItemHash).Where(c => c.Count() > 1);
                    var faultingRow = declarations.FirstOrDefault(x => x.Id == (Guid)ex.Id);
                    ArgumentNullException.ThrowIfNull(faultingRow);
                    var idsFoundInCollisionsFromCurrentFile = groupings.SelectMany(c => c.Select(o => o.Id));
                    if (!idsFoundInCollisionsFromCurrentFile.Contains(faultingRow.Id))
                    {
                        // Something found in the database
                        response.Add(new IncomingDeclarationParseResponse(faultingRow.DeclarationRowNumber.Value,
                            faultingRow.PosNumber.Value, "Rækken er allerede indlæst!", false));
                    }
                    else
                    {
                        foreach (var grouping in groupings)
                        {
                            foreach (var declaration in grouping)
                            {
                                response.Add(new IncomingDeclarationParseResponse(declaration.DeclarationRowNumber.Value,
                                    declaration.PosNumber.Value, "Der er dubletter i filen", false));
                            }
                        }
                    }
                }

                var oldest = declarations.OrderBy(c => c.DateOfDispatch.Value).FirstOrDefault()?.DateOfDispatch.Value ?? DateOnly.MinValue;
                var newest = declarations.OrderByDescending(c => c.DateOfDispatch.Value).FirstOrDefault()?.DateOfDispatch.Value ?? DateOnly.MaxValue;

                var incomingDeclarationUploadIdResponse =  MapIncomingDeclarationUploadId(incomingDeclarationUploadId);
                return new UploadIncomingDeclarationCommandResponse(incomingDeclarationUploadIdResponse, response, oldest, newest);
            }

            private IncomingDeclarationUploadIdResponse MapIncomingDeclarationUploadId(IncomingDeclarationUploadId incomingDeclarationUploadId)
            {
                return new IncomingDeclarationUploadIdResponse(incomingDeclarationUploadId.Value);
            }
        }
        internal class UploadIncomingDeclarationCommandAuthorizer : IAuthorizer<UploadIncomingDeclarationCommand>
        {
            private readonly IExecutionContext executionContext;

            public UploadIncomingDeclarationCommandAuthorizer(IExecutionContext executionContext)
            {
                this.executionContext = executionContext;
            }

            public async Task<AuthorizationResult> Authorize(UploadIncomingDeclarationCommand query,
                CancellationToken cancellation)
            {
                if (await executionContext.GetAdminPrivileges(cancellation))
                {
                    return AuthorizationResult.Succeed();
                }
                return AuthorizationResult.Fail();
            }
        }
    }
