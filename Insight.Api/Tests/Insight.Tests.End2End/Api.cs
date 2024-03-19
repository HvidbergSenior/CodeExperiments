using AutoFixture;
using Insight.UserAccess.Application;
using Insight.UserAccess.Application.ChangePassword;
using Insight.UserAccess.Application.GetAccessToken;
using Insight.UserAccess.Application.LoginUser;
using Insight.UserAccess.Application.RegisterUser;
using System.Net;
using System.Net.Http.Json;
using Insight.FuelTransactions.Application;
using Insight.FuelTransactions.Application.GetOutgoingFuelTransactions;
using System.Globalization;
using FluentAssertions;
using Insight.BuildingBlocks.Serialization;
using Insight.Customers.Application;
using Insight.Customers.Application.GetAvailableCustomersPermissions;
using Insight.Customers.Application.GetPossibleCustomerPermissions;
using Insight.Customers.Application.GetPossibleCustomerPermissionsForGivenUser;
using Insight.IncomingDeclarations.Application;
using Insight.IncomingDeclarations.Application.ApproveIncomingDeclarationUploads;
using Insight.IncomingDeclarations.Application.CancelIncomingDeclarationsByUploadId;
using Insight.IncomingDeclarations.Application.GetIncomingDeclarationsByPageAndPageSize;
using Insight.IncomingDeclarations.Application.Reconciliation;
using Insight.IncomingDeclarations.Application.UploadIncomingDeclaration;
using Insight.IncomingDeclarations.Domain.Incoming;
using Insight.IncomingDeclarations.Domain.Parsing;
using Insight.OutgoingDeclarations.Application;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationById;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarations;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByCustomerId;
using Insight.OutgoingDeclarations.Application.GetOutgoingDeclarationsByPageAndPageSize;
using Insight.OutgoingDeclarations.Application.GetSustainabilityReportPdf;
using Insight.OutgoingDeclarations.Domain;
using Insight.Services.AllocationEngine.Application;
using Insight.Services.AllocationEngine.Application.GetAllocations;
using Insight.Services.AllocationEngine.Application.GetAllocationSuggestions;
using Insight.Services.AllocationEngine.Application.PostAutomaticAllocation;
using Insight.Services.AllocationEngine.Application.PostManualAllocation;
using IncomingDeclarationId = Insight.IncomingDeclarations.Domain.Incoming.IncomingDeclarationId;
using Insight.FuelTransactions.Application.GetStockTransactions;
using Insight.FuelTransactions.Application.CreateStockTransaction;
using Insight.UserAccess.Application.UpdateUser;
using Insight.UserAccess.Domain.User;
using Insight.OutgoingDeclarations.Application.GetFuelConsumption;
using Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactions;
using Insight.OutgoingDeclarations.Application.GetFuelConsumptionTransactionsExcelFile;
using Insight.OutgoingDeclarations.Application.GetSustainabilityReport;
using Insight.Services.AllocationEngine.Application.GetAllocationById;
using Insight.UserAccess.Application.BlockUser;
using Insight.UserAccess.Application.ForgotPassword;
using Marten;
using Insight.UserAccess.Application.GetAllUsers;
using Insight.UserAccess.Application.ResetPassword;

namespace Insight.Tests.End2End
{
    public class Api
    {
        private readonly WebAppFixture fixture;

        private static readonly Fixture any = new();

        public HttpClient Client { get; private set; }

        public static T Instance<T>()
        {
            return any.Create<T>();
        }

        public WebAppFixture Fixture
        {
            get { return fixture; }
        }

        public Api(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
            Client = fixture.CreateClient;
        }

        public async Task<(string userName, string password)> RegisterUser(
            string userName = "", string? password = "", string firstName = "", string lastName = "",
            UserRole role = UserRole.User)
        {
            return await RegisterUser(new List<RegisterUserCustomerPermissionDto>(), userName, password, firstName, lastName, role);
        }

        public async Task<(string userName, string password)> RegisterUser(IEnumerable<RegisterUserCustomerPermissionDto> customerPermissions, string userName = "", string? password = "", string firstName = "", string lastName = "",
            UserRole role = UserRole.User, UserStatus status = UserStatus.Active)
        {
            if (string.IsNullOrEmpty(userName))
            {
                userName = $"{Instance<string>()}@{Instance<string>()}.com";
            }
            if (string.IsNullOrEmpty(firstName))
            {
                firstName = $"{Instance<string>()}";
            }
            if (string.IsNullOrEmpty(lastName))
            {
                lastName = $"{Instance<string>()}";
            }

            if (password != null && password.IsEmpty())
            {
                password = $"A{Instance<string>()}!";
            }

            var request = new RegisterUserRequest()
            {
                Email = userName,
                Password = password,
                ConfirmPassword = password,
                Username = userName,
                Role = role,
                CustomerPermissions = customerPermissions,
                Status = status,
                FirstName = firstName,
                LastName = lastName
            };
  
            var response = await Client.PostAsJsonAsync(UserEndpointUrls.REGISTER_USER_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            return (userName, password ?? "");
        }

        public async Task<AuthenticatedResponse> LoginUser(string username, string password)
        {
            var request = new LoginRequest()
            {
                Password = password,
                Username = username
            };
            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.LOGIN_USER_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResponse =
                await response.Content.ReadFromJsonAsync<AuthenticatedResponse>(fixture.JsonSerializerOptions());
            authResponse.Should().NotBeNull();
#pragma warning disable CS8603 // Possible null reference return.
            return authResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task ChangePassword(string userName, string oldPassword, string newPassword,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new ChangePasswordRequest()
            {
                CurrentPassword = oldPassword,
                ConfirmPassword = newPassword,
                NewPassword = newPassword,
                UserName = userName
            };

            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.CHANGE_PASSWORD_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task ResetPassword(string userName, string token, string newPassword,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new ResetPasswordRequest()
            {
                UserName = userName,
                Token = token,
                NewPassword = newPassword
            };

            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.RESET_PASSWORD_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task ForgotPassword(string? userName, string? email,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new ForgotPasswordRequest()
            {
                UserName = userName,
                Email = email
            };

            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.FORGOT_PASSWORD_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task BlockUser(string userName,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new BlockUserRequest()
            {
                UserName = userName
            };

            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.BLOCK_USER_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task UnblockUser(string userName,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new BlockUserRequest()
            {
                UserName = userName
            };

            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.UNBLOCK_USER_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task<AuthenticatedResponse> RenewAccessToken(AuthenticatedResponse tokenResponse)
        {
            var request = new AccessTokenRequest()
            {
                RefreshToken = tokenResponse.RefreshToken,
                AccessToken = tokenResponse.AccessToken
            };
            var response = await Client.PostAsJsonAsync(UserAccessEndpointUrls.GET_ACCESS_TOKEN_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResponse =
                await response.Content.ReadFromJsonAsync<AuthenticatedResponse>(fixture.JsonSerializerOptions());
            authResponse.Should().NotBeNull();
#pragma warning disable CS8603 // Possible null reference return.
            return authResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetIncomingDeclarationsByPageAndPageSizeResponse> GetIncomingDeclarationsByPageAndPageSize(
            DateOnly fromDate, DateOnly toDate, string product, string company, string supplier, int page, int pageSize,
            bool sortOrder, string orderByProperty, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);

            var url = IncomingDeclarationsEndpointUrls.GET_INCOMING_DECLARATIONS_BY_PAGE_AND_PAGE_SIZE_ENDPOINT +
                      $"?dateFrom={fromDateConverted}&dateTo={toDateConverted}&product={product}&company={company}&supplier={supplier}&page={page}&pageSize={pageSize}&isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var incomingResponse =
                await response.Content.ReadFromJsonAsync<GetIncomingDeclarationsByPageAndPageSizeResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetAvailableCustomersPermissionsResponse> GetAvailableCustomerPermissions(HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = CustomerEndpointUrls.GET_AVAILABLE_CUSTOMERS_PERMISSIONS;

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var responseParsed =
                await response.Content.ReadFromJsonAsync<GetAvailableCustomersPermissionsResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return responseParsed;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetPossibleCustomerPermissionsResponse> GetPossibleCustomerPermissions(HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = CustomerEndpointUrls.GET_POSSIBLE_CUSTOMER_PERMISSIONS;

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var responseParsed =
                await response.Content.ReadFromJsonAsync<GetPossibleCustomerPermissionsResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return responseParsed;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetPossibleCustomerPermissionsForGivenUserResponse> GetPossibleCustomerPermissionsForGivenUser(string userName, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = CustomerEndpointUrls.GET_POSSIBLE_CUSTOMER_PERMISSIONS_FOR_GIVEN_USER + "?userName=" + userName;

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var responseParsed =
                await response.Content.ReadFromJsonAsync<GetPossibleCustomerPermissionsForGivenUserResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return responseParsed;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetStockTransactionsQueryResponse> GetStocks(
            DateOnly fromDate, DateOnly toDate, string product, string company, int page, int pageSize,
            bool sortOrder, string orderByProperty, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);

            var url = StockTransactionsEndpointUrls.GET_STOCK_TRANSACTIONS_ENDPOINT +
                      $"?dateFrom={fromDateConverted}&dateTo={toDateConverted}&product={product}&company={company}&page={page}&pageSize={pageSize}&isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var incomingResponse =
                await response.Content.ReadFromJsonAsync<GetStockTransactionsQueryResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task PostStock(string productNumber, Guid companyId, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var req = new CreateStockTransactionRequest()
            {
                ProductNumber = productNumber,
                CompanyId = companyId,
                Country = "Country",
                Location = "Location",
                Quantity = 1,
                TransactionDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            var url = StockTransactionsEndpointUrls.CREATE_STOCK_TRANSACTIONS_ENDPOINT;

            var response = await Client.PostAsJsonAsync(url, req);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task<GetOutgoingFuelTransactionsQueryResponse> GetOutgoingFuelTransactions(DateOnly fromDate,
            DateOnly toDate, string product, string company, string customer, int page, int pageSize,
            bool isOrderDescending, string orderByProperty,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);

            var url = OutgoingFuelTransactionsEndpointUrls.GET_OUTGOING_FUEL_TRANSACTIONS_ENDPOINT +
                      $"?dateFrom={fromDateConverted}&dateTo={toDateConverted}&product={product}&company={company}&customer={customer}&page={page}&pageSize={pageSize}&isOrderDescending={isOrderDescending}&orderByProperty={orderByProperty}";

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var incomingResponse =
                await response.Content.ReadFromJsonAsync<GetOutgoingFuelTransactionsQueryResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task ApproveIncomingDeclarationUpload(Guid incomingDeclarationUploadId,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new ApproveIncomingDeclarationUploadRequest()
            {
                IncomingDeclarationUploadId = incomingDeclarationUploadId
            };

            var response =
                await Client.PostAsJsonAsync(
                    IncomingDeclarationsEndpointUrls.APPROVE_INCOMING_DECLARATION_UPLOAD_ENDPOINT, request,
                    options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task UpdateIncomingDeclaration(Guid updateIncomingDeclarationId,
            IncomingDeclarationUpdateParameters incomingDeclarationUpdateParameters,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = CreateUpdateRequest(incomingDeclarationUpdateParameters, updateIncomingDeclarationId);

            var response =
                await Client.PutAsJsonAsync(IncomingDeclarationsEndpointUrls.UPDATE_INCOMING_DECLARATION_ENDPOINT,
                    request);
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task ReconcileIncomingDeclarations(Guid[] incomingDeclarationIds,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new ReconcileIncomingDeclarationsRequest()
            {
                IncomingDeclarationIds = incomingDeclarationIds
            };

            var response =
                await Client.PostAsJsonAsync(IncomingDeclarationsEndpointUrls.RECONCILE_INCOMING_DECLARATION_ENDPOINT,
                    request, options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task<UploadIncomingDeclarationCommandResponse> UploadIncomingDeclaration(byte[] fileContent,
            IncomingDeclarationSupplier incomingDeclarationSupplier, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(incomingDeclarationSupplier.ToString()), "IncomingDeclarationSupplier");

                var fileContentStream = new MemoryStream(fileContent);
                var fileContentStreamContent = new StreamContent(fileContentStream);
                formData.Add(fileContentStreamContent, "ExcelFile", "example.xlsx");
                var response = await Client.PostAsync("api/incomingdeclarations/upload", formData);

                var uploadIncomingDeclarationCommandResponseResponse =
                    await response.Content.ReadFromJsonAsync<UploadIncomingDeclarationCommandResponse>(
                        fixture.JsonSerializerOptions());

#pragma warning disable CS8603 // Possible null reference return.
                return uploadIncomingDeclarationCommandResponseResponse;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public async Task GetFuelConsumptionTransactions(DateOnly fromDate, DateOnly toDate, ProductNameEnumeration[] productNames, Guid[] customerIds,  int page, int pageSize, bool isOrderDescending, string orderByProperty,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var productNamesPrefix = productNames.IsEmpty() ? "" : "&productNames=";
            
            var request = new GetFuelConsumptionTransactionsRequest()
            {
                ProductNames = productNames,
                CustomerNumbers = Array.Empty<string>(),
                CustomerIds = customerIds,
                MaxColumns = 30,
                DateFrom = fromDate,
                DateTo = toDate
            };
            
            var url = OutgoingDeclarationsEndpointUrls.GET_FUELCONSUMPTION_TRANSACTIONS_ENDPOINT +
                      $"?page={page}&pageSize={pageSize}&isOrderDescending={isOrderDescending}&orderByProperty={orderByProperty}{productNamesPrefix}{String.Join("&productNames=", productNames)}";

            var response = await Client.PostAsJsonAsync(url, request, options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning restore CS8603 // Possible null reference return.
            
           
        }
     
        public async Task GetFuelConsumption(DateOnly fromDate, DateOnly toDate, ProductNameEnumeration[] productNames, Guid[] customerIds, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new GetFuelConsumptionRequest()
            {
                ProductNames = productNames,
                CustomerNumbers = Array.Empty<string>(),
                CustomerIds = customerIds,
                MaxColumns = 30,
                DateFrom = fromDate,
                DateTo = toDate
            };
            
            var response = await Client.PostAsJsonAsync(OutgoingDeclarationsEndpointUrls.GET_FUELCONSUMPTION_ENDPOINT, request, options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task GetFuelConsumptionExcelFile(DateOnly fromDate, DateOnly toDate, ProductNameEnumeration[] productNames, Guid[] customerIds, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new GetFuelConsumptionTransactionsExcelFileRequest()
            {
                ProductNames = productNames,
                CustomerNumbers = Array.Empty<string>(),
                CustomerIds = customerIds,
                MaxColumns = 30,
                DateFrom = fromDate,
                DateTo = toDate
            };
            
            var response = await Client.PostAsJsonAsync(OutgoingDeclarationsEndpointUrls.GET_FUELCONSUMPTION_TRANSACTIONS_EXCEL_FILE_ENDPOINT, request, options: fixture.JsonSerializerOptions());

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task<GetIncomingDeclarationsByPageAndPageSizeResponse> GetIncomingDeclarationsReconciled(
            DateOnly fromDate, DateOnly toDate, string product, string company, string supplier, int page, int pageSize,
            bool sortOrder, string orderByProperty, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);

            var url = IncomingDeclarationsEndpointUrls.GET_INCOMING_DECLARATIONS_RECONCILED_ENDPOINT +
                      $"?dateFrom={fromDateConverted}&dateTo={toDateConverted}&product={product}&company={company}&supplier={supplier}&page={page}&pageSize={pageSize}&isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var incomingResponse =
                await response.Content.ReadFromJsonAsync<GetIncomingDeclarationsByPageAndPageSizeResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<IncomingDeclarationDto> GetIncomingDeclarationById(
            IncomingDeclarationId incomingDeclarationId, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = IncomingDeclarationsEndpointUrls.GET_INCOMING_DECLARATION_BY_ID_ENDPOINT;
            url = url.Replace("{id}", incomingDeclarationId.Value.ToString());
            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var incomingResponse =
                await response.Content.ReadFromJsonAsync<IncomingDeclarationDto>(fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetOutgoingDeclarationByIdResponse> GetOutgoingDeclarationById(
            OutgoingDeclarationId outgoingDeclarationId, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATION_BY_ID_ENDPOINT;
            url = url.Replace("{id}", outgoingDeclarationId.Value.ToString());
            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var outgoingDeclarationResponse =
                await response.Content.ReadFromJsonAsync<GetOutgoingDeclarationByIdResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return outgoingDeclarationResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetOutgoingDeclarationsByCustomerIdResponse> GetOutgoingDeclarationsByCustomerId(
            string outgoingDeclarationId, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATIONS_BY_CUSTOMER_ID_ENDPOINT;
            url = url.Replace("{customerId}", outgoingDeclarationId);
            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var outgoingDeclarationResponse =
                await response.Content.ReadFromJsonAsync<GetOutgoingDeclarationsByCustomerIdResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return outgoingDeclarationResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task GetSustainabilityReport(DateOnly fromDate, DateOnly toDate, ProductNameEnumeration[] productNames, Guid[] customerIds, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            //var productNamesPrefix = productNames.IsEmpty() ? "" : "&productNames=";
            //var customerIdsPrefix = customerIds.IsEmpty() ? "" : "&customerIds=";

            var request = new GetSustainabilityReportRequest
            {
                ProductNames = productNames,
                CustomerNumbers = Array.Empty<string>(),
                CustomerIds = customerIds,
                MaxColumns = 30,
                DateFrom = fromDate,
                DateTo = toDate
            };
            
            var response = await Client.PostAsJsonAsync(OutgoingDeclarationsEndpointUrls.GET_SUSTAINABILITY_REPORT_ENDPOINT, request, options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

            #pragma warning disable CS8603 // Possible null reference return.
            #pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task GetSustainabilityReportPdf(DateOnly fromDate, DateOnly toDate, ProductNameEnumeration[] productNames, Guid[] customerIds, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
           var date = new DateOnly(2024, 3, 4);
            var request = new GetSustainabilityReportPdfRequest
            {
                ProductNames = productNames,
                CustomerNumbers = Array.Empty<string>(),
                CustomerIds = customerIds,
                MaxColumns = 30,
                DateFrom = fromDate,
                DateTo = toDate
            };
          
            var response = await Client.PostAsJsonAsync(OutgoingDeclarationsEndpointUrls.GET_SUSTAINABILITY_REPORTPDF_ENDPOINT, request, options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

            #pragma warning disable CS8603 // Possible null reference return.
            #pragma warning restore CS8603 // Possible null reference return.
        }
      
        public async Task CancelIncomingDeclarationsWithUploadId(Guid incomingDeclarationUploadId,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new CancelIncomingDeclarationsByUploadIdRequest()
            {
                IncomingDeclarationUploadId = incomingDeclarationUploadId
            };

            var response = await Client.PostAsJsonAsync(
                IncomingDeclarationsEndpointUrls.CANCEL_INCOMING_DECLARATIONS_BY_UPLOAD_ID_ENDPOINT, request,
                options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task<GetOutgoingDeclarationsResponse> GetOutgoingDeclarations(
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATIONS_ENDPOINT;
            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var outgoingDeclarationResponse =
                await response.Content.ReadFromJsonAsync<GetOutgoingDeclarationsResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return outgoingDeclarationResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<GetOutgoingDeclarationsByPageAndPageSizeResponse> GetOutgoingDeclarationsByPageAndPageSize(
            DateOnly fromDate, DateOnly toDate, string product, string company, string supplier, int page, int pageSize,
            bool sortOrder, string orderByProperty, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);

            var url = OutgoingDeclarationsEndpointUrls.GET_OUTGOING_DECLARATIONS_BY_PAGE_AND_PAGE_SIZE_ENDPOINT +
                      $"?dateFrom={fromDateConverted}&dateTo={toDateConverted}&product={product}&company={company}&supplier={supplier}&page={page}&pageSize={pageSize}&isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var outgoingDeclarationsResponse =
                await response.Content.ReadFromJsonAsync<GetOutgoingDeclarationsByPageAndPageSizeResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);

#pragma warning disable CS8603 // Possible null reference return.
            return outgoingDeclarationsResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }
        
        public async Task<GetAllocationsResponse> GetAllocations(DateOnly fromDate, DateOnly toDate, string product, string company, string customer, int page, int pageSize,
            bool sortOrder, string orderByProperty, 
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT, CultureInfo.InvariantCulture);

            var url = AllocationEngineEndpointUrls.GET_ALLOCATIONS_ENDPOINT + $"?dateFrom={fromDateConverted}&dateTo={toDateConverted}&product={product}&company={company}&customer={customer}&page={page}&pageSize={pageSize}&isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";
            
            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var getAllocationsResponse =
                await response.Content.ReadFromJsonAsync<GetAllocationsResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return getAllocationsResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<GetAllocationByIdResponse> GetAllocationById(Guid allocationId, bool sortOrder, string orderByProperty, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {

            var url = AllocationEngineEndpointUrls.GET_ALLOCATION_BY_ID_ENDPOINT + $"?isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";
            
            url = url.Replace("{id}", allocationId.ToString());
            
            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var getAllocationsResponse =
                await response.Content.ReadFromJsonAsync<GetAllocationByIdResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return getAllocationsResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<GetAllocationSuggestionsResponse> GetAllocationSuggestions(Guid customerId, DateOnly fromDate, DateOnly toDate, string product, string country, string location, int page, int pageSize,
            bool sortOrder, string orderByProperty, 
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var fromDateConverted = fromDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT,
                CultureInfo.InvariantCulture);
            var toDateConverted = toDate.ToString(SystemTextDateOnlyJsonConverter.DATEONLY_DATE_FORMAT,
                CultureInfo.InvariantCulture);

            var url = AllocationEngineEndpointUrls.GET_ALLOCATION_SUGGESTIONS_ENDPOINT + $"?customerId={customerId}&startDate={fromDateConverted}&endDate={toDateConverted}&product={product}&country={country}&location={location}&page={page}&pageSize={pageSize}&isOrderDescending={sortOrder}&orderByProperty={orderByProperty}";

            var response = await Client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var getAllocationsResponse =
                await response.Content.ReadFromJsonAsync<GetAllocationSuggestionsResponse>(
                    fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return getAllocationsResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }
        
        public async Task UnlockAllocations(HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var response = await Client.PostAsync(AllocationEngineEndpointUrls.UNLOCK_ALLOCATIONS_ENDPOINT, null);
            response.StatusCode.Should().Be(expectedResult);
        }
        public async Task LockAllocations(HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var response = await Client.PostAsync(AllocationEngineEndpointUrls.LOCK_ALLOCATIONS_ENDPOINT, null);
            response.StatusCode.Should().Be(expectedResult);
        }
        public async Task PostAllocationsAutomatically(DateOnly startDate, DateOnly endDate, string product, string company, string customer,
            HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = new PostAutomaticAllocationRequest
            {
                StartDate = startDate,
                EndDate = endDate,
                Company = company,
                Customer = customer,
                Product = product
            };

            var response =
                await Client.PostAsJsonAsync(
                    AllocationEngineEndpointUrls.POST_AUTOMATIC_ALLOCATION_ENDPOINT, request,
                    options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }
        
        public async Task PostAllocationsManually(Guid incomingDeclarationId, decimal volume, string country, Guid customerId, DateOnly endDate, string locationId, string productName, string productNumber, DateOnly startDate, string stationName, string companyName, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var allocationRequest = new AllocationRequest()
            {
                IncomingDeclarationId = incomingDeclarationId,
                Volume = volume
            };

            var fuelTransactionsBatchRequest = new FuelTransactionsBatchRequest()
            {
                Country = country,
                CustomerId = customerId,
                EndDate = endDate,
                LocationId = locationId,
                ProductName = productName,
                ProductNumber = productNumber,
                StartDate = startDate,
                StationName = stationName
            };
            var request = new PostManualAllocationRequest
            {
                FuelTransactionsBatch = fuelTransactionsBatchRequest,
                Allocations = new AllocationRequest[] {allocationRequest}
            };

            var response =
                await Client.PostAsJsonAsync(
                    AllocationEngineEndpointUrls.POST_MANUAL_ALLOCATION_ENDPOINT, request,
                    options: fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
        }

        public async Task<GetAllUsersResponse> GetAllUsers(int page, int pageSize, bool isOrderDescending, string orderByProperty, string status, string accountId, string email, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = $"{UserEndpointUrls.GET_ALL_USERS_ENDPOINT}?page={page}&pageSize={pageSize}&isOrderDescending={isOrderDescending}&orderByProperty={orderByProperty}&status={status}&accountId={accountId}&email={email}";            

            var response = await Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var incomingResponse = await response.Content.ReadFromJsonAsync<GetAllUsersResponse>(fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task<GetAllUsersResponse> GetAllUsersAdmin(int page, int pageSize, bool isOrderDescending, string orderByProperty, string status, string accountId, string email, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var url = $"{UserAdministrationEndpointUrls.GET_ALL_USERS_ADMIN_ENDPOINT}?page={page}&pageSize={pageSize}&isOrderDescending={isOrderDescending}&orderByProperty={orderByProperty}&status={status}&accountId={accountId}&email={email}";            
        
            var response = await Client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var incomingResponse = await response.Content.ReadFromJsonAsync<GetAllUsersResponse>(fixture.JsonSerializerOptions());
            response.StatusCode.Should().Be(expectedResult);
#pragma warning disable CS8603 // Possible null reference return.
            return incomingResponse;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task UpdateUser(UserUpdateParameters userUpdateParameters, List<UpdateUserCustomerPermissionDto> customerPermissions, HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var request = CreateUpdateUserRequest(userUpdateParameters, customerPermissions);
            
            var response = await Client.PutAsJsonAsync(UserEndpointUrls.UPDATE_USER_ENDPOINT, request);
            
            response.StatusCode.Should().Be(expectedResult);
        }
        public async Task PublishAllocations(HttpStatusCode expectedResult = HttpStatusCode.OK)
        {
            var response = await Client.PostAsync(AllocationEngineEndpointUrls.PUBLISH_ALLOCATIONS_ENDPOINT, null);
            response.StatusCode.Should().Be(expectedResult);
        }
        private IncomingDeclarationDto CreateUpdateRequest(
            IncomingDeclarationUpdateParameters incomingDeclarationUpdateParameters, Guid updateIncomingDeclarationId)
        {
            var declarationDto = new IncomingDeclarationDto()
            {
                IncomingDeclarationId = updateIncomingDeclarationId,
                Company = incomingDeclarationUpdateParameters.Company.Value,
                Country = incomingDeclarationUpdateParameters.Country.Value,
                Product = incomingDeclarationUpdateParameters.Product.Value,
                Supplier = incomingDeclarationUpdateParameters.Supplier.Value,
                RawMaterial = incomingDeclarationUpdateParameters.RawMaterial.Value,
                PosNumber = incomingDeclarationUpdateParameters.PosNumber.Value,
                CountryOfOrigin = incomingDeclarationUpdateParameters.CountryOfOrigin.Value,
                DateOfDispatch = incomingDeclarationUpdateParameters.DateOfDispatch.Value,
                CertificationSystem = incomingDeclarationUpdateParameters.CountryOfOrigin.Value,
                SupplierCertificateNumber = incomingDeclarationUpdateParameters.CountryOfOrigin.Value,
                DateOfIssuance = incomingDeclarationUpdateParameters.DateOfIssuance.Value,
                PlaceOfDispatch = incomingDeclarationUpdateParameters.PlaceOfDispatch.Value,
                ProductionCountry = incomingDeclarationUpdateParameters.ProductionCountry.Value,
                DateOfInstallation = incomingDeclarationUpdateParameters.DateOfInstallation.Value,
                TypeOfProduct = incomingDeclarationUpdateParameters.TypeOfProduct.Value,
                AdditionalInformation = incomingDeclarationUpdateParameters.AdditionalInformation.Value,
                Quantity = incomingDeclarationUpdateParameters.Quantity.Value,
                SpecifyNUTS2Region = incomingDeclarationUpdateParameters.NUTS2Region.Value,
                EnergyContentMJ = incomingDeclarationUpdateParameters.EnergyContentMJ.Value,
                EnergyQuantityGJ = incomingDeclarationUpdateParameters.EnergyQuantityGJ.Value,
                ComplianceWithSustainabilityCriteria =
                    incomingDeclarationUpdateParameters.ComplianceWithSustainabilityCriteria.Value,
                CultivatedAsIntermediateCrop = incomingDeclarationUpdateParameters.CultivatedAsIntermediateCrop.Value,
                FulfillsMeasuresForLowILUCRiskFeedstocks = incomingDeclarationUpdateParameters
                    .FulfillsMeasuresForLowILUCRiskFeedstocks.Value,
                MeetsDefinitionOfWasteOrResidue =
                    incomingDeclarationUpdateParameters.MeetsDefinitionOfWasteOrResidue.Value,
                GHGEec = incomingDeclarationUpdateParameters.GHGEec.Value,
                GHGEl = incomingDeclarationUpdateParameters.GHGEl.Value,
                GHGEp = incomingDeclarationUpdateParameters.GHGEp.Value,
                GHGEtd = incomingDeclarationUpdateParameters.GHGEtd.Value,
                GHGEu = incomingDeclarationUpdateParameters.GHGEu.Value,
                GHGEsca = incomingDeclarationUpdateParameters.GHGEsca.Value,
                GHGEccs = incomingDeclarationUpdateParameters.GHGEccs.Value,
                GHGEccr = incomingDeclarationUpdateParameters.GHGEccr.Value,
                GHGEee = incomingDeclarationUpdateParameters.GHGEee.Value,
                GHGEmissionSaving = incomingDeclarationUpdateParameters.GHGEmissionSaving.Value,
                DeclarationRowNumber = incomingDeclarationUpdateParameters.DeclarationRowNumber.Value,
                IncomingDeclarationUploadId = incomingDeclarationUpdateParameters.IncomingDeclarationUploadId.Value,
                IncomingDeclarationState = incomingDeclarationUpdateParameters.IncomingDeclarationState,
                TotalDefaultValueAccordingToREDII =
                    incomingDeclarationUpdateParameters.TotalDefaultValueAccordingToRED2.Value,
                GHGgCO2EqPerMJ = incomingDeclarationUpdateParameters.GHGgCO2eqPerMJ.Value,
                FossilFuelComparatorgCO2EqPerMJ =
                    incomingDeclarationUpdateParameters.FossilFuelComparatorgCO2eqPerMJ.Value
            };
            return declarationDto;
        }
        private UpdateUserRequest CreateUpdateUserRequest(UserUpdateParameters userUpdateParameters, IEnumerable<UpdateUserCustomerPermissionDto> customerPermissions)
        {
            var updateUserRequest = new UpdateUserRequest()
            {
             UserId = userUpdateParameters.UserId.Value.ToString(),
             Username = userUpdateParameters.UserName.Value,
             FirstName = userUpdateParameters.FirstName.Value,
             LastName = userUpdateParameters.LastName.Value,
             Status = userUpdateParameters.Status,
             Email = userUpdateParameters.Email.Value,
             Role = userUpdateParameters.UserType,
             CustomerPermissions = customerPermissions
            };
            
            return updateUserRequest;
        }
    }
}
