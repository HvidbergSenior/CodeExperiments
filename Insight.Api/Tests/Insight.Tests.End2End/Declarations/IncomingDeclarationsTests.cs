using System.Net;
using FluentAssertions;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Domain.Parsing;
using Insight.Tests.End2End.TestData;
using Insight.UserAccess.Domain.User;
using Xunit;

namespace Insight.Tests.End2End.Declarations
{
    [Collection("End2End")]
    public class IncomingDeclarationsTests
    {
        private readonly WebAppFixture webAppFixture;
        private readonly Api api;

        public IncomingDeclarationsTests(WebAppFixture webAppFixture)
        {
            this.webAppFixture = webAppFixture;
            api = new Api(webAppFixture);
        }

        [Fact]
        public async Task UserCanNotGetDeclarations_WithoutValidToken()
        {
            var filteringParameters = Any.IncomingFilteringParameters();

            var errorResult =
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    api.GetIncomingDeclarationsByPageAndPageSize(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, filteringParameters.Company.Value, filteringParameters.Supplier.Value, 0, 0, true, "Company"));

            errorResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task UserCanGetDeclarations_WithValidToken()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var filteringParameters = Any.IncomingFilteringParameters();
            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(1,filteringParameters, webAppFixture);

            //Act
            var declarations = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, filteringParameters.Company.Value, filteringParameters.Supplier.Value, 1, 1, true, "Company");

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();

            //Assert
            declarations.IncomingDeclarationsByPageAndPageSize.Should().NotBeNull();
        }

        [Fact]
        public async Task UserWhoIsNotAdminCanNotGetDeclarations_WithValidNoAdminToken()
        {
            //Arrange
            var tokenPreMadeUser =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(tokenPreMadeUser.AccessToken);
            
            var (userName, password) = await api.RegisterUser(role: UserRole.User);
            var token = await api.LoginUser(userName, password);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            
            var filteringParameters = Any.IncomingFilteringParameters();
            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(1, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);

            var errorResult =
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    api.GetIncomingDeclarationsByPageAndPageSize(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, filteringParameters.Company.Value, filteringParameters.Supplier.Value, 0, 0, true, "Company"));

            errorResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Removes the token again, due to shared client.
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanGetIncomingDeclarationsByPageAndPageSize()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            var filteringParameters = Any.IncomingFilteringParameters();
            const int seededDeclarationsToFind = 10;
            const int seededDeclarationsToNotFind = 10;

            await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParameters, webAppFixture, IncomingDeclarationState.New, false);
            await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture, IncomingDeclarationState.New,false);

            //Act
            var declarationsByPageAndPageSize = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParameters.DatePeriod.StartDate, filteringParameters.DatePeriod.EndDate, filteringParameters.Product.Value, filteringParameters.Company.Value, filteringParameters.Supplier.Value, 1, 1, true, "Company");

            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSize.TotalAmountOfDeclarations.Should().Be(seededDeclarationsToFind);
        }
        [Fact]
        public async Task UserGetsExceptionWhenUpdatingNotExistingIncomingDeclarationId()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var declarationToUpdate = Any.IncomingDeclaration();
            var updateParameters = Any.IncomingDeclarationUpdateParameters();
            
            //Act and Assert
            await api.UpdateIncomingDeclaration(declarationToUpdate.IncomingDeclarationId.Value, updateParameters, HttpStatusCode.NotFound);
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanUpdateIncomingDeclaration()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(1, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var seededDeclaration = seededDeclarations.Single();

            var updateParameters = Any.IncomingDeclarationUpdateParameters();

            //Act and Assert
            await api.UpdateIncomingDeclaration(seededDeclaration.IncomingDeclarationId.Value, updateParameters);
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanApproveIncomingDeclarationUploads()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            //Act and Assert
            await api.ApproveIncomingDeclarationUpload(Guid.NewGuid());
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanReconcileIncomingDeclarations()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            //Act and Assert
            await api.ReconcileIncomingDeclarations(new Guid[] { Guid.NewGuid() });
            api.Client.RemoveToken();
        }

        [Fact]
        public async Task UserCanUploadIncomingDeclaration()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            byte[] fileContent;

            using (var fs = new FileStream("Resources/PoS.xlsx", FileMode.Open))
            {
                fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, (int)fs.Length);
            }

            //Act
            var uploadIncomingDeclarationCommandResponse =
                await api.UploadIncomingDeclaration(fileContent, IncomingDeclarationSupplier.BFE);
            api.Client.RemoveToken();

            //Assert
            uploadIncomingDeclarationCommandResponse.IncomingDeclarationParseResponses.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task UserGetsErrorWhenUploadingIncomingDeclarationWithError()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            byte[] fileContent;

            using (var fs = new FileStream("Resources/PoSNewWithError.xlsx", FileMode.Open))
            {
                fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, (int)fs.Length);
            }

            //Act
            var uploadIncomingDeclarationCommandResponse =
                await api.UploadIncomingDeclaration(fileContent, IncomingDeclarationSupplier.BFE);
            api.Client.RemoveToken();

            //Assert
            uploadIncomingDeclarationCommandResponse.IncomingDeclarationParseResponses.Count().Should().BePositive();
        }
        
        [Fact]
        public async Task UserGetsErrorWhenUploadingOldIncomingDeclarationVersion()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            byte[] fileContent;

            using (var fs = new FileStream("Resources/PoSOld.xlsx", FileMode.Open))
            {
                fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, (int)fs.Length);
            }

            //Act
            var uploadIncomingDeclarationCommandResponse =
                await api.UploadIncomingDeclaration(fileContent, IncomingDeclarationSupplier.BFE);
            api.Client.RemoveToken();
            
            //Assert
            uploadIncomingDeclarationCommandResponse.IncomingDeclarationParseResponses.Should().BeNull();
         
        }
        
        [Fact]
        public async Task UserCanGetIncomingDeclarationsReconciled()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            var supplier = Supplier.Create("GreatSupplier");
            var product = Product.Create("GreatProduct");
            var company = Company.Create("GreatCompany");
            var dateOfDispatch = DateOfDispatch.Create(new DateOnly(2020, 06, 14));
            var datePeriod = DatePeriod.Create(dateOfDispatch.Value, DateOnly.MaxValue);

            var filteringParametersForSeededDeclarations = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(datePeriod, product, company, supplier);
            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;
           
            var seededDeclarationsWithFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, IncomingDeclarationState.Reconciled, false);
            var seededDeclarationsWithoutFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(),  webAppFixture, IncomingDeclarationState.New,false);
            var seededDeclarationsWithoutReconciledState = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, IncomingDeclarationState.New, false);

            //Act
            var declarationsByPageAndPageSize = await api.GetIncomingDeclarationsReconciled(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, filteringParametersForSeededDeclarations.Product.Value, filteringParametersForSeededDeclarations.Company.Value, filteringParametersForSeededDeclarations.Supplier.Value, 1, seededDeclarationsToFind, true, "Company");

            api.Client.RemoveToken();
            
            //Assert
            declarationsByPageAndPageSize.IncomingDeclarationsByPageAndPageSize.Should().NotBeEmpty();
            declarationsByPageAndPageSize.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(seededDeclarationsToFind);
        }
        
        [Fact]
        public async Task UserCanGetIncomingDeclarationById()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(1, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(), webAppFixture);
            var seededDeclaration = seededDeclarations.First();
            
            var incomingDeclarationId = seededDeclaration.IncomingDeclarationId;
            //Act
            var incomingDeclarationResponse = await api.GetIncomingDeclarationById(incomingDeclarationId);

            api.Client.RemoveToken();

            //Assert
            incomingDeclarationResponse.Should().NotBeNull();
            incomingDeclarationResponse.Company.Should().Be(seededDeclaration.Company.Value);
        }
        
        [Fact]
        public async Task UserCanCancelIncomingDeclarationUploadsWithStateTemporary()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            var seededDeclarations = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(5, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(),  webAppFixture);
            
            //Act and Assert
            await api.CancelIncomingDeclarationsWithUploadId(Guid.NewGuid());
            
            api.Client.RemoveToken();
        }
        
        [Fact]
        public async Task UserCanGetIncomingDeclarationsByPageAndPageSizeWithFiltering()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var supplier = Supplier.Create("GreatSupplier");
            var product = Product.Create("GreatProduct");
            var company = Company.Create("GreatCompany");
            var dateOfDispatch = DateOfDispatch.Create(new DateOnly(2020, 06, 14));
            var datePeriod = DatePeriod.Create(dateOfDispatch.Value, DateOnly.MaxValue);

            var filteringParametersForSeededDeclarations = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(datePeriod, product, company, supplier);
            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;
           
            var seededDeclarationsWithFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, IncomingDeclarationState.New, false);
            var seededDeclarationsWithoutFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(),  webAppFixture, IncomingDeclarationState.New,false);

            //Act

            var declarationsByPageAndPageSizeFiltering = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, filteringParametersForSeededDeclarations.Product.Value, filteringParametersForSeededDeclarations.Company.Value, filteringParametersForSeededDeclarations.Supplier.Value, 1, seededDeclarationsToFind, true, "DateOfIssuance");

            api.Client.RemoveToken();

            //Assert
            declarationsByPageAndPageSizeFiltering.IncomingDeclarationsByPageAndPageSize.Should().NotBeNull();
            declarationsByPageAndPageSizeFiltering.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(seededDeclarationsToFind);

        }
        [Fact]
        public async Task UserCanGetIncomingDeclarationsByPageAndPageSizeWithFilteringDateOfIssuance()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);
            
            var dateOfIssuanceForDeclarationsToFind = DateOfIssuance.Create(new DateOnly(2020, 06, 14));
            var dateOfIssuanceForDeclarationsNotToFind = DateOfIssuance.Create(new DateOnly(2010, 06, 14));
        
            var datePeriodDateOfIssuanceForDeclarationsToFind = DatePeriod.Create(dateOfIssuanceForDeclarationsToFind.Value, DateOnly.MaxValue);
            var datePeriodDateOfIssuanceForDeclarationsNotToFind = DatePeriod.Create(dateOfIssuanceForDeclarationsNotToFind.Value, DateOnly.MaxValue);
        
            var filteringParametersForSeededDeclarationsToFind = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(datePeriodDateOfIssuanceForDeclarationsToFind, Product.Empty(), Company.Empty(), Supplier.Empty());
            var filteringParametersForSeededDeclarationsNotToFind = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(datePeriodDateOfIssuanceForDeclarationsNotToFind, Product.Empty(), Company.Empty(), Supplier.Empty());
        
            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;
           
            var seededDeclarationsWithFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarationsToFind, webAppFixture, IncomingDeclarationState.New,false);
            var seededDeclarationsWithoutFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, filteringParametersForSeededDeclarationsNotToFind,  webAppFixture, IncomingDeclarationState.New, false);
        
            //Act
            var filterDates = DatePeriod.Create(new DateOnly(2019, 01, 01), new DateOnly(2021, 01, 01));
            var filteringParametersDatePeriod = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(filterDates, Product.Empty(), Company.Empty(), Supplier.Empty());
        
            var declarationsByPageAndPageSizeDatePeriod = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParametersDatePeriod.DatePeriod.StartDate, filteringParametersDatePeriod.DatePeriod.EndDate, filteringParametersDatePeriod.Product.Value, filteringParametersDatePeriod.Company.Value, filteringParametersDatePeriod.Supplier.Value, 1, seededDeclarationsToFind, true, "DateOfIssuance");
        
            api.Client.RemoveToken();
        
            //Assert
            declarationsByPageAndPageSizeDatePeriod.IncomingDeclarationsByPageAndPageSize.Should().NotBeNull();
            declarationsByPageAndPageSizeDatePeriod.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(seededDeclarationsToFind);
            
        }
        [Fact]
        public async Task UserCanGetIncomingDeclarationsByPageAndPageSizeWithFilteringProduct()
        {
            //Arrange
            var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
            // Set the token on the client. (Remember to remove it again!)
            api.Client.SetToken(token.AccessToken);
            await api.RegisterUser(role: UserRole.Admin);

            var product = Product.Create("GreatProduct");
        
            var filteringParametersForSeededDeclarations = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(DatePeriod.Empty(), product, Company.Empty(), Supplier.Empty());
            const int seededDeclarationsToFind = 3;
            const int seededDeclarationsToNotFind = 1;
           
            var seededDeclarationsWithFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, IncomingDeclarationState.New, false);
            var seededDeclarationsWithoutFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(),  webAppFixture, IncomingDeclarationState.New, false);
        
            //Act
            var declarationsByPageAndPageSizeProduct = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, "reatp", filteringParametersForSeededDeclarations.Company.Value, filteringParametersForSeededDeclarations.Supplier.Value, 1, seededDeclarationsToFind, true, "Product");
        
            api.Client.RemoveToken();
        
            //Assert
            declarationsByPageAndPageSizeProduct.IncomingDeclarationsByPageAndPageSize.Should().NotBeNull();
            declarationsByPageAndPageSizeProduct.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(seededDeclarationsToFind);
        
        }
        [Fact]
        
        public async Task UserCanGetIncomingDeclarationsByPageAndPageSizeWithFilteringCompany()
         {
             //Arrange
             var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
             // Set the token on the client. (Remember to remove it again!)
             api.Client.SetToken(token.AccessToken);
             await api.RegisterUser(role: UserRole.Admin);
             
             var company = Company.Create("GreatCompany");
        
             var filteringParametersForSeededDeclarations = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), company, Supplier.Empty());
             const int seededDeclarationsToFind = 3;
             const int seededDeclarationsToNotFind = 1;
            
             var seededDeclarationsWithFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, IncomingDeclarationState.New, false);
             var seededDeclarationsWithoutFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(),  webAppFixture, IncomingDeclarationState.New, false);
        
             //Act
             var declarationsByPageAndPageSizeCompany = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, filteringParametersForSeededDeclarations.Product.Value, "reatco", filteringParametersForSeededDeclarations.Supplier.Value, 1, seededDeclarationsToFind, true, "Company");
        
             api.Client.RemoveToken();
        
             //Assert
             declarationsByPageAndPageSizeCompany.IncomingDeclarationsByPageAndPageSize.Should().NotBeNull();
             declarationsByPageAndPageSizeCompany.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(seededDeclarationsToFind);
         }
        [Fact]
        
        public async Task UserCanGetIncomingDeclarationsByPageAndPageSizeWithFilteringSupplier()
         {
             //Arrange
             var token =  await api.LoginUser(Logins.AdminLogin, Logins.LoginPassword);
             // Set the token on the client. (Remember to remove it again!)
             api.Client.SetToken(token.AccessToken);
             await api.RegisterUser(role: UserRole.Admin);

             var supplier = Supplier.Create("GreatSupplier");
        
             var filteringParametersForSeededDeclarations = Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Create(DatePeriod.Empty(), Product.Empty(), Company.Empty(), supplier);
             const int seededDeclarationsToFind = 3;
             const int seededDeclarationsToNotFind = 1;
            
             var seededDeclarationsWithFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToFind, filteringParametersForSeededDeclarations, webAppFixture, IncomingDeclarationState.New, false);
             var seededDeclarationsWithoutFilteringProperties = await IncomingDeclarationsTestData.SeedWithIncomingDeclaration(seededDeclarationsToNotFind, Insight.IncomingDeclarations.Domain.Incoming.FilteringParameters.Empty(),  webAppFixture, IncomingDeclarationState.New, false);
        
             //Act
             var declarationsByPageAndPageSizeSupplier = await api.GetIncomingDeclarationsByPageAndPageSize(filteringParametersForSeededDeclarations.DatePeriod.StartDate, filteringParametersForSeededDeclarations.DatePeriod.EndDate, filteringParametersForSeededDeclarations.Product.Value, filteringParametersForSeededDeclarations.Company.Value, "reatsu", 1, seededDeclarationsToFind, true, "Supplier");
        
             api.Client.RemoveToken();
        
             //Assert
             declarationsByPageAndPageSizeSupplier.IncomingDeclarationsByPageAndPageSize.Should().NotBeNull();
             declarationsByPageAndPageSizeSupplier.IncomingDeclarationsByPageAndPageSize.Count.Should().Be(seededDeclarationsToFind);
         }
    }
}