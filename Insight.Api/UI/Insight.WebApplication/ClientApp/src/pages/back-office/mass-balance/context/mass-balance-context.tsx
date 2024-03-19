import { FC, ReactNode, createContext, useContext, useState } from "react";
import { getStartAndEndDateOfLastMonth } from "../../../../util/date-utils";
import { formatDate } from "../../../../util/formatters/formatters";
import { MassBalanceData } from "../table/mass-balance-columns";
import useDeepCompare from "../../../../hooks/use-deep-compare/use-deep-compare";

interface Props {
  children: ReactNode;
}

export type BiofuelExpressCompanies =
  | "Biofuel Express AB"
  | "Biofuel Express AS"
  | "Biofuel Express Austria GmbH"
  | "Biofuel Express DMCC"
  | "Biofuel Express GmbH";

export const biofuelExpressCompanies: BiofuelExpressCompanies[] = [
  "Biofuel Express AB",
  "Biofuel Express AS",
  "Biofuel Express Austria GmbH",
  "Biofuel Express DMCC",
  "Biofuel Express GmbH",
];

type CompanyData = {
  locationName: string;
  data: MassBalanceData[];
};
type ParentData = {
  companies: CompanyData[];
};

interface MassBalanceContextProps {
  filter: CustomMassBalanceFilter;
  setFilter: React.Dispatch<React.SetStateAction<CustomMassBalanceFilter>>;
  setSelectedCompany: React.Dispatch<
    React.SetStateAction<BiofuelExpressCompanies>
  >;
  dataToDisplay: ParentData;
  toggleUnitOfMeasurement: (unit: UnitOfMeasurement) => void;
  unitOfMeasurement: UnitOfMeasurement;
}

const MassBalanceContext = createContext<MassBalanceContextProps>(
  {} as MassBalanceContextProps,
);

type CustomMassBalanceFilter = {
  dateFrom: string;
  dateTo: string;
};

const defaultFilter: CustomMassBalanceFilter = {
  dateFrom: formatDate(getStartAndEndDateOfLastMonth().dateFrom),
  dateTo: formatDate(getStartAndEndDateOfLastMonth().dateTo),
};

export type UnitOfMeasurement = "liter" | "mt";

export const MassBalanceContextProvider: FC<Props> = ({ children }: Props) => {
  const [filter, setFilter] = useState<CustomMassBalanceFilter>(defaultFilter);
  const [unitOfMeasurement, setUnitOfMeasurement] =
    useState<UnitOfMeasurement>("liter");
  const [selectedCompany, setSelectedCompany] =
    useState<BiofuelExpressCompanies>("Biofuel Express AB");

  const [dataToDisplay, setDataToDisplay] = useState<ParentData>({
    companies: [],
  });

  // TODO - KAH - Create method to fetch data from backend.
  // Put the data in a useState, update the useDeepCompare for selected companies to use that data,
  // replace types with types from backend, apply filter to endpoint

  //MOCK DATA
  const BFAB: ParentData = {
    companies: [
      {
        locationName: "Goteborg",
        data: [
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
        ],
      },
      {
        locationName: "Norrkoping",
        data: [
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
        ],
      },
      {
        locationName: "Malmo",
        data: [
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
        ],
      },
    ],
  };

  // MOCK
  const BFAS: ParentData = {
    companies: [
      {
        locationName: "Helsinki",
        data: [
          { data: "5001", fuelType: "HVO100" },
          { data: "5001", fuelType: "HVO100" },
        ],
      },
      {
        locationName: "Oslo",
        data: [
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
          { data: "5001", fuelType: "Diesel" },
        ],
      },
      {
        locationName: "Trondheim",
        data: [
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
          { data: "5001", fuelType: "Adblue" },
        ],
      },
    ],
  };

  const toggleUnitOfMeasurement = (unit: UnitOfMeasurement) => {
    setUnitOfMeasurement(unit);
  };

  useDeepCompare(() => {
    if (unitOfMeasurement === "liter") {
      //TODO - kah - switch to liter or mt
    }
    if (selectedCompany === "Biofuel Express AB") {
      setDataToDisplay(BFAB);
    }
    if (selectedCompany === "Biofuel Express AS") {
      setDataToDisplay(BFAS);
    }
    if (selectedCompany === "Biofuel Express Austria GmbH") {
      setDataToDisplay({ companies: [] });
    }
    if (selectedCompany === "Biofuel Express DMCC") {
      setDataToDisplay({ companies: [] });
    }
    if (selectedCompany === "Biofuel Express GmbH") {
      setDataToDisplay({ companies: [] });
    }
  }, [selectedCompany]);

  const props: MassBalanceContextProps = {
    filter,
    setFilter,
    setSelectedCompany,
    dataToDisplay,
    toggleUnitOfMeasurement,
    unitOfMeasurement,
  };

  return (
    <MassBalanceContext.Provider value={props}>
      {children}
    </MassBalanceContext.Provider>
  );
};

export const useMassBalanceContext = () => useContext(MassBalanceContext);
