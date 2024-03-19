using Insight.IncomingDeclarations.Integration.GetIncomingDeclarationsByIds;
using Insight.OutgoingDeclarations.Domain;
using System.Globalization;
using Marten;
using Quantity = Insight.OutgoingDeclarations.Domain.Quantity;

namespace Insight.OutgoingDeclarations.Application.Helpers;

public static class SustainabilityReportPdfHelper
{
    #region FuelConsumption

    public static Consumptionstatscontinued GetConsumptionStatsContinued(decimal totalConsumption,
        decimal renewalConsumption)
    {

        var renewablePercentage = totalConsumption == 0 ? 0 : (decimal)renewalConsumption / totalConsumption * 100;

        return new Consumptionstatscontinued()
        {
            CurrentConsumptions = Convert.ToInt32(totalConsumption),
            RenewCurrentConsumptions = Convert.ToInt32(renewalConsumption),
            ConsumptionPercentage = Convert.ToInt32(renewablePercentage)
        };
    }

    #endregion

    #region CarbonFootprint
    public static Progress GetProgress(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationsDto incomingDeclarations)
    {
        decimal emissions = 0;
        decimal emissionReductions = 0;
        var categoryList = new List<string>();
        List<decimal> emissionReductionList = new List<decimal>();
        List<decimal> emissionsList = new List<decimal>();
      
        if (outgoingDeclarations.IsEmpty())
        {
            return new Progress()
            {
                Categories = categoryList,
                EmissionReduction = emissionReductionList,
                Emissions = emissionsList
            };
        }
        var minDate = outgoingDeclarations.Select(o=> o.DatePeriod.StartDate).Min();
        var maxDate = outgoingDeclarations.Select(o=> o.DatePeriod.EndDate).Max();

        foreach (var outgoingDeclaration in outgoingDeclarations)
        {
            foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
            {
                var volume = incomingDeclarationPairing.Quantity.Value;
                var incomingDeclaration = incomingDeclarations.IncomingDeclarations.First(c =>
                    c.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                var ghgReduction = incomingDeclaration.GhgEmissionSaving;
                // Is 34 = GhGgCO2EqPerMJ
                var baseLine = (volume * incomingDeclaration.GhGgCO2EqPerMJ * incomingDeclaration.FossilFuelComparatorgCO2EqPerMJ) / 1000000;
                var reduction = baseLine * ghgReduction;
                emissionReductions += reduction;

                var emission = baseLine - reduction;
                emissions += emission;
            }
        }

        for (var curPeriod = minDate; curPeriod < maxDate; curPeriod = curPeriod.AddMonths(1))
        {
            categoryList.Add(curPeriod.ToString("M/yyyy", CultureInfo.InvariantCulture));
            var emissionsForPeriod = 0m;
            var emissionReductionsForPeriod = 0m;

            var outgoingDeclarationsForPeriod = outgoingDeclarations.Where(o => o.DatePeriod.StartDate.Month == curPeriod.Month && o.DatePeriod.StartDate.Year == curPeriod.Year);
            foreach (var outgoingDeclaration in outgoingDeclarationsForPeriod)
            {
                foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
                {
                    var volume = incomingDeclarationPairing.Quantity.Value;
                    var incomingDeclaration = incomingDeclarations.IncomingDeclarations.First(c =>
                        c.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                    var ghgReduction = incomingDeclaration.GhgEmissionSaving;
                    // Is 34 = GhGgCO2EqPerMJ, 1.000.000 is gram to ton co2
                    var baseLine = (volume * incomingDeclaration.GhGgCO2EqPerMJ *
                                    incomingDeclaration.FossilFuelComparatorgCO2EqPerMJ) / 1000000;
                    var reduction = baseLine * ghgReduction;
                    emissionReductionsForPeriod += reduction;

                    var emission = baseLine - reduction;
                    emissionsForPeriod += emission;
                }
            }

            emissionReductionList.Add(decimal.Round(emissionReductionsForPeriod, 2));
            emissionsList.Add(decimal.Round(emissionsForPeriod, 2));
        }

        return new Progress()
        {
            Categories = categoryList,
            EmissionReduction = emissionReductionList,
            Emissions = emissionsList
        };
    }

    public static Emissionsstats GetEmissionStats(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationsDto incomingDeclarations)
    {
        decimal emissions = 0;
        decimal achievedEmissionReductions = 0;
        decimal totalBaseLine = 0;

        if (outgoingDeclarations.IsEmpty())
        {
            return new Emissionsstats()
            {
                NetEmission = 0,
                AchievedEmissionReductions = 0,
                EmissionSavingsForCircle = 0
            };
        }
        foreach (var outgoingDeclaration in outgoingDeclarations)
        {
            foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
            {
                var volume = incomingDeclarationPairing.Quantity.Value;
                var incomingDeclaration = incomingDeclarations.IncomingDeclarations.First(c => c.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                var ghgEmissionSaving = incomingDeclaration.GhgEmissionSaving;
                // Is 34 = GhGgCO2EqPerMJ
                var baseLine = (volume * incomingDeclaration.GhGgCO2EqPerMJ * incomingDeclaration.FossilFuelComparatorgCO2EqPerMJ) / 1000000;
                totalBaseLine += baseLine;
                var reduction = baseLine * ghgEmissionSaving;
                achievedEmissionReductions += reduction;

                var emission = baseLine - reduction;
                emissions += emission;
            }
        }

        var ghgEmissionSavings = achievedEmissionReductions / totalBaseLine;

        return new Emissionsstats()
        {
            AchievedEmissionReductions = Convert.ToInt32(achievedEmissionReductions),
            NetEmission = Convert.ToInt32(emissions), 
            EmissionSavingsForCircle = decimal.Round(ghgEmissionSavings,2) * 100,
        };
    }

    #endregion

    #region ProofOfSustainability

    public static Declarationinfo GetDeclarationInfo(GetIncomingDeclarationDto? incomingDeclaration, OutgoingDeclaration outgoingDeclaration)
    {
        return new Declarationinfo()
        {
            Id = incomingDeclaration!.BatchId.ToString(),
            DateOfIssuance = outgoingDeclaration.DateOfCreation.Value.ToDateTime(TimeOnly.MinValue)
        };
    }

    public static Recipient GetRecipient(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationDto incomingDeclaration)
    {
        var outgoingDeclaration = outgoingDeclarations.FirstOrDefault(o => o.IncomingDeclarationPairings.Any(c => c.IncomingDeclarationId.Value == incomingDeclaration.IncomingDeclarationId));
        if (outgoingDeclaration == null)
        {
            throw new ArgumentException("Can't find outgoing declaration for incoming declaration");
        }

        var creationDate = outgoingDeclaration.DateOfCreation.Value.ToDateTime(TimeOnly.MinValue);
        var firstOfCreationMonth = new DateTime(creationDate.Year, creationDate.Month, 1);
        var lastOfCreationMonth = firstOfCreationMonth.AddMonths(1).AddDays(-1);

        return new Recipient()
        {
            Address = new Address()
            {
                Name = outgoingDeclaration.CustomerDetails.CustomerName.Value,
                Street = outgoingDeclaration.CustomerDetails.CustomerAddress.Value,
                StreetNumber = outgoingDeclaration.CustomerDetails.CustomerAddress.Value, //TODO: This is contained on address, should we remove the street number??
                ZipCode = outgoingDeclaration.CustomerDetails.CustomerPostCode.Value,
                City = outgoingDeclaration.CustomerDetails.CustomerCity.Value,
                Country = outgoingDeclaration.CustomerDetails.CustomerCountryRegion.Value,
            },

            PeriodFrom = firstOfCreationMonth,
            PeriodTo = lastOfCreationMonth
        };
    }

    public static Renewablefuelsupplier GetRenewableFuelSupplier(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationDto incomingDeclaration)
    {
        //TODO: How to do this?
        //Information is not on GetCompanies -> Biofuel tell us
        //CertificateNumber and System needs to be changed
        return new Renewablefuelsupplier()
        {
            Address = new Address()
            {
                Name = "This is not correct",
                Street = "Mariebergsgatan",
                StreetNumber = "6",
                ZipCode = "26151",
                City = "Landskrona",
                Country = "Sverige"
            },
            CertificateNumber = $"CHANGE:{incomingDeclaration.CertificationNumber}",
            CertificateSystem = $"CHANGE:{incomingDeclaration.CertificationSystem}"
        };
    }

    public static Renewablefuel GetRenewableFuel(GetIncomingDeclarationDto incomingDeclaration, Quantity quantity)
    {
        // Check with BFE
        var engergyContent = (incomingDeclaration.EnergyContent / incomingDeclaration.Quantity) * quantity.Value;

        return new Renewablefuel()
        {
            Volume = Convert.ToInt32(quantity.Value),
            Product = incomingDeclaration.TypeOfProduct,
            EnergyContent = Convert.ToInt32(engergyContent)
        };
    }

    public static Scopeofcertificationandghgemission GetScopeOfCertificationAndGhgEmission(GetIncomingDeclarationDto incomingDeclaration)
    {
        return new Scopeofcertificationandghgemission()
        {
            EuRedCompliantMaterial = incomingDeclaration.ComplianceWithEuRedMaterialCriteria,
            IsccCompliantMaterial = incomingDeclaration.ComplianceWithIsccMaterialCriteria,
            ChainOfCustodyOption = incomingDeclaration.ChainOfCustodyOption,
            TotalDefaultValueAccordingToRed2Applied = incomingDeclaration.TotalDefaultValueAccordingToREDII
        };
    }

    public static Rawmaterialsustainability GetRawMaterialSustainability(GetIncomingDeclarationDto incomingDeclaration)
    {
        return new Rawmaterialsustainability()
        {
            CountryOfOrigin = incomingDeclaration.CountryOfOrigin,
            DateOfInstallation = incomingDeclaration.DateOfInstallation,
            ProductionCountry = incomingDeclaration.ProductionCountry,
            RawMaterial = incomingDeclaration.RawMaterial
        };
    }

    public static Scopeofcertificationofrawmaterial GetScopeOfCertificationOfRawMaterial(GetIncomingDeclarationDto incomingDeclaration)
    {
        return new Scopeofcertificationofrawmaterial()
        {
            Option1 = incomingDeclaration.ComplianceWithSustainabilityCriteria,
            Option2 = incomingDeclaration.CultivatedAsIntermediateCrop,
            Option3 = incomingDeclaration.FulfillsMeasuresForLowILUCRiskFeedstocks,
            Option4 = incomingDeclaration.MeetsDefinitionOfWasteOrResidue,
            Option5 = incomingDeclaration.SpecifyNUTS2Region
        };
    }

    public static Greenhousegasemissionssavings GetGreenhouseGasEmissionsSavings(GetIncomingDeclarationDto incomingDeclaration)
    {
        return new Greenhousegasemissionssavings()
        {
            GhgPercent = incomingDeclaration.GhgEmissionSaving
        };
    }

    public static Lifecyclegreenhousegasemissions GetLifeCycleGreenhouseGasEmissions(GetIncomingDeclarationDto incomingDeclaration)
    {
        return new Lifecyclegreenhousegasemissions()
        {
            CarbonCaptureAndGeologicalStorage = Convert.ToInt32(incomingDeclaration.CarbonCaptureAndGeologicalStorage),
            CarbonCaptureAndReplacement = Convert.ToInt32(incomingDeclaration.CarbonCaptureAndReplacement),
            ExtractionOrCultivation = incomingDeclaration.ExtractionOrCultivation,
            FuelInUse = Convert.ToInt32(incomingDeclaration.FuelInUse),
            LandUse = Convert.ToInt32(incomingDeclaration.LandUse),
            Processing = incomingDeclaration.Processing,
            SoilCarbonAccumulation = Convert.ToInt32(incomingDeclaration.SoilCarbonAccumulation),
            TotalGHGEmissionFromSupplyAndUseOfFuel = incomingDeclaration.TotalGHGEmissionFromSupplyAndUseOfFuel,
            TransportAndDistribution = Convert.ToInt32(incomingDeclaration.TransportAndDistribution)
        };
    }

    #endregion

    #region Traceability
    public static Countryoforigin[] GetCountriesOfOrigin(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationsDto incomingDeclarations)
    {
        //Frontend needs data based on Country not country of origin
        var countriesOfOrigins = new Dictionary<string, Countryoforigin>();
        foreach (var outgoingDeclaration in outgoingDeclarations)
        {
            foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
            {
                var declaration = incomingDeclarations.IncomingDeclarations.FirstOrDefault(i => i.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                if (declaration != null)
                {
                    var country = declaration.CountryOfOrigin;
                    var volume = incomingDeclarationPairing.Quantity.Value;
                    var savings = declaration.GhgEmissionSaving * 100;

                    if (!countriesOfOrigins.TryAdd(country, new Countryoforigin()
                    {
                        CooFeedstock = country,
                        Volume = volume,
                        AverageSavings = savings
                    }))
                    {
                        countriesOfOrigins[country].AverageSavings = ((countriesOfOrigins[country].AverageSavings * countriesOfOrigins[country].Volume) + (savings * volume)) / (countriesOfOrigins[country].Volume + volume);
                        countriesOfOrigins[country].Volume += volume;
                    }
                }
            }
        }

        return countriesOfOrigins.Select(c => c.Value).ToArray();
    }
    public static ProductSpecificationItem[] GetProductSpecificationItems(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationsDto incomingDeclarations)
    {
        //We only need info on the following ProductNames: HV0, Diesel, Petrol, B100
        //In addition the last key should be named Total -> Which is a summation of the other keys
        var productSpecifications = new Dictionary<string, ProductSpecificationItem>();

        if (outgoingDeclarations.IsEmpty())
        {
            return Array.Empty<ProductSpecificationItem>();
        }
        foreach (var outgoingDeclaration in outgoingDeclarations)
        {
            foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
            {
                var declaration = incomingDeclarations.IncomingDeclarations.FirstOrDefault(i => i.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                if (declaration != null)
                {
                    var volume = declaration.Quantity;
                    var fuelType = declaration.Product;
                    var ghgEmissionSaving = declaration.GhgEmissionSaving;
                    var baseLine = (volume * declaration.GhGgCO2EqPerMJ * declaration.FossilFuelComparatorgCO2EqPerMJ) / 1000000;
                    var achievedEmissionReduction = baseLine * ghgEmissionSaving;
                    var emission = baseLine - achievedEmissionReduction;
                    
                    var products = new List<string>()
                        {
                            "HVO100",
                            "DIESEL",
                            "PETROL",
                            "B100"
                        };

                    if (products.Contains(fuelType.ToUpperInvariant(), StringComparer.InvariantCulture))
                    {
                        if (!productSpecifications.TryAdd(fuelType, new ProductSpecificationItem()
                        {
                            FuelType = fuelType,
                            Volume = volume,
                            NetEmission = emission,
                            AchievedEmissionReduction = achievedEmissionReduction,
                            GhgBaseline = baseLine,
                            GhgEmissionSaving = ghgEmissionSaving
                        }))
                        {
                            productSpecifications[fuelType].Volume += volume;
                            productSpecifications[fuelType].NetEmission += emission;
                            productSpecifications[fuelType].AchievedEmissionReduction += achievedEmissionReduction;
                            productSpecifications[fuelType].GhgEmissionSaving += ghgEmissionSaving;

                        }
                    }
                }
            }
        }

        productSpecifications.TryAdd("Total", new ProductSpecificationItem()
        {
            FuelType = "Total",
            Volume = decimal.Round(productSpecifications.Sum(c => c.Value.Volume),2),
            GhgBaseline = decimal.Round(productSpecifications.Sum(c => c.Value.GhgBaseline), 2),
            GhgEmissionSaving = decimal.Round(productSpecifications.Sum(c => c.Value.GhgEmissionSaving), 2),
            NetEmission = decimal.Round(productSpecifications.Sum(c => c.Value.NetEmission), 2),
            AchievedEmissionReduction = decimal.Round(productSpecifications.Sum(c => c.Value.AchievedEmissionReduction),2)
        });

        return productSpecifications.Select(c => new ProductSpecificationItem()
        { FuelType = c.Key,
            Volume = decimal.Round(c.Value.Volume,2),
            NetEmission = decimal.Round(c.Value.NetEmission,2),
            GhgEmissionSaving = decimal.Round(c.Value.GhgEmissionSaving,2),
            GhgBaseline = decimal.Round(c.Value.GhgBaseline,2),
            AchievedEmissionReduction = decimal.Round(c.Value.AchievedEmissionReduction,2) }).ToArray();
    }
    public static Country[] GetCountries(IEnumerable<OutgoingDeclaration> outgoingDeclarations,
        GetIncomingDeclarationsDto incomingDeclarations)
    {
        if (outgoingDeclarations.IsEmpty())
        {
            return Array.Empty<Country>();
        }
        var countriesQuantities = new Dictionary<string, decimal>();
        foreach (var outgoingDeclaration in outgoingDeclarations)
        {
            foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
            {
                var declaration = incomingDeclarations.IncomingDeclarations.FirstOrDefault(i => i.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                if (declaration != null)
                {
                    var country = declaration.Country;
                    var quantity = incomingDeclarationPairing.Quantity.Value;

                    if (!countriesQuantities.TryAdd(country, quantity))
                    {
                        countriesQuantities[country] += quantity;
                    }
                }
            }
        }

        var totalQuantities = countriesQuantities.Sum(c => c.Value);
        var countries = countriesQuantities.Select(c => new Country()
        { Name = c.Key, Percentage = c.Value / totalQuantities * 100 }).ToArray();
        return countries;
    }
    public static Feedstock[] GetFeedstocks(IEnumerable<OutgoingDeclaration> outgoingDeclarations, GetIncomingDeclarationsDto incomingDeclarations)
    {
        var feedStocksQuantities = new Dictionary<string, decimal>();
        if (outgoingDeclarations.IsEmpty())
        {
            return Array.Empty<Feedstock>();
        }
        foreach (var outgoingDeclaration in outgoingDeclarations)
        {
            foreach (var incomingDeclarationPairing in outgoingDeclaration.IncomingDeclarationPairings)
            {
                var declaration = incomingDeclarations.IncomingDeclarations.FirstOrDefault(i => i.IncomingDeclarationId == incomingDeclarationPairing.IncomingDeclarationId.Value);
                if (declaration != null)
                {
                    var quantity = incomingDeclarationPairing.Quantity.Value;
                    var material = declaration.RawMaterial;
                    if (!feedStocksQuantities.TryAdd(material, quantity))
                    {
                        feedStocksQuantities[material] += quantity;
                    }
                }
            }
        }
        var totalQuantities = feedStocksQuantities.Sum(c => c.Value);
        var feedStocks = feedStocksQuantities.Select(c => new Feedstock()
        { Name = c.Key, Percentage = c.Value / totalQuantities * 100 }).ToArray();
        return feedStocks;
    }
    #endregion


}